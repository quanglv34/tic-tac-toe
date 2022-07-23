#ifndef _SERVER_H_
#define _SERVER_H_

#pragma once
#define _WINSOCK_DEPRECATED_NO_WARNINGS
#define _CRT_SECURE_NO_WARNINGS
#include <WinSock2.h>
#include <WS2tcpip.h>
#pragma comment (lib,"ws2_32.lib")
#include <stdio.h>
#include <math.h>
#include <time.h>
#include <string>
#include <process.h>
#include <windows.h>
#include <conio.h>
#include <vector>
#include <iostream>
#include "constants.h"
#include "connection.h"
#include "database.h"
#include "file.h"
#include "room.h"

#define NO_CLIENT -1

CLIENT clients[WSA_MAXIMUM_WAIT_EVENTS];
vector<Room> rooms;
WSAEVENT events[WSA_MAXIMUM_WAIT_EVENTS];
DWORD nEvents = 0;
DWORD index;
WSANETWORKEVENTS sockEvent;


CLIENT* findClientByUsername(char *username);
CLIENT* findClientBySocket(SOCKET socket);
int findClientIndexBySocket(SOCKET socket);
void prepareClientToSendFile(CLIENT* aClient, char* filename, int size);
void updateMatchPlayers(CLIENT* winner, CLIENT* loser);
void updateMatchLog(Room* aRoom, CLIENT* client1, CLIENT* client2);
void removeClient(int index);
void removeRoom(SOCKET socket);
void handleRecv(CLIENT* aClient);
void handleRecvSignUp(CLIENT* aClient);
void handleRecvSignIn(CLIENT* aClient);
void handleRecvSignOut(CLIENT* aClient);
void handleRecvList(CLIENT* aClient);
void handleRecvChallenge(CLIENT* aClient);
void handleRecvChallengeAccept(CLIENT* aClient);
void handleRecvChallengeRefuse(CLIENT* aClient);
void handleRecvInfo(CLIENT* aClient);
void handleRecvPlay(CLIENT* aClient);
void handleRecvSurrender(CLIENT* aClient);
int handleSend(CLIENT* aClient, int index);
int handleSendFile(CLIENT* aClient);

void removeClient(int index) {
	CLIENT* aClient = &clients[index];
	closesocket(aClient->socket);
	// Update user in database if user is signed in
	if (aClient->isLoggedIn) {
		updateUserIsFree(aClient->username, UPDATE_USER_NOT_BUSY);
		updateUserStatus(aClient->username, UPDATE_USER_STATUS_OFFLINE);
		updateUserChallenge(aClient->username, "");
	}
	// Move the last client to the client at current index
	*aClient = clients[nEvents - 1];
	initClient(&clients[nEvents - 1]);
	// Move the last event to the event at current index
	WSACloseEvent(events[index]);
	events[index] = events[nEvents - 1];
	events[nEvents - 1] = 0;
	nEvents--;
}

void removeRoom(SOCKET socket) {
	int i;
	for (i = 0; i < rooms.size(); i++)
		if (rooms[i].isPlayerInRoom(socket)) break;
	if (i != rooms.size())
		rooms.erase(rooms.begin() + i);
}

CLIENT* findClientByUsername(char *username) {
	for (int i = 0; i < (int)nEvents; i++)
		if (clients[i].isLoggedIn && strcmp(clients[i].username, username) == 0) return &clients[i];
	return NULL;
}

CLIENT* findClientBySocket(SOCKET socket) {
	for (int i = 0; i < (int)nEvents; i++)
		if (clients[i].socket == socket) return &clients[i];
	return NULL;
}

int findClientIndexBySocket(SOCKET socket) {
	int i;
	for (i = 0; i < (int)nEvents; i++)
		if (clients[i].socket == socket) return i;
	return NO_CLIENT;
}

Room* findRoomBySocket(SOCKET socket) {
	for (int i = 0; i < rooms.size(); i++) {
		if (rooms[i].isPlayerInRoom(socket)) {
			return &rooms[i];
		}
	}
	return NULL;
}

/*
function handleRecvList: Assign client's request to a suitable handle
@param aClient: The client sent the request
*/
void handleRecv(CLIENT* aClient) {
	switch (aClient->opcode) {
	case OPCODE_LOG_IN:
		handleRecvSignIn(aClient);
		break;
	case OPCODE_LOG_OUT:
		handleRecvSignOut(aClient);
		break;
	case OPCODE_SIGN_UP:
		handleRecvSignUp(aClient);
		break;
	case OPCODE_LIST:
		handleRecvList(aClient);
		break;
	case OPCODE_INFO:
		handleRecvInfo(aClient);
		break;
	case OPCODE_CHALLENGE:
		handleRecvChallenge(aClient);
		break;
	case OPCODE_CHALLENGE_ACCEPT:
		handleRecvChallengeAccept(aClient);
		break;
	case OPCODE_CHALLENGE_REFUSE:
		handleRecvChallengeRefuse(aClient);
		break;
	case OPCODE_SURRENDER:
		handleRecvSurrender(aClient);
		break;
	case OPCODE_PLAY:
		handleRecvPlay(aClient);
		break;
	default:
		break;
	}
}

/*
function handleRecvList: Assign client's reply to a suitable handle
@param aClient: The client to reply
@param index: The index of the client
*/
int handleSend(CLIENT* aClient, int index) {
	int ret;
	switch (aClient->opcode) {
	case OPCODE_FILE:
		ret = handleSendFile(aClient);
		if (aClient->bytesInFile == aClient->bytesRead) {
			WSAEventSelect(aClient->socket, events[index], FD_READ | FD_CLOSE);
		}
		break;
	default:
		break;
	}
	return ret;
}

/*
function handleRecvList: Handle OPCODE_SIGN_UP request
@param aClient: The client that requested
*/
void handleRecvSignUp(CLIENT* aClient) {
	string payload(aClient->buff);
	char *username, *password;
	// Validate username and password
	if (count(payload.begin(), payload.end(), ' ') != 1) {
		Send(aClient, OPCODE_LOG_IN_UNKNOWN_ERROR, 0, NULL);
		return;
	}
	username = strtok(aClient->buff, " ");
	password = strtok(NULL, " ");
	if (username == NULL) {
		Send(aClient, OPCODE_LOG_IN_INVALID_USERNAME, 0, NULL);
		return;
	}
	if (password == NULL) {
		Send(aClient, OPCODE_LOG_IN_INVALID_PASSWORD, 0, NULL);
		return;
	}
	// Check for duplicate username
	int ret = updateSignUp(username, password);
	if (ret == OPCODE_SIGN_UP_DUPLICATED_USERNAME) {
		Send(aClient, OPCODE_SIGN_UP_DUPLICATED_USERNAME, 0, NULL);
		return;
	}
	else {
		Send(aClient, OPCODE_SIGN_UP_UNKNOWN_ERROR, 0, NULL);
		return;
	}
	// Confirm sign up complete
	Send(aClient, OPCODE_SIGN_UP_SUCESS, 0, NULL);
	return;
}

/*
function handleRecvList: Handle OPCODE_SIGN_IN request
@param aClient: The client that requested
*/
void handleRecvSignIn(CLIENT* aClient) {
	// Validate logged in user
	if (aClient->isLoggedIn) {
		Send(aClient, OPCODE_LOG_IN_ALREADY_LOGGED_IN, 0, NULL);
		return;
	}
	string payload(aClient->buff);
	char *username, *password;
	// Validate username and password
	if (count(payload.begin(), payload.end(), ' ') != 1) {
		Send(aClient, OPCODE_LOG_IN_UNKNOWN_ERROR, 0, NULL);
		return;
	}
	username = strtok(aClient->buff, " ");
	password = strtok(NULL, " ");
	if (username == NULL) {
		Send(aClient, OPCODE_LOG_IN_INVALID_USERNAME, 0, NULL);
		return;
	}
	if (password == NULL) {
		Send(aClient, OPCODE_LOG_IN_INVALID_PASSWORD, 0, NULL);
		return;
	}
	// Check for signed in user
	int ret = updateSignIn(username, password);
	switch (ret) {
	case OPCODE_LOG_IN_SUCESS:
		aClient->isLoggedIn = true;
		strcpy_s(aClient->username, username);
		Send(aClient, OPCODE_LOG_IN_SUCESS, 0, NULL);
		break;
	default:
		Send(aClient, ret, 0, NULL);
		break;
	}
};

/*
function handleRecvList: Handle OPCODE_SIGN_OUT request
@param aClient: The client that requested
*/
void handleRecvSignOut(CLIENT* aClient) {
	if (aClient->isLoggedIn) {
		aClient->isLoggedIn = false;
		updateUserIsFree(aClient->username, UPDATE_USER_NOT_BUSY);
		updateUserStatus(aClient->username, UPDATE_USER_STATUS_OFFLINE);
		Send(aClient, OPCODE_LOG_OUT_SUCCESS, 0, NULL);
		return;
	}
	else {
		Send(aClient, OPCODE_LOG_IN_ALREADY_LOGGED_IN, 0, NULL);
		return;
	}
}

/*
function handleRecvList: Handle OPCODE_LIST request
@param aClient: The client that requested
*/
void handleRecvList(CLIENT* aClient) {
	string payload = getFreePlayerList(aClient->username);
	string test = payload.c_str();
	Send(aClient, OPCODE_LIST_REPLY, (unsigned short)payload.size(), (char*)payload.c_str());
	return;
}

/*
function handleRecvList: Handle OPCODE_CHALLENGE request
@param aClient: The client that requested
*/
void handleRecvChallenge(CLIENT* challengeSender) {
	char* challengeReceiverUsername = challengeSender->buff;
	CLIENT* challengeReceiver = findClientByUsername(challengeReceiverUsername);
	// Check if challengeReceiver is online
	if (challengeReceiver == NULL) {
		Send(challengeSender, OPCODE_CHALLENGE_NOT_FOUND, 0, NULL);
		return;
	}
	// Check if challengeReceiver is free
	if (getUserFreeStatus(challengeReceiver->username) == UPDATE_USER_BUSY) {
		Send(challengeSender, OPCODE_CHALLENGE_BUSY, 0, NULL);
		return;
	}
	// Check if 2 players' rank are valid
	int rankDifference = abs(getRank(challengeSender->username) - getRank(challengeReceiverUsername));
	if (rankDifference > 10) {
		Send(challengeSender, OPCODE_CHALLENGE_INVALID_RANK, 0, NULL);
		return;
	}

	updateUserChallenge(challengeSender->username, challengeReceiverUsername);
	Send(challengeReceiver, OPCODE_CHALLENGE, (unsigned short)strlen(challengeSender->username) + 1, challengeSender->username);
	return;
}

/*
function handleRecvChallengeAccept: Handle OPCODE_CHALLENGE_ACCEPT request
@param aClient: The client that requested
*/
void handleRecvChallengeAccept(CLIENT* challengeReceiver) {
	char* challengeSenderUsername = challengeReceiver->buff;
	CLIENT* challengeSender = findClientByUsername(challengeSenderUsername);
	// Check if challengeSender is online
	if (challengeSender == NULL) {
		Send(challengeReceiver, OPCODE_CHALLENGE_NOT_FOUND, 0, NULL);
		return;
	}
	// Check if challengeSender did indeed challenged this receiver
	string senderChallenge = getUserCurrentChallenge(challengeSenderUsername);
	if (strcmp((char*)senderChallenge.c_str(), challengeReceiver->username) != 0) {
		Send(challengeReceiver, OPCODE_CHALLENGE_NOT_FOUND, 0, NULL);
		return;
	}
	updateUserIsFree(challengeReceiver->username, UPDATE_USER_BUSY);
	updateUserIsFree(challengeSenderUsername, UPDATE_USER_BUSY);
	updateUserChallenge(challengeSender->username, "");
	Send(challengeSender, OPCODE_CHALLENGE_ACCEPT, (unsigned short)size(senderChallenge), (char*)senderChallenge.c_str());
	Send(challengeReceiver, OPCODE_CHALLENGE_ACCEPT, 0, NULL);
	// Add room
	Room room = Room(challengeSender->socket, challengeReceiver->socket);
	rooms.push_back(room);
}

/*
function handleRecvChallengeRefuse: Handle OPCODE_CHALLENGE_REFUSE request
@param aClient: The client that requested
*/
void handleRecvChallengeRefuse(CLIENT* challengeReceiver) {
	char* challengeSenderUsername = challengeReceiver->buff;
	CLIENT* challengeSender = findClientByUsername(challengeSenderUsername);
	// Check if challengeSender is online
	if (challengeSender == NULL) {
		return;
	}
	// Check if challengeSender did indeed challenged this receiver
	string senderChallenge = getUserCurrentChallenge(challengeSenderUsername);
	if (strcmp((char*)senderChallenge.c_str(), challengeReceiver->username) != 0) {
		return;
	}
	updateUserChallenge(challengeSender->username, "");
	Send(challengeSender, OPCODE_CHALLENGE_REFUSE, (unsigned short)strlen(challengeReceiver->username) + 1, challengeReceiver->username);
	return;
}

/*
function handleRecvList: Handle OPCODE_INFO request
@param aClient: The client that requested
*/
void handleRecvInfo(CLIENT* aClient) {
	if (!aClient->isLoggedIn) {
		Send(aClient, OPCODE_INFO_NOT_FOUND, 0, NULL);
		return;
	}
	int score = getScore(aClient->username);
	int rank = getRank(aClient->username);
	string msg = to_string(score) + " " + to_string(rank);
	Send(aClient, OPCODE_INFO_FOUND, (unsigned short)msg.size(), (char*)msg.c_str());
}

/*
function handleRecvList: Handle OPCODE_PLAY request
@param aClient: The client that requested
*/
void handleRecvPlay(CLIENT* aClient) {
	Room* aRoom = findRoomBySocket(aClient->socket);
	if (aRoom == NULL || !aRoom->isPlayerTurn(aClient->socket)) {
		Send(aClient, OPCODE_PLAY_INVALID_TURN, 0, NULL);
		return;
	}
	char x, y;
	int moveType = aRoom->getPlayerMoveType(aClient->socket);
	getPlayerMoveCoordinate(aClient, &x, &y);
	PlayerMove aMove = { x, y, moveType };
	int ret = aRoom->addPlayerMove(aMove);
	if (ret != OPCODE_PLAY) {
		Send(aClient, (char)ret, 0, NULL);
		return;
	}
	SOCKET opponentSocket = aRoom->getPlayerOpponent(aClient->socket);
	CLIENT* opponentClient = findClientBySocket(opponentSocket);
	char buff[2], *winnerUsername;
	// Send the received player's move to the opponent
	buff[0] = x;
	buff[1] = y;
std:cout << "Player " << aClient->username << " move [" << aMove.x << ", " << aMove.y << "]" << std::endl;
	Send(opponentClient, OPCODE_PLAY_OPPONENT, 2, buff);
	// Check for match result then send the result if the match ends
	int matchResult = aRoom->getMatchResult();
	if (matchResult == MATCH_CONTINUE) { return; }
	switch (matchResult) {
	case MATCH_END_BY_DRAW:
		Send(aClient, OPCODE_RESULT, 0, NULL);
		Send(opponentClient, OPCODE_RESULT, 0, NULL);
		break;
	case MATCH_END_BY_WIN:
		winnerUsername = aClient->username;
		Send(aClient, OPCODE_RESULT, (unsigned short)strlen(winnerUsername), winnerUsername);
		Send(opponentClient, OPCODE_RESULT, (unsigned short)strlen(winnerUsername), winnerUsername);
		updateMatchPlayers(aClient, opponentClient);
		break;
	default:
		break;
	}
	updateMatchLog(aRoom, aClient, opponentClient);
	removeRoom(aClient->socket);
}

/*
function handleRecvSurrender: Handle OPCODE_SURRENDER request
@param aClient: The client that requested
*/
void handleRecvSurrender(CLIENT* aClient) {
	Room* aRoom = findRoomBySocket(aClient->socket);
	if (aRoom == NULL) {
		Send(aClient, OPCODE_SURRENDER_NO_ROOM, 0, NULL);
		return;
	}

	// Send the opponent match result;
	SOCKET opponentSocket = aRoom->getPlayerOpponent(aClient->socket);
	CLIENT* opponentClient = findClientBySocket(opponentSocket);
	Send(opponentClient, OPCODE_RESULT, (unsigned short)strlen(opponentClient->username), opponentClient->username);
	updateMatchPlayers(opponentClient, aClient);
	updateMatchLog(aRoom, aClient, opponentClient);
	removeRoom(aClient->socket);
}

void updateMatchPlayers(CLIENT* winner, CLIENT* loser) {
	updateScoreOfPlayer(winner->username, UPDATE_MATCH_WINNER);
	updateScoreOfPlayer(loser->username, UPDATE_MATCH_LOSER);
	updateUserIsFree(winner->username, UPDATE_USER_NOT_BUSY);
	updateUserIsFree(loser->username, UPDATE_USER_NOT_BUSY);
	updateRank();
}

void updateMatchLog(Room* aRoom, CLIENT* client1, CLIENT* client2) {
	std::vector<PlayerMove> movesList = aRoom->getMovesList();
	char filename[BUFF_SIZE];
	createTempFileName(client1->username, client2->username, filename);
	FILE* logFile = fopen(filename, "w+");
	if (logFile == NULL) {
		Send(client1, OPCODE_FILE_ERROR, 0, NULL);
		Send(client2, OPCODE_FILE_ERROR, 0, NULL);
		return;
	}
	string logString = "IP Address: " + string(client1->address) + "\tPlayer 1: " + string(client1->username)
		+ "\nIP Address: " + string(client2->address) + "\tPlayer 2: " + string(client2->username) + "\n"
		+ "Move Log\n";

	size_t movesCount = movesList.size();
	for (int i = 0; i < movesCount; i++) {
		string move = "{x: " + to_string(movesList[i].x)
			+ ", y: " + to_string(movesList[i].y)
			+ ", type: " + to_string(movesList[i].type) + "}\n";
		logString.append(move);
	}

	fwrite(logString.c_str(), sizeof(char), logString.size(), logFile);
	fclose(logFile);
	prepareClientToSendFile(client1, filename, (int)logString.size());
	prepareClientToSendFile(client2, filename, (int)logString.size());
	handleSendFile(client1);
	handleSendFile(client2);
};

void prepareClientToSendFile(CLIENT* aClient, char* filename, int size) {
	aClient->fPointer = fopen(filename, "r");
	aClient->bytesInFile = size;
	aClient->opcode = OPCODE_FILE;
	int index = findClientIndexBySocket(aClient->socket);
}

/*
function handleRecvList: Handle OPCODE_FILE reply
@param aClient: The client to reply
*/
int handleSendFile(CLIENT* aClient) {
	int ret;
	if (aClient->fPointer == NULL) {
		ret = Send(aClient, OPCODE_FILE_ERROR, 0, NULL);
		closeOpenendFile(aClient);
		return ret;
	}
	size_t bytesToSend, bytesLeft, bytesCanFitInBuff;
	char buff[BUFF_SIZE];
	bytesCanFitInBuff = BUFF_SIZE - OPCODE_SIZE - LENGTH_SIZE;
	bytesLeft = aClient->bytesInFile - aClient->bytesRead;
	bytesToSend = bytesLeft > bytesCanFitInBuff ? bytesCanFitInBuff : bytesLeft;
	bytesCanFitInBuff = fread(buff, sizeof(char), bytesToSend, aClient->fPointer);
	ret = Send(aClient, OPCODE_FILE_DATA, (unsigned short)bytesCanFitInBuff, buff);
	if (ret <= 0) return ret;
	aClient->bytesRead += ret - OPCODE_SIZE - LENGTH_SIZE;
	if (aClient->bytesInFile == aClient->bytesRead) {
		fclose(aClient->fPointer);
		Send(aClient, OPCODE_FILE_DATA, 0, NULL);
	}
	return ret;
}
#endif
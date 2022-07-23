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
#include <chrono>
#include <ctime>    
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
string getCurrentTime();
string getEndReason(int endReasonType, string winner = "");
int findClientIndexBySocket(SOCKET socket);
void prepareClientToSendFile(CLIENT* aClient, char* filename, int size);
void updateMatchPlayers(CLIENT* winner, CLIENT* loser);
void updateMatchLog(Room* aRoom, CLIENT* client1, CLIENT* client2, int endType, string winner);
void removeClient(int index);
void removeRoom(SOCKET socket);
void handleRecv(CLIENT* aClient);
void handleRecvFile(CLIENT* aClient);
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

/*

*/
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
/*

*/
void removeRoom(SOCKET socket) {
	int i;
	for (i = 0; i < rooms.size(); i++)
		if (rooms[i].isPlayerInRoom(socket)) break;
	if (i != rooms.size())
		rooms.erase(rooms.begin() + i);
}
/*


*/
CLIENT* findClientByUsername(char *username) {
	for (int i = 0; i < (int)nEvents; i++)
		if (clients[i].isLoggedIn && strcmp(clients[i].username, username) == 0) return &clients[i];
	return NULL;
}
/*


*/
CLIENT* findClientBySocket(SOCKET socket) {
	for (int i = 0; i < (int)nEvents; i++)
		if (clients[i].socket == socket) return &clients[i];
	return NULL;
}
/*


*/
int findClientIndexBySocket(SOCKET socket) {
	int i;
	for (i = 0; i < (int)nEvents; i++)
		if (clients[i].socket == socket) return i;
	return NO_CLIENT;
}
/*


*/
Room* findRoomBySocket(SOCKET socket) {
	for (int i = 0; i < rooms.size(); i++) {
		if (rooms[i].isPlayerInRoom(socket)) {
			return &rooms[i];
		}
	}
	return NULL;
}
/*
function handleRecvList: Assign client's to a suitable handle
@param aClient: The client sent the
*/
void handleRecv(CLIENT* aClient) {
	switch (aClient->opcode) {
	case OPCODE_PLAY:
		handleRecvPlay(aClient);
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
	case OPCODE_FILE:
		handleRecvFile(aClient);
		break;
	case OPCODE_LOG_IN:
		handleRecvSignIn(aClient);
		break;
	case OPCODE_LOG_OUT:
		handleRecvSignOut(aClient);
		break;
	case OPCODE_SIGN_UP:
		handleRecvSignUp(aClient);
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
function handleRecvList: Handle a request that has opcode equals OPCODE_SIGN_UP
@param aClient: The client that requested
*/
void handleRecvFile(CLIENT* aClient) {
	// Check if there is any ongoing file transfer
	if (aClient->fPointer == NULL) {
		Send(aClient, OPCODE_FILE_NO_FILE, 0, NULL);
		return;
	}
	handleSendFile(aClient);
	return;
}

/*
function handleRecvSignup: Handle a request that has opcode equals OPCODE_SIGN_UP
@param aClient: The client that requested
*/
void handleRecvSignUp(CLIENT* aClient) {
	string payload(aClient->buff);
	string username, password;
	// Validate username and password
	if (payload[20] != ' ' || payload.size() == 0) {
		Send(aClient, OPCODE_SIGN_UP_INVALID_USERNAME, 0, NULL);
		return;
	}
	else if (payload.size() > 41 || payload.size() < 22) {
		Send(aClient, OPCODE_SIGN_UP_INVALID_PASSWORD, 0, NULL);
		return;
	}
	else {
		string un = payload.substr(0, 20);
		password = payload.substr(21);
		if (un[19] == ' ') {
			int index = 19;
			for (int i = index; i >= 0; i--) {
				if (un[i] != ' ') {
					index = i;
					break;
				}
			}
			username = un.substr(0, index + 1);
		}
		else username = un;

		if (username.find(' ') != string::npos) {
			Send(aClient, OPCODE_SIGN_UP_INVALID_USERNAME, 0, NULL);
			return;
		}

		else if (password.find(' ') != string::npos) {
			Send(aClient, OPCODE_SIGN_UP_INVALID_PASSWORD, 0, NULL);
			return;
		}

	}
	// Check for duplicate username
	int ret = updateSignUp((char*)username.c_str(), (char*)password.c_str());
	switch (ret) {
	case OPCODE_SIGN_UP_DUPLICATED_USERNAME:
		Send(aClient, OPCODE_SIGN_UP_DUPLICATED_USERNAME, 0, NULL);
		break;
	case OPCODE_SIGN_UP_UNKNOWN_ERROR:
		Send(aClient, OPCODE_SIGN_UP_UNKNOWN_ERROR, 0, NULL);
		break;
	default:
		Send(aClient, OPCODE_SIGN_UP_SUCESS, 0, NULL);
		break;
	}
}

/*
function handleRecvSignin: Handle a request that has opcode equals OPCODE_SIGN_IN
@param aClient: The client that requested
*/
void handleRecvSignIn(CLIENT* aClient) {
	// Validate logged in user
	if (aClient->isLoggedIn) {
		Send(aClient, OPCODE_LOG_IN_ALREADY_LOGGED_IN, 0, NULL);
		return;
	}
	string payload(aClient->buff);
	string username, password;
	// Validate username and password
	if (payload[20] != ' ' || payload.size() == 0) {
		Send(aClient, OPCODE_LOG_IN_INVALID_USERNAME, 0, NULL);
		return;
	}
	else if (payload.size() > 41 || payload.size() < 22) {
		Send(aClient, OPCODE_LOG_IN_INVALID_PASSWORD, 0, NULL);
		return;
	}
	else {
		string un = payload.substr(0, 20);
		password = payload.substr(21);
		if (un[19] == ' ') {
			int index = 19;
			for (int i = index; i >= 0; i--) {
				if (un[i] != ' ') {
					index = i;
					break;
				}
			}
			username = un.substr(0, index + 1);
		}
		else username = un;

		if (username.find(' ') != string::npos) {
			Send(aClient, OPCODE_LOG_IN_INVALID_USERNAME, 0, NULL);
			return;
		}

		else if (password.find(' ') != string::npos) {
			Send(aClient, OPCODE_LOG_IN_INVALID_PASSWORD, 0, NULL);
			return;
		}

	}
	// Check for signed in user
	int ret = updateSignIn((char*) username.c_str(), (char*) password.c_str());
	switch (ret) {
	case OPCODE_LOG_IN_SUCESS:
		aClient->isLoggedIn = true;
		strcpy_s(aClient->username, (char*) username.c_str());
		Send(aClient, OPCODE_LOG_IN_SUCESS, 0, NULL);
		break;
	default:
		Send(aClient, ret, 0, NULL);
		break;
	}
};

/*
function handleRecvList: Handle a request that has opcode equals OPCODE_SIGN_OUT
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
function handleRecvList: Handle a request that has opcode equals OPCODE_LIST
@param aClient: The client that requested
*/
void handleRecvList(CLIENT* aClient) {
	string payload = getFreePlayerList(aClient->username);
	string test = payload.c_str();
	Send(aClient, OPCODE_LIST_REPLY, (unsigned short)payload.size(), (char*)payload.c_str());
	return;
}

/*
function handleRecvList: Handle a request that has opcode equals OPCODE_CHALLENGE
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
	Send(challengeReceiver, OPCODE_CHALLENGE, (unsigned short) strlen(challengeSender->username) + 1, challengeSender->username);
	return;
}

/*
function handleRecvChallengeAccept: Handle a request that has opcode equals OPCODE_CHALLENGE_ACCEPT
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
	Send(challengeSender, OPCODE_CHALLENGE_ACCEPT, (unsigned short) size(senderChallenge), (char*)senderChallenge.c_str());
	Send(challengeReceiver, OPCODE_CHALLENGE_ACCEPT, 0, NULL);
	// Add room
	Room room = Room(challengeSender->socket, challengeReceiver->socket);
	rooms.push_back(room);
}

/*
function handleRecvChallengeRefuse: Handle a request that has opcode equals OPCODE_CHALLENGE_REFUSE
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
	Send(challengeSender, OPCODE_CHALLENGE_REFUSE, (unsigned short) strlen(challengeReceiver->username) + 1, challengeReceiver->username);
	return;
}

/*
function handleRecvList: Handle a request that has opcode equals OPCODE_INFO
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
	Send(aClient, OPCODE_INFO_FOUND, (unsigned short) msg.size(), (char*)msg.c_str());
}

/*
function handleRecvPlay: Handle a request that has opcode equals OPCODE_PLAY
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
	std::cout << "Player " << aClient->username << " move [" << aMove.x << ", " << aMove.y << "]" << std::endl;
	Send(opponentClient, OPCODE_PLAY_OPPONENT, 2, buff);

	// Check for match result then send the result if the match ends
	int matchResult = aRoom->getMatchResult();
	if (matchResult == MATCH_CONTINUE) { return; }
	switch (matchResult) {
	case MATCH_END_BY_DRAW:
		Send(aClient, OPCODE_RESULT, 0, NULL);
		Send(opponentClient, OPCODE_RESULT, 0, NULL);
		updateMatchLog(aRoom, aClient, opponentClient, MATCH_END_BY_DRAW, "");
		break;
	case MATCH_END_BY_WIN:
		winnerUsername = aClient->username;
		Send(aClient, OPCODE_RESULT, (unsigned short)strlen(winnerUsername), winnerUsername);
		Send(opponentClient, OPCODE_RESULT, (unsigned short)strlen(winnerUsername), winnerUsername);
		updateMatchPlayers(aClient, opponentClient);
		updateMatchLog(aRoom, aClient, opponentClient, MATCH_END_BY_WIN, winnerUsername);
		break;
	default:
		break;
	}
	removeRoom(aClient->socket);
}

/*
function handleRecvSurrender: Handle a request that has opcode equals OPCODE_SURRENDER
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
	updateMatchLog(aRoom, aClient, opponentClient, MATCH_END_BY_SURRENDER, string(opponentClient->username));
	removeRoom(aClient->socket);
}

void updateMatchPlayers(CLIENT* winner, CLIENT* loser) {
	updateScoreOfPlayer(winner->username, UPDATE_MATCH_WINNER);
	updateScoreOfPlayer(loser->username, UPDATE_MATCH_LOSER);
	updateUserIsFree(winner->username, UPDATE_USER_NOT_BUSY);
	updateUserIsFree(loser->username, UPDATE_USER_NOT_BUSY);
	updateRank();
}

void updateMatchLog(Room* aRoom, CLIENT* client1, CLIENT* client2, int endReasonType, string winner) {
	std::vector<PlayerMove> movesList = aRoom->getMovesList();
	// Create log string
	string logString = getEndReason(endReasonType, winner) + "\n"
		+ "Match end at " + getCurrentTime() + "\n"
		+ "IP Address: " + string(client1->address) + "\tPlayer 1: " + string(client1->username) + "\n"
		+ "IP Address: " + string(client2->address) + "\tPlayer 2: " + string(client2->username) + "\n\n"
		+ "Move Log\n";

	size_t movesCount = movesList.size();
	for (int i = 0; i < movesCount; i++) {
		string move = "{x: " + to_string(movesList[i].x)
			+ ", y: " + to_string(movesList[i].y)
			+ ", type: " + to_string(movesList[i].type) + "}\n";
		logString.append(move);
	}

	// Create temp file
	char filename[BUFF_SIZE];
	createTempFileName(client1->username, client2->username, filename);
	FILE* logFile = fopen(filename, "w+");
	if (logFile == NULL) {
		Send(client1, OPCODE_FILE_ERROR, 0, NULL);
		Send(client2, OPCODE_FILE_ERROR, 0, NULL);
		return;
	}
	// Write log string to temp file
	fwrite(logString.c_str(), sizeof(char), logString.size(), logFile);
	fclose(logFile);

	// Prepare file to send to players
	prepareClientToSendFile(client1, filename, (int)logString.size());
	prepareClientToSendFile(client2, filename, (int)logString.size());
};
/*


*/
string getCurrentTime() {
	auto end = std::chrono::system_clock::now();
	std::time_t end_time = std::chrono::system_clock::to_time_t(end);
	return string(std::ctime(&end_time));
}
/*


*/
string getEndReason(int endReasonType, string winner) {
	string reason = "";
	switch (endReasonType)
	{
	case MATCH_END_BY_DRAW:
		reason = "Match end by draw. No winner.";
		break;
	case MATCH_END_BY_WIN:
		reason = "Match end by win. Winner: " + winner + ".";
		break;
	case MATCH_END_BY_SURRENDER:
		reason = "Match end by surrender. Winner: " + winner + ".";
		break;
	default:
		break;
	}
	return reason;
}
/*


*/
void prepareClientToSendFile(CLIENT* aClient, char* filename, int size) {
	aClient->fPointer = fopen(filename, "r");
	aClient->bytesInFile = size;
}

/*
function handleRecvList: Handle a request that has opcode equals OPCODE_FILE reply
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
	ret = Send(aClient, OPCODE_FILE_DATA, (unsigned short) bytesCanFitInBuff, buff);
	if (ret <= 0) return ret;
	aClient->bytesRead += ret - OPCODE_SIZE - LENGTH_SIZE;
	if (aClient->bytesInFile == aClient->bytesRead) {
		fclose(aClient->fPointer);
		Send(aClient, OPCODE_FILE_DATA, 0, NULL);
	}
	return ret;
}
#endif
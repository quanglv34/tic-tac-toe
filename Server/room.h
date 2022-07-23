#pragma once
#define _CRT_SECURE_NO_WARNINGS
#include <iostream>
#include <vector>
#include <iostream>
#include <WinSock2.h>
#include <WS2tcpip.h>
#include "constants.h"
#pragma comment (lib,"ws2_32.lib")

#define BOARD_HEIGHT 16
#define BOARD_WIDTH 16
#define BOARD_WIN_SCORE 3

#define TYPE_O 1
#define TYPE_X 2

struct PlayerMove {
	int x;
	int y;
	int type;
};

class Room {
private:
	SOCKET firstPlayerSocket;
	SOCKET secondPlayerSocket;
	std::vector<PlayerMove> movesList;
	int board[BOARD_HEIGHT][BOARD_WIDTH];
public:
	Room(SOCKET firstClient, SOCKET secondClient);
	bool isPlayerTurn(SOCKET socket);
	bool isBoardCellEmpty(int x, int y);
	bool isMoveInBoard(int x, int y);
	bool isMatchEndByDraw();
	bool isMatchEndByWin();
	bool isPlayerInRoom(SOCKET socket);
	int addPlayerMove(PlayerMove aMove);
	SOCKET getPlayerOpponent(SOCKET socket);
	int getMatchResult();
	int getPlayerMoveType(SOCKET socket);
	std::vector<PlayerMove> getMovesList();
};
/*
@Function:

@Params:

@Return:

*/
Room::Room(SOCKET firstPlayerSocket, SOCKET secondPlayerSocket) {
	this->firstPlayerSocket = firstPlayerSocket;
	this->secondPlayerSocket = secondPlayerSocket;
	this->movesList.clear();
	for (int i = 0; i < BOARD_HEIGHT; i++) {
		for (int j = 0; j < BOARD_WIDTH; j++) {
			this->board[i][j] = 0;
		}
	}
}
/*
@Function:

@Params:

@Return:

*/
std::vector<PlayerMove> Room::getMovesList() {
	return this->movesList;
}
/*
@Function:

@Params:

@Return:

*/
bool Room::isPlayerInRoom(SOCKET socket) {
	if (this->firstPlayerSocket == socket || this->secondPlayerSocket == socket)
		return true;
	return false;
}
/*
@Function:

@Params:

@Return:

*/
bool Room::isPlayerTurn(SOCKET socket) {
	int nextMoveType;
	if (this->movesList.size() == 0) {
		nextMoveType = TYPE_O;
	}
	else {
		nextMoveType = this->movesList.back().type == TYPE_O ? TYPE_X : TYPE_O;
	}

	if (socket == this->firstPlayerSocket) {
		return nextMoveType == TYPE_O;
	}
	else {
		return nextMoveType == TYPE_X;
	}
}
/*
@Function:

@Params:

@Return:

*/
bool Room::isMoveInBoard(int x, int y) {
	return x < BOARD_WIDTH && x >= 0 && y < BOARD_HEIGHT && y >= 0;
}
/*
@Function:

@Params:

@Return:

*/
bool Room::isBoardCellEmpty(int x, int y) {
	return this->board[y][x] == 0;
}
/*
@Function:

@Params:

@Return:

*/
bool Room::isMatchEndByWin() {
	int dx[] = { 0, 1, 1, 1 };
	int dy[] = { 1, 0, 1, -1 };
	int startX, startY, currentX, currentY;

	PlayerMove curMove = this->movesList.back();
	int type = curMove.type;
	int x = curMove.x;
	int y = curMove.y;

	// Print current board
	for (int i = 0; i < BOARD_HEIGHT; i++) {
		for (int j = 0; j < BOARD_WIDTH; j++) {
			std::cout << this->board[i][j] << " ";
		}
		std::cout << std::endl;
	}

	// Check row, column, diagonal and back diagonal for winning line
	int score;
	for (int lineType = 0; lineType < 4; lineType++) {
		score = 0;
		startX = x - (BOARD_WIN_SCORE - 1) * dx[lineType];
		startY = y - (BOARD_WIN_SCORE - 1) * dy[lineType];
		for (int i = 0; i < 2 * BOARD_WIN_SCORE - 1; i++) {
			currentX = startX + i * dx[lineType];
			currentY = startY + i * dy[lineType];
			if (!Room::isMoveInBoard(currentX, currentY)) continue;
			if (this->board[currentY][currentX] != type) score = 0;
			else {
				score++;
				if (score == BOARD_WIN_SCORE) return true;
			}
		}
	}
	return false;
}
/*
@Function:

@Params:

@Return:

*/
bool Room::isMatchEndByDraw() {
	return this->movesList.size() >= BOARD_WIDTH * BOARD_HEIGHT;
}
/*
@Function:

@Params:

@Return:

*/
int Room::getMatchResult() {
	if (this->isMatchEndByWin()) return MATCH_END_BY_WIN;
	if (this->isMatchEndByDraw()) return MATCH_END_BY_DRAW;
	return MATCH_CONTINUE;
}
/*
@Function:

@Params:

@Return:

*/
int Room::addPlayerMove(PlayerMove aMove) {
	if (!Room::isMoveInBoard(aMove.x, aMove.y) || !Room::isBoardCellEmpty(aMove.x, aMove.y)) {
		return OPCODE_PLAY_INVALID_CORDINATE;
	}

	this->board[aMove.y][aMove.x] = aMove.type;
	this->movesList.push_back(aMove);
	return OPCODE_PLAY;
}
/*
@Function:

@Params:

@Return:

*/
int Room::getPlayerMoveType(SOCKET socket) {
	if (socket == this->firstPlayerSocket) {
		return TYPE_O;
	}
	else {
		return TYPE_X;
	}
}
/*
@Function:

@Params:

@Return:

*/
SOCKET Room::getPlayerOpponent(SOCKET socket) {
	if (socket == this->firstPlayerSocket) {
		return this->secondPlayerSocket;
	}
	else {
		return this->firstPlayerSocket;
	}
}


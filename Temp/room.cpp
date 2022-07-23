#include "Room.h"
#include <iostream>


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

std::vector<PlayerMove> Room::getMovesList() {
	return this->movesList;
}

bool Room::isPlayerInRoom(SOCKET socket) {
	if (this->firstPlayerSocket == socket || this->secondPlayerSocket == socket)
		return true;
	return false;
}


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

bool Room::isMoveInBoard(int x, int y) {
	return x < BOARD_WIDTH && x >= 0 && y < BOARD_HEIGHT && y >= 0;
}

bool Room::isBoardCellEmpty(int x, int y) {
	return this->board[y][x] == 0;
}

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
		for (int i = 0; i < BOARD_WIN_SCORE; i++) {
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

bool Room::isMatchEndByDraw() {
	return this->movesList.size() >= BOARD_WIDTH * BOARD_HEIGHT;
}

int Room::getMatchResult() {
	if (this->isMatchEndByWin()) return MATCH_END_BY_WIN;
	if (this->isMatchEndByDraw()) return MATCH_END_BY_DRAW;
	return MATCH_CONTINUE;
}


int Room::addPlayerMove(PlayerMove aMove) {
	if (!Room::isMoveInBoard(aMove.x, aMove.y) || !Room::isBoardCellEmpty(aMove.x, aMove.y)) {
		return OPCODE_PLAY_INVALID_CORDINATE;
	}

	this->board[aMove.y][aMove.x] = aMove.type;
	this->movesList.push_back(aMove);
	return OPCODE_PLAY;
}

int Room::getPlayerMoveType(SOCKET socket) {
	if (socket == this->firstPlayerSocket) {
		return TYPE_O;
	}
	else {
		return TYPE_X;
	}
}

SOCKET Room::getPlayerOpponent(SOCKET socket) {
	if (socket == this->firstPlayerSocket) {
		return this->secondPlayerSocket;
	}
	else {
		return this->firstPlayerSocket;
	}
}

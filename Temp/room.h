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

#define MATCH_CONTINUE 0
#define MATCH_END_BY_WIN 1
#define MATCH_END_BY_DRAW 2

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

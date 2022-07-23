#ifndef _DATABASE_
#define _DATABASE_

#pragma once
#define _CRT_SECURE_NO_WARNINGS
#include <windows.h>
#include <sqlext.h>
#include <sqltypes.h>
#include <sql.h>
#include <iostream>
#include <string>
#include <vector>
#include "constants.h"


#define UPDATE_MATCH_LOSER 0
#define UPDATE_MATCH_WINNER 1

/* isFree */
#define UPDATE_USER_BUSY 0
#define UPDATE_USER_NOT_BUSY 1

/* status */
#define UPDATE_USER_STATUS_OFFLINE 0
#define UPDATE_USER_STATUS_ONLINE 1

using namespace std;
#define SQL_RESULT_LEN 240
#define SQL_RETURN_CODE_LEN 1000
#define SCORE 3

struct userScore {
	char username[30];
	int score;

	userScore(char name[30], int scoreUser) {
		strcpy_s(username, name);
		score = scoreUser;
	}
};

bool connectDB();
void disconnectDB();

string getUserCurrentChallenge(char *username);
string getFreePlayerList(char *);
int getRank(char *username);
int getUserFreeStatus(char *username);
int getScore(char *username);
int updateSignIn(char *username, char *password);
int updateSignUp(char *username, char *password);
void updateUserStatus(char *username, int status);
void updateRank();
void updateUserIsFree(char *username, int isFree);
void updateScoreOfPlayer(char *username, int win);
void updateUserChallenge(char *username, char *usernameChallenge);

void showSQLError(unsigned int handleType, const SQLHANDLE& handle);

#endif
#pragma once
/* START DEFINE CONSTANTS */
/*
Define sever variables
*/
#define SERVER_ADDR "127.0.0.1"
#define SERVER_PORT 5500
#define MAX_CLIENT 1024
#define BUFF_SIZE 2048

/*
Define room variables
*/
#define NO_ROOM -1

/*
Define message variables
*/
#define OPCODE_SIZE 1
#define LENGTH_SIZE 2
#define USERNAME_SIZE 30
#define PASSWORD_SIZE 30
#define PLAYER_MOVE_SIZE 1

/*
Define match variables
*/
#define MATCH_CONTINUE 0
#define MATCH_END_BY_WIN 1
#define MATCH_END_BY_DRAW 2
#define MATCH_END_BY_SURRENDER 3

/* END DEFINE CONSTANTS */

/* START DEFINE OPCODE */
/*
Opcode for sending and receiving file
*/
#define OPCODE_FILE 0
#define OPCODE_FILE_DATA 1
#define OPCODE_FILE_ERROR 2
#define OPCODE_FILE_NO_FILE 3

/*
Opcode for sending and receiving sign up request and reply
*/
#define OPCODE_SIGN_UP 10
#define OPCODE_SIGN_UP_SUCESS 11
#define OPCODE_SIGN_UP_DUPLICATED_USERNAME 12
#define OPCODE_SIGN_UP_INVALID_USERNAME 13
#define OPCODE_SIGN_UP_INVALID_PASSWORD 14
#define OPCODE_SIGN_UP_UNKNOWN_ERROR 19

/*
Opcode for sending and receiving sign up request and reply
*/
#define OPCODE_SIGN_IN 20
#define OPCODE_SIGN_IN_SUCESS 21
#define OPCODE_SIGN_IN_ALREADY_LOGGED_IN 22
#define OPCODE_SIGN_IN_USERNAME_NOT_FOUND 23
#define OPCODE_SIGN_IN_INVALID_USERNAME 24
#define OPCODE_SIGN_IN_INVALID_PASSWORD 25
#define OPCODE_SIGN_IN_WRONG_PASSWORD 26
#define OPCODE_SIGN_IN_UNKNOWN_ERROR 29

/*
Opcode for sending and receiving sign out request and reply
*/
#define OPCODE_SIGN_OUT 30
#define OPCODE_SIGN_OUT_SUCCESS 31
#define OPCODE_SIGN_OUT_NOT_LOGGED_IN 32
#define OPCODE_SIGN_OUT_ERROR_UNKNOWN 39

/*
Opcode for sending and receiving list of online player request and reply
*/
#define OPCODE_LIST 40
#define OPCODE_LIST_REPLY 41

/*
Opcode for sending and receiving challenge player request and reply
*/
#define OPCODE_CHALLENGE 50
#define OPCODE_CHALLENGE_ACCEPT 51
#define OPCODE_CHALLENGE_REFUSE 52
#define OPCODE_CHALLENGE_INVALID_RANK 53
#define OPCODE_CHALLENGE_BUSY 54
#define OPCODE_CHALLENGE_NOT_FOUND 55

/*
Opcode for sending and receiving querying player's info request and reply
*/
#define OPCODE_INFO 60
#define OPCODE_INFO_FOUND 61
#define OPCODE_INFO_NOT_FOUND 62

/*
Opcode for  receiving request and sending reply of players' move in a match 
*/
#define OPCODE_PLAY 70
#define OPCODE_PLAY_OPPONENT 71
#define OPCODE_PLAY_INVALID_CORDINATE 72
#define OPCODE_PLAY_INVALID_TURN 73

/*
Opcode for receiving request and sending reply players surrender while in a match 
*/
#define OPCODE_SURRENDER 80
#define OPCODE_SURRENDER_NO_ROOM 81

/*
Opcode for sending and receiving game result request and reply
*/
#define OPCODE_RESULT 90
/* END OPCODE */


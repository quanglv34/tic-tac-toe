#pragma once
#include <iostream>
#include <WinSock2.h>
#include <WS2tcpip.h>
#include "constants.h"
#pragma comment (lib,"ws2_32.lib")

struct CLIENT {
	SOCKET socket;
	char address[INET_ADDRSTRLEN];
	int port;
	int opcode;
	unsigned short buffLen;
	unsigned short recvBytes;
	unsigned short sentBytes;
	char buff[BUFF_SIZE];

	/* Player Info */
	char username[USERNAME_SIZE];
	bool isLoggedIn = false;
	bool isBusy = false;

	/* Files */
	FILE* fPointer;
	unsigned short bytesInFile;
	unsigned short bytesRead;
};

void initClient(CLIENT* aClient) {
	aClient->address[0] = 0;
	aClient->buff[0] = 0;
	aClient->port = 0;
	aClient->isLoggedIn = false;
	aClient->username[0] = 0;
	aClient->buffLen = 0;
	aClient->recvBytes = 0;
	aClient->sentBytes = 0;
	if (aClient->fPointer != NULL) fclose(aClient->fPointer);
	aClient->bytesInFile = 0;
	aClient->bytesRead = 0;
}

void getPlayerMoveCoordinate(CLIENT* aClient, char* x, char* y) {
	memcpy(x, aClient->buff, PLAYER_MOVE_SIZE);
	memcpy(y, aClient->buff + PLAYER_MOVE_SIZE, PLAYER_MOVE_SIZE);
}

void closeOpenendFile(CLIENT* aClient) {
	if (aClient->fPointer != NULL) fclose(aClient->fPointer);
	aClient->bytesInFile = 0;
	aClient->bytesRead = 0;
}



/*	
function Send:	A recv() wrapper, make sure everything from the message is received.
				Opcode, length and payload will be stored in the aClient struct.
@param aClient: The source client
*/
int Recv(CLIENT* aClient) {
	int ret;
	// recv header
	aClient->recvBytes = OPCODE_SIZE + LENGTH_SIZE;
	aClient->buffLen = 0;
	while (aClient->buffLen < aClient->recvBytes) {
		ret = recv(aClient->socket, aClient->buff + aClient->buffLen, aClient->recvBytes - aClient->buffLen, 0);
		if (ret == SOCKET_ERROR) {
			printf("Error: %d! Cannot receive message.\n", WSAGetLastError());
			return ret;
		}
		else if (ret == 0) {
			printf("Client disconnects.\n");
			return ret;
		}
	    aClient->buffLen += ret;
	}
	// recv payload
	aClient->opcode = aClient->buff[0];
	memcpy(&aClient->recvBytes, aClient->buff + OPCODE_SIZE, LENGTH_SIZE);
	aClient->buffLen = 0;
	if (aClient->recvBytes == 0) {
		return OPCODE_SIZE + LENGTH_SIZE;
	}
	while (aClient->buffLen < aClient->recvBytes) {
		ret = recv(aClient->socket, aClient->buff + aClient->buffLen, aClient->recvBytes - aClient->buffLen, 0);
		if (ret == SOCKET_ERROR) {
			printf("Error: %d! Cannot receive message.\n", WSAGetLastError());
			return ret;
		}
		else if (ret == 0) {
			printf("Client disconnects.\n");
			return ret;
		}
		aClient->buffLen += ret;
	}
	return OPCODE_SIZE + LENGTH_SIZE + aClient->recvBytes;
}

/*	
function Send:	A send() wrapper, make sure everything inside buffer is sent.
@param aClient:	The target client	
@param opcode:	The opcode of the message
@param length:	The length of the message
@param payload:	A char pointer points the the buffer that hold the payload content
*/
int Send(CLIENT* aClient, char opcode, unsigned short length, char* payload) {
	int ret;
	memcpy(aClient->buff, &opcode, OPCODE_SIZE);
	memcpy(aClient->buff + OPCODE_SIZE, &length, LENGTH_SIZE);
	if (length > 0) {
		memcpy(aClient->buff + OPCODE_SIZE + LENGTH_SIZE, payload, length);
	}
	aClient->buffLen = OPCODE_SIZE + LENGTH_SIZE + length;
	aClient->sentBytes = 0;

	while (aClient->sentBytes < aClient->buffLen) {
		ret = send(aClient->socket, aClient->buff + aClient->sentBytes, aClient->buffLen - aClient->sentBytes, 0);
		if (ret == SOCKET_ERROR) {
			printf("Error: %d! Cannot send message.\n", WSAGetLastError());
			return ret;
		}
		aClient->sentBytes += ret;
	}

	return aClient->sentBytes;
}
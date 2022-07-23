#define _WINSOCK_DEPRECATED_NO_WARNINGS
#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <conio.h>
#include<string>
#include <string.h>
#include <iostream>
#include <stdlib.h>
#include <WinSock2.h>
#include <WS2tcpip.h>
#include "process.h"
#define SERVER_PORT 5500
#define SERVER_ADDR "127.0.0.1"
#define BUFF_SIZE 2048
#pragma comment(lib, "Ws2_32.lib")
SOCKET client;
bool shouldStop = false;
unsigned __stdcall listenThread(void *param);
int main(int argc, char* argv[])
{
	//Step 1: Initiate WinSock
	WSADATA wsaData;
	WORD wVersion = MAKEWORD(2, 2);
	if (WSAStartup(wVersion, &wsaData)) {
		printf("Winsock 2.2 is not supportted\n");
		return 0;
	}

	//Step 2: Construct socket

	client = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (client == INVALID_SOCKET) {
		printf("Error %d: Cannot create server socket.", WSAGetLastError());
		return 0;
	}

	//Set time-out for receiving
	//int tv = 200;
	//setsockopt(client, SOL_SOCKET, SO_RCVTIMEO, (const char*)(&tv), sizeof(int));

	//Step 3: Specify server address
	sockaddr_in serverAddr;
	serverAddr.sin_family = AF_INET;
	serverAddr.sin_port = htons(SERVER_PORT);
	inet_pton(AF_INET, SERVER_ADDR, &serverAddr.sin_addr);

	//Step 4: Request to connet server
	if (connect(client, (sockaddr *)& serverAddr, sizeof(serverAddr))) {
		printf("Error %d: Cannot connect server.", WSAGetLastError());
		return 0;
	}

	printf("Connected server!\n");

	//Step 5: Communicate with server
	char buff[BUFF_SIZE];
	char input[BUFF_SIZE];
	int ret;
	unsigned short length = 0;
	char opcode = 0;
	_beginthreadex(0, 0, listenThread, 0, 0, 0); //start thread
	while (1) {
		//Send message
		printf("Opcode: ");
		scanf("%d", &opcode);
		length = 2;
		if (opcode == 0) break;
		if (opcode == 70) {
			char x, y;
			scanf("%d %d", &x, &y);
			printf("Sent %d %d.\n", x, y);
			memcpy(buff, &opcode, 1);
			memcpy(buff + 1, &length, 2);
			memcpy(buff + 3, &x, 1);
			memcpy(buff + 4, &y, 1);

		}
		else {
			getchar();
			printf("Content: ");
			gets_s(input, BUFF_SIZE);
			length = (unsigned short) strlen(input)  + 1;

			memcpy(buff, &opcode, 1);
			memcpy(buff + 1, &length, 2);
			memcpy(buff + 3, &input, length);
		}



		ret = send(client, buff, length + 3, 0);
		printf("Sent %d bytes.\n", length + 3);

		if (ret == SOCKET_ERROR)
			printf("Error %d: Cannot send data.\n", WSAGetLastError());
		ZeroMemory(buff, BUFF_SIZE);
	}
	shouldStop = true;
	//Step 6: Close socket
	closesocket(client);

	//Step 7: Terminate Winsock
	WSACleanup();
	return 0;

}

unsigned __stdcall listenThread(void *param) {
	char buff[BUFF_SIZE];
	char input[BUFF_SIZE];
	int ret;
	unsigned short length = 0;
	char opcode = 0;
	FILE* file = NULL;
	while (1) {
		if (shouldStop) break;
		ret = recv(client, buff, BUFF_SIZE, 0);
		if (ret <= 0) break;
		memcpy(&opcode, buff, 1);
		memcpy(&length, buff + 1, 2);
		memcpy(&input, buff + 3, length);
		if(opcode == 1){
			if (length == 0) {
				fclose(file);
				file = NULL;
				printf("\nReceive file complete.\n");
			}
			else {
				if (file == NULL) {
					char filename[BUFF_SIZE];
					filename[0] = 0;
					std::cout << std::to_string(client) << std::endl;
					strcat(filename, std::to_string(client).c_str());
					strcat(filename, "_temp.txt");
					file = fopen(filename, "w+");
				}
				fwrite(input, sizeof(char), length, file);
			}
		}
		input[length] = 0;
		printf("\nReceive from server\n");
		printf("Opcode: %d\n", opcode);
		printf("Content: [%s]\n\n", input);
		ZeroMemory(buff, BUFF_SIZE);
	}
}
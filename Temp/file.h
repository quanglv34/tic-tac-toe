#pragma once
#include <time.h>
#include <stdio.h>
#include <conio.h>
#include "constants.h"

// Create a unique name for temporary file
void createTempFileName(char* player1, char* player2, char* filename) {
	time_t rawtime;
	struct tm * timeinfo;
	time(&rawtime);
	timeinfo = localtime(&rawtime);
	snprintf(filename, BUFF_SIZE, "temp_%s_%s_%d%02d%02d_%02d%02d%02d.txt",
		player1,
		player2,
		timeinfo->tm_year + 1900, timeinfo->tm_mon, timeinfo->tm_mday,
		timeinfo->tm_hour, timeinfo->tm_min, timeinfo->tm_sec);
}

// Count the bytes left from the current file pointer
int countBytesLeftInFile(FILE* file) {
	if (file == NULL) return 0;
	int bytesInContent, bytesRead;
	bytesRead = ftell(file);
	fseek(file, 0, SEEK_END); // move file cursor to end of file to get file len
	bytesInContent = ftell(file);
	fseek(file, bytesRead, SEEK_SET); // move file cursor back to the previous position
	return bytesInContent - bytesRead;
}
#include<stdio.h>
#include<stdbool.h>
#include<stdlib.h>
#include<string.h>
#include<math.h>

#define TRUE 1
#define FALSE 0

const int disallowedLen = 3;
const char disallowed[disallowedLen] = {'i', 'o', 'l'};

bool checkValidity(const char* password) {
	
	const size_t passLen = strlen(password);
	bool hasContinuous = FALSE;
	bool hasDouble = FALSE;
	char firstDouble = 0;
	for (int i = 0; i < passLen; i++) {
		for (int j = 0; j < disallowedLen; j++) {
			if (password[i] == disallowed[j]) return FALSE;
		}
		if (!hasDouble && i >= 1) {
			if (password[i] == password[i-1]) {
				if (firstDouble == 0) firstDouble = password[i];
				else {
					if (password[i] != firstDouble) hasDouble = TRUE;
				}
			}
		}
		if (!hasContinuous && i >= 2) {
			if (password[i-1] - password[i-2] == 1
				&& password[i] - password[i-1] == 1) {
					hasContinuous = TRUE;
			}
		}
	}

	return hasContinuous & hasDouble;
}

void incrementPassword(char* input, int position) {
	
	int realPos = ((int)strlen(input) - position)-1;
	if (input[realPos] >= 'z') {
		incrementPassword(input, ++position);
		input[realPos] = 'a';
	} else {
		input[realPos]++;
		for (int i = 0; i < disallowedLen; i++) {
			if (input[realPos] == disallowed[i]) input[realPos]++;
		}
	}
}

int main(int argc, char** argv) {
	
	if (argc != 2) {
		printf("Please specify a starting password!\n");
		return -1;
	}
	
	const char* basePass = argv[1];
	const size_t passLen = strlen(basePass);
	if (passLen != 8) return -2;
	
	bool valid = FALSE;
	char newPass[passLen];
	strcpy(newPass, basePass);
	printf("Base password: '%s'\n", basePass);
	
	incrementPassword(newPass, 0);
	
	while (!checkValidity(newPass)) {
		incrementPassword(newPass, 0);
	}
	
	printf("Valid password: '%s'\n", newPass);

	return 0;
}

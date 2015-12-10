#!/usr/bin/env awk
# Run this using 'awk -v rounds=NUMBER_OF_ROUNDS -f ./task-2.awk INPUT_FILE'

{
printf("Running for %d rounds with input '%s'\n", rounds, $0);
for (round = 1; round <= rounds; round++) {
	printf("Starting round %d\n", round);
	if (length(result) > 0) val = result;
	else val = $0;
	result = "";
	lastChar = "";
	count = 0;
	valLength = length(val);
	for (i = 1; i <= valLength; i++) {
		curChar = substr(val, 1, 1);
		val = substr(val, 2)
		if (curChar == lastChar) {
			count++;
		} else {
			if (count > 0) {
				result = sprintf("%s%s%s", result, count, lastChar)
			}
			lastChar = curChar;
			count = 1;
		}
		printf("Complete: %d%%\r", (i/valLength)*100)
	}
	result = sprintf("%s%s%s", result, count, lastChar)
	printf("Length of result for round %d: %d\n", round, length(result))
}
}
END { printf("Length of result: %d", length(result)) }

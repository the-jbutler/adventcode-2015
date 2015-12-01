#!/bin/bash

curFloor=0;
curPos=0;
oldIFS=$IFS;

IFS=

while read -n 1 char
do
	curPos=$((curPos+1))
	if [[ $char = "(" ]]; then
#		echo "add"
		curFloor=$((curFloor+1));
	elif [[ $char = ")" ]]; then
#		echo "sub"
		curFloor=$((curFloor-1));
	fi
	if [[ $curFloor == -1 ]]; then
		echo "Entered floor -1 at: $curPos";
		break;
	fi
#	echo $curFloor
done < "${1:-/dev/stdin}";

IFS=$oldIFS;
echo $curFloor;

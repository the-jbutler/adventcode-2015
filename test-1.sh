#!/bin/bash

curFloor=0;
oldIFS=$IFS;

IFS=

while read -n 1 char
do
	if [[ $char = "(" ]]; then
#		echo "add"
		curFloor=$((curFloor+1));
	elif [[ $char = ")" ]]; then
#		echo "sub"
		curFloor=$((curFloor-1));
	fi
#	echo $curFloor
done < "${1:-/dev/stdin}";

IFS=$oldIFS;
echo $curFloor;

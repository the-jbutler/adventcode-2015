#!/usr/local/opt/coreutils/libexec/gnubin/env python

import sys
import heapq

def calcRibbonArea(dimen):
	if len(dimen) is not 3:
		return None
	
	a = min(dimen)
	b = max(dimen)
	for d in dimen:
		if ((d >= a and dimen.count(d) > 1) or d > a) and d < b:
			b = d
	length = (2 * a) + (2 * b)
	bow = (dimen[0] * dimen[1] * dimen[2])
	return length + bow 

def splitDimens(dimens):
	strDimens = dimens.split('x')
	return [int(i) for i in strDimens]

def getTotalArea(file):
	lines = file.readlines()
	areas = []
	for line in lines:
		line = line.rstrip()
		dimens = splitDimens(line)
		areas.append(calcRibbonArea(dimens))
	print areas
	return sum(areas)

try:
	fileName = sys.argv[1]
except:
	fileName = None

if fileName:
	inFile = open(fileName, 'r')
	print "Reading file " + inFile.name
else:
	inFile = sys.stdin
	print "Reading STDIN"

print getTotalArea(inFile)

if inFile is not sys.stdin:
	inFile.close()

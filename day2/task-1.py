#!/usr/bin/env python

import sys

def calcSurfaceArea(dimen):
	if len(dimen) is not 3:
		return 0

	sides = []
	sides.append(dimen[0]*dimen[1])
	sides.append(dimen[1]*dimen[2])
	sides.append(dimen[2]*dimen[0])
	slack=min(sides)
	return (2 * sum(sides)) + slack

def splitDimens(dimens):
	strDimens = dimens.split('x')
	return [int(i) for i in strDimens]

def getTotalArea(file):
	lines = file.readlines()
	areas = []
	for line in lines:
		line = line.rstrip()
		dimens = splitDimens(line)
		areas.append(calcSurfaceArea(dimens))
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

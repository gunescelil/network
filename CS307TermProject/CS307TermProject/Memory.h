/*
* Modified on: Dec 07, 2016
*	   Author: mbenlioglu
*
*	- Changes according to step-1 requirements
*/

#ifndef MEMORY_H
#define MEMORY_H

#include <iostream>
#include <string>

using namespace std;

class Memory {

private:
	char* memory;
	int memorySize;

	//HACK: temporary solution for step-1. This should be replaced with next-fit using linked lists.
	int emptyIndex; //simply track the end of last entry....

public:
	Memory(int size);
	~Memory();
	void addInstructions(char* buffer, unsigned int bufferSize, int BR);
	void removeInstructions(int bufferSize, int BR);
	char* getInstruction(int PC, int BR);
	int hasFreeSpace(int size);

	int getEmptyIndex();

};

#endif
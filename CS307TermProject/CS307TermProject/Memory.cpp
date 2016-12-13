#ifndef MEMORY_CPP
#define MEMORY_CPP
#include "Memory.h"

Memory::Memory(int size) {
	memorySize = size;
	memory = new char[size];

	emptyIndex = 0;
}

Memory::~Memory() {

	delete[] memory;
}

void Memory::addInstructions(char * buffer, unsigned int bufferSize, int BR)
{
	for (size_t i = BR; i < bufferSize + BR; i++)
	{
		this->memory[i] = buffer[i - BR];
	}

	emptyIndex += bufferSize;
}

char* Memory::getInstruction(int PC, int BR) {
	char * instruction = new char[4];
	instruction[0] = (unsigned int)memory[PC + BR];
	instruction[1] = (unsigned int)memory[PC + BR + 1];
	instruction[2] = (unsigned int)memory[PC + BR + 2];
	instruction[3] = (unsigned int)memory[PC + BR + 3];

	return instruction;
}

int Memory::getEmptyIndex()
{
	return this->emptyIndex;
}

#endif
/*
* Created on: Dec 07, 2016
*	  Author: mbenlioglu
*/
#pragma once

#include<string>
#include<queue>
#include<vector>
#include<thread>
#include<mutex>
#include "CPU.h"
#include "Memory.h"
#include "ProcessImage.h"
#include "Assembler.h"

using namespace std;

class OS
{
public:
	OS(int size);
	~OS();

	void LoadProcess(string processFile);
	void LoadProcess(vector<string>& processFiles);
	void Start();

protected:
	void enqueue(queue<ProcessImage>& queue, ProcessImage process);
	ProcessImage dequeue(queue<ProcessImage>& queue);

private:
	const unsigned int QUANTUM;
	CPU* cpu;
	Memory* memory;
	queue<ProcessImage> readyQ;
	queue<ProcessImage> blockedQ;
	mutex mtx;
	thread QManager;

	//update ready and blocked queues
	void AsyncConsumeInput(); //QManager runs this function....
	void BlockProcess(); //from ready Q to blocked Q
};


#ifndef MAIN_CPP
#define MAIN_CPP

#include<iostream>
#include<thread>
#include<mutex>
#include<string>
#include<fstream>
#include<sstream>
#include<queue>
#include<vector>
#include"OS.h"


#include <string>
#include <iostream>
#include <fstream>


using namespace std;

int main()
{
	static const int MEMORY_SIZE = 50000;
	OS os(MEMORY_SIZE);

	vector<string> FileNames;
	FileNames.push_back("Processes/process1.asm");
	FileNames.push_back("Processes/process2.asm");
	FileNames.push_back("Processes/process3.asm");
	FileNames.push_back("Processes/process4.asm");
	os.LoadProcess(FileNames);

	os.Start();

	return 0;
}


#endif


/*
* Created on: Dec 07, 2016
*	  Author: mbenlioglu
*/
#include "OS.h"


OS::OS(int size = 50000)
	: QUANTUM(5)
{
	cout << "Initilizing the OS....";
	this->memory = new Memory(size);
	this->cpu = new CPU(memory);

	this->readyQ = queue<ProcessImage>();
	this->blockedQ = queue<ProcessImage>();
	cout << "DONE!\n";
}

OS::~OS()
{
	cout << "****************************************************************\n"
		<< "Exiting the OS! GOODBYE!\n"
		<< "****************************************************************\n";
	system("pause");
}

void OS::LoadProcess(string processFile)
{
	cout << "Creating binary file for " << processFile << " ...";
	unsigned int instructionSize = createBinaryFile(processFile);
	char* process = readBinaryFile(instructionSize, processFile.substr(0, processFile.rfind('.')) + ".bin");

	cout << "Loading process to memory...";
	mtx.lock();
	enqueue(readyQ, ProcessImage(processFile.substr(0, processFile.rfind('.')), memory->getEmptyIndex(), instructionSize));
	mtx.unlock();

	this->memory->addInstructions(process, instructionSize, memory->getEmptyIndex());
	cout << "DONE!\n";
}

void OS::LoadProcess(vector<string>& processFiles)
{
	for (auto& s : processFiles)
		LoadProcess(s);
}

void OS::Start()
{
	cout << "****************************************************************\n"
		<< "Starting the OS with " << readyQ.size() << " processes...\n";

	QManager = thread(&OS::AsyncConsumeInput, this);

	mtx.lock();
	while (!readyQ.empty() || !blockedQ.empty())
	{
		mtx.unlock();
		if (!readyQ.empty())
		{
			cout << "Executing " << readyQ.front().processName << "...\n";
			cpu->transferFromImage(readyQ.front());
			for (size_t i = 0; i < QUANTUM; i++)
			{
				if (cpu->getLR() != cpu->getPC()) //check if process is finished
				{

					cpu->fetch();
					int returnCode = cpu->decodeExecute();

					if (returnCode == 0) //if process makes a system call
					{
						cout << "Process " << readyQ.front().processName << " made a system call for ";
						if (cpu->getV() == 0) //syscall for input
						{
							cout << "input, transfering to blocked queue and waiting for input...\n";
							BlockProcess();
						}
						else //syscall for output
						{
							cout << "output. Output Value:\n";
							ProcessImage p;
							cpu->transferToImage(p);
							mtx.lock();
							dequeue(readyQ);
							cout << p.V << endl;
							cout.flush();
							enqueue(readyQ, p);
							mtx.unlock();
						}
						//process blocked, need to end quantum prematurely
						break;
					}
				}
				else //process finished
				{
					cout << "Process " << readyQ.front().processName
						<< " have been finished! Removing from the queue...\n";
					ProcessImage p;
					cpu->transferToImage(p);

					cout << "Final " << p;

					mtx.lock();
					dequeue(readyQ);
					mtx.unlock();
					break;
				}

				if (i == QUANTUM - 1)
				{
					//quantum finished put the process at the end of readyQ
					cout << "Context Switch! Allocated quantum have been reached, switching to next process...\n";
					ProcessImage p;
					cpu->transferToImage(p);
					mtx.lock();
					dequeue(readyQ);
					enqueue(readyQ, p);
					mtx.unlock();
				}
			}
		}
		//lock to check the Q's in while condition
		mtx.lock();
	}
	mtx.unlock();

	QManager.join();

	cout << "Execution of all processes have finished!\n";
}

void OS::enqueue(queue<ProcessImage>& queue, ProcessImage process)
{
	queue.push(process);
}

ProcessImage OS::dequeue(queue<ProcessImage>& queue)
{
	ProcessImage p;

	if (!queue.empty())
	{
		p = queue.front();
		queue.pop();
	}

	return p;
}

void OS::AsyncConsumeInput()
{
	mtx.lock();
	while (!blockedQ.empty() || !readyQ.empty())
	{
		if (!blockedQ.empty())
		{
			mtx.unlock();

			int input;
			cin >> input;

			if (!this->blockedQ.empty()) //input is ignored if no process is blocking for input
			{
				mtx.lock();
				ProcessImage p = dequeue(blockedQ);

				//set input to V register
				p.V = input;

				enqueue(readyQ, p);
				mtx.unlock();
			}
		}
		else
			mtx.unlock();

		//lock to check Q's in while condition
		mtx.lock();
	}
	mtx.unlock();
}

void OS::BlockProcess()
{
	ProcessImage p;
	this->cpu->transferToImage(p);
	mtx.lock();
	dequeue(readyQ);
	enqueue(blockedQ, p);
	mtx.unlock();
}

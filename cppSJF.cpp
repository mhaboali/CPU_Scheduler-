/*
 * cppSJF.cpp
 *
 *  Created on: Apr 12, 2019
 *      Author: HP
 */
#include<iostream>
#include<vector>
#include<algorithm>
#include<queue>

using namespace std;

bool isLeast(vector<int> data, int size, int index) {
	int i, minimum;
	minimum = data[0];
	for (i = 1;i < size;i++) {
		if (data[i] < minimum)
			minimum = data[i];
	}
	return (data[index] == minimum);
}

bool isOver(vector<int> remaining, int size){
	int i,sum=0;
	for (i = 0;i < size;i++) {
		sum += remaining[i];
	}
	return (sum == 0);
}

int main() {
	int numberProcesses,currentTime,lastArrival,id,i;
	queue<int>  start;						  //each time for each process when it proceeds
	queue<int>  end;						  //the end for each start
	queue<int>  index;					      //to map each start and end for a certain process
	cout << "number"<<endl;
	cin >> numberProcesses;
	vector<int> remaining(numberProcesses,0);   //remaining time for each process initially = burst time
	vector<int> waiting(numberProcesses,0);	  //waiting for each process initially = 0
	vector<int> arrival(numberProcesses,0);	  //should be sorted from 1st to last one in arrival
	vector<int> inProgress(numberProcesses,0);//0 -> not in progress, 1 -> in progress; initially = 0
	vector<int> arrived(numberProcesses,1000);  //to store the remaining of the arrived processes only

	cout << "burst" << endl;
	//burst for each process
	for (i = 0;i < numberProcesses;i++) {
		cin >> remaining[i];
	}
	cout << "arrival" << endl;
	//arrival for each process
	for (i = 0;i < numberProcesses;i++) {
		cin >>arrival[i];
	}

	sort(arrival.begin(),arrival.end());
	lastArrival = arrival[numberProcesses - 1];

	for (currentTime = 0;!(isOver(remaining,numberProcesses));currentTime++) {

		if(currentTime<=lastArrival){ //add the arrived process only to the vector
			for (id = 0;id < numberProcesses;id++) {
				if (currentTime == arrival[id])
					arrived[id] = remaining[id];
			}
		}
		for (i=0;i<numberProcesses;i++) {
			if(arrival[i]<=currentTime) {
				if (isLeast(arrived, numberProcesses, i)) { //compare among the arrived processes only
					remaining[i]--;
					if (inProgress[i] == 0) {
						start.push(currentTime);
						index.push(i);
					}
					inProgress[i] = 1; //so that it can't be pushed more than one time
				}// 2nd if
				else {// of 2nd if
					waiting[i]++;
					if (inProgress[i] == 1)
						end.push(currentTime);
				}

			}// 1st if

			else {// of 1st if
				break; // break from 2nd for
			}


		}// 2nd for

	}// 1st for



	for (i = 0;i < numberProcesses;i++) {
			cout << " " << start.front();
			start.pop();
		}

		cout << endl;

		for (i = 0;i < numberProcesses;i++) {
			cout << " " << end.front();
			end.pop();
		}

		cout << endl;

		for (i = 0;i < numberProcesses;i++) {
			cout << " " << index.front();
			index.pop();
		}

}




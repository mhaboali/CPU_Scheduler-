/*
 * cppNPSJF.cpp
 *
 *  Created on: Apr 13, 2019
 *      Author: HP
 */
#include<iostream>
#include<vector>
#include<queue>
#include<algorithm>

using namespace std;

int main(){

	int numberProcesses,i,totalWaiting=0,avgWaiting;
	queue <int> start,end;
	vector <int> burst,waiting;

	cout<<"number"<<endl;
	cin>>numberProcesses;

	cout<<"burst"<<endl;
	for(i=0;i<numberProcesses;i++){
		cin>>burst[i];
	}

	sort(burst.begin(),burst.end());

	waiting[0]=0;
	start.push(0);
	end.push(0+burst[0]);

	for(i=1;i<numberProcesses;i++){
		waiting[i]=waiting[i-1]+burst[i-1];
		start.push(waiting[i]);
		end.push(waiting[i]+burst[i]);
	}

	for(i=0;i<numberProcesses;i++){
		totalWaiting+=waiting[i];
	}

	avgWaiting=totalWaiting/numberProcesses;

	for(i=0;i<numberProcesses;i++){
		cout<<start.front()<<" ";
		start.pop();
	}

	cout<<endl;

	for(i=0;i<numberProcesses;i++){
			cout<<end.front()<<" ";
			end.pop();
		}







}




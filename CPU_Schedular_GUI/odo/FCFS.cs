using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace odo
{
    class FCFS :process
    {

        public int number, avr_time, waiting_time=0;//processNo
       public int[] arrival;
     public List<int> dur;
      List<int> d=new List<int>();
       public int[] waiting;
       public string[] flag ;
      // public process[] pp = new process[10];
        public process[] pp ;
        
       int j = 0;

       public FCFS(int num, List<int> process_dur, int[] process_arriv)
        {
            number = num;                   //number of process
            dur = process_dur;             //arrray with durations (bursts)
            arrival = process_arriv;      //arrray with arrivals time of process
        }


        public void sets()
        {
            int e = 0,z=0,w=0;
            flag = new string[number];    // sorted processes
           waiting = new int[number];    // sorted processes
            pp = new process[number];
            while (j < number)
            {
                pp[j] = new process();
                pp[j].setArrival_time(arrival[j]);
                pp[j].setDuration(dur[j]);
                d.Add(dur[j]);
                j++;
            }
            Array.Sort(arrival);
            
            for (int k = 0; (k < number) && (z!=number);)
            {
                if (pp[k].getArrival_time() == arrival[e])
                {
                    flag[e] = "p" + k;
                    dur[e] = d[k];
                    if (e==0)
                    w += (dur[k ]+pp[k].getArrival_time());
                    else  w += dur[e];
                    waiting[e] = w-pp[k].getArrival_time() -  pp[k].getDuration();
                    e++;
                    k = 0;
                    z++;
                }
                else k++;
            }
            for (int k = 0; k < number; k++)
                waiting_time += waiting[k];
            avr_time = waiting_time / number;
        }

    }



}

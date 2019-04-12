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
    class RR : process
    {

        public int number, waiting_time, avr_time;//processNo
        public int[] arrival;
        public int[] waiting;
        public List<int> Dur;
        public List<int> d = new List<int>();
        public List<int> plot = new List<int>();
        public List<string> flag = new List<string>();

        public List<int> process_no = new List<int>();
        public process[] pp;
        public int j = 0, i = 0, q, z = 0, w = 0;

        public RR(int num, int r, List<int> process_dur, int[] process_arriv)
        {
            number = num;
            q = r;
            Dur = process_dur;
            arrival = process_arriv;
        }


        public void sets()
        {
            pp = new process[number];
            while (i < number)
            {
                pp[i] = new process();
                pp[i].setArrival_time(arrival[i]);
                pp[i].setDuration(Dur[i]);
                d.Add(Dur[i]);
                i++;
            }
            Array.Sort(arrival);
            int e = 0;
            for (int k = 0; (k < number) && (z != number); )
            {
                if (pp[k].getArrival_time() == arrival[e])
                {
                    Dur[e] = d[k];
                    process_no.Add(k);
                    k = 0;
                    e++;
                    z++;
                }
                else k++;
            }
            int t = 0;
            waiting = new int[number];    // sorted processes
            int time = arrival[0];
            int x = Dur.Count;
            w = arrival[0];
            while (x != 0)
            {
                if (q < Dur[j])
                {
                    plot.Add(q);
                    time += q;
                    flag.Add("p" + process_no[j]);
                    if (j < number)
                    {
                        if (arrival[j + 1] <= time)
                        {
                            Dur.Add(Dur[j] - q); process_no.Add(process_no[j]); j++;
                        }
                        else
                        {
                            Dur[j] = Dur[j] - q;
                        }
                    }
                    else
                    {
                        Dur.Add(Dur[j] - q);
                        process_no.Add(process_no[j]);
                        j++;
                    }
                    w += q;
                }
                else
                {
                    plot.Add(Dur[j]);
                    flag.Add("p" + process_no[j]);
                    time += Dur[j];
                    w += Dur[j];
                    waiting[t] = w - pp[process_no[j]].getDuration() - pp[process_no[j]].getArrival_time();
                    x--;
                    t++;
                    j++;
                }

            }
            for (int k = 0; k < number; k++)
                waiting_time += waiting[k];
            avr_time = waiting_time / number;
        }
    }
}






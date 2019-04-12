using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace odo
{
    public class process 
    {

       public int   duration, id;
       public  int arrival_time;
     
      

        public process()
        {
            id = 0;
            duration=0;
            arrival_time = 0;

         }
        public process(int a,int b,int c)
        {
            id = a;
            duration=(b);
            arrival_time = c;
        }
        public void setId(int Process_id)
        {
            id = Process_id;
        }
        public   void setDuration(int Process_duration)
        {
            duration= Process_duration;
        }
        public  void setArrival_time(int Process_arrv)
        {
            arrival_time = Process_arrv;
        }
        public int getId()
        {
            return id;
        }
        public int getDuration()
        {
            return duration;
        }
        public int getArrival_time()
        {
           return arrival_time;
        }
        public void draw(int d)
        {
          d= duration;
        }
    }


}

 

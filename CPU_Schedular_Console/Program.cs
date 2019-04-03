using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Schedular_Console
{
    class Program
    {
        static bool jobs_done(int[] pids)
        {
            for (int p = 0; p < pids.Length; p++)
            {
                if (pids[p] != -1)
                    return false;
            }
            return true;
        }
         static void Preemptive(string[] args)
        {
            Console.WriteLine("Please Enter Number of Processes: ");
            int numProcesses    = int.Parse(Console.ReadLine());
            int[] pids          = new int[numProcesses];         // It will hold the pids in a sorted way according to priority values 
            var finishedPids    = new Queue<int>(numProcesses);
            int[] arrival       = new int[numProcesses];
            int[] burst         = new int[numProcesses];
            int[] priority      = new int[numProcesses];
            var starting        = new Dictionary<int, Queue<int>>();
             //System.Collections.Generic.Dictionary<int, Queue<int>> starting;
            var finishing     = new Dictionary<int, Queue<int>>();
            int[] waiting       = new int[numProcesses];
            int[] turnAround    = new int[numProcesses];
            
            Console.WriteLine("Please Enter pid, arrivalTime, Burst, PriorityValue");
            for (int p = 0; p < numProcesses; p++)
            {
                pids[p]     = int.Parse(Console.ReadLine());
                arrival[p]  = int.Parse(Console.ReadLine());
                burst[p]    = int.Parse(Console.ReadLine());
                priority[p] = int.Parse(Console.ReadLine());
                Console.WriteLine("\n");
                waiting[p]  = 0;
                starting.Add(p, new Queue<int>());
                finishing.Add(p, new Queue<int>());
            }
            // Sort the pids array according to the priority values
            int[] tempPriority = new int[numProcesses];
            priority.CopyTo(tempPriority, 0);
            for (int p = 0; p < numProcesses; p++)
            {
                int minVal = tempPriority.Min();
                int indMin = tempPriority.ToList().IndexOf(minVal);
                tempPriority[indMin] = 99999999;
                int indexMin = priority.ToList().IndexOf(minVal);
                pids[p] = indexMin; 
            }
            // Here we have pids sorted by priority values
            int prevPid = -1;      // just initial value to avoid compiler error
            int[] burstCopy = new int[numProcesses];
            burst.CopyTo(burstCopy, 0);
            // Start simulating the real time operations 
            for (int currentTime = 0; !jobs_done(pids); currentTime++)
            {
                for (int p = 0; p < pids.Length; p++)
                {
                // loop over all processes to get the highest priority arrived job to be executed
                    int p_indx = pids[p];       // the pid of the next process
                    if (currentTime == 0)
                        prevPid = p_indx;
                    if (p_indx != -1 && arrival[p_indx] <= currentTime)
                    {
                        if (p_indx != prevPid)
                        {
                        // Detect 2 consecutive different processes
                            if (starting[p_indx].Count != 0)
                            {
                                waiting[p_indx] += (currentTime - starting[p_indx].Last());
                                starting[p_indx].Enqueue(currentTime);
                            }
                            else
                            {
                                starting[p_indx].Enqueue(currentTime);      // first starting of the job
                                waiting[p_indx] += (currentTime - arrival[p_indx]);
                            }
                            finishing[prevPid].Enqueue(currentTime);
                        } 
                        else if(currentTime == 0)
                            starting[p_indx].Enqueue(currentTime);      // starting of the first job

                        burst[p_indx]       -= 1 ;          // decrease one unit time from the burst
                        if (burst[p_indx] == 0)
                        {
                            finishedPids.Enqueue(p_indx);
                            if (finishedPids.Count == numProcesses)
                                finishing[p_indx].Enqueue(currentTime+1);             // last finishing time of the last job
                            turnAround[p_indx] = waiting[p_indx] + burstCopy[p_indx];
                            pids[p] = -1;  // it has just been executed
                        }
                        prevPid = p_indx;
                        break;
                    }
                }
            }
            for (int i = 0; i < pids.Length; i++)
            {
                int p = finishedPids.Dequeue();
                Console.WriteLine("The Values are {0}     {1}     {2}     {3}       {4}\n", p, starting[p].First(), waiting[p], finishing[p].Last(), turnAround[p]);
            }
                Console.ReadKey();
        }
        static void Non_Preemptive(string[] args)
        {
            Console.WriteLine("Please Enter Number of Processes: ");
            int numProcesses    = int.Parse(Console.ReadLine());
            // Declarations of our data structures
            int[] pids          = new int[numProcesses];         // It will hold the pids in a sorted way according to priority values 
            var finishedPids    = new Queue<int>(numProcesses);
            int[] arrival       = new int[numProcesses];
            int[] burst         = new int[numProcesses];
            int[] priority      = new int[numProcesses];
            int[] starting      = new int[numProcesses];
            int[] finishing     = new int[numProcesses];
            int[] waiting       = new int[numProcesses];
            int[] turnAround    = new int[numProcesses];
            
            // Get the input values from the user
            Console.WriteLine("Please Enter pid, arrivalTime, Burst, PriorityValue");
            for (int p = 0; p < numProcesses; p++)
            {
                pids[p]     = int.Parse(Console.ReadLine());
                arrival[p]  = int.Parse(Console.ReadLine());
                burst[p]    = int.Parse(Console.ReadLine());
                priority[p] = int.Parse(Console.ReadLine());
                Console.WriteLine("\n");

            }
            // Sort the pids array according to the priority values
            int[] tempPriority = new int[numProcesses];
            priority.CopyTo(tempPriority, 0);
            for (int p = 0; p < numProcesses; p++)
            {
                int minVal = tempPriority.Min();
                int indMin = tempPriority.ToList().IndexOf(minVal);
                tempPriority[indMin] = 99999999;
                int indexMin = priority.ToList().IndexOf(minVal);
                pids[p] = indexMin; 
            }
            // Here we have pids sorted by priority values
            
            // Start simulating the real time operations 
            for (int currentTime = 0; !jobs_done(pids); currentTime++)
            {
                for (int p = 0; p < pids.Length; p++)
                {
                // loop over all processes to get the highest priority arrived job to be executed
                    int p_indx = pids[p];       // the pid of the next process
                    if (p_indx != -1 && arrival[p_indx] <= currentTime)
                    {
                        starting[p_indx]    = currentTime;
                        waiting[p_indx]     = currentTime - arrival[p_indx];
                        currentTime         += burst[p_indx];
                        finishing[p_indx]   = currentTime; 
                        turnAround[p_indx]  = waiting[p_indx] + burst[p_indx];
                        pids[p] = -1;  // it has just been executed
                        finishedPids.Enqueue(p_indx);
                        break;
                    }
                }
                currentTime--;      // to keep accurate timing between the processes
            }
            for (int i = 0; i < pids.Length; i++)
            {
                int p = finishedPids.Dequeue();
                Console.WriteLine("The Values are {0}     {1}     {2}     {3}       {4}\n", p, starting[p], waiting[p], finishing[p], turnAround[p]);
            }
                Console.ReadKey();
        }
    }
}

/*
0
0
4
2
1
0
3
1
2
1
5
0

*/
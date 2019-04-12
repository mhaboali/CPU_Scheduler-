using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace odo
{
    public partial class Form1 : Form 
    {
       public  int[] arrivals;
       List<int> Durs, priorities;
       int s , j=0, input,Q, numProcesses;
       public string arriv, Durss,priors;

        public Form1()
        {
            InitializeComponent();
        }
       
         private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }
         private void textBox2_TextChanged(object sender, EventArgs e)
         {
             button1.Enabled = true;

         }
         private void textBox3_TextChanged(object sender, EventArgs e)
         {
             button1.Enabled = true;

         }
         private void textBox4_TextChanged(object sender, EventArgs e)
         {
             button1.Enabled = true;

         }
        public void button1_Click(object sender, EventArgs e)
         {
            //Enter button
            // input => number of processes
              input = int.Parse(textBox2.Text);
              arriv = textBox1.Text;
              Durss = textBox4.Text;
              priors = textBox3.Text;
              arrivals = arriv.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
              Durs = Durss.Split(',').Select(n => Convert.ToInt32(n)).ToList();
              try
              {
                  priorities = priors.Split(',').Select(n => Convert.ToInt32(n)).ToList();
              }
              catch (Exception ex) { }
              numProcesses = input;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ;
        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private void label1_Click(object sender, EventArgs e)
        {
            Label label1 = new Label();
            label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            label1.Text = "First &Name:";
            label1.Size = new Size(label1.PreferredWidth, label1.PreferredHeight);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Priority_Non_Preemptive();
        }

        private void button4_Click_2(object sender, EventArgs e)
        {
            Priority_Preemptive();
        }

        static bool jobs_done(int[] pids)
        {
            for (int p = 0; p < pids.Length; p++)
            {
                if (pids[p] != -1)
                    return false;
            }
            return true;
        }
        static Tuple<int,int> getNextFinishingTask(int numberProcesses, Dictionary<int, Queue<int>> map)
        {
            int nextPid;
            var temp = new List<int>(numberProcesses);
            for (int p = 0; p < numberProcesses; p++)
            {
                if (map[p].Count != 0)
                    temp.Add(map[p].First());
                else
                    temp.Add(99999999);

            }
            int minVal = (temp.Count != 0) ? temp.Min() : -1;
            if (minVal == -1)
                return Tuple.Create(-1, -1);
            nextPid = temp.ToList().IndexOf(minVal);
            map[nextPid].Dequeue();
            return Tuple.Create(nextPid,minVal);
        }
        static bool displayAll(int numberProcesses, Dictionary<int, Queue<int>> finishing){
            for (int p = 0; p < numberProcesses; p++)
            {
                if (finishing[p].Count != 0)
                    return false;
            }
            return true;
        }

        public void Priority_Preemptive()
        {
            int[] pids = new int[numProcesses];         // It will hold the pids in a sorted way according to priority values 
            var finishedPids = new Queue<int>(numProcesses);
            int[] arrival = new int[numProcesses];
            int[] burst = new int[numProcesses];
            int[] priority = new int[numProcesses];
            var starting = new Dictionary<int, Queue<int>>();
            //System.Collections.Generic.Dictionary<int, Queue<int>> starting;
            var finishing = new Dictionary<int, Queue<int>>();
            int[] waiting = new int[numProcesses];
            int[] turnAround = new int[numProcesses];

            arrival = arrivals;
            burst = Durs.ToArray();
            for (int p = 0; p < numProcesses; p++)
            {
                waiting[p] = 0;
                starting.Add(p, new Queue<int>());
                finishing.Add(p, new Queue<int>());
                pids[p] = p;
            }
            priority = priorities.ToArray();

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
                        else if (currentTime == 0)
                            starting[p_indx].Enqueue(currentTime);      // starting of the first job

                        burst[p_indx] -= 1;          // decrease one unit time from the burst
                        if (burst[p_indx] == 0)
                        {
                            finishedPids.Enqueue(p_indx);
                            if (finishedPids.Count == numProcesses)
                                finishing[p_indx].Enqueue(currentTime + 1);             // last finishing time of the last job
                            turnAround[p_indx] = waiting[p_indx] + burstCopy[p_indx];
                            pids[p] = -1;  // it has just been executed
                        }
                        prevPid = p_indx;
                        break;
                    }
                }
            }
            Font font1 = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point);
            int prev_s = 0;
            for (int i = 0; !displayAll(numProcesses, finishing); i++)
            {
                var retVal = getNextFinishingTask(numProcesses, finishing);
                int p = retVal.Item1;
                s = retVal.Item2;

                string text1 = "P" + p.ToString();
                Rectangle rect = new Rectangle(2, 2, s * 10, 50);
                System.Drawing.Graphics gravicsObject;
                gravicsObject = this.CreateGraphics();
                Pen blackPen = new Pen(System.Drawing.Color.Black, 3);
                gravicsObject.DrawRectangle(blackPen, rect);
                gravicsObject.DrawString(text1, font1, Brushes.Blue, (s + prev_s) * 5 - 10, 10);
                string f = "" + s;
                int avg_waiting = waiting.Sum() / waiting.Length;
                string ff = avg_waiting.ToString(); //+ q.avr_time;
                gravicsObject.DrawString(f, font1, Brushes.Blue, s * 10-7, 55);
                if (i == 0)
                {
                    gravicsObject.DrawString("AVR_waiting_time :", font1, Brushes.Blue, 50, 270);
                    gravicsObject.DrawString(ff, font1, Brushes.Blue, 300, 270);
                }
                prev_s = s;
            }
        }
         public void Priority_Non_Preemptive()
        {
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
            arrival = arrivals;
            burst = Durs.ToArray();
            for (int p = 0; p < numProcesses; p++)
                pids[p] = p;
            priority = priorities.ToArray();
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
            for (int currentTime = 0; !jobs_done(pids); currentTime+=1)
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
            Font font1 = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point);
            int prev_s = 0;
            for (int i = 0; i < pids.Length; i++)
            {
                int p = finishedPids.Dequeue();
                s = finishing[p];
                
                string text1 = "P" + p.ToString();
                Rectangle rect = new Rectangle(2, 2, s*10, 50);
                System.Drawing.Graphics gravicsObject;
                gravicsObject = this.CreateGraphics();
                Pen blackPen = new Pen(System.Drawing.Color.Black, 3);
                gravicsObject.DrawRectangle(blackPen, rect);
                gravicsObject.DrawString(text1, font1, Brushes.Blue, (s+prev_s)*10-30, 10);
                string f = "" + s;
                int avg_waiting = waiting.Sum() / waiting.Length;
                string ff = avg_waiting.ToString(); //+ q.avr_time;
                gravicsObject.DrawString(f, font1, Brushes.Blue, s*10, 50);
                if (i == 0)
                {
                    gravicsObject.DrawString("AVR_waiting_time :", font1, Brushes.Blue, 50, 270);
                    gravicsObject.DrawString(ff, font1, Brushes.Blue, 300, 270);
                }
                //Console.WriteLine("The Values are {0}     {1}     {2}     {3}       {4}\n", p, starting[p], waiting[p], finishing[p], turnAround[p]);
            }
            prev_s = s;
                //Console.ReadKey();
        }

        public void button2_Click(object sender, EventArgs e)
        {
           // button2_Click => FCFS
            FCFS q = new FCFS(input, Durs, arrivals);
            q.sets();
            s = q.arrival[0];
            int x = (q.dur[0] - q.arrival[0]) / 2;
            Font font1 = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point);
            int prev_s = 0;
           for (int i = 0; i <q.number; i++)
           {
               if (i != 0) x = (q.dur[i] - q.arrival[i-1]) / 2;
               s += q.dur[i];
               string text1 = q.flag[i];
               Rectangle rect = new Rectangle(0, 0, s * 10, 50);
               System.Drawing.Graphics gravicsObject;
               gravicsObject = this.CreateGraphics();
               Pen blackPen = new Pen(System.Drawing.Color.Black, 3);
               gravicsObject.DrawRectangle(blackPen, rect);
              gravicsObject.DrawString(text1, font1, Brushes.Blue,(s + prev_s) * 10 - 30,10);
               string f = ""+s;
               string ff = "" + q.avr_time;      
               gravicsObject.DrawString(f, font1, Brushes.Blue, s*10-6, 50);
               if (i == 0)
               {
                   gravicsObject.DrawString("AVR_waiting_time :", font1, Brushes.Blue, 50, 270);
                   gravicsObject.DrawString(ff, font1, Brushes.Blue, 300, 270);
               }
           }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
             
            int[] arrivals_ = arriv.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            List<int> Durs = Durss.Split(',').Select(int.Parse).ToList();
            RR q = new RR(input,100, Durs, arrivals_);
            q.sets();
            s = q.arrival[0];
            int prev_s = 0;
            for (int i = 0; i < q.j; i++)
            {
                s += q.plot[i];
                string text1 = q.flag[i];
                Rectangle rect = new Rectangle(0, 0, s * 10, 50);
                System.Drawing.Graphics gravicsObject;
                gravicsObject = this.CreateGraphics();
                Pen blackPen = new Pen(System.Drawing.Color.Black, 3);
                Font font1 = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point);
                gravicsObject.DrawRectangle(blackPen, rect);
                gravicsObject.DrawString(text1, font1, Brushes.Blue, (s+ prev_s) * 10 -30, 10);
                string f = "" + s;
                string ff = "" + q.avr_time;      
                gravicsObject.DrawString(f, font1, Brushes.Blue, s*10 -6, 50);
                if (i == 0)
                {
                    gravicsObject.DrawString("AVR_waiting_time :", font1, Brushes.Blue, 50, 270);
                    gravicsObject.DrawString(ff, font1, Brushes.Blue, 300, 270);
                }
            }
        }

        }
}


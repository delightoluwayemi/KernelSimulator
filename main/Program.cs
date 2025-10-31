using System;

using System.IO;
using System.Net.WebSockets;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace KernelSimulator
{

    public class Kernel
    {

        public List<Process> processes = new List<Process>();
        public int processCount;
        public Queue<Process> readyQueue = new Queue<Process>();
        public List<Process> waitingList = new List<Process>();
        public List<Process> terminatedList = new List<Process>();
        public bool processRunning = false;
        public int timer = 0;
        public List<Process> loadProcess(string path)
        {
            var file = File.ReadAllLines(path).Skip(1);
            foreach (var line in file)
            {
                var columns = line.Split(',');
                var process = new Process
                {
                    Pid = int.Parse(columns[0]),
                    ArrivalTime = int.Parse(columns[1]),
                    TotalCPUTime = int.Parse(columns[2]),
                    IoFrequency = int.Parse(columns[3]),
                    IoDuration = int.Parse(columns[4]),
                    state = States.NEW
                };
                processes.Add(process);
            }
            processCount = processes.Count;
            return processes;
        }/*
        public void changeState(Process process, States oldState, States newState){
            switch (oldState){
                case States.NEW:
                process.state = States.READY; //newState
                kernel.readyList.Add(process);
            }
            var outputMessage = String.Format("{0},{1},{2}, {3} ",kernel.timer, process.Pid, oldState, process.state);
            outputLog.Add(outputMessage);
        }*/

        static void Main(string[] args)
        {
            var terminatedCount = 0;

            var path = @"/Users/bamideleoluwayemi/Documents/Documents - Bamidele’s MacBook Pro/allThingsCode/C#/KernelSimulator/test_files/test_case_1.csv";
            var kernel = new Kernel();
            kernel.loadProcess(path);
            /*foreach (var process in kernel.processes)
            {
                Console.WriteLine(process.Pid);
                Console.WriteLine(process.ArrivalTime);
                Console.WriteLine(process.TotalCPUTime);
                Console.WriteLine(process.IoFrequency);
                Console.WriteLine(process.IoDuration);
                Console.WriteLine(process.state);
                Console.WriteLine("+++++++++++++++++++++++++++++");
            }*/
            Console.WriteLine(kernel.processCount);

            Console.WriteLine(String.Format("Time of transition, Pid, Old State, New State"));

            //FCFS
            var outputLog = new List<string>();
            while (terminatedCount != kernel.processCount)
            {
                //move the process from NEW to READY
                foreach (var process in kernel.processes)
                {
                    if (kernel.timer == process.ArrivalTime)
                    {
                        var oldState = process.state;
                        process.state = States.READY;
                        kernel.readyQueue.Enqueue(process);
                        var outputMessage = String.Format("{0},{1},{2}, {3} ",kernel.timer, process.Pid, oldState, process.state);
                        outputLog.Add(outputMessage);
                    }
                }

                //move the process from waiting to ready
                for (var i =0; i<kernel.waitingList.Count; i++)
                {
                    if (kernel.waitingList[i].waitingTime == kernel.waitingList[i].IoDuration)
                    {
                        var oldState = kernel.waitingList[i].state;
                        kernel.waitingList[i].state = States.READY;
                        kernel.readyQueue.Enqueue(kernel.waitingList[i]);
                        var outputMessage = String.Format("{0},{1},{2}, {3} ", kernel.timer, kernel.waitingList[i].Pid, oldState, kernel.waitingList[i].state);
                        kernel.waitingList.Remove(kernel.waitingList[i]);
                        outputLog.Add(outputMessage);
                    }
                    else
                    {
                        ++kernel.waitingList[i].waitingTime;
                    }
                }

                //move the process from running to waiting or to terminated
                foreach (var process in kernel.processes)
                {
                    if (process.state == States.RUNNING)
                    {
                        if (process.TotalCPUTime == process.cpuRunTIme)
                        {
                            var oldState = process.state;
                            process.state = States.TERMINATED;
                            kernel.terminatedList.Add(process);
                            var outputMessage = String.Format("{0},{1},{2}, {3}, {4} ", kernel.timer, process.Pid, oldState, process.state, process.TotalCPUTime);
                            outputLog.Add(outputMessage);
                            kernel.processRunning = false;
                            terminatedCount++;
                        }
                        //i might get away with removing the else if
                        else if ((process.IoFrequency < process.TotalCPUTime) && (process.IoFrequency > 0))
                        {
                            if ((process.cpuRunTIme > 0) && (process.cpuRunTIme % process.IoFrequency == 0))
                            {
                                var oldState = process.state;
                                process.state = States.WAITING;
                                kernel.waitingList.Add(process);
                                var outputMessage = String.Format("{0},{1},{2}, {3} ", kernel.timer, process.Pid, oldState, process.state);
                                outputLog.Add(outputMessage);
                                kernel.processRunning = false;
                            }
                        }
                            process.cpuRunTIme++;
                    }
                }

                //move the process from ready to running
                if (kernel.processRunning == false)
                {
                    //this is possibly when nothing running, nothing in the waitlist is done or is empty, or nothing in new list or arrival time not met yet
                    if (kernel.readyQueue.Count ==0)
                    {
                        kernel.timer++;
                        continue;
                    }
                    else
                    {
                        var runningProcess = kernel.readyQueue.Peek();

                        //idea was to match the process in the ready queue and then changs its state in the process queue
                        foreach (var process in kernel.processes)
                        {
                            if (process == runningProcess)
                            {
                                kernel.processRunning = true;
                                var oldState = process.state;
                                process.state = States.RUNNING;
                                var outputMessage = String.Format("{0},{1},{2}, {3} ", kernel.timer, process.Pid, oldState, process.state);
                                outputLog.Add(outputMessage);
                            }
                        }
                        kernel.readyQueue.Dequeue();
                    }
                }
                terminatedCount = kernel.terminatedList.Count;
                ++kernel.timer;
            }
            foreach (var line in outputLog)
            {
                Console.WriteLine(line);
            }

        }
    }
}

using System;

using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.VisualBasic;

namespace KernelSimulator
{
    public class Process
    {
        public int Pid { get; set; }
        public int ArrivalTime { get; set; }
        public int TotalCPUTime { get; set; }
        public int IoFrequency { get; set; }
        public int IoDuration { get; set; }

        public States state;

    }

    public enum States
    {
        NEW,
        READY,
        RUNNING,
        WAITING,
        TERMINATED
    }

    public class Kernel
    {

        public List<Process> processes = new List<Process>();
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

            return processes;
        }
        static void Main(string[] args)
        {
            var path = @"/Users/bamideleoluwayemi/Documents/allThingsCode/C#/KernelSimulator/test_files/test_case_1.csv";
            var kernel = new Kernel();
            var processList = kernel.loadProcess(path);
            foreach (var process in processList)
            {
                Console.WriteLine(process.Pid);
                Console.WriteLine(process.ArrivalTime);
                Console.WriteLine(process.TotalCPUTime);
                Console.WriteLine(process.IoFrequency);
                Console.WriteLine(process.IoDuration);
                Console.WriteLine(process.state);
                Console.WriteLine("+++++++++++++++++++++++++++++");
            }
        }
    }
}

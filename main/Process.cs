    
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

            public int cpuRunTIme;
            public int waitingTime;

            public Process(){}

        }
}
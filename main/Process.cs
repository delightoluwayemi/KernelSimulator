    
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

            public int CpuRunTIme { get; set; }
            public int WaitingTime { get; set; }

        public Process() { }

        public string changeState(Process process, States oldState, States newState, int timer)
        {
            oldState = process.state;
            process.state = newState;
            var outputMessage = String.Format("{0},{1},{2}, {3} ", timer, process.Pid, oldState, process.state);
            return outputMessage;
            
        }


    }
}
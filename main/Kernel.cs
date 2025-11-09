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
                process.CpuRunTIme = process.TotalCPUTime;
                process.WaitingTime = process.IoDuration;
                processes.Add(process);
            }
            processCount = processes.Count;
            return processes;
        }
    }

}
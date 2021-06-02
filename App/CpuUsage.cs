using System;
using System.Threading;
using System.Diagnostics;

namespace NetCore.Docker
{
    public class CpuUsage
    {
        private static DateTime lastTime;
        private static TimeSpan lastTotalProcessorTime;
        private static DateTime curTime;
        private static TimeSpan curTotalProcessorTime;

        public static string GetInfo()
        {
            Process p = Process.GetCurrentProcess();
            string processName = p.ProcessName;

            while (true)
            {
                {
                    if (lastTime == null || lastTime == new DateTime())
                    {
                        lastTime = DateTime.Now;
                        lastTotalProcessorTime = p.TotalProcessorTime;
                    }
                    else
                    {
                        curTime = DateTime.Now;
                        curTotalProcessorTime = p.TotalProcessorTime;

                        double CPUUsage = (curTotalProcessorTime.TotalMilliseconds - lastTotalProcessorTime.TotalMilliseconds) / curTime.Subtract(lastTime).TotalMilliseconds / Convert.ToDouble(Environment.ProcessorCount);
                        return String.Format("{0} CPU: {1:0.000}%", processName, CPUUsage * 100);

                        lastTime = curTime;
                        lastTotalProcessorTime = curTotalProcessorTime;
                    }
                }

                Thread.Sleep(100);
            }

            return "ERROR";
        }
    }
}
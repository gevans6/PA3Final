using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Performance : TableEntity
    {
        public string Status { get; set; }
        public string CPU { get; set; }
        public string RAM { get; set; }
        //public int NumCrawled { get; set; }
        //public int SizeQueue { get; set; }
        //public int SizeIndex { get; set; }

        public Performance() { }

        public Performance(string stats, float cpu, float ram)
        {
            this.PartitionKey = "Performance";
            this.RowKey = String.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks) + "_" + Guid.NewGuid().ToString();

            this.Status = stats;
            this.CPU = cpu.ToString();
            this.RAM = ram.ToString();
            //this.NumCrawled = numCrawled;
            //this.SizeQueue = SizeQueue;
            //this.SizeIndex = sizeIndex;
        }

        public static void insertPerformance(string status)
        {
            PerformanceCounter theCPUCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            var cpu = theCPUCounter.NextValue();
            Thread.Sleep(100);
            var cpu2 = theCPUCounter.NextValue();

            PerformanceCounter theMemCounter = new PerformanceCounter("Memory", "Available MBytes");
            var ram = theMemCounter.NextValue();

            //Create performance entries
            Performance performance = new ClassLibrary1.Performance(status, cpu2, ram);

            //add exception to table
            TableOperation insertOperation = TableOperation.Insert(performance);
            StorageManager.getPerformanceTable().Execute(insertOperation);
        }
    }
}

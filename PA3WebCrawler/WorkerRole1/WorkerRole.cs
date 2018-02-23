using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using ClassLibrary1;
using System.Security.Cryptography;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;
using System.IO;
using HtmlAgilityPack;
using System.Xml;

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);       

        public override void Run()
        {
            RobotParser  parser = new RobotParser();
           
            HtmlCrawler htmlCrawler = new HtmlCrawler(new HashSet<string>());

            Status.status = "Idle";

            SizeCounter.NumCrawled = 0;
            SizeCounter.SizeIndex = 0;
            SizeCounter.SizeQueue = 0;

            bool loading = false;

            bool crawling = false;

            bool idle = true;



            Trace.TraceInformation("WorkerRole1 is running");
   
            while (true)
            {
                Thread.Sleep(50);
                string status = "";
                if(idle == true)
                {
                    status = "Idle";
                } else if(crawling == true)
                {
                    status = "Crawling";
                } else if(loading == true)
                {
                    status = "Loading";
                }

                CloudQueueMessage message2 = new CloudQueueMessage(SizeCounter.SizeQueue.ToString());

                StorageManager.getNumQueue().AddMessage(message2);

                CloudQueueMessage message3 = new CloudQueueMessage(SizeCounter.SizeIndex.ToString());

                StorageManager.getNumIndex().AddMessage(message3);

                CloudQueueMessage message4 = new CloudQueueMessage(SizeCounter.NumCrawled.ToString());

                StorageManager.getNumCrawled().AddMessage(message4);

                Performance.insertPerformance(status);


                //Handle Command Queue
                CloudQueueMessage commandMessage = StorageManager.getCommandQueue().GetMessage(TimeSpan.FromMinutes(5));

                //In the case there is no more Urls to crawl, or at the beginning, this command message will be called
                if (commandMessage != null)
                {
                    StorageManager.getCommandQueue().DeleteMessage(commandMessage);

                    //command message is stop
                    if (commandMessage.AsString == "stop")
                    {
                        //clear queue and table
                        StorageManager.deleteAllQueues();
                        StorageManager.deleteTables();
                        //reset parser and crawler
                        parser = new RobotParser("");
                        htmlCrawler.crawlable = false;
                        htmlCrawler.Visited = new HashSet<string>();
                        htmlCrawler.Disallow = new HashSet<string>();
                        SizeCounter.Clear();
                        loading = false;
                        crawling = false;
                        idle = true;
                        Performance.insertPerformance("Idle");
                    }

                    //command message is start
                    if (commandMessage.AsString.StartsWith("start:"))
                    {
                        crawling = false;
                        idle = false;
                        loading = true;
                        Performance.insertPerformance("Loading");

                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        var robotFile = commandMessage.AsString.Substring(6);

                        string contents;
                        using (var wc = new System.Net.WebClient())
                        {
                            contents = wc.DownloadString(robotFile);
                        }

                        //create and parse through robots.txt
                        parser = new RobotParser(contents);

                        foreach(string filepath in parser.XMLFiles)
                        {
                            //only XMLs from cnn and nba
                            if(filepath.Contains("cnn") || filepath.Contains("nba"))
                            {
                                CloudQueueMessage filepathMessage = new CloudQueueMessage(filepath);
                                StorageManager.getXMLQueue().AddMessage(filepathMessage);
                            }                          
                        }

                        //set the crawler with the disallows
                        htmlCrawler = new HtmlCrawler(parser.Disallow);
                        Performance.insertPerformance("Idle");
                    }   
                }

                

                //Handle XML Queue
                CloudQueueMessage XML = StorageManager.getXMLQueue().GetMessage(TimeSpan.FromMinutes(5));
                if(XML != null)
                {
                    crawling = false;
                    idle = false;
                    loading = true;
                    Performance.insertPerformance("Loading");
                }
                while(XML != null)
                {
                    StorageManager.getXMLQueue().DeleteMessage(XML);
                    Performance.insertPerformance(status);
                    htmlCrawler.readXMLUrl(XML.AsString);

                    XML = StorageManager.getXMLQueue().GetMessage(TimeSpan.FromMinutes(5));
                }

                //Handle HTML Queue
                CloudQueueMessage HTML = StorageManager.getUrlQueue().GetMessage(TimeSpan.FromMinutes(5));
                if(HTML != null && htmlCrawler.crawlable == true)
                {
                    idle = false;
                    loading = false;
                    crawling = true;
                    StorageManager.getUrlQueue().DeleteMessage(HTML);
                    SizeCounter.SizeQueue--;

                    CloudQueueMessage message5= new CloudQueueMessage(SizeCounter.SizeQueue.ToString());

                    StorageManager.getNumQueue().AddMessage(message5);

                    Performance.insertPerformance("Crawling");

                    htmlCrawler.parseHTML(HTML.AsString);
                } 
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("WorkerRole1 has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole1 is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("WorkerRole1 has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }
}

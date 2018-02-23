using ClassLibrary1;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;

namespace WebRole1
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        private static string queue = "0";
        private static string index = "0";
        private static string crawled = "0";

        [WebMethod]
        public void BeginCrawling()
        {
            CloudQueueMessage startMessage = new CloudQueueMessage("start:https://www.cnn.com/robots.txt");

            StorageManager.getCommandQueue().AddMessage(startMessage);

            CloudQueueMessage startMessage2 = new CloudQueueMessage("start:https://www.bleacherreport.com/robots.txt");

            StorageManager.getCommandQueue().AddMessage(startMessage2);
        }

        [WebMethod]
        public void StopCrawling()
        {
            CloudQueueMessage stopMessage = new CloudQueueMessage("stop");

            StorageManager.getCommandQueue().AddMessage(stopMessage);
        }

        

        [WebMethod]
        public string[] ReadLinksFromTableStorage()
        {
            TableQuery<WebPage> rangeQuery = new TableQuery<WebPage>()
                .Take(10);

            List<string> answers = new List<string>();

            foreach (WebPage entity in StorageManager.getTable().ExecuteQuery(rangeQuery))
            {
                answers.Add(entity.Url);
            }

            return answers.ToArray();
        }

        [WebMethod]
        public string [] GetTitleLinkDate(string url)
        {
            List<string> result = new List<string>();

            var keyBytes = System.Text.Encoding.UTF8.GetBytes(url);
            var base64 = System.Convert.ToBase64String(keyBytes);
            base64 = base64.Replace('/', '_');

            TableQuery<WebPage> query = new TableQuery<WebPage>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, base64));

            foreach(WebPage entity in StorageManager.getTable().ExecuteQuery(query))
            {
                result.Add(entity.Title);
                result.Add(entity.Url);
                result.Add(entity.Date);
            }

            return result.ToArray();
        }

        [WebMethod]
        public string[] GetErrors()
        {
            List<string> results = new List<string>();

            TableQuery<ExceptionUrl> query2 = new TableQuery<ExceptionUrl>()
                .Take(10);

            foreach(ExceptionUrl item in StorageManager.getExceptionTable().ExecuteQuery(query2))
            {
                results.Add(item.Url + "/////// Error: " + item.Message);
            }

            return results.ToArray();
        }

        [WebMethod]
        public string GetCPU()
        {
            var result = "";
            TableQuery<Performance> query3 = new TableQuery<Performance>()
                .Take(1);

            foreach(Performance cpu in StorageManager.getPerformanceTable().ExecuteQuery(query3))
            {
                result = cpu.CPU;
            }

            return result.ToString();
        }

        [WebMethod]
        public string GetRAM()
        {
            var result = "";
            TableQuery<Performance> query3 = new TableQuery<Performance>()
                .Take(1);

            foreach (Performance ram in StorageManager.getPerformanceTable().ExecuteQuery(query3))
            {
                result = ram.RAM;
            }

            return result.ToString();
        }

        [WebMethod]
        public string GetStatus()
        {
            var result = "";
            TableQuery<Performance> query4 = new TableQuery<Performance>()
                .Take(1);

            foreach (Performance status in StorageManager.getPerformanceTable().ExecuteQuery(query4))
            {
                result = status.Status;
            }

            return result;
        }

        [WebMethod]
        public string GetSizeQueue()
        {
            CloudQueueMessage queueMessage = StorageManager.getNumQueue().GetMessage(TimeSpan.FromMinutes(5));
            if(queueMessage != null)
            {
                StorageManager.getNumQueue().DeleteMessage(queueMessage);
                if(queueMessage.AsString != queue)
                {
                    queue = queueMessage.AsString;                  
                }   
            }
            return queue;
        }

        [WebMethod]
        public string GetSizeIndex()
        {
            CloudQueueMessage queueMessage = StorageManager.getNumIndex().GetMessage(TimeSpan.FromMinutes(5));
            if (queueMessage != null)
            {
                StorageManager.getNumIndex().DeleteMessage(queueMessage);
                if (queueMessage.AsString != index)
                {
                    index = queueMessage.AsString;
                }
            }
            return index;        
        }

        [WebMethod]
        public string GetNumCrawled()
        {
            CloudQueueMessage queueMessage = StorageManager.getNumCrawled().GetMessage(TimeSpan.FromMinutes(5));
            if(queueMessage != null)
            {
                StorageManager.getNumCrawled().DeleteMessage(queueMessage);
                if (queueMessage.AsString != crawled)
                {
                    crawled = queueMessage.AsString;
                    
                }  
            }
            return crawled;
        }
    }
}

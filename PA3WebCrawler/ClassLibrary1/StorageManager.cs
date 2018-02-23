using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class StorageManager
    {
        public static CloudTable getTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("urltable");
            table.CreateIfNotExists();

            return table;
        }

        public static CloudQueue getCommandQueue()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("commandqueue");
            queue.CreateIfNotExists();

            return queue;
        }

        public static CloudQueue getUrlQueue()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("urlqueue");
            queue.CreateIfNotExists();

            return queue;
        }

        public static CloudQueue getXMLQueue()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("xmlqueue");
            queue.CreateIfNotExists();

            return queue;
        }

        public static void deleteAllQueues()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("xmlqueue");
            if(queue.Exists())
            {
                queue.Clear();
            }
            
            CloudQueue queue2 = queueClient.GetQueueReference("urlqueue");
            if(queue2.Exists())
            {
                queue2.Clear();
            }
            
            CloudQueue queue3 = queueClient.GetQueueReference("urlqueue");
            if(queue3.Exists())
            {
                queue3.Clear();
            }  
        }

        public static void deleteTables()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("urltable");
            if(table.Exists())
            {
                table.Delete();
            }
            CloudTable table2 = tableClient.GetTableReference("exceptiontable");
            if(table2.Exists())
            {
                table2.Delete();
            }
        }

        public static CloudTable getExceptionTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("exceptiontable");
            table.CreateIfNotExists();

            return table;
        }

        public static CloudTable getPerformanceTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("performancetable");
            table.CreateIfNotExists();

            return table;
        }

        public static CloudQueue getNumQueue()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("numqueuequeue");
            queue.CreateIfNotExists();

            return queue;
        }

        public static CloudQueue getNumIndex()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("numindexqueue");
            queue.CreateIfNotExists();

            return queue;
        }

        public static CloudQueue getNumCrawled()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("numcrawledqueue");
            queue.CreateIfNotExists();

            return queue;
        }
    }
}

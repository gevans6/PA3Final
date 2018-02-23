using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class WebPage : TableEntity
    { 
        public string Url { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }

        public WebPage() { }

        public WebPage(string url, string title, string date) {
            var keyBytes = System.Text.Encoding.UTF8.GetBytes(url);
            var base64 = System.Convert.ToBase64String(keyBytes);
            base64 = base64.Replace('/', '_');

            this.PartitionKey = base64;
            this.RowKey = String.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks) + "_" + Guid.NewGuid().ToString();

            this.Url = url;
            this.Title = title;
            this.Date = date;
        }
    }
}

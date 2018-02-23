using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class RobotParser
    {
        public List<string> XMLFiles { get; set; }

        public HashSet<string> Disallow { get; set; }

        public RobotParser() { }

        public RobotParser(string textFile)
        {
            this.XMLFiles = new List<string>();
            this.Disallow = new HashSet<string>();
              
            using(StringReader reader = new StringReader(textFile))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    if(line.StartsWith("Sitemap: "))
                    {
                        string sitemap = line.Substring(9);
                        this.XMLFiles.Add(sitemap);
                    }

                    if(line.StartsWith("Disallow: "))
                    {
                        string disallow = line.Substring(10);
                        this.Disallow.Add(disallow);
                    }
                }
            }
        }
    }
}

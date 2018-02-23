# PA3Final

Link to Github repo: https://github.com/gevans6/PA3Final

Link to WebCrawler CloudApp: http://webcrawlerpa3.cloudapp.net/

The state of the worker role is shown on the dashboard using a a performance table that is updating whenever an item is put into a queue, table, or the program reaches the top of the while loop of the Run() function.

Machine counters are also stored in this table. 

The number of URLs, size of queue, and size of index are updated when there is a change in the size of each variable, and that change is reflected in a queue that is then read by the WebService1.asmx and reflected on the dashboard.

The last 10 URLs and last 10 Errors are stored in their own table and with a RowKey denoting when they were inserted. The first 10 rows are taken and reflected on the dashboard.

The Dashboard will let you start crawling the internet, however you may not stop until the crawling has started. Stopping clears all the tables and variables and resets the process.

I used an XMLreader and the HTMLAgilityClass to parse through XML and HTML files to grab URLs, Titles, and Dates. This functionality can be seen using the search function on the dashboard.

I used a RobotParser Class to parse through the robot.txts.

HTMLCrawler Class to crawl and parse XML and HTML files.

SizeCounter class to keep track of the size of the queue, size of the index, and number of urls crawled.

StorageManager class to keep track and manage creation and deletion of all tables and queues.

Performance, WebPage, Error table entities to keep track of perforamnce counters, webpages, and errors.

XML, command, HTML, SizeQueue, SizeIndex, and NumberCrawled queues to keep track of urls, sizes, and commands in chronological order.

All data recieved by the WebService is given and recieved from either queues or tables to keep communication with the Worker Role.
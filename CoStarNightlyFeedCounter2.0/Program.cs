using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CoStarNightlyFeedCounter2_0
{
    class Program
    {
        static void Main(string[] args)
        {
            // about this tool.
            Console.WriteLine("This tool counts the total number of points CoStar runs through the nightly feed process.\n");


            // Instructions on how to run.
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Instructions: ");
            Console.WriteLine("Download a months worth of CoStar nightly feed emails.");
            Console.WriteLine("Save all emails from target month as a .txt file to your PC.");
            Console.WriteLine("Follow the rest of the instructions from this tool.\n");
            Console.ResetColor();

            // get file
            string inputfile = FunctionTools.GetAFile();

            // rolling months with point count.
            Dictionary<int, int> rollingmonthscount = new Dictionary<int, int>();

            // rolling count of occurances of points counted.
            int pointcount = 0;

            // read lines of file.
            using (StreamReader readfile = new StreamReader(inputfile))
            {
                // test string for line of info we need.
                string testlinestring = "records were loaded into table costar_feed_sites";
                
                // file line.
                string line;

                // get starting date. we count backwards so we can use todays date.
                DateTime lastdate = DateTime.Now;

                while ((line = readfile.ReadLine()) != null)
                {
                    // read until specific line is found. 
                    if (line.Contains(testlinestring))
                    {
                        // get new date time from line.
                        DateTime newdate = LineParser.CurrentDate(line);

                        // compare dates.
                        bool tocountnew = LineParser.CompareDateTime(lastdate, newdate);

                        if (tocountnew == true)
                        {
                            // getnumber of points run through nightly feed
                            int numbertoadd = LineParser.NewPointsAdded(line);

                            // log months and counts per month read
                            int newmonth = newdate.Month;

                            if (!rollingmonthscount.ContainsKey(newmonth))
                            {
                                rollingmonthscount.Add(newmonth, numbertoadd);

                                // log number of times points added to running total.
                                pointcount += 1;
                            }
                            else
                            {
                                // if month already exists just add number.
                                rollingmonthscount[newmonth] += numbertoadd;

                                // log number of times points added to running total.
                                pointcount += 1;
                            }

                            // update date run.
                            lastdate = newdate;
                        }
                    }

                    // read to end of file.
                }

            }

            // output
            string outpuffile = FunctionTools.GetDesktopDirectory() + "\\CoStarNightlyFeedReport.txt";

            using (StreamWriter writefile = new StreamWriter(outpuffile))
            {
                // title
                writefile.WriteLine("CoStar Nightly Feed Report:");

                //date time.
                writefile.Write("Data pulled: ");
                writefile.WriteLine(DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));

                // pull results
                writefile.WriteLine($"Total Nightly Processes run: {pointcount}");

                //total
                int totalpointsrun = 0;

                foreach (var k in rollingmonthscount.Keys)
                {
                    totalpointsrun += rollingmonthscount[k];
                }

                writefile.WriteLine($"Total points run through nightly process: {totalpointsrun}\n");
                writefile.WriteLine("Month(1-12) | PointCount");

                // totals by month
                foreach (var kvp in rollingmonthscount)
                {
                    writefile.WriteLine($"{kvp.Key} | {kvp.Value}");
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Output written to file: {outpuffile}");
                Console.ResetColor();
            }


        }


    }
}

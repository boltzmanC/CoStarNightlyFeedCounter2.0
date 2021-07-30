using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoStarNightlyFeedCounter2_0
{
    public class LineParser
    {

        const char leftbracket = '[';
        const char rightbracket = ']';


        // parse date time + numebers
        // date compare and if new date is after last date then add number


        public static int NewPointsAdded (string line)
        {
            int index = line.IndexOf(rightbracket);
            string newnumberstring = string.Empty;

            string linedataremoved = line.Substring(index + 1, (line.Length - 1) - index); // read from ] to end of line.

            foreach (char c in linedataremoved)
            {
                if (Char.IsNumber(c))
                {
                    newnumberstring += c; //build string of numbers.
                }
            }

            int newnumber;
            bool validnumber = Int32.TryParse(newnumberstring, out newnumber);

            return newnumber;
        }

        public static DateTime CurrentDate (string line)
        {
            DateTime newdatetime;
            int index = line.IndexOf(rightbracket);

            // pull out datetime string. Format: [2021 / 07 / 20 23:21:10]
            string linedataremoved = line.Substring(0, index + 1);

            // format string for parsing.
            linedataremoved = linedataremoved.Trim();
            linedataremoved = linedataremoved.Replace(leftbracket.ToString(), string.Empty);
            linedataremoved = linedataremoved.Replace(rightbracket.ToString(), string.Empty);

            bool validdate = DateTime.TryParse(linedataremoved, out newdatetime);

            return newdatetime;
        }

        public static bool CompareDateTime (DateTime lastdate, DateTime newdatetime)
        {
            // return true if new date is same or before last date read. 
            // file output goes backwards when you download the files. so you wil read from end of month to beginning.
            // return false if we are going to skip adding the new number.

            int compareresult = DateTime.Compare(lastdate, newdatetime);

            if (compareresult >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}

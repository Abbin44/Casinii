using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esolang
{
    class FileHandler
    {
        public FileHandler(string file)
        {
            CheckLines(file);
        }
        void CheckLines(string filePath)
        {
            var lines = File.ReadLines(filePath);
            var Minimum = "";
            var Maximum = "";

            //Get longest and shortest line
            Maximum = lines.OrderByDescending(a => a.Length).First().ToString();
            Minimum = lines.OrderBy(a => a.Length).First().ToString();

            //Edit the text file to make all lines evenly long
            ReadFile(Maximum.Length, filePath, filePath);
        }
        void ReadFile(int maxLength, string file, string filePath)
        {
            string[] lines = new string[5];
            int lineCounter = 0;

            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(file))
            using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    //Reads file and adds lines to an array
                    lines[lineCounter] = line;
                    Console.WriteLine(lines[lineCounter]); //THIS IS THE GOOD LOOKING DEBUG PRINT
                    lineCounter++;
                }

                for (int i = 0; i < lines.Length; i++)
                {
                    //Check if line is less than longest line
                    if (lines[i].Length < maxLength)
                    {
                        int diff = maxLength - lines[i].Length;
                        char blank = ' ';

                        for (int j = 0; j < diff; j++)
                        {
                            //Add a blank space diff ammount of times to the end of the line
                            lines[i] += blank;
                        }
                    }
                    else
                        continue;
                }
            }
            //Write edited text to file
            File.WriteAllLines(filePath, lines);

            Program.CreateArray(maxLength);
        }
    }
}

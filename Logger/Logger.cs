﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Program
{
    public static class Logger
    {
        public static void InfoLog(string input)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            string path = @"../../../Logger/LogFiles/" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".txt";
            //string path = @"C:\Users\roee9\Desktop\Studyings\Semester 2\Introduction to Software Enginering\Project for SE\ISE172_project\Logger\LogFiles\" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".txt";
            System.IO.StreamWriter log = new System.IO.StreamWriter(path, true);
            log.Write("Type: Message ,");
            log.Write(DateTime.Now + " , ");
            log.Write(input);
            log.WriteLine();
            log.Close();
        }
        public static void ErrorLog(MethodBase file, int line, string message)
        {
            Console.WriteLine("\n\nTom\n\nTom\n\n");
            Console.WriteLine(Directory.GetCurrentDirectory());
            string path = @"../../../Logger/LogFiles/" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".txt";
            //string path = @"C:\Users\roee9\Desktop\Studyings\Semester 2\Introduction to Software Enginering\Project for SE\ISE172_project\Logger\LogFiles\" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".txt";
            System.IO.StreamWriter log = new System.IO.StreamWriter(path, true);
            log.Write("Type: Error. ");
            log.Write("Occured in " + DateTime.Now + " ,");
            log.Write(" in method " + file + " in Line " + line);
            log.WriteLine();
            log.Write("Reason: " + message);
            log.WriteLine();
            log.Close();
        }
        public static void DebugLog(string input)
        {
            string path = @"../../../Logger/LogFiles/" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".txt";
            //string path = @"C:\Users\roee9\Desktop\Studyings\Semester 2\Introduction to Software Enginering\Project for SE\ISE172_project\Logger\LogFiles\" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".txt";
            System.IO.StreamWriter log = new System.IO.StreamWriter(path, true);
            log.Write("Type: Debug.");
            log.Write("Occured in " + DateTime.Now + ",");
            log.Write("Status: " + input);
            log.WriteLine();
            log.Close();
        } 
    }
}

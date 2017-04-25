using Program;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{

    public class Presentation
    {
        
        public static void MainLoop()  //Print the main communication loop with the user.
        {

                AsciiSilverTongue ast = new AsciiSilverTongue();
            try
            {
                Logger.logMessage("The user opens the system");
                ast.PrintWelcome();
                ast.PrintMenu();
                string input = ast.ReadLine().ToLower();
                while (!input.Equals("exit"))
                {
                    if (input.Equals("menu"))
                        ast.PrintMenu();
                    else if (input.Equals("clear"))
                    {
                        Console.Clear();
                        ast.PrintWelcome();
                        ast.PrintMenu();
                    }
                    else
                    {
                        Logger.logMessage("The request is sent to the parser");
                        Parser.parse(input); // Parse and handle the input using the BusinessLayer
                    }
                    input = ast.ReadLine();

                }
            }
            catch
            {
                StackFrame st = new StackFrame(0, true);
                String file = st.GetFileName();
                String line = Convert.ToString(st.GetFileLineNumber());
                Logger.logError(file, line,"Unexcepted error");
            }

            // "exit" was recieved as input - print Goodbye and exit program
            Logger.logMessage("The user exits the system");
            ast.PrintGoodbye();
        }
    }
}

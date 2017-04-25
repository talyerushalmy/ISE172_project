using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public static class Parser
    {
        // Recieves the input string from the communication module and parses it, the sends it to the relevant function in the socket
        public static void parse(String str)
        {
            Socket socket = new Socket();
            string [] words = str.Split(' ');
            string command = words[0];
            switch (command.ToLower())
            {
                case "buy":
                    {
                        if (words.Length == 4)
                        {
                            Logger.logMessage("The buy request is sent to the socket");
                            socket.buy(str.Substring(command.Length + 1));
                        }
                        else
                        {
                            StackFrame st = new StackFrame(0, true);
                            String file = st.GetFileName();
                            String line = Convert.ToString(st.GetFileLineNumber());
                            Logger.logError(file, line,"Can not perform buy request");
                            socket.printNoValidCommandError();
                        }
                    }
                    break;
                case "sell":
                    {
                        if (words.Length == 4)
                        {
                            Logger.logMessage("The sell request is sent to the socket");
                            socket.sell(str.Substring(command.Length + 1));
                        }
                        else
                        {
                            StackFrame st = new StackFrame(0, true);
                            String file = st.GetFileName();
                            String line = Convert.ToString(st.GetFileLineNumber());
                            Logger.logError(file, line, "Can not perform sell request");
                            socket.printNoValidCommandError();
                        }
                    }
                    break;
                case "cancel":
                    {
                        if (words.Length == 2)
                        {
                            Logger.logMessage("The cancel request is sent to the socket");
                            socket.cancel(words[1]);
                        }
                        else
                        {
                            StackFrame st = new StackFrame(0, true);
                            String file = st.GetFileName();
                            String line = Convert.ToString(st.GetFileLineNumber());
                            Logger.logError(file, line, "Can not perform cancel request");
                            socket.printNoValidCommandError();
                        }
                    }
                    break;
                case "info":
                    {
                        if (words.Length == 1)
                        {
                            Logger.logMessage("The user's information request is sent to the socket");
                            socket.userInfo();
                        }
                        else
                        {
                            StackFrame st = new StackFrame(0, true);
                            String file = st.GetFileName();
                            String line = Convert.ToString(st.GetFileLineNumber());
                            Logger.logError(file, line, "Can not perform user's information request");
                            socket.printNoValidCommandError();
                        }
                    }
                    break;
                case "find":
                    {
                        if(words.Length == 3)
                        {
                            Logger.logMessage("Find information request is sent to the socket");
                            socket.findInfo(str.Substring(words[0].Length + 1));
                        }
                        else
                        {
                            StackFrame st = new StackFrame(0, true);
                            String file = st.GetFileName();
                            String line = Convert.ToString(st.GetFileLineNumber());
                            Logger.logError(file, line, "Can not find information");
                            socket.printNoValidCommandError();
                        }
                    }
                    break;
                default:
                    {
                        // no command was identified
                        StackFrame st = new StackFrame(0, true);
                        String file = st.GetFileName();
                        String line = Convert.ToString(st.GetFileLineNumber());
                        Logger.logError(file, line, "The user enters invalid input");
                        socket.printNoValidCommandError();
                    }
                    break;
            }
        }


    }
}

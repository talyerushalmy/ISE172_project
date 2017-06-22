using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public static class Parser
    {
        // Recieves the input string from the communication module and parses it, the sends it to the relevant function in the socket
        public static bool parse(String str)
        {
            Socket socket = new Socket();
            string[] words = str.Split(' ');
            string command = words[0];
            switch (command.ToLower())
            {
                case "buy":
                    {
                        if (words.Length == 4)
                            socket.buy(str.Substring(command.Length + 1));
                        else
                            goto default;
                    }
                    break;
                case "sell":
                    {
                        if (words.Length == 4)
                            socket.sell(str.Substring(command.Length + 1));
                        else
                            goto default;
                    }
                    break;
                case "cancel":
                    {
                        if (words.Length == 2)
                        {
                            if (words[1].Equals("all"))
                                socket.cancelAll();                 
                            else
                                socket.cancel(words[1]);
                        }
                        else
                            goto default;
                    }
                    break;
                case "info":
                    {
                        if (words.Length == 1)
                            //print info about the user
                            socket.userInfo();
                        else
                            goto default;
                    }
                    break;
                case "requests":
                    if (words.Length == 1)
                        socket.userRequestsInfo();
                    else
                        goto default;
                    break;
                case "market":
                    if (words.Length == 1)
                        socket.allMarketRequest();
                    else
                        goto default;
                    break;
                case "find":
                    {
                        if (words.Length == 3)
                            socket.findInfo(str.Substring(words[0].Length + 1));
                        else
                            goto default;
                    }
                    break;
                case "auto":
                    {
                        if (words.Length == 1)
                            socket.runAutoMarketAgent();
                        else
                            goto default;
                    }
                    break;
                case "history":
                    {
                        if (words.Length == 1)
                        {
                            socket.allHistory();
                        }   
                        else
                            goto default;
                    }
                    break;
                case "share":
                    {
                        if (words.Length == 1)
                        {
                            socket.marketShare();
                        }
                        else
                            goto default;
                    }
                    break;
                default:
                    {
                        // no command was identified
                        socket.printNoValidCommandError();
                        return false;
                    }
            }
            return true;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public static class Parser
    {
        public static void parser(String str)
        {
            ErrorPrinter errorPrinter = new ErrorPrinter();
            Socket socket = new Socket();
            String[] words = str.Split(' ');
            switch (words[0].ToLower())
            {
                case "buy":
                    {
                        if (words.Length == 4)
                            socket.buy(str.Substring(4));
                        else
                            errorPrinter.printError(0);
                    }
                    break;
                case "sell":
                    {
                        if (words.Length == 4)
                            socket.sell(str.Substring(5));
                        else
                            errorPrinter.printError(0);
                    }
                    break;
                case "cancel":
                    {
                        if (words.Length == 2)
                        {
                            socket.cancel(words[1]);
                        }
                        else
                            socket.printNoValidCommandError();
                    }
                    break;
                case "info":
                    {
                        if (words.Length == 2)
                            socket.info(words[1]);
                        else if (words.Length == 1)
                            //print info about the user
                            ;
                        else
                            socket.printNoValidCommandError();
                    }
                    break;
                default:
                    {
                        // no command was identified
                        errorPrinter.printError(9);
                    }
                    break;
            }
        }


    }
}

using System;

namespace Esolang
{
    public class Errors
    {
        static public void ErrorMessage(int errorCode)
        {
            if(errorCode == 1)
            {
                Console.WriteLine("Exited with error code 1");
                Console.WriteLine("Can't interpret code :: Missing an operator somewhere.");
                Console.ReadLine();

                Environment.Exit(errorCode);
            }
            else if(errorCode == 2)
            {
                Console.WriteLine("Exited with error code 2");
                Console.WriteLine("Can't interpret code :: Missing a dice somewhere.");
                Console.ReadLine();

                Environment.Exit(errorCode);
            }
        }
    }
}

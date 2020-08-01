using System;
using System.Collections.Generic;

namespace Esolang
{
    class MainBus
    {
        public static List<string> outputList = new List<string>();

        public MainBus(List<Dice> ints, List<StringVar> strings, List<Operator> operators)
        {
            int result = 0;
            int stringIndex = 0;
            int iterations = 0;

            int outputIndex = 0;

            string output = null;

            for (int i = 1; i < ints.Count; i++)
            {
                Console.WriteLine("Debug : Dice = " + ints[i].Value);
                Console.WriteLine("Debug : Result = " + result);

                    if(i <= strings.Count)
                    {
                        if (strings[0].Index == iterations)
                        {
                            Console.Write(result + strings[stringIndex].Text);
                            outputList[outputIndex] = strings[stringIndex].Text;
                            outputIndex++;
                            result = 0;
                            stringIndex++;
                            Console.Write("EEEEEEEK");
                        }
                    }

                if (operators[i - 1].Type == "*")
                {
                    if (result == 0)
                        result = ints[i - 1].Value;

                    result *= ints[i].Value;
                    outputList.Add(result.ToString());
                    Console.WriteLine("Performed a * :: " + result);

                    iterations++;
                }
                else if (operators[i - 1].Type == "/")
                {
                    if (result == 0)
                        result = ints[i - 1].Value;

                    result /= ints[i].Value;
                    outputList.Add(result.ToString());
                    Console.WriteLine("Performed a / :: " + result);

                    iterations++;
                }
            }

            for (int i = 1; i < ints.Count; i++)
            {
                Console.WriteLine("Debug : Dice = " + ints[i].Value);
                Console.WriteLine("Debug : Result = " + result);

                if (operators[i - 1].Type == "-")
                {
                    result -= ints[i - 1].Value;
                    outputList.Add(result.ToString());
                    Console.WriteLine("Performed a - :: " + result);

                    iterations++;
                }
                else if (operators[i - 1].Type == "+")
                {
                    result += ints[i - 1].Value;
                    outputList.Add(result.ToString());
                    Console.WriteLine("Performed a + :: " + result);

                    iterations++;
                }
            }
            Console.Write("The result is: " + result);
        }
    }
}
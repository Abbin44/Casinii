using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Esolang
{
    class Program
    {
        #region Description
        // /\ Ruter === *
        // \/
        //
        // /\/\
        // \  / Hjärter === +
        //  \/
        //
        //  o
        // o|o Klöver === /  
        //
        //  /\
        // /  \ Spader === -
        // \||/
        #endregion
        static int result;
        static int globalIndex = 0;

        //{Environment.UserName}
        //F:\Programmering\C#\School\Esolang with bus\Esolang\bin\Debug\code.txt
        static string filePath;
        static void Main(string[] args)
        {
            // Check for correct file format and length
            
            if (!(args.Length == 1 && args[0].EndsWith(".cas")))
            {
                Console.WriteLine("Code not a valid file");
                Console.ReadKey();
                return;
            }
            filePath = args[0];
            

            //filePath = @"F:\Programmering\C#\School\Esolang with bus\Esolang\bin\Debug\code.txt";

            int fileLineCount = File.ReadAllLines(filePath).Length;
            FileHandler fh = new FileHandler(filePath);
        }

        static public void CreateArray(int length)
        {
            //Lenght is the horizontal lenght which varies depending on the code inputed
            var map = new char[5, length];

            StreamReader sr = new StreamReader(filePath);

            //line is the temp string that carries each read line
            string line;

            //lineCount is used to index the vertical lines
            var lineCount = 0;
            while ((line = sr.ReadLine()) != null)
            {
                if (lineCount <= 5)
                {
                    for (int i = 0; i < length; i++)
                    {
                        //Add all chars of line to the correct positions in the map array
                        map[lineCount, i] = line[i];
                    }
                    if (lineCount < 5)
                        lineCount++;
                }
            }
            //Close streamreader to avoid memory leak
            sr.Close();

            #region PrintArray
            /*
            //Print array
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(string.Format("{0} ", map[i, j]));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            */
            #endregion

            ScanForDices(length, map);
        }

        //Store all found dices to index them
        static List<Dice> dices = new List<Dice>();
        static List<StringVar> strings = new List<StringVar>();

        static int diceIndex = 0;
        static void ScanForDices(int arrayLenght, char[,] _map)
        {
            int diceValue = 0;

            for (int j = 0; j < arrayLenght; j++)
            {
                #region Dice
                //if this happens a dice has been detected
                if (_map[2, j] == '|' && _map[2, j + 6] == '|' && _map[2 - 2, j + 3] == '_' && _map[2 + 1, j + 3] == '_')
                {
                    //List of all indexes to scan
                    List<char> diceNumbers = new List<char>();

                    diceNumbers.Add(_map[1, j + 1]);
                    diceNumbers.Add(_map[1, j + 3]);
                    diceNumbers.Add(_map[1, j + 5]);
                    diceNumbers.Add(_map[2, j + 1]);
                    diceNumbers.Add(_map[2, j + 3]);
                    diceNumbers.Add(_map[2, j + 5]);

                    for (int z = 0; z < 6; z++)
                    {
                        diceValue += Convert.ToInt32(Convert.ToString(diceNumbers[z]));
                    }

                    //Check for variable here, add variable parameter in dice object
                    string variableName = null;
                    if (_map[4, j] == '(' && _map[4, j + 1] == '#' && _map[4, j + 2] == ')')
                    {
                        //Console.Write("Found dice with variable: \n");

                        for (int y = 3 + j; y < arrayLenght; y++)
                        {
                            if (Char.IsLetter(_map[4, y]))
                                variableName += _map[4, y].ToString();
                            else
                            {
                                dices.Add(new Dice(globalIndex, diceValue, variableName, true));
                                globalIndex++;
                                break;
                            }
                        }
                        variableName = null;
                    }
                    else
                    {
                        dices.Add(new Dice(globalIndex, diceValue, null, false));
                        globalIndex++;
                    }

                    //Reset dice value after each dice has been counted
                    diceValue = 0;
                }
                #endregion


                #region VariableCallls
                string foundVariable = null;
                if (_map[2, j] == '(' && _map[2, j + 1] == '#' && _map[2, j + 2] == ')')
                {
                    for (int l = 3 + j; l < arrayLenght; l++)
                    {
                        if (Char.IsLetter(_map[2, l]))
                            foundVariable += _map[2, l].ToString();
                        else if (foundVariable != null)
                        {
                            //Int variable call - NOT INIT
                            //Console.Write("Found a variable call for: " + foundVariable);
                            for (int c = 0; c < dices.Count; c++)
                            {
                                if (foundVariable == dices[c].VariableName)
                                {
                                    dices.Add(new Dice(globalIndex, dices[c].Value, foundVariable, true));
                                    //Console.WriteLine("\nAdded variable: " + foundVariable);
                                    foundVariable = null;
                                    break;
                                }
                            }

                            //String variable call - NOT INIT
                            for (int s = 0; s < strings.Count; s++)
                            {
                                if (foundVariable == strings[s].Name)
                                {
                                    //
                                    //NEEDS TO BE HANDLED SOMEHOW?!?!?!
                                    //
                                    //Console.WriteLine("\nAdded string: " + foundVariable);
                                    foundVariable = null;
                                    break;
                                }
                            }
                        }
                    }
                }
                #endregion

                if (_map[2, j] == '(' && _map[2, j + 1] == '$' && _map[2, j + 2] == ')')
                {
                    string stringName = null;
                    string stringText = null;
                    for (int l = 3 + j; l < arrayLenght; l++)
                    {
                        if (Char.IsLetter(_map[2, l]))
                            stringName += _map[2, l].ToString();
                        else if ((_map[2, l] == '='))
                        {
                            for (int z = l + 2; z < arrayLenght; z++)
                            {
                                if (Char.IsLetter(_map[2, z]))
                                    stringText += _map[2, z].ToString();
                                else
                                {
                                    strings.Add(new StringVar(stringName, stringText, globalIndex));
                                    globalIndex++;

                                    //Console.WriteLine("String debug: Name: " + stringName + " Text: " + stringText);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            ScanForOperators(arrayLenght, _map);
        }

        static List<Operator> operators = new List<Operator>();

        static void ScanForOperators(int arrayLenght, char[,] _map)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < arrayLenght; j++)
                {
                    #region OperatorIndexes
                    if (i == 1)
                    {
                        //Make sure you don't check outside the grid
                        if (j < arrayLenght - 2)
                        {
                            if (dices.Count > 2)
                            {
                                if (_map[i, j] == 'o' && _map[i + 1, j - 1] == 'o' && _map[i + 1, j + 1] == 'o' && _map[i + 1, j] == '|')
                                    operators.Add(new Operator("/"));
                                else if (_map[i, j] == '/' && _map[i, j + 1] == '\\' && _map[i + 1, j] == '\\' && _map[i + 1, j + 1] == '/')
                                    operators.Add(new Operator("*"));
                                else if (_map[i, j] == '/' && _map[i, j + 1] == '\\' && _map[i, j + 2] == '/' && _map[i, j + 3] == '\\' && _map[i + 1, j] == '\\' && _map[i + 1, j + 3] == '/' && _map[i + 2, j + 1] == '\\' && _map[i + 2, j + 2] == '/')
                                    operators.Add(new Operator("+"));
                                else if (_map[i, j] == '/' && _map[i, j + 1] == '\\' && _map[i + 1, j - 1] == '/' && _map[i + 1, j + 2] == '\\' && _map[i + 2, j - 1] == '\\' && _map[i + 2, j] == '|' && _map[i + 2, j + 1] == '|' && _map[i + 2, j + 2] == '/')
                                    operators.Add(new Operator("-"));
                            }
                            else if (dices.Count <= 2)
                            {
                                if (_map[i, j] == 'o' && _map[i + 1, j - 1] == 'o' && _map[i + 1, j + 1] == 'o' && _map[i + 1, j] == '|')
                                    Clover(dices[0].Value, dices[1].Value);
                                else if (_map[i, j] == '/' && _map[i, j + 1] == '\\' && _map[i + 1, j] == '\\' && _map[i + 1, j + 1] == '/')
                                    Diamond(dices[0].Value, dices[1].Value);
                                else if (_map[i, j] == '/' && _map[i, j + 1] == '\\' && _map[i, j + 2] == '/' && _map[i, j + 3] == '\\' && _map[i + 1, j] == '\\' && _map[i + 1, j + 3] == '/' && _map[i + 2, j + 1] == '\\' && _map[i + 2, j + 2] == '/')
                                    Heart(dices[0].Value, dices[1].Value);
                                else if (_map[i, j] == '/' && _map[i, j + 1] == '\\' && _map[i + 1, j - 1] == '/' && _map[i + 1, j + 2] == '\\' && _map[i + 2, j - 1] == '\\' && _map[i + 2, j] == '|' && _map[i + 2, j + 1] == '|' && _map[i + 2, j + 2] == '/')
                                    Spade(dices[0].Value, dices[1].Value);
                            }
                        }
                    }
                    #endregion
                }
            }

            #region Error Messages
            //If there is one operator to little, give an error message and close the interpreter.
            if (dices.Count - operators.Count > 1)
                Errors.ErrorMessage(1);
            else if (dices.Count - operators.Count <= 0)
                Errors.ErrorMessage(2);
            #endregion

            if (dices.Count > 2)
            {
                MainBus mainBus = new MainBus(dices, strings, operators);
            }

            Console.ReadKey();
        }

        //Används inte än!!
        static void CheckForPrint(int arrayLenght, char[,] _map)
        {
            //Console.Write("PrintMethod");
            Console.ReadKey();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < arrayLenght; j++)
                {
                    if (i == 2 && j < arrayLenght - 5)
                    {
                        if (_map[i, j] == '[' && _map[i, j + 1] == '-' && _map[i, j + 2] == '$' && _map[i, j + 3] == '-' && _map[i, j] == ']')
                            Console.Write(result);
                    }
                }
            }
        }

        //Only used if there are two dices
        #region OperatorMethods
        static void Clover(int D1, int D2)
        {
            result = D1 / D2;
            //Console.Write("Performed a / operation, result is " + result);
        }

        static void Heart(int D1, int D2)
        {
            result = D1 + D2;
            //Console.Write("Performed a + operation, result is " + result);
        }

        static void Diamond(int D1, int D2)
        {
            result = D1 * D2;
            //Console.Write("Performed a * operation, result is " + result);
        }

        static void Spade(int D1, int D2)
        {
            result = D1 - D2;
            //Console.Write("Performed a - operation, result is " + result);
        }
        #endregion
    }
}

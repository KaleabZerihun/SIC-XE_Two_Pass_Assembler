/********************************************************************
*** NAME : Kaleab Zerihun
*** CLASS : CSc 354
*** ASSIGNMENT : Assignment 
*** DUE DATE : 12-4-24
*** INSTRUCTOR : Hamer
*********************************************************************
*** DESCRIPTION : Create a location counter column to the sci/xe code provided and create the literal and symbol table.****
********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Markup;
using System.Xml.Linq;
using System.Linq.Expressions;
using System.Diagnostics.CodeAnalysis;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Collections;
using System.Reflection.Emit;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using Zerihun4;

namespace Zerihun4
{
    public class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> opcodeDictionary = ReadOPCODE();
            Pass2 passtwo = new Pass2();
            string fileName = string.Empty;
            if (opcodeDictionary != null)
            {

                // If the expression file name has not been provided in the command line ask the user for a expression file name
                do
                {
                    if (args.Length < 1)
                    {
                        Console.Write($"Please enter the program file name: ");
                        fileName = Console.ReadLine();
                    }
                    // If the experssion file name has been provided in the command the pass it to the searchSymbol function 
                    else
                    {
                        fileName = args[0];
                    }
                } while (String.IsNullOrEmpty(fileName));

                ReadProgram(opcodeDictionary, fileName);
                passtwo.PassTwo(fileName, opcodeDictionary);
            }
            else
            {
                Console.WriteLine("ERROR - The opcode file is not provided");
                Environment.Exit(0);
            }



        }

        static Dictionary<string, string> ReadOPCODE()
        {
            string mnemonic = string.Empty;
            string opcode = string.Empty;
            string format = string.Empty;
            Dictionary<string, string> opcodeDictionary = new Dictionary<string, string>();
            string fileName = Path.Combine(Directory.GetCurrentDirectory(), "opcodes.DAT");
            if (File.Exists(fileName))
            {
                //Read the file and for each line
                string[] lines = File.ReadAllLines(fileName);
                foreach (string line in lines)
                {
                    // get the symbol, value and rflag and remove all white spaces befor and after it
                    string[] fields = line.Split(' ');
                    opcodeDictionary.Add(fields[0], fields[1] + " " + fields[2]);
                }
                return opcodeDictionary;

            }
            else
            {
                Console.WriteLine("ERROR - The opcode file is not found.");
                Environment.Exit(0);
                return opcodeDictionary;
            }
        }

        static void ReadProgram(Dictionary<string, string> opcodeDictionary, string fileName)
        {
            string fullFileName = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            string[] splitfileName = fileName.Split('.');
            string writeFileName = Path.Combine(Directory.GetCurrentDirectory(), splitfileName[0] + ".int");
            Pass1 pass1 = new Pass1();
            if (File.Exists(fullFileName))
            {
                //Read the file and for each line
                string[] lines = File.ReadAllLines(fileName);
                string trimmedLine = string.Empty;
                List<string> errorOpcode = new List<string>();
                bool invalidOPcode = false;
                List<List<string>> passone = new();
                string locationCounter = string.Empty;
                string opcodeValue = string.Empty;
                string opcodeFormat = string.Empty;
                string nextLocationCounter = string.Empty;
                Node? root = null;
                string symbol = string.Empty;
                string RFLAG = "true";
                string value = string.Empty;
                List<Literal> literals = new List<Literal>();
                bool duplicatedLiteral = false;
                int literalLength = 0;
                string hexexpression = string.Empty;
                int addressCounter = 1;
                ExecuteExpression executeExpression = new ExecuteExpression();
                //for every line in the asm file
                foreach (string line in lines)
                {
                    string label = string.Empty;
                    string opcode = string.Empty;
                    string operand = string.Empty;
                    string modifiedLine = line.Replace("\t", "    ");
                    string comment = string.Empty;
                    trimmedLine = modifiedLine.Trim();
                    // get the symbol, value and rflag and remove all white spaces befor and after it
                    //if ther is comment in the file
                    if (trimmedLine.Contains('.'))
                    {
                        string[] wholeLine = trimmedLine.Split(".");
                        trimmedLine = wholeLine[0];
                        comment = "." + wholeLine[1];
                    }
                    //split the line and figureout the first secodn and third columns
                    string[] fields = trimmedLine.Split(' ');
                    string elementOne = string.Empty;
                    string elementTwo = string.Empty;
                    string elementThree = string.Empty;
                    foreach (var element in fields)
                    {
                        if (element != "")
                        {
                            if (elementOne == "EXTDEF" || elementOne == "EXTREF")
                            {
                                elementTwo = elementOne;
                            }
                            if (elementOne == "")
                            {
                                elementOne = element;
                            }
                            else if (elementOne != "" && elementTwo == "")
                            {
                                
                                elementTwo = element;
                            }
                            else if (elementOne != "" && elementTwo != "")
                            {
                                elementThree += element;
                            }

                        }
                    }


                    
                    // if label opcode and operan
                    if (!string.IsNullOrEmpty(elementOne) && !string.IsNullOrEmpty(elementTwo) && !string.IsNullOrEmpty(elementThree))
                    {
                        if (elementOne == "EXTDEF" || elementOne == "EXTREF")
                        {
                            elementOne = "";
                        }
                        label = elementOne;
                        opcode = elementTwo;
                        operand = elementThree;

                        if (!opcodeDictionary.ContainsKey(opcode) && !opcode.Contains('+') && !(elementTwo == "BASE" || elementTwo == "START" || elementTwo == "END" || elementTwo == "BYTE" || elementTwo == "WORD" || elementTwo == "RESB" || elementTwo == "RESW" || elementTwo == "EQU" || elementTwo == "EXTDEF" || elementTwo == "EXTREF")
)
                        {
                            label = string.Empty;
                            opcode = string.Empty;
                            operand = string.Empty;
                            invalidOPcode = true;
                            errorOpcode.Add($"{elementOne} {elementTwo} {elementThree}");
                        }

                    }
                    //if label opcode or opcode operand
                    else if (!string.IsNullOrEmpty(elementOne) && !string.IsNullOrEmpty(elementTwo) && string.IsNullOrEmpty(elementThree))
                    {
                        //if it is opcode operand
                        if (opcodeDictionary.ContainsKey(elementOne))
                        {
                            label = string.Empty;
                            opcode = elementOne;
                            operand = elementTwo;
                        }
                        //if it is opcdoe and operand
                        else if (elementOne.Contains('+') && opcodeDictionary.ContainsKey(elementOne.Substring(1)))
                        {
                            label = string.Empty;
                            opcode = elementOne;
                            operand = elementTwo;
                        }
                        //if label and opcode
                        else if (opcodeDictionary.ContainsKey(elementTwo))
                        {
                            label = elementOne;
                            opcode = elementTwo;
                            operand = string.Empty;
                        }
                        //if it is label and opcdoe
                        else if (elementTwo.Contains('+') && opcodeDictionary.ContainsKey(elementTwo.Substring(1)))
                        {
                            label = elementOne;
                            opcode = elementTwo;
                            operand = string.Empty;
                        }
                        // if the opcode is either base start or end
                        else if (elementOne == "BASE" || elementOne == "START" || elementOne == "END" || elementOne == "BYTE" || elementOne == "WORD" || elementOne == "RESB" || elementOne == "RESW" || elementOne == "EQU" || elementOne == "EXTDEF" || elementOne == "EXTREF")
                        {
                            label = string.Empty;
                            opcode = elementOne;
                            operand = elementTwo;
                        }
                        // if it is label and opcode
                        else if (elementTwo == "BASE" || elementTwo == "START" || elementTwo == "END" || elementTwo == "BYTE" || elementTwo == "WORD" || elementTwo == "RESB" || elementTwo == "RESW" || elementTwo == "EQU" || elementTwo == "EXTDEF" || elementTwo == "EXTREF")
                        {
                            label = elementOne;
                            opcode = elementTwo;
                            operand = string.Empty;
                        }
                        //if it is just and empty line
                        else
                        {
                            label = string.Empty;
                            opcode = string.Empty;
                            operand = string.Empty;
                            invalidOPcode = true;
                            errorOpcode.Add($"{elementOne} {elementTwo} {elementThree}");
                        }
                    }
                    //if opcode
                    else if (!string.IsNullOrEmpty(elementOne) && string.IsNullOrEmpty(elementTwo) && string.IsNullOrEmpty(elementThree))
                    {
                        opcode = elementOne;
                        label = string.Empty;
                        operand = string.Empty;
                        //check if the opcode is valid
                        if (!opcodeDictionary.ContainsKey(opcode) && opcode != "END")
                        {
                            label = string.Empty;
                            opcode = string.Empty;
                            operand = string.Empty;
                            invalidOPcode = true;
                            errorOpcode.Add($"{elementOne} {elementTwo} {elementThree}");
                        }
                    }
                    //if opcode is staet check for # 
                    if (opcode == "START" && locationCounter == "")
                    {
                        if (operand.Contains('#'))
                        {
                            operand = operand.Trim('#');
                            locationCounter = operand;
                        }
                        else
                        {
                            locationCounter = operand;
                        }
                    }

                    if (!(string.IsNullOrEmpty(label) && string.IsNullOrEmpty(opcode) && string.IsNullOrEmpty(operand)))
                    {
                        //if the operand is a literal
                        if (!string.IsNullOrEmpty(operand) && operand.ToCharArray()[0] == '=')
                        {
                            hexexpression = string.Empty;
                            if (operand.ToCharArray()[2] == 'X' || operand.ToCharArray()[2] == 'x')
                            {
                                if (operand.Substring(3).Length % 2 != 0)
                                {
                                    Console.WriteLine("{0,-20} {1,-7}", operand, "ERROR -  A hex Litteral should be even");
                                    Environment.Exit(0);
                                }
                                else
                                {
                                    bool isHex = Regex.IsMatch(operand.Substring(3), @"\A\b[0-9a-fA-F]+\b\Z");
                                    if (isHex)
                                    {
                                        //get the lireral length and the hexrep
                                        hexexpression = operand.Substring(3);
                                        literalLength = operand.Substring(3).Length / 2;
                                    }
                                    else
                                    {
                                        Console.WriteLine("{0,-20} {1,-7}", operand, "ERROR -  provided value is not hex");
                                        Environment.Exit(0);
                                    }


                                }

                            }
                            //if char
                            else if (operand.ToCharArray()[2] == 'C' || operand.ToCharArray()[2] == 'c')
                            {
                                // Convert each character to its hexadecimal equivalent
                                for (int j = 3; j < operand.Length; j++)
                                {
                                    //get the lireral length and the hexrep
                                    int val = Convert.ToInt32(operand.ToCharArray()[j]);
                                    hexexpression += String.Format("{0:X2}", val);
                                    literalLength = operand.Substring(3).Length;
                                }
                            }
                            else
                            {
                                Console.WriteLine("{0,-20} {1,-7}", operand, "ERROR -  Litteral not valid");
                                Environment.Exit(0);
                            }

                            //look for duplicate literal
                            foreach (Literal literal in literals)
                            {
                                if (literal.NAME == operand)
                                {
                                    duplicatedLiteral = true;
                                    break;
                                }
                                else
                                {
                                    duplicatedLiteral = false;
                                }
                            }

                            //calculate literal length
                            //if there is no duplicate literal
                            if (!duplicatedLiteral)
                            {
                                //add into the literal table
                                literals.Add(new Literal { NAME = operand, VALUE = hexexpression, LENGTH = literalLength, ADDRESS = addressCounter });
                                addressCounter++;
                            }

                        }
                        passone.Add(pass1.PassOne(label, opcode, operand, comment, opcodeDictionary, ref locationCounter, ref nextLocationCounter));
                        //insert the label into the symbol table
                        if (!string.IsNullOrEmpty(label))
                        {
                            bool validSymbol = ValidateSymbol(label);
                            string symbolValue = string.Empty;

                            // If the symbol is valid
                            if (validSymbol)
                            {
                                //if the symbol has more than 4 chars get the first 4
                                if (label.Length > 4)
                                {
                                    label = label.Substring(0, 4);
                                }
                                //if the last char has a Colon remove it
                                if (label.EndsWith(":"))
                                {
                                    // Remove the last character
                                    label = label.Substring(0, label.Length - 1);
                                }
                                if (opcode == "EQU" && operand != "*")
                                {
                                    executeExpression.executeexpression(ref root, operand, label);
                                }
                                else
                                {
                                    //if the BST is empty, intanciate it and insert to the BST
                                    if (root == null)
                                    {
                                        root = new Node(label.ToUpper(), Int32.Parse(passone[passone.Count() - 1][0]), bool.Parse(RFLAG));
                                        continue;
                                    }
                                    // Insert to BST
                                    BST.Insert(root, label.ToUpper(), Int32.Parse(passone[passone.Count() - 1][0]), bool.Parse(RFLAG));
                                }
                            }
                        }



                    }
                }



                if (errorOpcode.Count != 0)
                {
                    Console.WriteLine("\n-----------------------------------------INVALID OPCODE---------------------------------------------------------");
                    //display invalid opcodes
                    foreach (string list in errorOpcode)
                    {
                        Console.WriteLine($"ERROR - Invalid opcode in \"{list.Trim()}\"");

                    }
                    Environment.Exit(0);
                }

                //Console.WriteLine("-----------------------------------------PASS 1---------------------------------------------------------");
                //Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15} {4,-15} {5,-15}", "Line", "LC", "Label", "Opcode", "Operand", "Comment");
                //print out pass one
                int i = 1;
                //foreach (List<string> list in passone)
                //{
                //    Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15} {4,-15} {5,-15}", i, Int32.Parse(list[0]).ToString("X5"), list[1], list[2], list[3], list[4]);
                //    i++;
                //}
                string lastLocationCounter = string.Empty;

                // Assuming passone[passone.Count - 1][0] is a char or int
                lastLocationCounter = passone[passone.Count - 1][0].ToString();
                //add the literals in to the passone list
                if (literals.Count != 0)
                {


                    foreach (var list in literals)
                    {
                        list.ADDRESS = Int32.Parse(lastLocationCounter);
                        // Parse lastLocationCounter as an integer, format it as hex, and print
                       // Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15}", i, int.Parse(lastLocationCounter).ToString("X5"), "*", list.NAME);

                        // Update lastLocationCounter by adding list.LENGTH, converting back to a string
                        lastLocationCounter = (int.Parse(lastLocationCounter) + list.LENGTH).ToString();
                        i++; // Increment i if necessary
                    }

                }
                string programLength = (Int32.Parse(lastLocationCounter) + Int32.Parse(passone[0][0])).ToString("X5");
                //print out total memory count
                //Console.WriteLine($"Program Length: {programLength}");
                //Console.WriteLine("\n-----------------------------------------SYMBOL TABLE---------------------------------------------------------");
                //Console.WriteLine("{0,-10} {1,-10} {2,-10} {3,-10} {4,-10}", "Symbol", "Value", "RFALG", "IFLAG", "MFALG");
                //BST.InorderTraversal(root);

                //Console.WriteLine("\n-----------------------------------------Literal TABLE---------------------------------------------------------");
                //Console.WriteLine("{0,-10} {1,-10} {2,-10} {3,-20}", "Literal", "Length", "VALUE", "ADDRESS");
                //foreach (var list in literals)
                //{
                //    Console.WriteLine("{0,-10} {1,-10} {2,-10} {3,-20}", list.NAME, list.LENGTH, list.VALUE, list.ADDRESS.ToString("X5"));
                //}

                writeToFile(root, literals, passone, writeFileName, errorOpcode);

            }
        }
        static bool ValidateSymbol(string symbol)
        {
            bool returnVal = true;
            //if symbol is not empty
            if (!String.IsNullOrEmpty(symbol))
            {
                //if the symbol is greater than 10 display a detailed error message
                if (symbol.Length > 10)
                {
                    Console.WriteLine($"ERROR - symbol should not be more than 10 characters: {symbol}");
                    returnVal = false;
                    Environment.Exit(0);
                }
                //check if first character is number and display a detailed error message
                if (Char.IsDigit(symbol.ToCharArray()[0]))
                {
                    Console.WriteLine($"ERROR - symbols should start with a letter: {symbol}");
                    returnVal = false;
                    Environment.Exit(0);
                }
                //if the last char is not a colon
                if (symbol.ToCharArray()[symbol.Length - 1] != ':')
                {
                    Console.WriteLine($"ERROR - symbols should have a colon at the end: {symbol}");
                    returnVal = false;
                    Environment.Exit(0);
                }
                //check if the symbol has any other char other than _ in it and display a detailed error message
                foreach (char c in symbol)
                {
                    if (!(Char.IsLetterOrDigit(c) || c == '_' || c == ':'))
                    {
                        Console.WriteLine($"ERROR - Symbols should contain letters, digits and underscore: {symbol}");
                        returnVal = false;
                        Environment.Exit(0);
                    }
                }
            }


            return returnVal;
        }

        static void writeToFile(Node? root, List<Literal> literals, List<List<string>> passone, string writeFileName, List<string> errorOpcode)
        {
            try
            {
                // Use StreamWriter to open the file for writing
                using (StreamWriter writer = new StreamWriter(new FileStream(writeFileName, FileMode.Create, FileAccess.Write)))
                {
                    writer.AutoFlush = true;
                    writer.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15} {4,-15} {5,-15}", "Line", "LC", "Label", "Opcode", "Operand", "Comment");
                    //print out pass one
                    int i = 1;
                    foreach (List<string> list in passone)
                    {
                        writer.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15} {4,-15} {5,-15}", i, Int32.Parse(list[0]).ToString("X5"), list[1], list[2], list[3], list[4]);
                        i++;
                    }
                    string lastLocationCounter = string.Empty;

                    // Assuming passone[passone.Count - 1][0] is a char or int
                    lastLocationCounter = passone[passone.Count - 1][0].ToString();
                    //add the literals in to the passone list
                    if (literals.Count != 0)
                    {

                        foreach (var list in literals)
                        {
                            list.ADDRESS = Int32.Parse(lastLocationCounter);
                            // Parse lastLocationCounter as an integer, format it as hex, and print
                            writer.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15}", i, int.Parse(lastLocationCounter).ToString("X5"), "*", list.NAME);

                            // Update lastLocationCounter by adding list.LENGTH, converting back to a string
                            lastLocationCounter = (int.Parse(lastLocationCounter) + list.LENGTH).ToString();
                            i++; // Increment i if necessary
                        }

                    }
                    string programLength = (Int32.Parse(lastLocationCounter) + Int32.Parse(passone[0][0])).ToString("X5");
                    //print out total memory count
                    writer.WriteLine($"Program Length: {programLength}");
                    writer.WriteLine("\nSYMBOL TABLE:");
                    writer.WriteLine("{0,-10} {1,-10} {2,-10} {3,-10} {4,-10}", "Symbol", "Value", "RFALG", "IFLAG", "MFALG");
                    BST.InorderTraversalFile(root, writer);

                    writer.WriteLine("\nLiteral TABLE:");
                    writer.WriteLine("{0,-10} {1,-10} {2,-10} {3,-20}", "Literal", "Length", "VALUE", "ADDRESS");
                    foreach (var list in literals)
                    {
                        writer.WriteLine("{0,-10} {1,-10} {2,-10} {3,-20}", list.NAME, list.LENGTH, list.VALUE, list.ADDRESS.ToString("X5"));
                    }
                }

                Console.WriteLine("File created and lines written successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing the output to the file: {ex.Message}");
                Environment.Exit(0);
            }
        }
    }
}
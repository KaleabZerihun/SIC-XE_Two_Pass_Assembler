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
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Zerihun4;
using static System.Net.Mime.MediaTypeNames;

namespace Zerihun4
{
    class ExecuteExpression
    {
        /********************************************************************
        *** METHOD: ExecuteExpression
        *********************************************************************
        *** DESCRIPTION : This function reads a file and then validate it's values
        *and then evaluate each line and then prints the approprate output
        *** INPUT ARGS : Node? root, string file
        *** OUTPUT ARGS : N/A
        *** IN/OUT ARGS : N/A
        *** RETURN : void
        ********************************************************************/
        public string executeexpression(ref Node? root, string expression, string label)
        {
            // get the search file's full path
            Node? foundexpression = null;
            var hexexpression = string.Empty;
            int expresssionValue = 1;
            string expressionRelocate = string.Empty;
            bool NBit = false;
            bool IBit = false;
            bool xBit = false;
            bool nonexpression = false;
            string returnstring = string.Empty;
            // Set the number of lines per page


            if (ValidateExpression(expression))
            {
                //if expression is indexed
                if (expression.Contains(','))
                {
                    //assign xBit to true
                    //if expression has additition in it
                    if (expression.Contains('+'))
                    {
                        //split the thing with + and then evaluate it
                        string[] addedValues = expression.Split('+');
                        //remove whate spaces
                        addedValues[0] = addedValues[0].Trim();
                        addedValues[1] = addedValues[1].Trim();

                        //split the second statment with ,
                        string[] addedvalues23 = addedValues[1].Split(",");
                        //remove white spaces
                        addedvalues23[0] = addedvalues23[0].Trim();
                        addedvalues23[1] = addedvalues23[1].Trim();
                        //op1+op2,x
                        if (addedValues[0].ToCharArray()[0] != '#' && addedValues[0].ToCharArray()[0] != '@' &&
                            addedvalues23[0].ToCharArray()[0] != '#' && addedvalues23[0].ToCharArray()[0] != '@')
                        {
                            if (addedvalues23[0].All(char.IsDigit))
                            {
                                foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].ToUpper());
                                if ((foundexpression != null))
                                {
                                    //calculate the value
                                    expresssionValue = foundexpression.VALUE + Int32.Parse(addedvalues23[0]);
                                    //calculate relocatable
                                    expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, false, "+", 2);
                                    //if we encounter error return to the next itteration
                                    if (expressionRelocate == "ERROR")
                                    {
                                        Environment.Exit(0);
                                    }
                                }
                            }
                            else if (addedValues[0].All(char.IsDigit))
                            {
                                Node? foundexpression1 = addedvalues23[0].Length > 4 ? BST.Search(root, addedvalues23[0].ToUpper().Substring(0, 4)) : BST.Search(root, addedvalues23[0].ToUpper());
                                if (foundexpression1 != null)
                                {
                                    //calculate the value
                                    expresssionValue = Int32.Parse(addedValues[0]) + foundexpression1.VALUE;
                                    //calculate relocatable
                                    expressionRelocate = CalculateRelocate(expression, false, foundexpression1.RFLAG, "+", 2);
                                    //if we encounter error return to the next itteration
                                    if (expressionRelocate == "ERROR")
                                    {
                                        Environment.Exit(0);
                                    }
                                }
                                if (foundexpression1 == null)
                                {
                                    Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                                    Environment.Exit(0);
                                }
                            }
                            else if (!addedValues[0].All(char.IsDigit) && !addedvalues23[0].All(char.IsDigit))
                            {
                                foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].ToUpper());
                                Node? foundexpression1 = addedvalues23[0].Length > 4 ? BST.Search(root, addedvalues23[0].ToUpper().Substring(0, 4)) : BST.Search(root, addedvalues23[0].ToUpper());
                                if (foundexpression1 != null && (foundexpression != null))
                                {
                                    //calculate the value
                                    expresssionValue = foundexpression.VALUE + foundexpression1.VALUE;
                                    //calculate relocatable
                                    expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, foundexpression1.RFLAG, "+", 2);
                                    //if we encounter error return to the next itteration
                                    if (expressionRelocate == "ERROR")
                                    {
                                        Environment.Exit(0);
                                    }
                                }
                                if (foundexpression1 == null)
                                {
                                    Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                                    Environment.Exit(0);
                                }
                            }
                            //find op1 and op2

                        }
                        else
                        {
                            //if we encounter error return to the next itteration
                            Console.WriteLine("{0,-20} {1,-7}", expression, " ERROR - You can not have indexed and Immidate/indirect at the same time");
                            Environment.Exit(0);
                        }
                    }
                    //if expression has -
                    else if (expression.Contains('-'))
                    {
                        //split the thing with - and the evaluate it
                        string[] addedValues = expression.Split('-');
                        //remove white spacces
                        addedValues[0] = addedValues[0].Trim();
                        addedValues[1] = addedValues[1].Trim();
                        addedValues[0] = addedValues[0].All(char.IsDigit) ? "#" + addedValues[0].Trim() : addedValues[0].Trim();

                        string[] addedvalues23 = addedValues[1].Split(",");
                        addedvalues23[0] = addedvalues23[0].Trim();
                        addedvalues23[1] = addedvalues23[1].Trim();
                        addedvalues23[0] = addedvalues23[0].All(char.IsDigit) ? "#" + addedvalues23[0].Trim() : addedvalues23[0].Trim();


                        if (addedValues[0].ToCharArray()[0] != '#' && addedValues[0].ToCharArray()[0] != '@' &&
                            addedvalues23[0].ToCharArray()[0] != '#' && addedvalues23[0].ToCharArray()[0] != '@')
                        {
                            if (addedvalues23[0].All(char.IsDigit))
                            {
                                foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].ToUpper());
                                if ((foundexpression != null))
                                {
                                    //calculate the value
                                    expresssionValue = foundexpression.VALUE - Int32.Parse(addedvalues23[0]);
                                    //calculate relocatable
                                    expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, false, "-", 2);
                                    //if we encounter error return to the next itteration
                                    if (expressionRelocate == "ERROR")
                                    {
                                        Environment.Exit(0);
                                    }
                                }
                            }
                            else if (addedValues[0].All(char.IsDigit))
                            {
                                Node? foundexpression1 = addedValues[0].Length > 4 ? BST.Search(root, addedvalues23[0].ToUpper().Substring(0, 4)) : BST.Search(root, addedvalues23[0].ToUpper());
                                if (foundexpression1 != null)
                                {
                                    //calculate the value
                                    expresssionValue = Int32.Parse(addedvalues23[0]) - foundexpression1.VALUE;
                                    //calculate relocatable
                                    expressionRelocate = CalculateRelocate(expression, false, foundexpression1.RFLAG, "-", 2);
                                    //if we encounter error return to the next itteration
                                    if (expressionRelocate == "ERROR")
                                    {
                                        Environment.Exit(0);
                                    }
                                }
                                if (foundexpression1 == null)
                                {
                                    Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                                    Environment.Exit(0);
                                }
                            }
                            else if (!addedValues[0].All(char.IsDigit) && !addedvalues23[0].All(char.IsDigit))
                            {
                                foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].ToUpper());
                                Node? foundexpression1 = addedvalues23[0].Length > 4 ? BST.Search(root, addedvalues23[0].ToUpper().Substring(0, 4)) : BST.Search(root, addedvalues23[0].ToUpper());
                                if (foundexpression1 != null && (foundexpression != null))
                                {
                                    //calculate the value
                                    expresssionValue = foundexpression.VALUE - foundexpression1.VALUE;
                                    //calculate relocatable
                                    expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, foundexpression1.RFLAG, "-", 2);
                                    //if we encounter error return to the next itteration
                                    if (expressionRelocate == "ERROR")
                                    {
                                        Environment.Exit(0);
                                    }
                                }
                                if (foundexpression1 == null)
                                {
                                    Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                                    Environment.Exit(0);
                                }
                            }
                        }
                        else
                        {
                            // if we encounter error return
                            Console.WriteLine("{0,-20} {1,-7}", expression, " ERROR - You can not have indexed and Immidate/indirect at the same time");
                            Environment.Exit(0);
                        }
                    }
                    //if it does not have + or -
                    else
                    {
                        //split op1 and x
                        string[] values = expression.Split(",");
                        //remove white spaces
                        values[0] = values[0].Trim();
                        values[1] = values[1].Trim();
                        //if op1 is not indexed or immediate
                        if (values[0].ToCharArray()[0] != '#' && values[0].ToCharArray()[0] != '@')
                        {
                            //find op1
                            foundexpression = values[0].Length > 4 ? BST.Search(root, values[0].ToUpper().Substring(0, 4)) : BST.Search(root, values[0].ToUpper());
                            if (foundexpression != null)
                            {
                                //evaluate expression value
                                expresssionValue = foundexpression.VALUE;
                                //calculate relocatable
                                expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, false, "-", 1);
                                //if we encounter error return
                                if (expressionRelocate == "ERROR")
                                {
                                    Environment.Exit(0);
                                }
                            }
                        }
                        else
                        {
                            //if we encounter error contune
                            Console.WriteLine("{0,-20} {1,-7}", expression, " ERROR - You can not have indexed and Immidate/indirect at the same time");
                            Environment.Exit(0);
                        }
                    }
                }
                //if expression is neither indexed or litteral
                else
                {
                    //if the expression is immediate
                    if (expression.ToCharArray()[0] == '#')
                    {
                        //if expression contains +
                        if (expression.Contains('+'))
                        {
                            //split the thing with + and then evaluate it
                            string[] addedValues = expression.Split('+');
                            //remove white spaces
                            addedValues[0] = addedValues[0].Trim();
                            addedValues[1] = addedValues[1].Trim();
                            addedValues[0] = addedValues[0].All(char.IsDigit) ? "#" + addedValues[0].Trim() : addedValues[0].Trim();
                            addedValues[1] = addedValues[1].All(char.IsDigit) ? "#" + addedValues[1].Trim() : addedValues[1].Trim();
                            //if #pink + red
                            if (addedValues[0].ToCharArray()[0] == '#' && addedValues[1].ToCharArray()[0] != '#')
                            {
                                //if second value = @
                                if (addedValues[1].ToCharArray()[0] == '@')
                                {
                                    //get op1 and op2
                                    foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].Substring(1).ToUpper());
                                    Node? foundexpression1 = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].Substring(1).ToUpper());
                                    if (foundexpression != null && foundexpression1 != null)
                                    {
                                        //evaluate expression value ena relocatable
                                        expresssionValue = foundexpression.VALUE + foundexpression1.VALUE;
                                        expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, foundexpression1.RFLAG, "+", 2);
                                        //if we encounter error return
                                        if (expressionRelocate == "ERROR")
                                        {
                                            Environment.Exit(0);
                                        }
                                    }
                                    if (foundexpression1 == null)
                                    {
                                        Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                                        Environment.Exit(0);
                                    }
                                }
                                //if #2 + PINK
                                else if ((addedValues[0].Substring(1).All(char.IsDigit)))
                                {
                                    //get op2
                                    foundexpression = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].ToUpper());
                                    if (foundexpression != null)
                                    {
                                        //calculate value and relocatable
                                        expresssionValue = Int32.Parse(addedValues[0].Substring(1)) + foundexpression.VALUE;
                                        expressionRelocate = CalculateRelocate(expression, false, foundexpression.RFLAG, "+", 2);
                                        //if we encounter error return
                                        if (expressionRelocate == "ERROR")
                                        {
                                            Environment.Exit(0);
                                        }
                                    }
                                }
                                //if #pink + red
                                else
                                {
                                    //find op1 and op2
                                    foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].Substring(1).ToUpper());
                                    Node? foundexpression1 = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].ToUpper());
                                    if (foundexpression != null && foundexpression1 != null)
                                    {
                                        //evaluate the value and erlocatable
                                        expresssionValue = foundexpression.VALUE + foundexpression1.VALUE;
                                        expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, foundexpression1.RFLAG, "+", 2);
                                        //if we encounter any error return
                                        if (expressionRelocate == "ERROR")
                                        {
                                            Environment.Exit(0);
                                        }
                                    }
                                    if (foundexpression1 == null)
                                    {
                                        Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                                        Environment.Exit(0);
                                    }
                                }
                            }
                            //if #pink + #pink
                            else if (addedValues[0].ToCharArray()[0] == '#' && addedValues[1].ToCharArray()[0] == '#')
                            {
                                //if #2 + #2
                                if (addedValues[1].Substring(1).All(char.IsDigit) && addedValues[0].Substring(1).All(char.IsDigit))
                                {
                                    //evaluate expression and relocatable
                                    expresssionValue = Int32.Parse(addedValues[0].Substring(1)) + Int32.Parse(addedValues[1].Substring(1));
                                    expressionRelocate = "ABSOLUTE";
                                    nonexpression = true;
                                }
                                // if #pink + #2
                                else if ((!addedValues[0].Substring(0).All(char.IsDigit)) && addedValues[1].Substring(1).All(char.IsDigit))
                                {
                                    //find op1
                                    foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].Substring(1).ToUpper());
                                    if (foundexpression != null)
                                    {
                                        //calculate value and relocatable
                                        expresssionValue = foundexpression.VALUE + Int32.Parse(addedValues[1].Substring(1));
                                        expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, false, "+", 2);
                                        //if we encounter error return
                                        if (expressionRelocate == "ERROR")
                                        {
                                            Environment.Exit(0);
                                        }
                                    }

                                }
                                //if #2 + #pink 
                                else if (addedValues[0].Substring(1).All(char.IsDigit) && (!addedValues[1].Substring(1).All(char.IsDigit)))
                                {
                                    //find op2
                                    foundexpression = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].Substring(1).ToUpper());
                                    if (foundexpression != null)
                                    {
                                        //calculate value and relocatable
                                        expresssionValue = Int32.Parse(addedValues[0].Substring(1)) + foundexpression.VALUE;
                                        expressionRelocate = CalculateRelocate(expression, false, foundexpression.RFLAG, "+", 2);
                                        //if we encounter error return
                                        if (expressionRelocate == "ERROR")
                                        {
                                            Environment.Exit(0);
                                        }
                                    }
                                }
                                //if #2 + @pink
                                else if (addedValues[0].Substring(1).All(char.IsDigit) && addedValues[1].ToCharArray()[0] == '@')
                                {
                                    //find op2
                                    foundexpression = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].Substring(1).ToUpper());
                                    if (foundexpression != null)
                                    {
                                        //evaluate expression value and relocatable
                                        expresssionValue = Int32.Parse(addedValues[0].Substring(1)) + foundexpression.VALUE;
                                        expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, false, "+", 2);
                                        //if we encounter any error return
                                        if (expressionRelocate == "ERROR")
                                        {
                                            Environment.Exit(0);
                                        }
                                    }
                                }
                                // if #pink + #pink
                                else
                                {
                                    //find op1 and op2
                                    foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].Substring(1).ToUpper());
                                    Node? foundexpression1 = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].Substring(1).ToUpper());
                                    if (foundexpression != null && foundexpression1 != null)
                                    {
                                        //evaluate expression value and relocatable
                                        expresssionValue = foundexpression.VALUE + foundexpression1.VALUE;
                                        expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, foundexpression1.RFLAG, "+", 2);
                                        //if we encounter any error return
                                        if (expressionRelocate == "ERROR")
                                        {
                                            Environment.Exit(0);
                                        }
                                    }
                                    if (foundexpression1 == null)
                                    {
                                        Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                                        Environment.Exit(0);
                                    }
                                }


                            }


                        }
                        else if (expression.Contains('-'))
                        {
                            //split the thing with - and then evaluate it
                            string[] addedValues = expression.Split('-');
                            //remove white spaces
                            addedValues[0] = addedValues[0].Trim();
                            addedValues[1] = addedValues[1].Trim();
                            addedValues[0] = addedValues[0].All(char.IsDigit) ? "#" + addedValues[0].Trim() : addedValues[0].Trim();
                            addedValues[1] = addedValues[1].All(char.IsDigit) ? "#" + addedValues[1].Trim() : addedValues[1].Trim();
                            //if #pink - red
                            if (addedValues[0].ToCharArray()[0] == '#' && addedValues[1].ToCharArray()[0] != '#')
                            {
                                //if second value = @
                                if (addedValues[1].ToCharArray()[0] == '@')
                                {
                                    //find op1 and op2
                                    foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].Substring(1).ToUpper());
                                    Node? foundexpression1 = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].Substring(1).ToUpper());
                                    if (foundexpression != null && foundexpression1 != null)
                                    {
                                        //evaluate value and relocatable
                                        expresssionValue = foundexpression.VALUE - foundexpression1.VALUE;
                                        expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, foundexpression1.RFLAG, "-", 2);
                                        //if we encounter any error return
                                        if (expressionRelocate == "ERROR")
                                        {
                                            Environment.Exit(0);
                                        }
                                    }
                                    if (foundexpression1 == null)
                                    {
                                        Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                                        Environment.Exit(0);
                                    }
                                }
                                //if #pink - red
                                else
                                {
                                    //find op1 and op2
                                    foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].Substring(1).ToUpper());
                                    Node? foundexpression1 = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].ToUpper());
                                    if (foundexpression != null && foundexpression1 != null)
                                    {
                                        //evaluate value and relocatable
                                        expresssionValue = foundexpression.VALUE - foundexpression1.VALUE;
                                        expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, foundexpression1.RFLAG, "-", 2);
                                        //if we encounter any error return
                                        if (expressionRelocate == "ERROR")
                                        {
                                            return "";
                                        }
                                    }
                                    if (foundexpression1 == null)
                                    {
                                        Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                                        Environment.Exit(0);
                                    }

                                }

                            }
                            //if #pink - #pink
                            else if (addedValues[0].ToCharArray()[0] == '#' && addedValues[1].ToCharArray()[0] == '#')
                            {
                                //if #2 - #2
                                if (addedValues[1].Substring(1).All(char.IsDigit) && addedValues[0].Substring(1).All(char.IsDigit))
                                {
                                    //evaluate value and relocatable
                                    expresssionValue = Int32.Parse(addedValues[0].Substring(1)) - Int32.Parse(addedValues[1].Substring(1));
                                    expressionRelocate = "ABSOLUTE";
                                    nonexpression = true;
                                }
                                // if #pink - #2
                                else if ((!addedValues[1].Substring(0).All(char.IsDigit)) && addedValues[1].Substring(1).All(char.IsDigit))
                                {
                                    //find op1
                                    foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].Substring(1).ToUpper());
                                    if (foundexpression != null)
                                    {
                                        //evaluate value and relocatable
                                        expresssionValue = foundexpression.VALUE - Int32.Parse(addedValues[1].Substring(1));
                                        expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, false, "-", 2);
                                        if (expressionRelocate == "ERROR")
                                        {
                                            Environment.Exit(0);
                                        }
                                    }

                                }
                                //if #2 - #pink 
                                else if (addedValues[0].Substring(1).All(char.IsDigit) && (!addedValues[1].Substring(1).All(char.IsDigit)))
                                {
                                    //find op1
                                    foundexpression = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].Substring(1).ToUpper());
                                    if (foundexpression != null)
                                    {
                                        //evaluate value and relocatable
                                        expresssionValue = Int32.Parse(addedValues[0].Substring(1)) - foundexpression.VALUE;
                                        expressionRelocate = CalculateRelocate(expression, false, foundexpression.RFLAG, "-", 2);
                                        if (expressionRelocate == "ERROR")
                                        {
                                            Environment.Exit(0);
                                        }
                                    }
                                }
                                //if #2 - @pink
                                else if (addedValues[0].Substring(1).All(char.IsDigit) && addedValues[1].ToCharArray()[0] == '@')
                                {
                                    //find op2
                                    foundexpression = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].Substring(1).ToUpper());
                                    if (foundexpression != null)
                                    {
                                        //find relocatable
                                        expresssionValue = Int32.Parse(addedValues[0].Substring(1)) - foundexpression.VALUE;
                                        expressionRelocate = CalculateRelocate(expression, false, foundexpression.RFLAG, "-", 2);
                                        if (expressionRelocate == "ERROR")
                                        {
                                            Environment.Exit(0);
                                        }
                                    }
                                }
                                // if #pink - #pink
                                else
                                {
                                    //find op1 and op2
                                    foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].Substring(1).ToUpper());
                                    Node? foundexpression1 = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].Substring(1).ToUpper());
                                    if (foundexpression != null && foundexpression1 != null)
                                    {
                                        //evaliate value and relocatable
                                        expresssionValue = foundexpression.VALUE - foundexpression1.VALUE;
                                        expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, foundexpression1.RFLAG, "-", 2);
                                        if (expressionRelocate == "ERROR")
                                        {
                                            Environment.Exit(0);
                                        }
                                    }
                                    if (foundexpression1 == null)
                                    {
                                        Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                                        Environment.Exit(0);
                                    }
                                }


                            }
                        }
                        //#name
                        else
                        {
                            //#2
                            if (expression.Substring(1).All(char.IsDigit))
                            {
                                //evaluate value and relocatable
                                expresssionValue = Int32.Parse(expression.Substring(1));
                                expressionRelocate = "ABSOLUTE";
                                nonexpression = true;
                            }
                            //#red
                            else
                            {
                                //find op1
                                foundexpression = expression.Length > 4 ? BST.Search(root, expression.Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, expression.Substring(1).ToUpper());
                                if (foundexpression != null)
                                {
                                    //evaluate value and relocatbale
                                    expresssionValue = foundexpression.VALUE;
                                    expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, false, "-", 1);
                                    if (expressionRelocate == "ERROR")
                                    {
                                        Environment.Exit(0);
                                    }
                                }

                            }

                        }
                    }
                    // indirect
                    else if (expression.ToCharArray()[0] == '@')
                    {
                        //n bit
                        if (expression.Contains('+'))
                        {
                            string[] addedValues = expression.Split('+');
                            //removed white spaces
                            addedValues[0] = addedValues[0].Trim();
                            addedValues[1] = addedValues[1].Trim();
                            addedValues[0] = addedValues[0].All(char.IsDigit) ? "#" + addedValues[0].Trim() : addedValues[0].Trim();
                            addedValues[1] = addedValues[1].All(char.IsDigit) ? "#" + addedValues[1].Trim() : addedValues[1].Trim();


                            //if @pink + red
                            if (addedValues[0].ToCharArray()[0] == '@' && addedValues[1].ToCharArray()[0] != '@')
                            {
                                //if @#2 + pink
                                if (addedValues[0].ToCharArray()[1] == '#' && addedValues[1].ToCharArray()[0] != '@' && addedValues[1].ToCharArray()[0] != '#')
                                {
                                    //find op2
                                    foundexpression = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].ToUpper());
                                    if (foundexpression != null)
                                    {
                                        //evaluate value and relocatable
                                        expresssionValue = Int32.Parse(addedValues[0].Substring(2)) + foundexpression.VALUE;
                                        expressionRelocate = CalculateRelocate(expression, false, foundexpression.RFLAG, "+", 2);
                                        if (expressionRelocate == "ERROR")
                                        {
                                            Environment.Exit(0);
                                        }
                                    }
                                }
                                //if second value is # but not a number
                                else if (addedValues[1].ToCharArray()[0] == '#' && (!addedValues[1].Substring(1).All(char.IsDigit)))
                                {
                                    //find op1 and op2
                                    foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].Substring(1).ToUpper());
                                    Node? foundexpression1 = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].Substring(1).ToUpper());
                                    if (foundexpression != null && foundexpression1 != null)
                                    {
                                        //evaluate value and expression
                                        expresssionValue = foundexpression.VALUE + foundexpression1.VALUE;
                                        expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, foundexpression1.RFLAG, "+", 2);
                                        if (expressionRelocate == "ERROR")
                                        {
                                            Environment.Exit(0);
                                        }
                                    }
                                    if (foundexpression1 == null)
                                    {
                                        Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                                        Environment.Exit(0);
                                    }
                                }
                                //if @pink - red
                                else if ((addedValues[1].ToCharArray()[0] != '#') && (!addedValues[1].Substring(1).All(char.IsDigit)))
                                {
                                    //find op1 and op2
                                    foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].Substring(1).ToUpper());
                                    Node? foundexpression1 = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].ToUpper());
                                    if (foundexpression != null && foundexpression1 != null)
                                    {
                                        //evaluate value and relocatable
                                        expresssionValue = foundexpression.VALUE + foundexpression1.VALUE;
                                        expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, foundexpression1.RFLAG, "+", 2);
                                        if (expressionRelocate == "ERROR")
                                        {
                                            Environment.Exit(0);
                                        }
                                    }
                                    if (foundexpression1 == null)
                                    {
                                        Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                                        Environment.Exit(0);
                                    }

                                }
                            }
                            //if @pink + @pink
                            else if (addedValues[0].ToCharArray()[0] == '@' && addedValues[1].ToCharArray()[0] == '@')
                            {
                                //find op1 and op2
                                foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].Substring(1).ToUpper());
                                Node? foundexpression1 = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].Substring(1).ToUpper());
                                if (foundexpression != null && foundexpression1 != null)
                                {
                                    //evaluate value and reloactable
                                    expresssionValue = foundexpression.VALUE + foundexpression1.VALUE;
                                    expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, foundexpression1.RFLAG, "+", 2);
                                    if (expressionRelocate == "ERROR")
                                    {
                                        Environment.Exit(0);
                                    }
                                }
                                if (foundexpression1 == null)
                                {
                                    Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                                    Environment.Exit(0);
                                }

                            }
                            //if @pink + #2
                            else if (addedValues[0].ToCharArray()[0] == '@' && addedValues[1].Substring(1).All(char.IsDigit))
                            {
                                //find op1
                                foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].Substring(1).ToUpper());
                                if (foundexpression != null)
                                {
                                    //evaluate relocatable and value
                                    expresssionValue = foundexpression.VALUE + Int32.Parse(addedValues[1].Substring(1));
                                    expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, false, "+", 2);
                                    //if there is error return
                                    if (expressionRelocate == "ERROR")
                                    {
                                        Environment.Exit(0);
                                    }
                                }
                            }

                        }
                        else if (expression.Contains('-'))
                        {
                            string[] addedValues = expression.Split('-');
                            //remove white spaces
                            addedValues[0] = addedValues[0].Trim();
                            addedValues[1] = addedValues[1].Trim();
                            addedValues[0] = addedValues[0].All(char.IsDigit) ? "#" + addedValues[0].Trim() : addedValues[0].Trim();
                            addedValues[1] = addedValues[1].All(char.IsDigit) ? "#" + addedValues[1].Trim() : addedValues[1].Trim();
                            //if @pink - red
                            if (addedValues[0].ToCharArray()[0] == '@' && addedValues[1].ToCharArray()[0] != '@')
                            {
                                //if @#2 - pink
                                if (addedValues[0].ToCharArray()[1] == '#' && addedValues[1].ToCharArray()[0] != '@' && addedValues[1].ToCharArray()[0] != '#')
                                {
                                    //find op2
                                    foundexpression = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].ToUpper());
                                    if (foundexpression != null)
                                    {
                                        //evaluate value and relocatable
                                        expresssionValue = Int32.Parse(addedValues[0].Substring(2)) - foundexpression.VALUE;
                                        expressionRelocate = CalculateRelocate(expression, false, foundexpression.RFLAG, "-", 2);
                                        //if ther is error return
                                        if (expressionRelocate == "ERROR")
                                        {
                                            Environment.Exit(0);
                                        }
                                    }
                                }
                                //if second value is # but not a number
                                else if (addedValues[1].ToCharArray()[0] == '#' && (!addedValues[1].Substring(1).All(char.IsDigit)))
                                {
                                    //find op1 and op2
                                    foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].Substring(1).ToUpper());
                                    Node? foundexpression1 = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].Substring(1).ToUpper());
                                    if (foundexpression != null && foundexpression1 != null)
                                    {
                                        //evaluate value and relocatable
                                        expresssionValue = foundexpression.VALUE - foundexpression1.VALUE;
                                        expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, foundexpression1.RFLAG, "-", 2);
                                        if (expressionRelocate == "ERROR")
                                        {
                                            Environment.Exit(0);
                                        }
                                    }
                                    if (foundexpression1 == null)
                                    {
                                        Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                                        Environment.Exit(0);
                                    }
                                }
                                //if @pink - red
                                else if ((addedValues[1].ToCharArray()[0] != '#') && (!addedValues[1].Substring(1).All(char.IsDigit)))
                                {
                                    //find op1 and op2
                                    foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].Substring(1).ToUpper());
                                    Node? foundexpression1 = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].ToUpper());
                                    if (foundexpression != null && foundexpression1 != null)
                                    {
                                        //evaluate value and relocatable
                                        expresssionValue = foundexpression.VALUE - foundexpression1.VALUE;
                                        expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, foundexpression1.RFLAG, "-", 2);
                                        if (expressionRelocate == "ERROR")
                                        {
                                            Environment.Exit(0);
                                        }
                                    }
                                    if (foundexpression1 == null)
                                    {
                                        Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                                        Environment.Exit(0);
                                    }

                                }
                            }
                            //if @pink - @pink
                            else if (addedValues[0].ToCharArray()[0] == '@' && addedValues[1].ToCharArray()[0] == '@')
                            {
                                //find op1 and op2
                                foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].Substring(1).ToUpper());
                                Node? foundexpression1 = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].Substring(1).ToUpper());
                                if (foundexpression != null && foundexpression1 != null)
                                {
                                    //evaluate value and relocatable
                                    expresssionValue = foundexpression.VALUE - foundexpression1.VALUE;
                                    expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, foundexpression1.RFLAG, "-", 2);
                                    if (expressionRelocate == "ERROR")
                                    {
                                        Environment.Exit(0);
                                    }
                                }
                                if (foundexpression1 == null)
                                {
                                    Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                                    Environment.Exit(0);
                                }

                            }
                            //if @pink - #2
                            else if (addedValues[0].ToCharArray()[0] == '@' && addedValues[1].Substring(1).All(char.IsDigit))
                            {
                                //find op1
                                foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].Substring(1).ToUpper());
                                if (foundexpression != null)
                                {
                                    //evaluate value and relocatable
                                    expresssionValue = foundexpression.VALUE - Int32.Parse(addedValues[1].Substring(1));
                                    expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, false, "-", 2);
                                    if (expressionRelocate == "ERROR")
                                    {
                                        Environment.Exit(0);
                                    }
                                }
                            }
                        }
                        //@pink
                        else
                        {
                            //@2
                            if (expression.Substring(1).All(char.IsDigit))
                            {
                                //evaluate value and relocatable
                                expresssionValue = Int32.Parse(expression.Substring(1));
                                expressionRelocate = "ABSOLUTE";
                                nonexpression = true;
                            }
                            else
                            {
                                //find op1
                                foundexpression = expression.Length > 4 ? BST.Search(root, expression.Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, expression.Substring(1).ToUpper());
                                if (foundexpression != null)
                                {
                                    //evaluate value and relocatable
                                    expresssionValue = foundexpression.VALUE;
                                    expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, false, "-", 1);
                                    if (expressionRelocate == "ERROR")
                                    {
                                        Environment.Exit(0);
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        //both n and i 11
                        if (expression.Contains('+'))
                        {
                            string[] addedValues = expression.Split('+');
                            //trim the values(remove white space)
                            addedValues[0] = addedValues[0].Trim();
                            addedValues[1] = addedValues[1].Trim();
                            addedValues[0] = addedValues[0].All(char.IsDigit) ? "#" + addedValues[0].Trim() : addedValues[0].Trim();
                            addedValues[1] = addedValues[1].All(char.IsDigit) ? "#" + addedValues[1].Trim() : addedValues[1].Trim();


                            //if pink + #3
                            if (addedValues[0].ToCharArray()[0] != '#' && addedValues[0].ToCharArray()[0] != '@'
                                && (addedValues[1].Substring(1).All(char.IsDigit)))
                            {
                                //find op1
                                foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].ToUpper());
                                if (foundexpression != null)
                                {
                                    //evaluate value and relocatable
                                    expresssionValue = foundexpression.VALUE + Int32.Parse(addedValues[1].Substring(1));
                                    expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, false, "+", 2);
                                    if (expressionRelocate == "ERROR")
                                    {
                                        Environment.Exit(0);
                                    }
                                }
                            }
                            //if pink + #red
                            else if (addedValues[0].ToCharArray()[0] != '#' && addedValues[0].ToCharArray()[0] != '@'
                                && (addedValues[1].ToCharArray()[0] == '#') && (!addedValues[1].Substring(1).All(char.IsDigit)))
                            {
                                //find op1 and op2
                                foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].ToUpper());
                                Node? foundexpression1 = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].Substring(1).ToUpper());
                                if (foundexpression != null && foundexpression1 != null)
                                {
                                    //evaluate value and relocatable
                                    expresssionValue = foundexpression.VALUE + foundexpression1.VALUE;
                                    expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, foundexpression1.RFLAG, "+", 2);
                                    if (expressionRelocate == "ERROR")
                                    {
                                        Environment.Exit(0);
                                    }
                                }
                                if (foundexpression1 == null)
                                {
                                    Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                                    Environment.Exit(0);
                                }
                            }
                            //if pink + pink
                            else if (addedValues[0].ToCharArray()[0] != '#' && addedValues[0].ToCharArray()[0] != '@'
                                && (addedValues[1].ToCharArray()[0] != '#' && addedValues[1].ToCharArray()[0] != '@'))
                            {
                                //find op1 and op2
                                foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].ToUpper());
                                Node? foundexpression1 = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].ToUpper());
                                if (foundexpression != null && foundexpression1 != null)
                                {
                                    //evaliuate value and relocatable
                                    expresssionValue = foundexpression.VALUE + foundexpression1.VALUE;
                                    expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, foundexpression1.RFLAG, "+", 2);
                                    if (expressionRelocate == "ERROR")
                                    {
                                        Environment.Exit(0);
                                    }
                                }
                                if (foundexpression1 == null)
                                {
                                    Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                                    Environment.Exit(0);
                                }
                            }
                            //if pink + @pink
                            else if (addedValues[0].ToCharArray()[0] != '#' && addedValues[0].ToCharArray()[0] != '@' &&
                                addedValues[1].ToCharArray()[0] == '@')
                            {
                                //find op1 and op2
                                foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].ToUpper());
                                Node? foundexpression1 = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].Substring(1).ToUpper());
                                if (foundexpression != null && foundexpression1 != null)
                                {
                                    //evaluate value and relocatable
                                    expresssionValue = foundexpression.VALUE + foundexpression1.VALUE;
                                    expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, foundexpression1.RFLAG, "+", 2);
                                    if (expressionRelocate == "ERROR")
                                    {
                                        Environment.Exit(0);
                                    }
                                }
                                if (foundexpression1 == null)
                                {
                                    Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                                    Environment.Exit(0);
                                }
                            }
                        }
                        else if (expression.Contains('-'))
                        {
                            //split the word in -
                            string[] addedValues = expression.Split('-');
                            //remove white spaces
                            addedValues[0] = addedValues[0].Trim();
                            addedValues[1] = addedValues[1].Trim();
                            addedValues[0] = addedValues[0].All(char.IsDigit) ? "#" + addedValues[0].Trim() : addedValues[0].Trim();
                            addedValues[1] = addedValues[1].All(char.IsDigit) ? "#" + addedValues[1].Trim() : addedValues[1].Trim();

                            //if pink - #3
                            if (addedValues[0].ToCharArray()[0] != '#' && addedValues[0].ToCharArray()[0] != '@'
                                && (addedValues[1].Substring(1).All(char.IsDigit)))
                            {
                                //find op1 
                                foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].ToUpper());
                                if (foundexpression != null)
                                {
                                    //evaluate value and relocatable
                                    expresssionValue = foundexpression.VALUE - Int32.Parse(addedValues[1].Substring(1));
                                    expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, false, "-", 2);
                                    if (expressionRelocate == "ERROR")
                                    {
                                        Environment.Exit(0);
                                    }
                                }
                            }
                            //if pink - #red
                            else if (addedValues[0].ToCharArray()[0] != '#' && addedValues[0].ToCharArray()[0] != '@'
                                && (addedValues[1].ToCharArray()[0] == '#') && (!addedValues[1].Substring(1).All(char.IsDigit)))
                            {
                                //find op1 qnd op2
                                foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].ToUpper());
                                Node? foundexpression1 = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].Substring(1).ToUpper());
                                if (foundexpression != null && foundexpression1 != null)
                                {
                                    //find value and relocatable
                                    expresssionValue = foundexpression.VALUE - foundexpression1.VALUE;
                                    expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, foundexpression1.RFLAG, "-", 2);
                                    if (expressionRelocate == "ERROR")
                                    {
                                        Environment.Exit(0);
                                    }
                                }
                                if (foundexpression1 == null)
                                {
                                    Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                                    Environment.Exit(0);
                                }
                            }
                            //if pink - pink
                            else if (addedValues[0].ToCharArray()[0] != '#' && addedValues[0].ToCharArray()[0] != '@'
                                && (addedValues[1].ToCharArray()[0] != '#' && addedValues[1].ToCharArray()[0] != '@'))
                            {
                                //find op1 and op2
                                foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].ToUpper());
                                Node? foundexpression1 = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].ToUpper());
                                if (foundexpression != null && foundexpression1 != null)
                                {
                                    //find value and relocatable
                                    expresssionValue = foundexpression.VALUE - foundexpression1.VALUE;
                                    expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, foundexpression1.RFLAG, "-", 2);
                                    if (expressionRelocate == "ERROR")
                                    {
                                        Environment.Exit(0);
                                    }
                                }
                                if (foundexpression1 == null)
                                {
                                    Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                                    Environment.Exit(0);
                                }
                            }
                            //if pink - @pink
                            else if (addedValues[0].ToCharArray()[0] != '#' && addedValues[0].ToCharArray()[0] != '@' &&
                                addedValues[1].ToCharArray()[0] == '@')
                            {
                                //find op1 and op2
                                foundexpression = addedValues[0].Length > 4 ? BST.Search(root, addedValues[0].ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[0].ToUpper());
                                Node? foundexpression1 = addedValues[1].Length > 4 ? BST.Search(root, addedValues[1].Substring(1).ToUpper().Substring(0, 4)) : BST.Search(root, addedValues[1].Substring(1).ToUpper());
                                if (foundexpression != null && foundexpression1 != null)
                                {
                                    //evlauate value and relocatable
                                    expresssionValue = foundexpression.VALUE - foundexpression1.VALUE;
                                    expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, foundexpression1.RFLAG, "-", 2);
                                    if (expressionRelocate == "ERROR")
                                    {
                                        Environment.Exit(0);
                                    }
                                }
                                if (foundexpression1 == null)
                                {
                                    Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                                    Environment.Exit(0);
                                }
                            }
                        }
                        //num
                        else
                        {
                            //if digit 
                            if (expression.All(char.IsDigit))
                            {
                                //evaluate value and relocatable
                                expresssionValue = Int32.Parse(expression);
                                expressionRelocate = "ABSOLUTE";
                                nonexpression = true;
                            }
                            else
                            {
                                //if not digit evaluate value and reloctable
                                foundexpression = expression.Length > 4 ? BST.Search(root, expression.ToUpper().Substring(0, 4)) : BST.Search(root, expression.ToUpper());
                                if (foundexpression != null)
                                {
                                    expresssionValue = foundexpression.VALUE;
                                    expressionRelocate = CalculateRelocate(expression, foundexpression.RFLAG, false, "-", 1);
                                    if (expressionRelocate == "ERROR")
                                    {
                                        Environment.Exit(0);
                                    }
                                }
                            }

                        }

                    }
                }
                //calculate N I X Bits
                //If indexed
                if (expression.Contains(','))
                {
                    //set x bit to 1
                    xBit = true;
                }
                else
                {
                    //if not indexed set x bit to 0
                    xBit = false;
                }
                // if immidate
                if (expression.ToCharArray()[0] == '#' || expression.All(char.IsDigit))
                {
                    //set i bit to 1 and n to 0
                    IBit = true;
                    NBit = false;

                    expression = expression.All(char.IsDigit) ? "#" + expression : expression;

                }
                //if indirect 
                if (expression.ToCharArray()[0] == '@')
                {
                    //set N to 1 and I to 0
                    NBit = true;
                    IBit = false;
                }
                //if nothing before char set N and I to 1
                if (expression.ToCharArray()[0] != '@' && expression.ToCharArray()[0] != '#')
                {
                    //set bot n and i to 1
                    NBit = true;
                    IBit = true;
                }

                // if expression found in current print current expression and it's values
                if (foundexpression != null || nonexpression)
                {
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
                    bool relocate = false;
                    //if expressionvalue is ABSOLUTE or RELATIVE set the bit values
                    if (expressionRelocate == "RELATIVE")
                    {
                        relocate = true;
                    }
                    else if (expressionRelocate == "ABSOLUTE")
                    {
                        relocate = false;
                    }
                    if(label != "")
                    {
                        //if the BST is empty, intanciate it and insert to the BST
                        if (root == null)
                        {
                            root = new Node(label.ToUpper(), expresssionValue, bool.Parse(expressionRelocate));
                            return "";
                        }
                        if (BST.Search(root, label = label.Length > 4 ? label.Substring(0, 4) : label) == null)
                        {
                            // Insert to BST
                            BST.Insert(root, label.ToUpper(), expresssionValue, relocate);
                        }
                    }
                   
                    
                }
                else
                {
                    Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR -  expression not found in symbol table");
                    Environment.Exit(0);
                }

                returnstring = expresssionValue.ToString("X") + ", " + Convert.ToInt32(NBit) + ", " + Convert.ToInt32(IBit);

            }
            else
            {
                Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR - Expression not Valid");
                Environment.Exit(0);
            }

            return returnstring;
        }
        /********************************************************************
      *** METHOD: CalculateRelocate
      *********************************************************************
      *** DESCRIPTION : This function calculates the relocatable of the expression
      *based on the values given
      *** Input ARGS  : string expression, bool a, bool b, string operation, int opCount
      *** OUTPUT ARGS : N/A
      *** IN/OUT ARGS : N/A
      *** RETURN : string
      ********************************************************************/
        static string CalculateRelocate(string expression, bool a, bool b, string operation, int opCount)
        {
            string returnString = string.Empty;

            //if there is no + or - in the evaluation
            if (opCount == 1)
            {
                //if a is true set to relative 
                if (a)
                {
                    returnString = "RELATIVE";
                }
                //else set to absolute
                else
                {
                    returnString = "ABSOLUTE";
                }

            }
            //if there is + or - in the expression
            else if (opCount == 2)
            {
                //if ther is + in the expression
                if (operation == "+")
                {
                    //if relative + relative
                    if (a && b)
                    {
                        //Console.WriteLine($"ERROR - {expression} Can't have both values as relative");
                        Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR - Can't add when both values are relative");
                        returnString = "ERROR";
                    }
                    //if relative + absolute
                    else if (a && !b)
                    {
                        //set to relative
                        returnString = "RELATIVE";
                    }
                    //if absolute + relative
                    else if (!a && b)
                    {
                        //set relative
                        returnString = "RELATIVE";
                    }
                    //if absoulte + absoulute
                    else if (!a && !b)
                    {
                        //set absoulte
                        returnString = "ABSOLUTE";
                    }
                }
                //if the operation is -
                else if (operation == "-")
                {
                    //if absolute - absolute
                    if (!a && !b)
                    {
                        //set to absoulte
                        returnString = "ABSOLUTE";
                    }
                    //if absolute - relative
                    else if (!a && b)
                    {
                        //print error message
                        Console.WriteLine("{0,-20} {1,-7}", expression, "ERROR - Can't substract Absolute and relative");
                        returnString = "ERROR";
                    }
                    //if relative - absolute
                    else if (a && !b)
                    {
                        //set to relative
                        returnString = "RELATIVE";
                    }
                    //if relative - relative
                    else if (a && b)
                    {
                        //set absolute
                        returnString = "ABSOLUTE";
                    }
                }
            }
            return returnString;
        }

        /********************************************************************
        *** METHOD: ValidateExpression
        *********************************************************************
        *** DESCRIPTION : This function validates the expression file
        *** Input ARGS  : string expression
        *** OUTPUT ARGS : N/A
        *** IN/OUT ARGS : N/A
        *** RETURN : bool
        ********************************************************************/
        static bool ValidateExpression(string expression)
        {
            bool ret = true;
            //if expression has +
            if (expression.Contains('+'))
            {
                //if there are more than 3 operation added
                string[] testExpression = expression.Split('+');
                if (testExpression.Length > 2)
                {
                    //return false
                    ret = false;
                }
            }
            //if ther eare more than 3 values subed then return false
            if (expression.Contains('-'))
            {
                string[] testExpression = expression.Split('-');
                if (testExpression.Length > 2)
                {
                    ret = false;
                }
            }
            return ret;
        }
    }
}

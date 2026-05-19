/********************************************************************
*** NAME : Kaleab Zerihun
*** CLASS : CSc 354
*** ASSIGNMENT : Assignment 
*** DUE DATE : 12-4-24
*** INSTRUCTOR : Hamer
*********************************************************************
*** DESCRIPTION : perform pass2 and create the assembly listings and then the object file****
********************************************************************/
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Zerihun4
{
    internal class Pass2
    {
        /********************************************************************
        *** METHOD: PassTwo
        *********************************************************************
        *** DESCRIPTION : get each section from the .int file
        *** INPUT ARGS : string label, string opcode, string operand, string? comment, Dictionary<string, string> opcodeDictionary, ref string locationCounter, ref string nextlocationCounter
        *** OUTPUT ARGS : N/A
        *** IN/OUT ARGS :n/a
        *** RETURN : void
        ********************************************************************/
        public void PassTwo(string fileName, Dictionary<string, string> opcodeDictionary)
        {
            string[] splitfileName = fileName.Split('.');
            fileName = splitfileName[0] + ".int";
            string trimmedLine = string.Empty;
            string fullFileName = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            if (File.Exists(fullFileName))
            {
                //Read the file and for each line
                string[] lines = File.ReadAllLines(fileName);
                bool lcSection = false;
                bool symbolSection = false;
                bool literalSection = false;
                //assign variables
                List<string> symbols = new List<string>();
                List<string> literals = new List<string>();
                Node? root = null;
                List<List<string>> codePart = new List<List<string>>();
                string programLength = string.Empty;
                string programName = string.Empty;
                string startingAddress = string.Empty;
                //for each line in the int file
                foreach (string line in lines)
                {
                    //get the opcode label and operand and eveything
                    string label = string.Empty;

                    string opcode = string.Empty;
                    string operand = string.Empty;
                    string comment = string.Empty;
                    string LC = string.Empty;
                    string lineNumber = string.Empty;

                    string objCode = string.Empty;
                    string symbolName = string.Empty;
                    string symbolValue = string.Empty;
                    string Iflag = string.Empty;
                    string Rflag = string.Empty;
                    string Mflag = string.Empty;
                    string literalName = string.Empty;
                    string literalLength = string.Empty;
                    string literalValue = string.Empty;
                    string literalAddress = string.Empty;
                    trimmedLine = line.Trim();
                    string[] fields = trimmedLine.Split(' ');
                    //if there is a comment
                    if (trimmedLine.Contains('.'))
                    {
                        comment = trimmedLine.Split('.')[1];
                    }
                    trimmedLine = trimmedLine.Split('.')[0];
                    //location counter section
                    if (trimmedLine.StartsWith("Line       LC              Label           Opcode"))
                    {
                        lcSection = true;
                        symbolSection = false;
                        literalSection = false;
                    }
                    //if symbol section
                    else if (trimmedLine.StartsWith("SYMBOL TABLE:"))
                    {
                        lcSection = false;
                        symbolSection = true;
                        literalSection = false;
                    }
                    //if literal section
                    else if (trimmedLine.StartsWith("Literal TABLE:"))
                    {
                        lcSection = false;
                        symbolSection = false;
                        literalSection = true;
                    }
                    //if program length
                    else if (trimmedLine.StartsWith("Program Length: "))
                    {
                        string[] val = trimmedLine.Split(" ");
                        programLength = val[val.Length - 1];
                    }
                    
                    string elementOne = string.Empty;
                    string elementTwo = string.Empty;
                    string elementThree = string.Empty;
                    string elementFour = string.Empty;
                    string elementFive = string.Empty;
                    string elementSix = string.Empty;
                    //if code section get every section
                    if (lcSection)
                    {
                       
                        foreach (var element in fields)
                        {
                            //if it is the heading skip it
                            if (trimmedLine.Contains("Line") || trimmedLine.Contains("Program Length:"))
                            {
                                continue;
                            }
                            //get each element
                            if (element != "")
                            {
                                if (elementOne == "")
                                {
                                    elementOne = element;
                                }
                                else if (elementTwo == "")
                                {
                                    elementTwo = element;
                                }
                                else if (elementThree == "")
                                {
                                    elementThree = element;
                                }
                                else if (elementThree != "" && elementFour == "")
                                {
                                    elementFour = element;
                                }
                                else if (elementThree != "" && elementFour != "" && elementFive == "")
                                {
                                    elementFive = element;
                                }
                                else if (elementSix == "")
                                {
                                    elementSix = element;
                                }
                            }
                        }
                        //get linenumber and lc
                        lineNumber = elementOne;
                        LC = elementTwo;
                    }
                    else if (symbolSection)
                    {
                        //assign the symbol name value and i r m flag
                        foreach (var element in fields)
                        {
                            if (trimmedLine.Contains("Symbol     Value ") || trimmedLine.Contains("SYMBOL TABLE:"))
                            {
                                continue;
                            }
                            
                            if (symbolName == "")    
                            {
                                symbolName = element;
                            }
                            else if (symbolValue == "")
                            {
                                symbolValue = element;
                            }
                            else if (Iflag == "")
                            {
                                Iflag = element;
                            }
                            else if (Rflag == "")
                            {
                                Rflag = element;
                            }
                            else if (Mflag == "")
                            {
                                Mflag = element;
                            }

                            

                        }
                        if (symbolName != "" && symbolValue != "" && Rflag != "" && trimmedLine != "")
                        {
                            //if the BST is empty, intanciate it and insert to the BST
                            if (root == null)
                            {
                                root = new Node(symbolName.ToUpper(), int.Parse(symbolValue, System.Globalization.NumberStyles.HexNumber), Rflag == "1");
                                continue;
                            }
                            // Insert to BST
                            BST.Insert(root, symbolName.ToUpper(), int.Parse(symbolValue, System.Globalization.NumberStyles.HexNumber), Rflag == "1");
                        }
                    }
                    //if literal section
                    else if (literalSection)
                    {
                        //for each val get the sections
                        foreach(var element in fields)
                        {
                            if (trimmedLine.Contains("Literal    Length     VALUE") || trimmedLine.Contains("Literal TABLE:"))
                            {
                                continue;
                            }
                            if (!literals.Contains(trimmedLine) && trimmedLine != "")
                            {
                                literals.Add(trimmedLine);
                            }
                            if (literalName == "")
                            {
                                literalName = element;
                            }
                            else if(literalLength == "")
                            {
                                literalLength = element;
                            }
                            else if(literalValue == "")
                            {
                                literalValue = element;
                            }
                            else if(literalAddress == "")
                            {
                                literalAddress = element;
                            }
                        }
                    }

                    // if label opcode and operand
                    if (!string.IsNullOrEmpty(elementThree) && !string.IsNullOrEmpty(elementFour) && !string.IsNullOrEmpty(elementFive))
                    {
                        label = elementThree;
                        opcode = elementFour;
                        operand = elementFive;
                        if (!opcodeDictionary.ContainsKey(opcode) && !opcode.Contains('+') && !(elementFour == "BASE" || elementFour == "START" || elementFour == "END" || elementFour == "BYTE" || elementFour == "WORD" || elementFour == "RESB" || elementFour == "RESW" || elementFour == "EQU" || elementFour == "EXTDEF" || elementFour == "EXTREF"))
                        {
                            label = string.Empty;
                            opcode = string.Empty;
                            operand = string.Empty;
                        }
                    }
                    // if label opcode or opcode operand
                    else if (!string.IsNullOrEmpty(elementThree) && !string.IsNullOrEmpty(elementFour) && string.IsNullOrEmpty(elementFive))
                    {
                        // if it is opcode operand
                        if (opcodeDictionary.ContainsKey(elementThree))
                        {
                            label = string.Empty;
                            opcode = elementThree;
                            operand = elementFour;
                        }
                        // if it is opcode and operand
                        else if (elementThree.Contains('+') && opcodeDictionary.ContainsKey(elementThree.Substring(1)))
                        {
                            label = string.Empty;
                            opcode = elementThree;
                            operand = elementFour;
                        }
                        // if label and opcode
                        else if (opcodeDictionary.ContainsKey(elementFour))
                        {
                            label = elementThree;
                            opcode = elementFour;
                            operand = string.Empty;
                        }
                        // if it is label and opcode
                        else if (elementFour.Contains('+') && opcodeDictionary.ContainsKey(elementFour.Substring(1)))
                        {
                            label = elementThree;
                            opcode = elementFour;
                            operand = string.Empty;
                        }
                        // if the opcode is either base, start, or end
                        else if (elementThree == "BASE" || elementThree == "START" || elementThree == "END" || elementThree == "BYTE" || elementThree == "WORD" || elementThree == "RESB" || elementThree == "RESW" || elementThree == "EQU" || elementThree == "EXTDEF" || elementThree == "EXTREF")
                        {
                            label = string.Empty;
                            opcode = elementThree;
                            operand = elementFour;
                        }
                        // if it is label and opcode
                        else if (elementFour == "BASE" || elementFour == "START" || elementFour == "END" || elementFour == "BYTE" || elementFour == "WORD" || elementFour == "RESB" || elementFour == "RESW" || elementFour == "EQU" || elementFour == "EXTDEF" || elementFour == "EXTREF")
                        {
                            label = elementThree;
                            opcode = elementFour;
                            operand = string.Empty;
                        }
                        // if it is just an empty line
                        else if(elementFour.Contains("=0") && elementThree == "*")
                        {
                            label = elementThree;
                            opcode = elementFour;
                            operand = "";
                        }
                        else
                        {
                            label = string.Empty;
                            opcode = string.Empty;
                            operand = string.Empty;
                        }
                    }
                    // if opcode
                    else if (!string.IsNullOrEmpty(elementThree) && string.IsNullOrEmpty(elementFour) && string.IsNullOrEmpty(elementFive))
                    {
                        opcode = elementThree;
                        label = string.Empty;
                        operand = string.Empty;
                        // check if the opcode is valid
                        if (!opcodeDictionary.ContainsKey(opcode) && opcode != "END")
                        {
                            label = string.Empty;
                            opcode = string.Empty;
                            operand = string.Empty;
                        }
                    }

                    if ( lcSection && LC != "")
                    {
                        codePart.Add(new List<string> { LC, label, opcode, operand, comment });
                    }
                    if (opcode == "START")
                    {
                        programName = label.Substring(0, label.Length - 1);
                        if (operand.StartsWith('#'))
                        {
                            startingAddress = operand.Substring(1);
                        }
                        else
                        {
                            startingAddress = operand;
                        }
                    }
                }
                //generate the objcode 
                generateObjcode(literals, root, codePart, opcodeDictionary, programLength, programName, startingAddress, fileName);
            }

        }
        /********************************************************************
       *** METHOD: generateObjcode
       *********************************************************************
       *** DESCRIPTION : perform  generate the object code and the object file
       *** INPUT ARGS : string label, string opcode, string operand, string? comment, Dictionary<string, string> opcodeDictionary, ref string locationCounter, ref string nextlocationCounter
       *** OUTPUT ARGS : N/A
       *** IN/OUT ARGS : void
       *** RETURN : void
       ********************************************************************/
        public void generateObjcode(List<string> literals, Node? root, List<List<string>> codePart, Dictionary<string, string> opcodeDictionary, string programLength, string programName, string startingAddress, string fileNmae)
        {
            //assign varibales to be used in this function
            string LC = string.Empty;
            string label = string.Empty;
            string opcode = string.Empty;
            string operand = string.Empty;
            string comment = string.Empty;
            string objCode = string.Empty;
            ExecuteExpression executeExpression = new ExecuteExpression();
            Dictionary<string, int> registerTable = new Dictionary<string, int>
            {
                { "A", 0 },
                { "X", 1 },
                { "L", 2 },
                { "B", 3 },
                { "S", 4 },
                { "T", 5 },
                { "F", 6 },
                { "PC", 8 },
                { "SW", 9 }
            };
            //print the heading
            Console.WriteLine("-----------------------------------------PASS 2---------------------------------------------------------");
            Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15} {4,-15} {5,-15} {6, -15}", "Line", "LC", "Label", "Opcode", "Operand", "Object Code", "Comment");
            //name varibales to be used in the future
            int j = 1;
            int z = 1;
            List<string> extdefList = new List<string>();
            List<string> extrefList = new List<string>();
            List<string> extdeflabel = new List<string>();
            Node? node = root;
            string headerRecord = string.Empty;
            List<string> textRecord = new List<string>();
            string DRecord = string.Empty;
            string mREcord = string.Empty;
            string rRecord = string.Empty;
            string eRecord = string.Empty;
            List<string> mRecordList = new List<string>();
            string writeFileName = fileNmae.Split('.')[0] + ".lst";
            List<string> modificationRecord = new List<string>();
            //use stream writer to write on the files
            using (StreamWriter writer = new StreamWriter(new FileStream(writeFileName, FileMode.Create, FileAccess.Write)))
            {
                //print the heading to the file
                writer.AutoFlush = true;
                writer.WriteLine("-----------------------------------------PASS 2---------------------------------------------------------");
                writer.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15} {4,-15} {5,-15} {6, -15}", "Line", "LC", "Label", "Opcode", "Operand", "Object Code", "Comment");
                bool firsttext = true;
                int zoo = 0;
                string baseRegister = "0";
                //for each line in the codePart
                foreach (var line in codePart)
                {
                    //assign each section
                    bool indexed = false;
                    LC = line[0];
                    label = line[1];
                    opcode = line[2];
                    operand = line[3];
                    comment = line[4];
                    string opcodeFormat = string.Empty;
                    string opcodeValue = string.Empty;
                    string literalValue = string.Empty;
                    string literalAddress = string.Empty;
                    string expressionValue = string.Empty;

                    //if there is get the extdeflist
                    foreach (var e in extdeflabel)
                    {
                        if (e + ":" == label)
                        {
                            extdefList.Add(label.Substring(0, label.Length-1) + "^" + LC);

                        }
                    }

                    //if base start end resb resw equ exted extdrf
                    if (opcode == "BASE" || opcode == "START" || opcode == "END" || opcode == "RESB" || opcode == "RESW" || opcode == "EQU" || opcode == "EXTDEF" || opcode == "EXTREF")
                    {
                        objCode = "";
                        //if base get the baseRegister
                        if (opcode == "BASE")
                        {

                            foreach (var lin in codePart)
                            {
                                if (operand + ":" == lin[1])
                                {
                                    baseRegister = lin[0];
                                    break;
                                }
                            }
                        }
                        //if ectdef get the list and add to the symbol table
                        if (opcode == "EXTDEF")
                        {
                            string[] symbols = operand.Split(',');
                            foreach (string symbol in symbols)
                            {
                                string val = symbol.Trim();
                                extdeflabel.Add(symbol.Trim());

                                if(BST.Search(root, val = val.Length > 4 ? val.Substring(0, 4) : val) == null)
                                {
                                    //if the BST is empty, intanciate it and insert to the BST
                                    if (root == null)
                                    {
                                        root = new Node(val = val.Length > 4 ? val.Substring(0, 4).ToUpper() : val.ToUpper(), 00000, true);
                                        continue;
                                    }
                                    // Insert to BST
                                    BST.Insert(root, symbol.ToUpper(), 00000, true);
                                }
                                

                            }
                        }
                        //if extref get the list and add to the symbol table
                        if (opcode == "EXTREF")
                        {
                            string[] symbols = operand.Split(',');
                            foreach (string symbol in symbols)
                            {
                                string val = symbol.Trim();
                                extrefList.Add(symbol.Trim());
                                if (BST.Search(root, val = val.Length > 4 ? val.Substring(0, 4) : val) == null)
                                {
                                    //if the BST is empty, intanciate it and insert to the BST
                                    if (root == null)
                                    {
                                        root = new Node(val = val.Length > 4 ? val.Substring(0, 4).ToUpper() : val.ToUpper(), 00000, true);
                                        continue;
                                    }
                                    // Insert to BST
                                    BST.Insert(root, symbol.ToUpper(), 00000, true);
                                }
                            }
                        }
                    }
                    //if word get the objcode
                    else if (opcode == "WORD")
                    {
                        //add to modification record
                        modificationRecord.Add($"^{LC}^06");
                        operand = operand.Trim();
                        //if an excpression evaluate it
                        if (operand.Contains("+") || operand.Contains("-"))
                        {
                            string[] vals = executeExpression.executeexpression(ref node, operand, label).Split(", ");
                            objCode = (Convert.ToInt32(vals[0], 16)).ToString("X6");
                        }
                        //if not get he objcode directly
                        else
                        {
                            //if all int get the resutl
                            if (int.TryParse(operand, out int result))
                            {
                                objCode = result.ToString("X6");
                            }
                            //if int and there is a @ or #
                            else if (int.TryParse(operand.Substring(1), out int res))
                            {
                                objCode = res.ToString("X6");
                            }
                            //if symbol
                            else if (BST.Search(root, operand = operand.Length > 4 ? operand.Substring(0, 4) : operand) != null)
                            {
                                var val = BST.Search(root, operand = operand.Length > 4 ? operand.Substring(0, 4) : operand);
                                objCode = val.VALUE.ToString("X6");
                            }
                            //else say undefined sybmol
                            else
                            {
                                Console.WriteLine("ERROR - Undefined SYMBOL in operand");
                                Environment.Exit(0);
                            }
                        }
                        
                    }
                    //if byte
                    else if (opcode == "BYTE")
                    {
                        //if 0c get the objcode
                        if (operand.StartsWith("0C"))
                        {
                            objCode = string.Join("", operand.Substring(2).Select(c => ((int)c).ToString("X2")));
                        }
                        //if 0x get the objcode
                        else if (operand.StartsWith("0X"))
                        {
                            objCode = operand.Substring(2);
                        }
                        //if all digit get the objcode
                        else if (operand.All(char.IsDigit))
                        {
                            objCode = operand;
                        }
                        //else invalid byte operand
                        else
                        {
                            Console.WriteLine("ERROR - Invalid BYTE operand");
                            Environment.Exit(0);
                        }
                    }
                    //if literal
                    else if (opcode.StartsWith("=0") && label == "*")
                    {
                        
                        string[] splitLiteral = null;
                        string literalName = string.Empty;
                        string literalLength = string.Empty;
                        foreach (string lit in literals)
                        {
                            if (lit.StartsWith(opcode))
                            {
                                splitLiteral = lit.Split(' ');
                                foreach (string lit2 in splitLiteral)
                                {
                                    if (literalName == "")
                                    {
                                        literalName = lit2;
                                    }
                                    else if (literalName != "" && literalLength == "")
                                    {
                                        literalLength = lit2;
                                    }
                                    else if (literalName != "" && literalLength != "" && literalValue == "")
                                    {
                                        literalValue = lit2;
                                    }
                                }

                            }
                        }
                        objCode = literalValue;
                    }
                    //if format 1 2 3 or 4
                    else
                    {
                        string n = string.Empty;
                        string i = string.Empty;
                        string x = string.Empty;
                        string b = string.Empty;
                        string p = string.Empty;
                        string e = string.Empty;
                        string offset = string.Empty;
                        string beforeNix = string.Empty;
                        string targetAddr = string.Empty;
                        string displacement = string.Empty;
                        //if indexed assign x to 1
                        if (operand.Contains(",X"))
                        {
                            x = "1";
                            operand = operand.Split(',')[0];
                            indexed = true;
                        }
                        //else assign x to 0
                        else
                        {
                            x = "0";
                        }
                        //if immidate asssign i to 1 and n to 0
                        if (operand.StartsWith('#'))
                        {
                            n = "0";
                            i = "1";
                            b = "0";
                            p = "0";
                            e = "0";
                            operand = operand.Substring(1);
                        }
                        //if indirect assign n to 1 and i to 0
                        else if (operand.StartsWith('@'))
                        {
                            n = "1";
                            i = "0";
                            b = "0";
                            p = "0";
                            e = "0";
                            operand = operand.Substring(1);
                        }
                        //if simple assign n and i to  1
                        else
                        {
                            n = "1";
                            i = "1";
                            b = "0";
                            p = "0";
                            e = "0";
                        }


                        //if format 4 up ly else if belek chemerek aderegew
                        if (opcode.StartsWith('+'))
                        {
                            //add to modification record
                            modificationRecord.Add($"^{(Convert.ToInt32(LC, 16) + 1).ToString("X6")}^05");
                            string[] value = opcodeDictionary[opcode.Substring(1)].Split(' ');
                            opcodeValue = value[0];
                            opcodeFormat = "4";
                            //assign e and b 
                            e = "1";
                            b = "0";
                            p = "0";
                            //get the beforeNIX
                            foreach (char c in opcodeValue)
                            {
                                beforeNix += Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0');
                            }
                            beforeNix = beforeNix.Substring(0, 6);
                            //if literal gert the target addr
                            if (operand.StartsWith("=0"))
                            {
                                if (operand.Contains("+") || operand.Contains("-") && (operand.Substring(3) != "+" || operand.Substring(3) != "-"))
                                {
                                    string[] vals = executeExpression.executeexpression(ref node, operand, label).Split(", ");
                                    targetAddr = vals[0];
                                    n = vals[1];
                                    i = vals[2];


                                }
                                else
                                {
                                    //if it is not an expression get the targer addr directly
                                    foreach (var val in literals)
                                    {
                                        if (val.StartsWith(operand))
                                        {
                                            string[] literalParts = val.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                            literalValue = literalParts[2];
                                            targetAddr = literalParts[3];
                                        }
                                    }
                                }


                            }
                            //if it is an expression
                            else
                            {
                                //get the target addr and n and i
                                if (operand.Contains("+") || operand.Contains("-"))
                                {
                                    string[] vals = executeExpression.executeexpression(ref node, operand, label).Split(", ");
                                    targetAddr = (Convert.ToInt32(vals[0], 16)).ToString("X3");
                                    n = vals[1];
                                    i = vals[2];
                                }
                                //else get it directly
                                else
                                {
                                    foreach (var val in codePart)
                                    {
                                        if (operand + ":" == val[1])
                                        {
                                            targetAddr = val[0];
                                            break;
                                        }
                                    }
                                    
                                    if (operand.Substring(1).All(char.IsDigit))
                                    {
                                        targetAddr = Int32.Parse(operand).ToString("X3");
                                    }
                                }

                            }
                            //if target addr is emply assign to 0000
                            if(targetAddr == "")
                            {
                                targetAddr = "0000";
                            }
                            //get the opcode after or the whole thing
                            string opcodeAfter = Convert.ToInt32((beforeNix + n + i + x + b + p + e), 2).ToString("X3");
                            //get the objcode by adding the targetaddr and the opcodeAfter
                            objCode = opcodeAfter + targetAddr;


                        }
                        //if format 1 2 or 3
                        else
                        {
                            //if the opcode exists
                            if (opcodeDictionary.ContainsKey(opcode))
                            {
                                string[] value = opcodeDictionary[opcode].Split(' ');
                                opcodeValue = value[0];
                                opcodeFormat = value[1];
                                //if format 1
                                if (opcodeFormat == "1")
                                {
                                    objCode = opcodeValue;
                                }
                                //if format 2
                                else if (opcodeFormat == "2")
                                {
                                    //if indexed get the ,X
                                    if (indexed)
                                    {
                                        operand = operand + ",X";
                                    }
                                    //split by ,
                                    string[] operands = operand.Split(',');
                                    int register1 = 0;
                                    int register2 = 0;
                                    if (operands.Length == 2)
                                    {
                                        // two operands (e.g., "ADDR R1,R2")
                                        if (!registerTable.TryGetValue(operands[0].Trim(), out register1))
                                        {
                                            throw new Exception($"Invalid register: {operands[0]}");
                                        }
                                        if (!registerTable.TryGetValue(operands[1].Trim(), out register2))
                                        {
                                            throw new Exception($"Invalid register: {operands[1]}");
                                        }
                                    }
                                    //if operand length is 1
                                    else if (operands.Length == 1)
                                    {
                                        // One operand
                                        string op = operands[0].Trim();
                                        if (registerTable.TryGetValue(op, out register1))
                                        {
                                            // Single register instruction (e.g., "CLEAR X")
                                            register2 = 0;
                                        }
                                        else if (int.TryParse(op, out register2))
                                        {
                                            // Immediate value (e.g., "SVC 2")
                                            register1 = 0;
                                            if (register2 < 0 || register2 > 15)
                                            {
                                                throw new Exception($"Immediate value out of range (0-15): {register2}");
                                            }
                                        }
                                        else
                                        {
                                            throw new Exception($"Invalid operand: {op}");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Invalid number of operands.");
                                    }

                                    // Step 3: Assemble the object code
                                    int opcodeInt = Convert.ToInt32(opcodeValue, 16);
                                    int secondByte = (register1 << 4) | register2;

                                    // Combine bytes into object code
                                    objCode = $"{opcodeInt:X2}{secondByte:X2}";
                                }
                                //if format 3
                                else if (opcodeFormat == "3")
                                {
                                    //get the beforeNix
                                    foreach (char c in opcodeValue)
                                    {
                                        beforeNix += Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0');
                                    }
                                    beforeNix = beforeNix.Substring(0, 6);
                                    //if literal
                                    if (operand.StartsWith("=0"))
                                    {
                                        //if )C+ or 0C-
                                        if ((operand != "=0C+" && operand != "=0C-") && (operand.Contains("+") || operand.Contains("-")) )
                                        {
                                            string[] vals = executeExpression.executeexpression(ref node, operand, label).Split(", ");
                                            targetAddr = vals[0];
                                            n = vals[1];
                                            i = vals[2];
                                        }
                                        //else
                                        else
                                        {
                                            //get the literal value and the targerAddr
                                            foreach (var val in literals)
                                            {
                                                if (val.StartsWith(operand))
                                                {
                                                    string[] literalParts = val.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                                    literalValue = literalParts[2];
                                                    targetAddr = literalParts[3];
                                                }
                                            }
                                        }
                                        //calculate the offset
                                        int k = (Convert.ToInt32(targetAddr, 16));
                                        int o = Convert.ToInt32(codePart[j][0], 16);
                                        offset = (k - o).ToString();
                                        //n = "0";
                                        //i = "1";
                                        e = "0";
                                        if (Int32.Parse(offset) >= -2048 && Int32.Parse(offset) <= 2047)
                                        {
                                            p = "1";
                                            b = "0";
                                        }
                                        else if (Int32.Parse(offset) < 4095)
                                        {
                                            b = "1";
                                            p = "0";
                                            offset = (Convert.ToInt32(targetAddr, 16) - Convert.ToInt32(baseRegister, 16)).ToString();
                                        }
                                        else
                                        {
                                            Console.WriteLine("ERROR - Out of range.");
                                        }
                                    }
                                    //if an expression
                                    else
                                    {
                                        //get the targer addr and n i
                                        if (operand.Contains("+") || operand.Contains("-"))
                                        {
                                            string[] vals = executeExpression.executeexpression(ref node, operand, label).Split(", ");
                                            targetAddr = vals[0];
                                            n = vals[1];
                                            i = vals[2];
                                        }
                                        else
                                        {
                                            foreach (var val in codePart)
                                            {
                                                if (operand + ":" == val[1])
                                                {
                                                    targetAddr = val[0];
                                                    break;
                                                }
                                            }
                                        }

                                        //if opcode is just a number
                                        if (operand != "" && operand.Substring(1).All(char.IsDigit))
                                        {
                                            offset = operand;
                                        }
                                        else if (operand != "" && !operand.Substring(1).All(char.IsDigit))
                                        {
                                            //calculate the offset
                                            int k = (Convert.ToInt32(targetAddr, 16));
                                            int o = Convert.ToInt32(codePart[j][0], 16);
                                            offset = (k - o).ToString();
                                            if (Int32.Parse(offset) >= -2048 && Int32.Parse(offset) <= 2047)
                                            {
                                                p = "1";
                                                b = "0";
                                            }
                                            else if(Int32.Parse(offset) < 4095)
                                            {
                                                b = "1";
                                                p = "0";
                                                offset = (Convert.ToInt32(targetAddr, 16) - Convert.ToInt32(baseRegister, 16)).ToString();
                                            }
                                            else
                                            {
                                                Console.WriteLine("ERROR - Out of range");
                                            }
                                        }
                                    }




                                    //if offset is empty assign to 0000
                                    if(offset == "")
                                    {
                                        offset = "0000";
                                    }
                                    //get the thing after the opcde
                                    string opcodeAfter = Convert.ToInt32((beforeNix + n + i + x + b + p + e), 2).ToString("X3");
                                    //if offset empty
                                    if (offset != "")
                                    {
                                        objCode = opcodeAfter + ((Int32.Parse(offset) & 0xFFF).ToString("X3"));

                                    }
                                    else
                                    {
                                        objCode = opcodeAfter;
                                    }

                                }
                            }




                        }

                    }
                    j++;
                    //if objcode is empty and the opcode is end
                    if (objCode != "" && opcode != "END")
                    {
                        //ger the text reord
                        string sup = string.Empty;
                        if (j < codePart.Count - 1)
                        {
                            int er = Convert.ToInt32(codePart[zoo + 1][0], 16);
                            int tt = Convert.ToInt32(codePart[zoo][0], 16);
                            sup = (Convert.ToInt32(er - tt).ToString("X3"));
                        }
                        textRecord.Add($"{label}^{LC}^{sup}^{objCode}");
                    }

                    //print out the values you got to the file and to the 
                    Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15} {4,-15} {5,-15} {6,-15}",
                        z, LC, label, opcode, operand, objCode, comment);
                    writer.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15} {4,-15} {5,-15} {6,-15}",
                       z, LC, label, opcode, operand, objCode, comment);
                    z++;
                    zoo++;
                }
                //print out the header file
                headerRecord = $"H^{programName}^{startingAddress}^{programLength}";
                //print out the r record
                if (extrefList != null)
                {
                    rRecord = "R^";
                    foreach (var r in extrefList)
                    {
                        rRecord += r + "^";
                    }
                    rRecord = rRecord.Substring(0, rRecord.Length - 1);
                }

                //print out the e record
                eRecord = $"E^{Convert.ToInt32(startingAddress).ToString("X6")}";






                Console.WriteLine($"Program Length: {programLength}");
                Console.WriteLine("\n-----------------------------------------SYMBOL TABLE---------------------------------------------------------");
                Console.WriteLine("{0,-10} {1,-10} {2,-10} {3,-10} {4,-10}", "Symbol", "Value", "RFALG", "IFLAG", "MFALG");
                BST.InorderTraversal(root);
                Console.WriteLine("\n-----------------------------------------Literal TABLE---------------------------------------------------------");
                Console.WriteLine("{0,-10} {1,-10} {2,-10} {3,-20}", "Literal", "Length", "VALUE", "ADDRESS");
                foreach (var list in literals)
                {
                    Console.WriteLine(list);
                }
                writer.WriteLine($"Program Length: {programLength}");
                writer.WriteLine("\n-----------------------------------------SYMBOL TABLE---------------------------------------------------------");
                writer.WriteLine("{0,-10} {1,-10} {2,-10} {3,-10} {4,-10}", "Symbol", "Value", "RFALG", "IFLAG", "MFALG");
                BST.InorderTraversalFile(root, writer);
                writer.WriteLine("\n-----------------------------------------Literal TABLE---------------------------------------------------------");
                writer.WriteLine("{0,-10} {1,-10} {2,-10} {3,-20}", "Literal", "Length", "VALUE", "ADDRESS");
                //print out the literals for the file
                foreach (var list in literals)
                {
                    writer.WriteLine(list);
                }

                int m = 1;
                Console.WriteLine(headerRecord);
                //if the textrecord is not null
                if (textRecord != null)
                {
                    for (int i = 0; i < textRecord.Count; i++)
                    {
                        if (textRecord[i].StartsWith('*'))
                        {
                            Console.WriteLine("T^" + textRecord[i].Substring(2));
                            m++;
                        }
                        else
                        {
                            string[] val = textRecord[i].Split('^');
                            if (i == 0)
                            {
                                Console.Write($"T^{val[1]}^{val[2]}^{val[3]}");
                            }
                            else
                            {
                                Console.Write($"^{val[3]}");
                            }
                        }
                    }
                    int firstCaretIndex = (textRecord[textRecord.Count - m].IndexOf('^'));

                    // If there's a ^, remove the first word
                    string result = firstCaretIndex >= 0 ? textRecord[textRecord.Count - m].Substring(firstCaretIndex + 1) : string.Empty;

                    Console.WriteLine("T^" + result);
                }
                //if modification file is not null
                if(modificationRecord != null)
                {
                    
                    foreach (var value in modificationRecord)
                    {
                        Console.WriteLine($"M{value}");
                    }
                }
                foreach (var e in extdefList)
                {
                    Console.Write(e);

                }
                if (rRecord != "R")
                {
                    Console.WriteLine(rRecord);
                }
                Console.WriteLine(eRecord);

            }
            //create the obj file and print everthing
            string objname = fileNmae.Split('.')[0] + ".obj";
            using (StreamWriter writer = new StreamWriter(new FileStream(objname, FileMode.Create, FileAccess.Write)))
            {
                int m = 1;
                writer.WriteLine(headerRecord);
                if (textRecord != null)
                {
                    for (int i = 0; i < textRecord.Count; i++)
                    {
                        if (textRecord[i].StartsWith('*'))
                        {
                            writer.WriteLine("T^" + textRecord[i].Substring(2));
                            m++;
                        }
                        else
                        {
                            string[] val = textRecord[i].Split('^');
                            if (i == 0)
                            {
                                writer.Write($"T^{val[1]}^{val[2]}^{val[3]}");
                            }
                            else
                            {
                                writer.Write($"^{val[3]}");
                            }
                        }
                    }
                    int firstCaretIndex = (textRecord[textRecord.Count - m].IndexOf('^'));

                    // If there's a ^, remove the first word
                    string result = firstCaretIndex >= 0 ? textRecord[textRecord.Count - m].Substring(firstCaretIndex + 1) : string.Empty;

                    writer.WriteLine("T^" + result);
                }
                if (modificationRecord != null)
                {

                    foreach (var value in modificationRecord)
                    {
                        writer.WriteLine($"M{value}");
                    }
                }
                writer.Write("D^");
                foreach (var e in extdefList)
                {
                    writer.Write(e);

                }
                writer.WriteLine();
                if (rRecord != "R")
                {
                    writer.WriteLine(rRecord);
                }
                writer.WriteLine(eRecord);

            }


        }
    }
}

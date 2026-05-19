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
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zerihun4
{
    public class Pass1
    {
        /********************************************************************
        *** METHOD: PassOne
        *********************************************************************
        *** DESCRIPTION : This function executes passOnce functionality and calculates the nextLocation counter
        *** INPUT ARGS : string label, string opcode, string operand, string? comment, Dictionary<string, string> opcodeDictionary, ref string locationCounter, ref string nextlocationCounter
        *** OUTPUT ARGS : N/A
        *** IN/OUT ARGS : locationcounter, nextlocationcounter
        *** RETURN : List<string>
        ********************************************************************/
        public List<string> PassOne(string label, string opcode, string operand, string? comment, Dictionary<string, string> opcodeDictionary, ref string locationCounter, ref string nextlocationCounter)
        {
            //this file is going to add a column called location counter
            // add to the symbol table
            // add to the litral table
            // validate symbols and literls
            string opcodeValue = string.Empty;
            string opcodeFormat = string.Empty;
            List<string> returnString = new();
            //if the opcode is START
            if (opcode == "START")
            {
                //the next location counter is the current locationc ounter
                nextlocationCounter = locationCounter;
                List<string> returnString2 = new();
                returnString2.Add(locationCounter);
                returnString2.Add(label);
                returnString2.Add(opcode);
                returnString2.Add(operand);
                returnString2.Add(comment);
                return returnString2;
            }
            //if the Opcode is BASE
            else if (opcode == "BASE")
            {
                //the next locationCounter is the current reprated
                returnString.Add(nextlocationCounter);
                returnString.Add(label);
                returnString.Add(opcode);
                returnString.Add(operand);
                returnString.Add(comment);
                nextlocationCounter = ((Int32.Parse(nextlocationCounter) + 0).ToString());

            }
            //if the OPCode is END 
            else if (opcode == "END")
            {
                //do not calculate the nextlocation counter
                returnString.Add(nextlocationCounter);
                returnString.Add(label);
                returnString.Add(opcode);
                returnString.Add(operand);
                returnString.Add(comment);
            }
            //if the OPCODE is RESB
            else if (opcode == "RESB")
            {
                //Calculate the next location counter
                returnString.Add(nextlocationCounter);
                returnString.Add(label);
                returnString.Add(opcode);
                returnString.Add(operand);
                returnString.Add(comment);
                //if the operand contains # or @
                if (operand.Contains('#') || operand.Contains('@'))
                {
                    nextlocationCounter = (Int32.Parse(nextlocationCounter) + (Int32.Parse(operand.Substring(1)) * 1)).ToString();
                }
                //if the operand does not contain # or @
                else
                {
                    nextlocationCounter = (Int32.Parse(nextlocationCounter) + (Int32.Parse(operand) * 1)).ToString();
                }

            }
            //if the opcode is RESW
            else if (opcode == "RESW")
            {
                //caluclte the next location counter
                returnString.Add(nextlocationCounter);
                returnString.Add(label);
                returnString.Add(opcode);
                returnString.Add(operand);
                returnString.Add(comment);
                //if the operand countains # or @
                if (operand.Contains('#') || operand.Contains('@'))
                {
                    nextlocationCounter = (Int32.Parse(nextlocationCounter) + (Int32.Parse((operand.Substring(1))) * 3)).ToString();
                }
                //if the operand does not contain # or @
                else
                {
                    nextlocationCounter = (Int32.Parse(nextlocationCounter) + (Int32.Parse((operand)) * 3)).ToString();
                }

            }
            //if the opcode is BYTE
            else if (opcode == "BYTE")
            {
                //if it is hex divide the opcde length by 2
                if (operand.Contains('X'))
                {
                    returnString.Add(nextlocationCounter);
                    returnString.Add(label);
                    returnString.Add(opcode);
                    returnString.Add(operand);
                    returnString.Add(comment); operand = operand.Substring(2);
                    nextlocationCounter = (Int32.Parse(nextlocationCounter) + (operand.Length / 2)).ToString();
                }
                //if it is a char just andd the length
                else if (operand.Contains('C'))
                {
                    returnString.Add(nextlocationCounter);
                    returnString.Add(label);
                    returnString.Add(opcode);
                    returnString.Add(operand);
                    returnString.Add(comment); operand = operand.Substring(2);
                    nextlocationCounter = (Int32.Parse(nextlocationCounter) + (operand.Length)).ToString();
                }
            }
            //if the opcode is WORD
            else if (opcode == "WORD")
            {
                //calculate the next location counter
                returnString.Add(nextlocationCounter);
                returnString.Add(label);
                returnString.Add(opcode);
                returnString.Add(operand);
                returnString.Add(comment);
                nextlocationCounter = ((3) + Int32.Parse(nextlocationCounter)).ToString();
            }
            //if the opcode is EQU
            else if (opcode == "EQU")
            {
                //calculate the next location counter
                returnString.Add(nextlocationCounter);
                returnString.Add(label);
                returnString.Add(opcode);
                returnString.Add(operand);
                returnString.Add(comment);
                nextlocationCounter = ((Int32.Parse(nextlocationCounter) + 0)).ToString();
            }
            //if the opcode is EXTREF
            else if (opcode == "EXTREF")
            {
                //recognize it and skip it
                //it is used in pass 2
                returnString.Add("00000");
                returnString.Add(label);
                returnString.Add(opcode);
                returnString.Add(operand);
                returnString.Add(comment);
            }
            //if the opcode is EXTDEF
            else if (opcode == "EXTDEF")
            {
                //recognize it and skip it
                //it is used in pass 2
                returnString.Add("00000");
                returnString.Add(label);
                returnString.Add(opcode);
                returnString.Add(operand);
                returnString.Add(comment);
            }
            else
            {
                //calculate the next location counter
                returnString.Add(nextlocationCounter);
                returnString.Add(label);
                returnString.Add(opcode);
                returnString.Add(operand);
                returnString.Add(comment);
                //get the value of the opcode and it's format
                string[] opcodeLines;
                //if the opcode is format 4
                if (opcode.Contains('+'))
                {
                    opcodeLines = opcodeDictionary[opcode.Substring(1)].Split(' ');
                }
                //if the opcode is format3
                else
                {
                    opcodeLines = opcodeDictionary[opcode].Split(' ');
                }
                opcodeValue = opcodeLines[0].Trim();
                opcodeFormat = opcodeLines[1].Trim();
                //if it is format 4 add 1 to it
                if (opcode.Contains('+'))
                {
                    opcodeFormat = (Int32.Parse(opcodeFormat) + 1).ToString();
                }
                nextlocationCounter = (Int32.Parse(nextlocationCounter) + Int32.Parse(opcodeFormat)).ToString();
            }


            return returnString;

        }
        static string StringToHex(string text)
        {
            // Try to parse the string as an integer
            if (int.TryParse(text, out int number))
            {
                // If it's a valid number, convert it to hex
                return number.ToString("X");
            }
            else
            {
                // If it's not a number, convert each character to hex
                StringBuilder hex = new StringBuilder();
                foreach (char c in text)
                {
                    hex.Append(((int)c).ToString("X2"));
                }
                return hex.ToString();
            }
        }
    }
}

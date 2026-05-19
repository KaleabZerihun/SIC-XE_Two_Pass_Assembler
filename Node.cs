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
using System.Text;
using System.Threading.Tasks;

namespace Zerihun4
{
    public class Node
    {
        public string SYMBOL { get; set; }
        public int VALUE { get; set; }
        public bool RFLAG { get; set; }
        public bool IFLAG { get; set; }
        public bool MFLAG { get; set; }
        public Node? LEFTNODE { get; set; }
        public Node? RIGHTNODE { get; set; }

        /********************************************************************
        *** METHOD: Node
        *********************************************************************
        *** DESCRIPTION : parametrized constructor for Node class
        *** INPUT ARGS : string symbol, int value, bool rflag
        *** OUTPUT ARGS : N/A
        *** IN/OUT ARGS : N/A
        *** RETURN : N/A
        ********************************************************************/
        public Node(string symbol, int value, bool rflag)
        {
            SYMBOL = symbol;
            VALUE = value;
            RFLAG = rflag;
            IFLAG = true;
            MFLAG = false;
        }
    }
}

/********************************************************************
*** NAME : Kaleab Zerihun
*** CLASS : CSc 354
*** ASSIGNMENT : Assignment 4
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
using Zerihun4;

namespace Zerihun4
{
    public class BST
    {
        /********************************************************************
       *** METHOD: Insert
       *********************************************************************
       *** DESCRIPTION : Insetts the node into the BST
       *** INPUT ARGS : Node? node, string symbol, int value, bool rflag
       *** OUTPUT ARGS : N/A
       *** IN/OUT ARGS : N/A
       *** RETURN : Node
       ********************************************************************/
        public static Node Insert(Node? node, string symbol, int value, bool rflag)
        {
            //if we do not have a root node or if the node is null
            //create a new one
            if (node == null)
            {
                return new Node(symbol, value, rflag);
            }

            //look for duplicates
            if (symbol == node.SYMBOL)
            {
                Console.WriteLine($"ERROR - The symbol " + symbol + " already exists");
                node.MFLAG = true;
                Environment.Exit(0);
            }

            //if symbole is less than current node symbole
            if (string.Compare(symbol, node.SYMBOL) < 0)
            {
                node.LEFTNODE = Insert(node.LEFTNODE, symbol, value, rflag);
            }
            //if symbol is greater than current node symbol
            else if ((string.Compare(symbol, node.SYMBOL) > 0))
            {
                node.RIGHTNODE = Insert(node.RIGHTNODE, symbol, value, rflag);
            }
            return node;
        }
        /********************************************************************
        *** METHOD: Search
        *********************************************************************
        *** DESCRIPTION : finds the value that is being searched in the binary search tree
        *** INPUT ARGS : Node? node, string searchSymbole
        *** OUTPUT ARGS : N/A
        *** IN/OUT ARGS : N/A
        *** RETURN : Node?
        ********************************************************************/
        public static Node? Search(Node? node, string searchSymbole)
        {
            // if node is null tell the user that the symbole they searched for is not found     
            if (node == null)
            {
                //Console.WriteLine($"ERROR - {searchSymbole} not found in symbol table");
                //Environment.Exit(0);
                return null;
            }

            //if symbole is greater than current node symbole
            if ((string.Compare(searchSymbole, node.SYMBOL) > 0))
            {
                return Search(node.RIGHTNODE, searchSymbole);
            }
            //if symbol is less than current node symbol
            else if ((string.Compare(searchSymbole, node.SYMBOL) < 0))
            {
                return Search(node.LEFTNODE, searchSymbole);
            }
            // else tell the user that we have found the symbol
            else
            {
                //Console.WriteLine($"Found - {node.SYMBOL}, {node.VALUE}, {node.RFLAG}, {node.IFLAG}, {node.MFLAG}");
                return node;
            }

        }
        /********************************************************************
        *** METHOD: InorderTraversal
        *********************************************************************
        *** DESCRIPTION : prints the symbols in increasing order
        *** INPUT ARGS : Node? node
        *** OUTPUT ARGS : N/A
        *** IN/OUT ARGS : N/A
        *** RETURN : void
        ********************************************************************/
        public static void InorderTraversal(Node? node)
        {
            //if node is null 
            if (node == null)
            {
                return;
            }
            //traverse left node
            InorderTraversal(node.LEFTNODE);
            //print node
            Console.WriteLine("{0,-10} {1,-10} {2,-10} {3,-10} {4,-10}", node.SYMBOL, (node.VALUE).ToString("X5"), node.RFLAG, Convert.ToInt32(node.IFLAG), Convert.ToInt32(node.MFLAG));
            //traverse right node
            InorderTraversal(node.RIGHTNODE);
        }

        /********************************************************************
       *** METHOD: InorderTraversal
       *********************************************************************
       *** DESCRIPTION : prints the symbols in increasing order on the lst file
       *** INPUT ARGS : Node? node, StreamWriter writer
       *** OUTPUT ARGS : N/A
       *** IN/OUT ARGS : N/A
       *** RETURN : void
       ********************************************************************/
        public static void InorderTraversalFile(Node? node, StreamWriter writer)
        {
            //if node is null 
            if (node == null)
            {
                return;
            }
            //traverse left node
            InorderTraversalFile(node.LEFTNODE, writer);
            //print node
            writer.WriteLine("{0,-10} {1,-10} {2,-10} {3,-10} {4,-10}", node.SYMBOL, (node.VALUE).ToString("X5"), node.RFLAG, Convert.ToInt32(node.IFLAG), Convert.ToInt32(node.MFLAG));
            //traverse right node
            InorderTraversalFile(node.RIGHTNODE, writer);
        }
    }
}

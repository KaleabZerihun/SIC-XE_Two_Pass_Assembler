# Two-Pass Assembler

Two-Pass Assembler is a C# console application that simulates the two-pass assembly process used by assemblers. The program reads assembly-style source code, processes labels, symbols, literals, and expressions, and then generates the information needed to translate the source program into machine-level object code.

The project demonstrates how an assembler works internally by separating the process into two major stages: Pass 1 and Pass 2. Pass 1 focuses on scanning the source program, building symbol and literal tables, and calculating addresses. Pass 2 uses that information to generate the final translated output.

## Project Purpose

This project was created to better understand how system software works, especially how assemblers translate assembly language into machine code.

The main goal of the project is to demonstrate the logic behind a two-pass assembler, including address calculation, symbol management, literal handling, and expression evaluation. It also shows how data structures such as binary search trees can be used to store and search assembler-related information efficiently.

## Features

- Two-pass assembler structure
- Pass 1 processing
- Pass 2 processing
- Symbol table management
- Literal table management
- Binary search tree implementation
- Expression evaluation
- Address and location counter handling
- Assembly-style source processing
- Console-based output
- Object-oriented C# design

## How It Works

### Pass 1

Pass 1 reads the source program and collects important information before final translation happens.

Pass 1 is responsible for:

- Reading each line of source code
- Tracking the location counter
- Identifying labels and symbols
- Storing symbols in the symbol table
- Detecting literals
- Building the literal table
- Preparing information needed for Pass 2

### Pass 2

Pass 2 uses the symbol table, literal table, and address information created during Pass 1.

Pass 2 is responsible for:

- Resolving symbols and literals
- Evaluating expressions
- Generating translated output
- Producing assembler output based on the processed source program

## Technologies Used

- C#
- .NET
- Console Application
- Object-Oriented Programming
- Binary Search Tree
- Data Structures
- Assembly Language Concepts
- Two-Pass Assembler Design

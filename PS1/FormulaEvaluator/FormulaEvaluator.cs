using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    /// <summary>
    /// Evaluator class that contains a function that evaluates a mathematical string expression 
    /// and returns its numerical value.
    /// SORRY FOR POOR MODULARIZING, WILL DO BETTER ON THAT NEXT TIME :/
    /// </summary>
    public static class Evaluator
    {
        /// <summary>
        /// Declares a lookup function to lookup variables.
        /// Must be provided by tester.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public delegate int Lookup(String v);

        /// <summary>
        /// Takes in a mathematical expression as a string and returns its numerical value
        /// Does not accept negative numbers
        /// </summary>
        /// <param name="expression">Mathematical string expression</param>
        /// <param name="VariableEvaluator">I don't know what this is</param>
        /// <returns></returns>
        public static int Evaluate(String expression, Lookup VariableEvaluator)
        {
            string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");       //Creates an array of strings consists of substrings of original expression
            Stack<double> operandStack = new Stack<double>();                                       //Creates an int stack to keep track of operands
            Stack<string> operatorStack = new Stack<string>();                                      //Creates a string stack to keep track of operators
            string current;
            double num;
            double intTemp;
            double intTemp2;
            string stringTemp;

            for (int i = 0; i < substrings.Length; i++)
            {
                current = substrings[i];                                    //Step through substring
                current = current.Trim();                                   //Trim ignores leading and trailing white space

                if (double.TryParse(current, out num))                      //Try and parse string to int
                {
                    if (operandStack.Count() == 0)                          //If value stack is empty, push current int onto stack
                    {
                        operandStack.Push(num);
                    }
                    else
                    {
                        if (operatorStack.Peek().Equals("*"))               //If operator is *
                        {
                            intTemp = operandStack.Pop();                   //Pop operand from operand stack
                            stringTemp = operatorStack.Pop();               //Pop * operator
                            intTemp = num * intTemp;                        //Multiply the two operands
                            operandStack.Push(intTemp);                     //Push result onto stack
                        }

                        else if (operatorStack.Peek().Equals("/"))          //If operator is /
                        {
                            try
                            {
                                if (num == 0)
                                {
                                    throw new ArgumentException("Division by zero error.");
                                }
                                intTemp = operandStack.Pop();                   //Pop operand from operand stack
                                stringTemp = operatorStack.Pop();               //Pop / operator
                                intTemp = intTemp / num;                        //Divide first operand by second operand
                                operandStack.Push(intTemp);                     //Push result onto stack
                            }
                            catch (ArgumentException e)
                            {
                                Console.WriteLine("Division by zero error.");
                            }
                        }

                        else                                                //If neither, then push operand onto operand stack
                        {
                            operandStack.Push(num);
                        }
                    }
                }

                else if (isValidVariable(current))                          //Check to see whether current sub string is potentially a variable, if so proceed as if it were integer
                {
                    try
                    {
                        num = VariableEvaluator(current);                   //Look up the variable
                    }
                    catch (ArgumentException e)                             //Throw exception if variable not found
                    {
                        Console.WriteLine("The variable is undefined.");
                    }

                    if (operandStack.Count() == 0)                          //If value stack is empty, push current int onto stack
                    {
                        operandStack.Push(num);
                    }
                    else
                    {
                        if (operatorStack.Peek().Equals("*"))               //If operator is *
                        {
                            intTemp = operandStack.Pop();                   //Pop operand from operand stack
                            stringTemp = operatorStack.Pop();               //Pop * operator
                            intTemp = num * intTemp;                        //Multiply the two operands
                            operandStack.Push(intTemp);                     //Push result onto stack
                        }

                        else if (operatorStack.Peek().Equals("/"))          //If operator is /
                        {
                            try
                            {
                                if (num == 0)
                                {
                                    throw new ArgumentException("Division by zero error.");
                                }
                                intTemp = operandStack.Pop();                   //Pop operand from operand stack
                                stringTemp = operatorStack.Pop();               //Pop / operator
                                intTemp = intTemp / num;                        //Divide first operand by second operand
                                operandStack.Push(intTemp);                     //Push result onto stack
                            }
                            catch (ArgumentException e)
                            {
                                Console.WriteLine("Division by zero error.");
                            }
                        }

                        else                                                //If neither, then push operand onto operand stack
                        {
                            operandStack.Push(num);
                        }
                    }
                }

                else
                {
                    if (current.Equals("*"))                                //Process a * string by pushing onto stack
                    {
                        operatorStack.Push(current);
                    }

                    else if (current.Equals("/"))                           //Process a / string by pushing onto stack
                    {
                        operatorStack.Push(current);
                    }

                    else if (current.Equals("("))                           //Process a ( string by pushing on to stack
                    {
                        operatorStack.Push(current);
                    }

                    else if (current.Equals(")"))                           //Process a ) string
                    {
                        if (operatorStack.Peek().Equals("+") && operandStack.Count > 1)
                        {
                            intTemp = operandStack.Pop();                   //Pop first operand
                            intTemp2 = operandStack.Pop();                  //Pop second operand
                            operatorStack.Pop();
                            intTemp = intTemp + intTemp2;                   //Add the two operands 
                            operandStack.Push(intTemp);                     //Push result onto stack
                        }

                        else if (operatorStack.Peek().Equals("-") && operandStack.Count > 1)
                        {
                            intTemp = operandStack.Pop();                   //Pop first operand
                            intTemp2 = operandStack.Pop();                  //Pop second operand
                            operatorStack.Pop();
                            intTemp = intTemp2 - intTemp;                   //Subtract second operand from first operand
                            operandStack.Push(intTemp);                     //Push result onto stack
                        }

                        else if (current.Equals("-"))                       //Process a - string 
                        {
                            if (operatorStack.Peek().Equals("+") && operandStack.Count > 1)
                            {
                                intTemp = operandStack.Pop();               //Pop first operand
                                intTemp2 = operandStack.Pop();              //Pop second operand
                                operatorStack.Pop();
                                intTemp = intTemp + intTemp2;               //Add the two operands 
                                operandStack.Push(intTemp);                 //Push result onto stack
                            }

                            else if (operatorStack.Peek().Equals("-") && operandStack.Count > 1)
                            {
                                intTemp = operandStack.Pop();               //Pop first operand
                                intTemp2 = operandStack.Pop();              //Pop second operand
                                operatorStack.Pop();
                                intTemp = intTemp2 - intTemp;               //Subtract second operand from first operand
                                operandStack.Push(intTemp);                 //Push result onto stack
                            }

                            operatorStack.Push(current);                    //Push + operator onto stack
                        }

                        try
                        {
                            if (operatorStack.Peek().Equals("("))           //Try and look for opening parenthesis
                            {
                                operatorStack.Pop();
                            }
                            else
                            {
                                throw new ArgumentException("Opening parenthesis ( not found.");        //If not found, throw error
                            }
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine("Opening parenthesis ( not found.");
                        }

                        if (operatorStack.Count != 0)
                        {
                            if (operatorStack.Peek().Equals("*"))               //If operator is *
                            {
                                intTemp = operandStack.Pop();                   //Pop operand from operand stack
                                intTemp2 = operandStack.Pop();
                                stringTemp = operatorStack.Pop();               //Pop * operator
                                intTemp = intTemp * intTemp2;                   //Multiply the two operands
                                operandStack.Push(intTemp);                     //Push result onto stack
                            }

                            else if (operatorStack.Peek().Equals("/"))          //If operator is /
                            {
                                intTemp = operandStack.Pop();                   //Pop operand from operand stack
                                stringTemp = operatorStack.Pop();               //Pop / operator
                                intTemp2 = operandStack.Pop();
                                if (intTemp != 0)
                                {
                                    intTemp = intTemp2 / intTemp;               //Divide first operand by second operand
                                    operandStack.Push(intTemp);                 //Push result onto stack
                                }
                                else
                                {
                                    Console.WriteLine("Divide by zero error");
                                    throw new ArgumentException("Cannot divide by 0");  
                                }
                            }
                        }
                    }

                    else if (current.Equals("+"))                           //Process a + string
                    {
                        if (operatorStack.Count != 0 && operatorStack.Peek() == "+" && (operandStack.Count() > 1.0))
                        {
                            intTemp = operandStack.Pop();                   //Pop first operand
                            intTemp2 = operandStack.Pop();                  //Pop second operand
                            operatorStack.Pop();
                            intTemp = intTemp + intTemp2;                   //Add the two operands 
                            operandStack.Push(intTemp);                     //Push result onto stack
                        }

                        else if (operatorStack.Count != 0 && operatorStack.Peek() == "-" && (operandStack.Count > 1))
                        {
                            intTemp = operandStack.Pop();                   //Pop first operand
                            intTemp2 = operandStack.Pop();                  //Pop second operand
                            operatorStack.Pop();
                            intTemp = intTemp2 - intTemp;                   //Subtract second operand from first operand
                            operandStack.Push(intTemp);                     //Push result onto stack
                        }

                        operatorStack.Push(current);                        //Push + operator onto stack
                    }

                    else if (current.Equals("-"))                           //Process a - string 
                    {
                        if (operatorStack.Count != 0 && operatorStack.Peek() == "+" && (operandStack.Count > 1))
                        {
                            intTemp = operandStack.Pop();                   //Pop first operand
                            intTemp2 = operandStack.Pop();                  //Pop second operand
                            operatorStack.Pop();
                            intTemp = intTemp + intTemp2;                   //Add the two operands 
                            operandStack.Push(intTemp);                     //Push result onto stack
                        }

                        else if (operatorStack.Count != 0 && operatorStack.Peek() == "-" && (operandStack.Count > 1))
                        {
                            intTemp = operandStack.Pop();                   //Pop first operand
                            intTemp2 = operandStack.Pop();                  //Pop second operand
                            operatorStack.Pop();
                            intTemp = intTemp2 - intTemp;                   //Subtract second operand from first operand
                            operandStack.Push(intTemp);                     //Push result onto stack
                        }

                        operatorStack.Push(current);                        //Push + operator onto stack
                    }

                    else if (current.Equals(""))                            //Deal with null character, ignore and move on to next character
                    {
                        continue;
                    }

                    else
                    {                                                           //If character is not a digit or a variable or a mathematical operator then throw error
                        Console.WriteLine("Undefined character encountered");
                        throw new ArgumentException("Undefined character encountered");
                    }
                }
            }

            if (operatorStack.Count == 0)                                   //After processing through string, if operator stack is empty then pop operand stack and return value. 
            {
                if (operandStack.Count == 1)
                {
                    return (int)operandStack.Pop();
                }

                else
                {                                                            //Report error if there isn't exactly 1 value on the operand stack
                    throw new ArgumentException("There isn't exactly 1 value on the operand stack");

                }
            }

            else
            {                                                               //If operator stack not empty, then there should be 2 operands and 1 operator. The operator must be + or -, else report error
                if (operandStack.Count == 2 && operatorStack.Count == 1)
                {
                    if (operatorStack.Peek() == "+")                        //If operator is +, pop and add two remaining operands and return value
                    {
                        operatorStack.Pop();
                        intTemp = operandStack.Pop();
                        intTemp2 = operandStack.Pop();
                        num = intTemp + intTemp2;

                        return (int)num;
                    }

                    else if (operatorStack.Peek() == "-")                   //If operator is -, pop and subtract two remaining operands and return value
                    {
                        operatorStack.Pop();
                        intTemp = operandStack.Pop();
                        intTemp2 = operandStack.Pop();
                        num = intTemp2 - intTemp;

                        return (int)num;
                    }

                    else
                    {                                                       //If remaining operator is not + or -
                        throw new ArgumentException("The last operator is not a + or -");
                    }
                }

                else
                {                                                           //If there isn't 2 operands or 1 operator remaining, report error
                    throw new ArgumentException("There isn't exactly 2 values on the operand stack and 1 operator on the operator stack");
                }
            }
        }

        /// <summary>
        /// Takes a string a test whether it is in valid ASCII
        /// If it is, then it is a valid variable
        /// </summary>
        /// <param name="s">string s</param>
        /// <returns>true if valid ascii, false otherwise</returns>
        public static Boolean isValidVariable(String s)
        {
            char[] test = s.ToCharArray();                                  //Split the string up into characters and store in char array
            int testCharASCII;
            int length = test.Length;

            if (test.Length == 0)                                          //To deal with null character
            {
                return false;
            }

            for (int i = 0; i < test.Length; i++)
            {
                testCharASCII = test[i];

                if (i == 0)                                                                                                                 //Check to see if first character of variable is a letter.
                {
                    if (!(((int)testCharASCII >= 65 & (int)testCharASCII <= 90) | ((int)testCharASCII >= 97 & (int)testCharASCII <= 122)))
                    {
                        return false;
                    }
                    continue;
                }

                else if (!(((int)testCharASCII >= 65 & (int)testCharASCII <= 90) | ((int)testCharASCII >= 97 & (int)testCharASCII <= 122))) //Check to see if current character is an uppercase or lowercase letter
                {
                    if (!(((int)testCharASCII >= 48 & (int)testCharASCII <= 57)))                                                           //Check to see if current character is a digit
                    {
                        return false;                                                                                                       //If neither, return false
                    }
                }       
            }
            return true;
        }
    }
}

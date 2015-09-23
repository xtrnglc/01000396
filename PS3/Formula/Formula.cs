/*
Author: Trung Le
Course: CS 3500
Date: 09/21/2015
Purpose: Formula Class
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax; variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        private LinkedList<string> variableList = new LinkedList<string>();
        private String validFormula;
        public delegate int lookup(String v);

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
           
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            int numberOfLeftParentheses = 0;
            int numberOfRightParentheses = 0;
            string temp;
            double number;
            string previous = null;
            StringBuilder formulaString = new StringBuilder();

            // Check for empty string, throw error if empty
            if (formula == "")
            {
                throw new FormulaFormatException("The formula string cannot be empty");
            }

            foreach (string s in GetTokens(formula))
            {
                // Deal with first token
                if (previous == null)
                {
                    if (s == "(")
                    {
                        formulaString.Append(s);
                        numberOfLeftParentheses++;
                        previous = s;
                    }

                    else if (isVariable(s))
                    {
                        temp = normalize(s);
                        if (!isValid(temp))
                        {
                            throw new FormulaFormatException("Variable failed the is valid test");
                        }

                        formulaString.Append(temp);
                        previous = temp;
                        variableList.AddFirst(temp);
                    }

                    else if (Double.TryParse(s, out number))
                    {
                        formulaString.Append(number);
                        previous = s;
                    }

                    else
                        throw new FormulaFormatException("First token needs to be either a double, a variable or a left parenthesis");

                    continue;
                }

                // Deal with variables
                if (isVariable(s))
                {

                    temp = normalize(s);
                    if (!isValid(temp))
                    {
                        throw new FormulaFormatException("Variable failed the is valid test");
                    }

                    if (previous == "(" | isOperator(previous))
                    {
                        formulaString.Append(temp);
                        previous = temp;

                        if (!variableList.Contains(temp))
                        {
                            variableList.AddLast(temp);
                        }
                    }

                    else
                    {
                        throw new FormulaFormatException("A variable must be preceded by an opening parenthesis or an operator");
                    }
                }

                // Deal with operators
                else if (isOperator(s))
                {
                    if (isVariable(previous) | previous == ")" | double.TryParse(previous, out number))
                    {
                        formulaString.Append(s);
                        previous = s;
                    }
                    else
                    {
                        throw new FormulaFormatException("An operator must be preceded by either a variable, a number or a closing parenthesis");
                    }
                }

                // Deal with numbers
                else if (double.TryParse(s, out number))
                {
                    if (isOperator(previous) | previous == "(")
                    {
                        formulaString.Append(number);
                        previous = s;
                    }
                    else
                    {
                        throw new FormulaFormatException("A number must be preceded by either an operator or an opening parenthesis");
                    }
                }

                // Deal with opening parenthesis
                else if (s == "(")
                {
                    if (isOperator(previous) | previous == "(")
                    {
                        formulaString.Append(s);
                        previous = s;
                        numberOfLeftParentheses++;
                    }
                    else
                    {
                        throw new FormulaFormatException("An opening parenthesis must be preceded by either an operator or another opening parenthesis");
                    }
                }

                // Deal with closing parenthesis
                else if (s == ")")
                {
                    if (isVariable(previous) | double.TryParse(previous, out number) | previous == ")")
                    {
                        formulaString.Append(s);
                        previous = s;
                        numberOfRightParentheses++;
                    }
                }

                else
                {
                    throw new FormulaFormatException("Unrecognized token");
                }
            }

            // If unbalanced parentheses encountered, throw error
            if (numberOfLeftParentheses != numberOfRightParentheses)
            {
                throw new FormulaFormatException("Number of opening parentheses do not match number of closing parentheses");
            }

            // If last token is not a number, a variable or a closing parenthesis, then throw error
            if (Double.TryParse(previous, out number) | isVariable(previous) | previous == ")")
            {
                
            }
            else
            {
                throw new FormulaFormatException("Last token must be a variable, a number of a closing parenthesis");
            }

            // If all above cases passed, then formula is valid. Create a string of the valid formula
            validFormula = formulaString.ToString();
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            {
                string[] substrings = Regex.Split(validFormula, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");     //Creates an array of strings consists of substrings of original expression
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
                                if (num == 0)
                                {
                                    return new FormulaError("Division by zero error.");
                                }
                                intTemp = operandStack.Pop();                   //Pop operand from operand stack
                                stringTemp = operatorStack.Pop();               //Pop / operator
                                intTemp = intTemp / num;                        //Divide first operand by second operand
                                operandStack.Push(intTemp);                     //Push result onto stack
                            }

                            else                                                //If neither, then push operand onto operand stack
                            {
                                operandStack.Push(num);
                            }
                        }
                    }

                    else if (isVariable(current))                               //Check to see whether current sub string is potentially a variable, if so proceed as if it were integer
                    {

                        num = lookup(current);                              //Look up the variable

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
                                    if (num == 0)
                                    {
                                        return new FormulaError("Division by zero error.");
                                    }
                                    intTemp = operandStack.Pop();                   //Pop operand from operand stack
                                    stringTemp = operatorStack.Pop();               //Pop / operator
                                    intTemp = intTemp / num;                        //Divide first operand by second operand
                                    operandStack.Push(intTemp);                     //Push result onto stack
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

                            if (operatorStack.Peek().Equals("("))           //Try and look for opening parenthesis
                            {
                                operatorStack.Pop();
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
                                        return new FormulaError("Cannot divide by 0");
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

                    }
                }

                if (operatorStack.Count == 0)                                   //After processing through string, if operator stack is empty then pop operand stack and return value. 
                {
                        return operandStack.Pop();
                }

                else
                {                                                               //If operator stack not empty, then there should be 2 operands and 1 operator. The operator must be + or -, else report error
                    if (operatorStack.Peek() == "+")                        //If operator is +, pop and add two remaining operands and return value
                    {
                        operatorStack.Pop();
                        intTemp = operandStack.Pop();
                        intTemp2 = operandStack.Pop();
                        num = intTemp + intTemp2;

                        return num;
                    }

                    else                  //If operator is -, pop and subtract two remaining operands and return value
                    {
                        operatorStack.Pop();
                        intTemp = operandStack.Pop();
                        intTemp2 = operandStack.Pop();
                        num = intTemp2 - intTemp;

                        return num;
                    }

                }
            }
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            foreach (string s in variableList)
            {
                yield return s;
            }
        }


        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            return validFormula.Trim();
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens, which are compared as doubles, and variable tokens,
        /// whose normalized forms are compared as strings.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null)      
                return false;

            if (obj.GetType() != this.GetType())
                return false;

            Formula temp = (Formula)obj;

            string tempString = this.ToString();
            string objTempString = obj.ToString();

            if (tempString.Equals(objTempString))
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            if (f1.Equals(f2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
                if (f1.Equals(f2))
                {
                    return false;
                }
                else
                {
                    return true;
                }
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return validFormula.GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }

        /// <summary>
        /// Check to see if a string is a valid variable.
        /// </summary>
        /// <param name="s">String</param>
        /// <returns>Boolean</returns>
        private static Boolean isVariable(String s)
        {
            return Regex.IsMatch(s, "^[a-zA-Z]+[1-9][0-9]*$");
        }

        /// <summary>
        /// Check to see if a string is a valid operator.
        /// </summary>
        /// <param name="s">String</param>
        /// <returns>Boolean</returns>
        private static Boolean isOperator(String s)
        {
            if (s == "+" | s == "*" | s == "-" | s == "/")
                return true;
            else
                return false;
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}




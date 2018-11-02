using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SN_Maths_Eval
{

    public enum TokenType // Each different type of value that can be encountered
    {
        None,
        Number,
        Add,
        Subtract,
        Multiply,
        Divide,
        LeftBracket,
        RightBracket
    }

    public struct Token //Tokens for each value encountered
    {
        public TokenType tokenValueType;
        public string tokenValue;
    }

    class MathsEvaluator //This class contains methods which allow users to solve simple algebraic expressions. Converts string to Reverse Polish Notation to allow it to be evaluated.
    {
        private Queue output;
        private Stack operators;


        public bool Evaluate(string expression) // Evaluates a provided string containing an algebraic expression
        {
            String parsedExpression;
            if (CheckBrackets(expression)) {
                expression = FormatExpression(expression);
                parsedExpression = ParseExpression(expression);
                Stack result = new Stack();
                int operator1 = 0, operator2 = 0;
                Token token = new Token();
                // if there are any tokens left
                foreach (object obj in output)
                {
                    token = (Token)obj;
                    switch (token.tokenValueType)
                    {
                        case TokenType.Number:
                            // if the token is a number push it onto the stack
                            result.Push(int.Parse(token.tokenValue));
                            break;
                        case TokenType.Add:
                            // if there are less than 2 numbers in the stack -- require 2 for any calculation
                            if (result.Count >= 2)
                            {
                                // pop the top n values from the stack
                                operator2 = (int)result.Pop();
                                operator1 = (int)result.Pop();
                                // Evaluate the function, with the values as arguments.
                                // Push the returned results, if any, back onto the stack.
                                result.Push(operator1 + operator2);
                            }
                            else
                            {
                                throw new Exception("Calculation error!");
                            }
                            break;
                        case TokenType.Subtract:
                            // if there are less than 2 numbers in the stack -- require 2 for any calculation
                            if (result.Count >= 2)
                            {
                                // pop the top n values from the stack
                                operator2 = (int)result.Pop();
                                operator1 = (int)result.Pop();
                                // evaluate and push results back in to stack
                                result.Push(operator1 - operator2);
                            }
                            else
                            {
                                throw new Exception("Calculation error!");
                            }
                            break;
                        case TokenType.Multiply:
                            // if there are less than 2 numbers in the stack -- require 2 for any calculation
                            if (result.Count >= 2)
                            {
                                // pop the top n values from the stack
                                operator2 = (int)result.Pop();
                                operator1 = (int)result.Pop();
                                // evaluate and push results back in to stack
                                result.Push(operator1 * operator2);
                            }
                            else
                            {
                                throw new Exception("Calculation error!");
                            }
                            break;
                        case TokenType.Divide:
                            // if there are less than 2 numbers in the stack -- require 2 for any calculation
                            if (result.Count >= 2)
                            {
                                // pop the top n values from the stack
                                operator2 = (int)result.Pop();
                                operator1 = (int)result.Pop();
                                // evaluate and push results back in to stack
                                result.Push(operator1 / operator2);
                            }
                            else
                            {
                                throw new Exception("Calculation error!");
                            }
                            break;
                    }
                }

                // If there is only one value in the stack
                if (result.Count == 1)
                {
                    // final result
                    int finalResult = (int)result.Pop();
                    System.Diagnostics.Debug.WriteLine(finalResult);
                    
                }
                else
                {
                    throw new Exception("Calculation error!");
                }

                return true;
            }
            else
            {
                return false;
            }
            
            
        }

        private String ParseExpression(string expression) //Converts the string expression into RPN. Making use of the Shunting Yard Algorithm
            
        {
            output = new Queue();
            operators = new Stack();

            String parsingExpression = expression; // String used during parsing phase

            // looking for numbers
            parsingExpression = Regex.Replace(parsingExpression, @"(?<number>\d+(\.\d+)?)", " ${number} ");
            // looking for symbols: + - * / ^ ( )
            parsingExpression = Regex.Replace(parsingExpression, @"(?<operators>[+\-*/^()])", " ${operators} ").Trim();

            //Tokenise the parsing expression

            String[] parsingArray = parsingExpression.Split(" ".ToCharArray());
            int tokenValue;
            Token token;
            Token operatorstoken;
            int i = 0;
            for (i = 0; i < parsingArray.Length; i++) // Iterate through the array to build tokens
            {
                token = new Token();
                token.tokenValue = parsingArray[i];
                token.tokenValueType = TokenType.None;
            

                try
                { // If the current value parses as an integer then it is set as a number
                    tokenValue = int.Parse(parsingArray[i]);
                    token.tokenValueType = TokenType.Number;
                    output.Enqueue(token);
                }
                catch
                {
                    switch (parsingArray[i])
                    {
                        case "+":
                         token.tokenValueType = TokenType.Add;
                         if (operators.Count > 0)
                           {
                            operatorstoken = (Token)operators.Peek();
                            // while there is an operator at the top of the stack
                            while (IsOperatorToken(operatorstoken.tokenValueType))
                            {
                                // pop o2 off the stack, onto the output queue;
                                output.Enqueue(operators.Pop());
                                if (operators.Count > 0)
                                {
                                    operatorstoken = (Token)operators.Peek();
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        // push o1 onto the operator stack.
                        operators.Push(token);
                        break;
                        case "-":
                            token.tokenValueType = TokenType.Subtract;
                            if (operators.Count > 0)
                            {
                                operatorstoken = (Token)operators.Peek();
                                // while there is an operator at the top of the stack
                                while (IsOperatorToken(operatorstoken.tokenValueType))
                                {
                                    // pop o2 off the stack, onto the output queue;
                                    output.Enqueue(operators.Pop());
                                    if (operators.Count > 0)
                                    {
                                        operatorstoken = (Token)operators.Peek();
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            // push o1 onto the operator stack.
                            operators.Push(token);
                            break;
                            case "*":
                                token.tokenValueType = TokenType.Multiply;
                                if (operators.Count > 0)
                                {
                                operatorstoken = (Token)operators.Peek();
                                // while there is an operator, o2, at the top of the stack
                                while (IsOperatorToken(operatorstoken.tokenValueType))
                                     {
                                        if (operatorstoken.tokenValueType == TokenType.Add || operatorstoken.tokenValueType == TokenType.Subtract)
                                        {
                                        break;
                                        }
                                        else
                                        {
                                        // Once we're in here, the following algorithm condition is satisfied.
                                        // o1 is associative or left-associative and its precedence is less than (lower precedence) or equal to that of o2, or
                                        // o1 is right-associative and its precedence is less than (lower precedence) that of o2,

                                        // pop o2 off the stack, onto the output queue;
                                            output.Enqueue(operators.Pop());
                                            if (operators.Count > 0)
                                        {
                                            operatorstoken = (Token)operators.Peek();
                                        }
                                            else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            // push o1 onto the operator stack.
                            operators.Push(token);
                            break;
                            case "/":
                                token.tokenValueType = TokenType.Divide;
                                if (operators.Count > 0)
                                {
                                operatorstoken = (Token)operators.Peek();
                                // while there is an operator, o2, at the top of the stack
                                while (IsOperatorToken(operatorstoken.tokenValueType))
                                     {
                                        if (operatorstoken.tokenValueType == TokenType.Add || operatorstoken.tokenValueType == TokenType.Subtract)
                                        {
                                        break;
                                        }
                                        else
                                        {
                                        // Once we're in here, the following algorithm condition is satisfied.
                                        // o1 is associative or left-associative and its precedence is less than (lower precedence) or equal to that of o2, or
                                        // o1 is right-associative and its precedence is less than (lower precedence) that of o2,

                                        // pop o2 off the stack, onto the output queue;
                                            output.Enqueue(operators.Pop());
                                            if (operators.Count > 0)
                                        {
                                            operatorstoken = (Token)operators.Peek();
                                        }
                                            else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            // push o1 onto the operator stack.
                            operators.Push(token);
                            break;
                            case "(":
                            token.tokenValueType = TokenType.LeftBracket;
                            // If the token is a left parenthesis, then push it onto the stack.
                            operators.Push(token);
                            break;
                            case ")":
                            token.tokenValueType = TokenType.RightBracket;
                            if (operators.Count > 0)
                            {
                                operatorstoken = (Token)operators.Peek();
                                // Until the token at the top of the stack is a left parenthesis
                                while (operatorstoken.tokenValueType != TokenType.LeftBracket)
                                {
                                    // pop operators off the stack onto the output queue
                                    output.Enqueue(operators.Pop());
                                    if (operators.Count > 0)
                                    {
                                        operatorstoken = (Token)operators.Peek();
                                    }
                                    else
                                    {
                                        // If the stack runs out without finding a left parenthesis,
                                        // then there are mismatched parentheses.
                                        throw new Exception("Unbalanced parenthesis!");
                                    }

                                }
                                // Pop the left parenthesis from the stack, but not onto the output queue.
                                operators.Pop();
                            }
                            break;
                    }

                }
            }

            while (operators.Count != 0) // When there are still operators left in the stack
            {
                operatorstoken = (Token)operators.Pop();
                // Pop the operator onto the output queue.
                output.Enqueue(operatorstoken);
            }

            String result = string.Empty;
            foreach (object obj in output)
            {
                operatorstoken = (Token)obj;
                result += string.Format("{0} ", operatorstoken.tokenValue);
            }
            System.Diagnostics.Debug.WriteLine(result);
            return result;
        }
        

        private bool IsOperatorToken(TokenType t) //Checks if the item at the top of the stack is currently an operator
        {
            bool result = false;
            switch (t)
            {
                case TokenType.Add:
                case TokenType.Subtract:
                case TokenType.Multiply:
                case TokenType.Divide:
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }

        private bool CheckBrackets(string expression) //Checks if the number of brackets is equal - if the total of "(" and ")" brackets match then proceed
        { 
        
            if ((expression.Split('(').Length - 1) == (expression.Split(')').Length - 1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private String FormatExpression(string expression) // Need to format the string to remove whitespace
        {
            StringBuilder result = new StringBuilder(); // Build string of formatted expression

            for(int i = 0; i < expression.Length; i++) // Remove any whitespace
            {
                Char cha = expression[i];
                if (Char.IsWhiteSpace(cha)) {
                    continue;
                }
                else
                {
                    result.Append(cha);
                }
            }
            return result.ToString();
        }
    }
}

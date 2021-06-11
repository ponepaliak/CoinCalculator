using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace CoinCalculator.Interpreter
{
    public class ReversePolishContainer : IReversePolishContainer
    {
        private Stack<IToken> _stack = new Stack<IToken>();
        private Stack<IToken> _output = new Stack<IToken>();

        private Dictionary<TokenTypesEnum, uint> _tokenTypeToPriority = new Dictionary<TokenTypesEnum, uint> {
            { TokenTypesEnum.plus, 2 },
            { TokenTypesEnum.minus, 2 },
            { TokenTypesEnum.multiplication, 3 },
            { TokenTypesEnum.division, 3 },
            { TokenTypesEnum.persentage, 3 },
            { TokenTypesEnum.openScope, 1 },
            { TokenTypesEnum.closedScope, 4 }
        };

        public Func<string, string, float> CurrenciesRateFinder { private get; set; }

        public Func<float, IToken> NumberToTokenConverter { private get; set; }
        public string ToCurrency { private get; set; }

        public string GetStackString()
        {
            var result = "";



            //var newStack = _stack.ToArray().ToList<IToken>();
            //newStack.Reverse();

            while (_stack.Count != 0)
            {
                _output.Push(_stack.Pop());
            }

            var newOutput = _output.ToArray().ToList<IToken>();
            newOutput.Reverse();

            foreach (var item in newOutput)
            {
                result += item.Type == TokenTypesEnum.number || item.Type == TokenTypesEnum.currency ? item.Value : item.Type.ToString();
                result += " ";
            }

            return result;
        }

        public void AddToken(IToken token)
        {
            if (token.Type == TokenTypesEnum.number || token.Type == TokenTypesEnum.currency)
            {
                _output.Push(token);
            } 
            else if (token.Type == TokenTypesEnum.closedScope)
            {
                while(_stack.Count > 0 && _stack.Peek().Type != TokenTypesEnum.openScope)
                {
                    var poppedToken = _stack.Pop();
                    _output.Push(poppedToken);
                }

                if (_stack.Count == 0)
                {
                    throw new Exception("Wrong scope direction");
                }

                _stack.Pop();
            }
            else
            {
                if (token.Type == TokenTypesEnum.openScope)
                {
                    _stack.Push(token);
                    return;
                }

                while (_stack.Count != 0 && GetPriority(_stack.Peek()) >= GetPriority(token))
                {
                    var poppedToken = _stack.Pop();
                    _output.Push(poppedToken);
                }

                _stack.Push(token);
            }
        }

        public float Calculate()
        {
            while(_stack.Count != 0)
            {
                _output.Push(_stack.Pop());
            }

            _output = ReverseStack(_output);

            while (_output.Count != 0)
            {
                if (_output.Peek().Type == TokenTypesEnum.number)
                {
                    _stack.Push(_output.Pop());
                } 
                else if (_output.Peek().Type == TokenTypesEnum.currency)
                {
                    var rate = CurrenciesRateFinder(_output.Pop().Value, ToCurrency);
                    _stack.Push(NumberToTokenConverter(rate));
                }
                else
                {
                    var secondArg = _stack.Pop();
                    var firstArg = _stack.Pop();
                    var numberResulst = GetResultOfOperation(firstArg, secondArg, _output.Pop());
                    _stack.Push(NumberToTokenConverter(numberResulst));
                }
            }

            if (_stack.Count != 1 || _stack.Peek().Type != TokenTypesEnum.number)
            {
                throw new Exception("Whrong count of arguments on stack");
            }

            var result = float.Parse(_stack.Pop().Value, CultureInfo.InvariantCulture.NumberFormat);

            return result;
        }

        private uint GetPriority(IToken token)
        {
            return _tokenTypeToPriority[token.Type];
        }

        private float GetResultOfOperation(IToken firstArg, IToken secondArg, IToken operration)
        {
            float firstNumber, secondNumber;

            try
            {
                firstNumber = float.Parse(firstArg.Value, CultureInfo.InvariantCulture.NumberFormat);
                secondNumber = float.Parse(secondArg.Value, CultureInfo.InvariantCulture.NumberFormat);
            } 
            catch
            {
                throw new Exception("Wrong number in stack");
            }

            float result = 0;

            switch (operration.Type)
            {
                case TokenTypesEnum.plus:
                    return firstNumber + secondNumber;
                case TokenTypesEnum.minus:
                    return firstNumber - secondNumber;
                case TokenTypesEnum.multiplication:
                    result = firstNumber * secondNumber;
                    return result;
                case TokenTypesEnum.division:
                    result = firstNumber / secondNumber;
                    return result;
                case TokenTypesEnum.persentage:
                    result = firstNumber * (secondNumber / 100);
                    return result;
                default:
                    throw new Exception("Operation " + operration.Type + " not recoginze");
            }
        }

        private Stack<IToken> ReverseStack(Stack<IToken> stack)
        {
            var tempStack = new Stack<IToken>();

            while(stack.Count != 0)
            {
                tempStack.Push(stack.Pop());
            }

            return tempStack;
        }
    }
}

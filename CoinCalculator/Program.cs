using CoinCalculator.Interpreter;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace CoinCalculator
{
    class Program
    {
        private static ReversePolishContainer _reversePolishContainer;
        static void Main(string[] args)
        {
            var calculator = new CoinCalculator();
            string expression = "(USD10+UAH10)%50+(RUB10*10+EUR1)%25 to UAH";
            calculator.CurrenicesList = new List<string> { "BTC", "USD", "UAH", "EUR", "RUB" };
            calculator.CurrenciesRateFinder = (fromCurrency, toCurrency) =>
            {
                var rateDict = new Dictionary<string, float>
                {
                    { "USD", 27 },
                    { "EUR", 30 },
                    { "RUB", 0.3f },
                    { "BTC", 600000 },
                    { "UAH", 1 }
                };

                return rateDict[fromCurrency];
            };
            calculator.GetCalculationResult(expression);

        }

        private static void PolishNotTest1()
        {
            _reversePolishContainer = new ReversePolishContainer();
            _reversePolishContainer.NumberToTokenConverter = number => new Token { Type = TokenTypesEnum.number, Value = number.ToString() };
            var tokens = new List<IToken>
            {
                new Token { Type = TokenTypesEnum.number, Value = "1" },
                new Token { Type = TokenTypesEnum.plus },
                new Token { Type = TokenTypesEnum.openScope },
                new Token { Type = TokenTypesEnum.number, Value = "2" },
                new Token { Type = TokenTypesEnum.minus },
                new Token { Type = TokenTypesEnum.number, Value = "3" },
                new Token { Type = TokenTypesEnum.closedScope },
                new Token { Type = TokenTypesEnum.multiplication },
                new Token { Type = TokenTypesEnum.number, Value = "4" },
            };

            var expected = "1 2 3 minus 4 multiplication plus ";

            foreach (var token in tokens)
            {
                _reversePolishContainer.AddToken(token);
            }

            var result = _reversePolishContainer.GetStackString();

            if (!expected.Equals(result))
            {
                throw new Exception("Wrong in polish test 1");
            }
        }

        private static void PolishNotTest2()
        {
            _reversePolishContainer = new ReversePolishContainer();
            _reversePolishContainer.NumberToTokenConverter = number => new Token { Type = TokenTypesEnum.number, Value = number.ToString() };

            var tokens = new List<IToken>
            {
                new Token { Type = TokenTypesEnum.number, Value = "1" },
                new Token { Type = TokenTypesEnum.plus, Value = null },
                new Token { Type = TokenTypesEnum.number, Value = "20" },
                new Token { Type = TokenTypesEnum.multiplication, Value = null },
                new Token { Type = TokenTypesEnum.openScope, Value = null },
                new Token { Type = TokenTypesEnum.number, Value = "30" },
                new Token { Type = TokenTypesEnum.minus, Value = null },
                new Token { Type = TokenTypesEnum.number, Value = "10" },
                new Token { Type = TokenTypesEnum.division, Value = null },
                new Token { Type = TokenTypesEnum.number, Value = "11.2" },
                new Token { Type = TokenTypesEnum.multiplication, Value = null },
                new Token { Type = TokenTypesEnum.openScope, Value = null },
                new Token { Type = TokenTypesEnum.number, Value = "5" },
                new Token { Type = TokenTypesEnum.plus, Value = null },
                new Token { Type = TokenTypesEnum.number, Value = "2" },
                new Token { Type = TokenTypesEnum.minus, Value = null },
                new Token { Type = TokenTypesEnum.openScope, Value = null },
                new Token { Type = TokenTypesEnum.number, Value = "11" },
                new Token { Type = TokenTypesEnum.multiplication, Value = null },
                new Token { Type = TokenTypesEnum.number, Value = "5" },
                new Token { Type = TokenTypesEnum.closedScope, Value = null },
                new Token { Type = TokenTypesEnum.closedScope, Value = null },
                new Token { Type = TokenTypesEnum.closedScope, Value = null }
            };
            

            var expected = "1 20 30 10 11.2 division 5 2 plus 11 5 multiplication minus multiplication minus multiplication plus ";

            foreach (var token in tokens)
            {
                _reversePolishContainer.AddToken(token);
            }

            var result = _reversePolishContainer.GetStackString();

            if (!expected.Equals(result))
            {
                throw new Exception("Wrong in polish test 1");
            }
        }

        private static void PolishNotTest3()
        {
            _reversePolishContainer = new ReversePolishContainer();
            _reversePolishContainer.NumberToTokenConverter = number => new Token { Type = TokenTypesEnum.number, Value = number.ToString(CultureInfo.InvariantCulture.NumberFormat) };
            var tokens = new List<IToken>
            {
                new Token { Type = TokenTypesEnum.number, Value = "1" },
                new Token { Type = TokenTypesEnum.plus },
                new Token { Type = TokenTypesEnum.openScope },
                new Token { Type = TokenTypesEnum.number, Value = "2" },
                new Token { Type = TokenTypesEnum.minus },
                new Token { Type = TokenTypesEnum.number, Value = "3" },
                new Token { Type = TokenTypesEnum.closedScope },
                new Token { Type = TokenTypesEnum.multiplication },
                new Token { Type = TokenTypesEnum.number, Value = "4" },
            };

            var expected = -3f;

            foreach (var token in tokens)
            {
                _reversePolishContainer.AddToken(token);
            }

            var result = _reversePolishContainer.Calculate();

            if (!expected.Equals(result))
            {
                throw new Exception("Wrong in polish test 1");
            }
        }

        private static void PolishNotTest4()
        {
            _reversePolishContainer = new ReversePolishContainer();
            _reversePolishContainer.NumberToTokenConverter = number => new Token { Type = TokenTypesEnum.number, Value = number.ToString() };

            var tokens = new List<IToken>
            {
                new Token { Type = TokenTypesEnum.number, Value = "1" },
                new Token { Type = TokenTypesEnum.plus, Value = null },
                new Token { Type = TokenTypesEnum.number, Value = "20.5" },
                new Token { Type = TokenTypesEnum.multiplication, Value = null },
                new Token { Type = TokenTypesEnum.openScope, Value = null },
                new Token { Type = TokenTypesEnum.number, Value = "30" },
                new Token { Type = TokenTypesEnum.minus, Value = null },
                new Token { Type = TokenTypesEnum.number, Value = "10" },
                new Token { Type = TokenTypesEnum.division, Value = null },
                new Token { Type = TokenTypesEnum.number, Value = "2" },
                new Token { Type = TokenTypesEnum.multiplication, Value = null },
                new Token { Type = TokenTypesEnum.openScope, Value = null },
                new Token { Type = TokenTypesEnum.number, Value = "5" },
                new Token { Type = TokenTypesEnum.plus, Value = null },
                new Token { Type = TokenTypesEnum.number, Value = "2" },
                new Token { Type = TokenTypesEnum.minus, Value = null },
                new Token { Type = TokenTypesEnum.openScope, Value = null },
                new Token { Type = TokenTypesEnum.number, Value = "11" },
                new Token { Type = TokenTypesEnum.multiplication, Value = null },
                new Token { Type = TokenTypesEnum.number, Value = "5" },
                new Token { Type = TokenTypesEnum.closedScope, Value = null },
                new Token { Type = TokenTypesEnum.closedScope, Value = null },
                new Token { Type = TokenTypesEnum.closedScope, Value = null }
            };


            var expected = 5536f;

            foreach (var token in tokens)
            {
                _reversePolishContainer.AddToken(token);
            }

            var result = _reversePolishContainer.Calculate();

            if (!expected.Equals(result))
            {
                throw new Exception("Wrong in polish test 1");
            }
        }

        private static void PolishNotTest5()
        {
            _reversePolishContainer = new ReversePolishContainer();
            _reversePolishContainer.NumberToTokenConverter = number => new Token { Type = TokenTypesEnum.number, Value = number.ToString().Replace(',', '.') };
            var tokens = new List<IToken>
            {
                new Token { Type = TokenTypesEnum.number, Value = "10" },
                new Token { Type = TokenTypesEnum.division },
                new Token { Type = TokenTypesEnum.number, Value = "11.2" },
                new Token { Type = TokenTypesEnum.multiplication },
                new Token { Type = TokenTypesEnum.number, Value = "2" }
            };

            var expected = 1.78571427f;

            foreach (var token in tokens)
            {
                _reversePolishContainer.AddToken(token);
            }

            var result = _reversePolishContainer.Calculate();

            if (!expected.Equals(result))
            {
                throw new Exception("Wrong in polish test 1");
            }
        }
    }
}

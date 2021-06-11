using CoinCalculator.Interpreter;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using CoinCalculator;

namespace CoinCalculatorTest
{
    public class ReversePolishContainerTest
    {
        private ReversePolishContainer _reversePolishContainer;

        [SetUp]
        public void Setup()
        {
            _reversePolishContainer = new ReversePolishContainer();
            _reversePolishContainer.NumberToTokenConverter = number => new Token { Type = TokenTypesEnum.number, Value = number.ToString() };
        }

        [Test]
        public void Test1()
        {
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

            var expected = "1 2 3 minus 4 plus multiplication ";

            foreach(var token in tokens)
            {
                _reversePolishContainer.AddToken(token);
            }

            var result = _reversePolishContainer.GetStackString();

            Console.WriteLine(result);
            

            Assert.Equals(null, expected);
        }
    }
}
using CoinCalculator.CodeReader;
using CoinCalculator.Interpreter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace CoinCalculator
{
    public class CoinCalculator : ICoinCalculator
    {
        public List<string> CurrenicesList { get; set; }

        public Func<string, string, float> CurrenciesRateFinder { get; set; }

        public KeyValuePair<string, float> GetCalculationResult(string expression)
        {
            var codeReader = new StringCodeReader();
            codeReader.Code = expression;
            var scaner = new Scaner();
            scaner.CurrenciesList = CurrenicesList;
            scaner.CodeReader = codeReader;
            var parser = new Parser();
            var stackContainer = new ReversePolishContainer();
            stackContainer.CurrenciesRateFinder = CurrenciesRateFinder;
            stackContainer.NumberToTokenConverter = number => new Token { 
                Type = TokenTypesEnum.number, 
                Value = number.ToString(CultureInfo.InvariantCulture.NumberFormat) 
            };
            parser.Scaner = scaner;
            parser.CalcContainer = stackContainer;
            scaner.ToNextToken();
            var result = parser.Calculate();

             return new KeyValuePair<string, float>("", 0);
        }
    }
}

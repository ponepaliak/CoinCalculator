using System;
using System.Collections.Generic;
using System.Text;

namespace CoinCalculator.Interpreter
{
    interface IReversePolishContainer
    {
        void AddToken(IToken token);
        string ToCurrency { set; }
        Func<string, string, float> CurrenciesRateFinder { set; }
        Func<float, IToken> NumberToTokenConverter { set; }
        float Calculate();
    }
}

using CoinCalculator.CodeReader;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoinCalculator.Interpreter
{
    interface IScaner
    {
        ICodeReader CodeReader { get; set; }
        IToken Token { get; }
        List<string> CurrenciesList { set; }
        void ToNextToken();
        bool IsEnd();
    }
}

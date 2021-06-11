using System;
using System.Collections.Generic;
using System.Text;

namespace CoinCalculator.Interpreter
{
    interface IParser
    {
        IScaner Scaner { set; }
        IReversePolishContainer CalcContainer { set; }
        KeyValuePair<string, float> Calculate();
    }
}

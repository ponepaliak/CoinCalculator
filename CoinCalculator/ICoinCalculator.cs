using System;
using System.Collections.Generic;
using System.Text;

namespace CoinCalculator
{
    interface ICoinCalculator
    {
        public List<string> CurrenicesList { set; }
        public Func<string, string, float> CurrenciesRateFinder { set; }
        public KeyValuePair<string, float> GetCalculationResult(string expression);
    }
}
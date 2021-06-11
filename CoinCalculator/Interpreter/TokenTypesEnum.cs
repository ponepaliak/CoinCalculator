using System;
using System.Collections.Generic;
using System.Text;

namespace CoinCalculator.Interpreter
{
    public enum TokenTypesEnum
    {
        currency,
        number,
        plus,
        minus,
        multiplication,
        division,
        persentage,
        openScope,
        closedScope,
        defaultValue,
        toValue,
        whitespace
    }
}

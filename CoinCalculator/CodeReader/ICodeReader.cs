using System;
using System.Collections.Generic;
using System.Text;

namespace CoinCalculator.CodeReader
{
    interface ICodeReader
    {
        void ToNextChar();
        void ToPrevChar();
        char GetCurrentChar();
        bool IsEnd();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CoinCalculator.CodeReader
{
    class StringCodeReader : ICodeReader, IReadFromString
    {
        public string Code { get; set; }
        private int _position;

        public char GetCurrentChar()
        {
            return Code[IsEnd() ? Code.Length - 1 : _position];
        }

        public bool IsEnd()
        {
            return Code.Length <= _position;
        }

        public void ToNextChar()
        {
            _position++;
        }

        public void ToPrevChar()
        {
            _position--;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoinCalculator.CodeReader
{
    class FileCodeReader : ICodeReader, IReaderFromFile
    {
        private int _position;
        private string _code;

        public char GetCurrentChar()
        {
            return _code[_position];
        }

        public bool IsEnd()
        {
            return _code.Length <= _position;
        }

        public void ToNextChar()
        {
            _position++;
        }

        public void ToPrevChar()
        {
            _position--;
        }

        public void OpenFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new Exception("File with path " + path + " not exist");
            }

            _code = File.ReadAllText(path);
        }
    }
}

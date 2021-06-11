using CoinCalculator.CodeReader;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoinCalculator.Interpreter
{
    class Scaner : IScaner
    {
        private Dictionary<string, TokenTypesEnum> _keywordTokens = new Dictionary<string, TokenTypesEnum> {
            { "to", TokenTypesEnum.toValue },
            { "default", TokenTypesEnum.toValue }
        };

        private Dictionary<string, TokenTypesEnum> _symbolTokens = new Dictionary<string, TokenTypesEnum>
        {
            { "+", TokenTypesEnum.plus },
            { "-", TokenTypesEnum.minus },
            { "*", TokenTypesEnum.multiplication },
            { "/", TokenTypesEnum.division },
            { "(", TokenTypesEnum.openScope },
            { ")", TokenTypesEnum.closedScope },
            { "%", TokenTypesEnum.persentage },
        };


        public ICodeReader CodeReader { get; set; }

        public IToken Token { private set; get; }
        public List<string> CurrenciesList { private get; set; }

        public bool IsEnd()
        {
            return CodeReader.IsEnd();
        }

        public void ToNextToken()
        {
            Token = new Token();
            RecognizeToken();
        }

        private void RecognizeToken()
        {
            char symbol = CodeReader.GetCurrentChar();

            if (Char.IsLetter(symbol))
            {
                RecognizeCharToken();
            }
            else if (Char.IsWhiteSpace(symbol))
            {
                Token.Type = TokenTypesEnum.whitespace;
                CodeReader.ToNextChar();
            }
            else if (Char.IsDigit(symbol))
            {
                RecognizeNumberToken();
            }
            else
            {
                RecognizeSymbolToken();
            }
        }

        private void RecognizeCharToken()
        {
            string tokenString = CodeReader.GetCurrentChar().ToString();
            CodeReader.ToNextChar();

            while (!CodeReader.IsEnd() && Char.IsLetter(CodeReader.GetCurrentChar()))
            {
                tokenString += CodeReader.GetCurrentChar();
                CodeReader.ToNextChar();
            }

            if (CurrenciesList.Contains(tokenString))
            {
                Token.Type = TokenTypesEnum.currency;
                Token.Value = tokenString;
                return;
            } 

            if (_keywordTokens.ContainsKey(tokenString))
            {
                Token.Type = _keywordTokens[tokenString];
                return;
            }

            throw new Exception("token '" + tokenString + "' is not valid");
        }

        private void RecognizeNumberToken()
        {
            string tokenString = CodeReader.GetCurrentChar().ToString();
            CodeReader.ToNextChar();

            while (!CodeReader.IsEnd() && (Char.IsDigit(CodeReader.GetCurrentChar()) || CodeReader.GetCurrentChar() == '.'))
            {
                tokenString += CodeReader.GetCurrentChar();
                CodeReader.ToNextChar();
            }

            float number;

            if (!float.TryParse(tokenString, out number))
            {
                throw new Exception("token '" + tokenString + "' is not valid");
            }

            Token.Type = TokenTypesEnum.number;
            Token.Value = tokenString;
        }

        private void RecognizeSymbolToken()
        {
            string tokenString = CodeReader.GetCurrentChar().ToString();
            
            if (!_symbolTokens.ContainsKey(tokenString))
            {
                throw new Exception("token '" + tokenString + "' is not valid");
            }

            Token.Type = _symbolTokens[tokenString];

            CodeReader.ToNextChar();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CoinCalculator.Interpreter
{
    class Parser : IParser
    {
        public IScaner Scaner { private get; set; }
        public IReversePolishContainer CalcContainer { private get; set; }

        private string ToCurrency { get; set; }

        public KeyValuePair<string, float> Calculate()
        {
            FullExpression();
            CalcContainer.ToCurrency = ToCurrency;
            var result = CalcContainer.Calculate();
            return new KeyValuePair<string, float>("UAH", result);
        }

        private void FullExpression()
        {
            MathExpression();

            //if (IsDefaultExpression())
            //    DefaultExpression();

            //if (IsToExpression())
                
            ToExpression();
        }

        private void MathExpression()
        {
            MultiplierExpression();
            while (IsAddition())
            {
                Addition();
                MultiplierExpression();
            }
        }

        private void DefaultExpression()
        {
            Check(TokenTypesEnum.defaultValue);
            Scaner.ToNextToken();
            Check(TokenTypesEnum.currency);
            Scaner.ToNextToken();
        }

        private void ToExpression()
        {
            Whitespace();
            To();
            Whitespace();
            Check(TokenTypesEnum.currency);
            ToCurrency = Scaner.Token.Value;
        }

        private void MultiplierExpression()
        {
            MultiplierItem();

            while (IsMultipliaction())
            {
                Multiplication();
                MultiplierItem();
            }
        }

        private void MultiplierItem()
        {
            if (Scaner.Token.Type == TokenTypesEnum.openScope)
            {
                OpenedScope();
                MathExpression();
                ClosedScope();
            } 
            else if (Scaner.Token.Type == TokenTypesEnum.number)
            {
                Number();
            }
            else
            {
                CurrencyExpression();
            }
        }

        private void CurrencyExpression()
        {
            Currency();
            Number();
        }

        private void Currency()
        {
            Check(TokenTypesEnum.currency);
            CalcContainer.AddToken(Scaner.Token);
            CalcContainer.AddToken(new Token { Type = TokenTypesEnum.multiplication });
            Scaner.ToNextToken();
        }

        private void Number()
        {
            Check(TokenTypesEnum.number);
            CalcContainer.AddToken(Scaner.Token);
            Scaner.ToNextToken();
        }

        private void Whitespace()
        {
            Check(TokenTypesEnum.whitespace);
            Scaner.ToNextToken();
        }

        private void To()
        {
            Check(TokenTypesEnum.toValue);
            Scaner.ToNextToken();
        }

        private void Addition()
        {
            CalcContainer.AddToken(Scaner.Token);
            Scaner.ToNextToken();
        }

        private void Multiplication()
        {
            CalcContainer.AddToken(Scaner.Token);
            Scaner.ToNextToken();
        }

        private bool IsMultipliaction()
        {
            var type = Scaner.Token.Type;
            var isMultiplication = type == TokenTypesEnum.multiplication || type == TokenTypesEnum.division || type == TokenTypesEnum.persentage;
            return isMultiplication;
        }

        private bool IsAddition()
        {
            var type = Scaner.Token.Type;
            var isAddition = type == TokenTypesEnum.plus || type == TokenTypesEnum.minus;
            return isAddition;
        }

        private bool IsDefaultExpression()
        {
            return Scaner.Token.Type == TokenTypesEnum.defaultValue;
        }

        private bool IsToExpression()
        {
            return Scaner.Token.Type == TokenTypesEnum.toValue;
        }

        private void OpenedScope()
        {
            Check(TokenTypesEnum.openScope);
            CalcContainer.AddToken(Scaner.Token);
            Scaner.ToNextToken();
        }

        private void ClosedScope()
        {
            Check(TokenTypesEnum.closedScope);
            CalcContainer.AddToken(Scaner.Token);
            Scaner.ToNextToken();
        }

        private void Check(TokenTypesEnum expectedType)
        {
            if (expectedType != Scaner.Token.Type)
                throw new Exception("Wrong token. Expected is " + expectedType);
        }
    }
}

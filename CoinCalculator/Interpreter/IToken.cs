﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CoinCalculator.Interpreter
{
    public interface IToken
    {
        public TokenTypesEnum Type { get; set; }
        public string Value { get; set; }
    }
}

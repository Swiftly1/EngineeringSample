﻿using System;
using System.Globalization;
using Common.Lexing;

namespace Text2Abstraction.LexicalElements
{
    public class LexNumericalLiteral : LexElement
    {
        public LexNumericalLiteral(string tmp, DiagnosticInfo diagnostic) : base(LexingElement.Numerical, diagnostic)
        {
            Value = tmp;
        }

        public string Value { get; set; }

        public int IntegerValue => Convert.ToInt32(Value);

        public double DoubleValue => Convert.ToDouble(Value, CultureInfo.InvariantCulture);

        public bool IsDouble => Value.Contains(".");

        public bool IsInteger => !Value.Contains(".");

        public override string ToString()
        {
            return $"Numerical: {Value}";
        }
    }
}
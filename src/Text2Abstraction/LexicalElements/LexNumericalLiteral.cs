using System;
using System.Globalization;
using Common;
using Common.Lexing;

namespace Text2Abstraction.LexicalElements
{
    public class LexNumericalLiteral : LexElement
    {
        public LexNumericalLiteral(string tmp, DiagnosticInfo diagnostic) : base(LexingElement.Numerical, diagnostic)
        {
            StringValue = tmp;
        }

        public string StringValue { get; set; }

        public int IntegerValue => Convert.ToInt32(StringValue);

        public double DoubleValue => Convert.ToDouble(StringValue, CultureInfo.InvariantCulture);

        public bool IsDouble => StringValue.Contains(".");

        public bool IsInteger => !StringValue.Contains(".");

        public object GetNumericalValue()
        {
            if (IsDouble)
                return DoubleValue;

            if (IsInteger)
                return IntegerValue;

            throw new NotImplementedException($"Unable to determine value of {nameof(LexNumericalLiteral)}");
        }

        public override string ToString()
        {
            return $"Numerical: {StringValue}";
        }
    }
}
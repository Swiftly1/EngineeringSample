using Common;
using System;
using System.Linq;
using System.Collections.Generic;
using Text2Abstraction.LexicalElements;
using System.Globalization;

namespace Text2Abstraction
{
    public class TextTransformer : IMovable<char>
    {
        private LexingState _State { get; set; }

        private List<LexElement> _Elements = new List<LexElement>();

        public TextTransformer(string code)
        {
            _Collection = code.ToList();
            _State = LexingState.Root;
        }

        public List<LexElement> Walk()
        {
            _Elements = new List<LexElement>();

            do
            {
                if (char.IsWhiteSpace(_Current))
                    _State = LexingState.WhiteCharacter;
                else if (char.IsLetter(_Current))
                    _State = LexingState.Word;
                else if (char.IsNumber(_Current))
                    _State = LexingState.NumericalValue;
                else if (LexingFacts.OtherTokens.Contains(_Current))
                    _State = LexingState.Character;
                else if (_Current == '"')
                    _State = LexingState.String;
                else
                    _State = LexingState.Unknown;

                Handle(_State);

            } while (MoveNext());

            return _Elements;
        }

        private void Handle(LexingState state)
        {
            switch(state)
            {
                case LexingState.Word:
                    HandleWord();
                    break;
                case LexingState.String:
                    HandleString();
                    break;
                case LexingState.NumericalValue:
                    HandleNumericalValue();
                    break;
                    case LexingState.Character:
                    HandleOtherCharacter();
                    break;
                case LexingState.WhiteCharacter:
                    break;
            }
        }

        private void HandleOtherCharacter()
        {
            var diag = GetDiagnostic();

            if (LexingFacts.Char2LexElementMap.ContainsKey(_Current))
            {
                var lexElement = LexingFacts.Char2LexElementMap[_Current];
                _Elements.Add(new LexCharacter(lexElement, diag));
            }
            else
            {
                Error($"Unknown character '{_Current}'");
            }
        }

        private void HandleNumericalValue()
        {
            var tmp = "";

            do
            {
                if (!char.IsNumber(_Current) && _Current != '.' && !char.IsWhiteSpace(_Current))
                {
                    Error($"Unexpected character '{_Current}'. Expected number or dot.");
                }

                if (char.IsWhiteSpace(_Current) || IsLast())
                {
                    if (IsLast()) tmp += _Current;

                    if (!int.TryParse(tmp, out var _) && !double.TryParse(tmp, NumberStyles.Float, CultureInfo.InvariantCulture, out var _))
                        Error($"'{tmp}' is not correct numerical value");

                    var element = new LexNumericalLiteral(tmp, GetDiagnostic());
                    _Elements.Add(element);
                    return;
                }

                tmp += _Current;
            } while (MoveNext());
        }

        private void HandleString()
        {
            var tmp = "";
            do
            {
                if (_Current == '"' && HasPreviousElement() && ElementAt(_Index - 1) == LexingFacts.EscapeChar)
                {
                    var element = new LexStringLiteral(tmp, GetDiagnostic());
                    _Elements.Add(element);
                    return;
                }

                tmp += _Current;
            } while (MoveNext());

            Error("Unclosed string");
        }

        private void HandleWord()
        {
            var tmp = "";

            do
            {
                if (char.IsWhiteSpace(_Current) || IsLast())
                {
                    if (IsLast()) tmp += _Current;

                    var element = new LexWord(tmp, GetDiagnostic());
                    _Elements.Add(element);
                    return;
                }

                if (LexingFacts.OtherTokens.Contains(_Current))
                {
                    var element = new LexWord(tmp, GetDiagnostic());
                    _Elements.Add(element);
                    MoveBehind();
                    return;
                }

                tmp += _Current;
            } while (MoveNext());
        }

        public DiagnosticInfo GetDiagnostic()
        {
            return new DiagnosticInfo();
        }

        private void Error(string s)
        {
            var diag = GetDiagnostic();

            throw new Exception($"{s} {diag}");
        }
    }
}

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

        public int _CurrentLine { get; set; } = 0;

        public int _LastIndexOfNewLine { get; set; } = 0;

        public Settings Settings { get; set; } = new Settings();

        public TextTransformer(string code)
        {
            _Collection = code.ToList();
            _State = LexingState.Root;
        }

        public List<LexElement> Walk()
        {
            Reset();

            do
            {
                if (char.IsWhiteSpace(_Current))
                    _State = LexingState.WhiteCharacter;
                else if (char.IsLetter(_Current))
                    _State = LexingState.Word;
                else if (char.IsNumber(_Current))
                    _State = LexingState.NumericalValue;
                else if (_Current == '"')
                    _State = LexingState.String;
                else if (LexingFacts.OtherTokens.Contains(_Current))
                    _State = LexingState.Character;
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
                    HandleWhiteCharacter();
                    break;
            }
        }

        private void HandleWhiteCharacter()
        {
            if (_Current.ToString() == Environment.NewLine)
            {
                _CurrentLine++;
                _LastIndexOfNewLine = _Index;

                if (Settings.NewLineAware)
                {
                    _Elements.Add(new LexElement(LexingElement.NewLine, GetDiagnostics()));
                }
                return;
            }

            if (_Current == '\r')
            {
                var ahead = TryGetAhead(1);

                if (ahead.Count == 1 && ahead[0] == '\n')
                {
                    _CurrentLine++;
                    _LastIndexOfNewLine = _Index;

                    if (Settings.NewLineAware)
                    {
                        _Elements.Add(new LexElement(LexingElement.NewLine, GetDiagnostics()));
                    }

                    return;
                }
            }
        }

        private void HandleOtherCharacter()
        {
            var diag = GetDiagnostics();

            if (LexingFacts.Char2LexElementMap.ContainsKey(_Current))
            {
                var lexElement = LexingFacts.Char2LexElementMap[_Current];
                _Elements.Add(new LexCharacter(lexElement, diag));
                return;
            }

            var temp = "";
            var ordered = LexingFacts.String2OperatorMap.OrderByDescending(x => x.Key.Length);
            var lookAhead = false;
            do
            {
                if (ordered.Any(x => x.Key == temp))
                {
                    lookAhead = true;
                }
                else
                {
                    if (lookAhead)
                    {
                        var first = ordered.First(x => x.Key == temp);
                        _Elements.Add(new LexCharacter(first.Value, diag));
                    }
                    break;
                }

                temp += _Current;
            } while (MoveNext() && lookAhead);
        }

        private void HandleNumericalValue()
        {
            var tmp = "";

            var legal = new char[] { '.', ';' };

            do
            {
                if (!char.IsNumber(_Current) && !char.IsWhiteSpace(_Current) && !legal.Contains(_Current))
                {
                    Error($"Unexpected character '{_Current}'. Expected number or dot.");
                }

                if (char.IsWhiteSpace(_Current) || _Current == ';' || IsLast())
                {
                    if (IsLast()) tmp += _Current;

                    if (!int.TryParse(tmp, out var _) && !double.TryParse(tmp, NumberStyles.Float, CultureInfo.InvariantCulture, out var _))
                        Error($"'{tmp}' is not correct numerical value");

                    var element = new LexNumericalLiteral(tmp, GetDiagnostics());
                    _Elements.Add(element);
                    return;
                }

                tmp += _Current;
            } while (MoveNext());
        }

        private void HandleString()
        {
            var tmp = "";

            while (MoveNext())
            {
                if (_Current == '"' && HasPreviousElement() && ElementAt(_Index - 1) != LexingFacts.EscapeChar)
                {
                    var element = new LexStringLiteral(tmp, GetDiagnostics());
                    _Elements.Add(element);
                    return;
                }

                tmp += _Current;
            }

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

                    var element = new LexWord(tmp, GetDiagnostics());
                    _Elements.Add(element);

                    if (!IsLast())
                        MoveBehind();

                    return;
                }

                if (LexingFacts.OtherTokens.Contains(_Current))
                {
                    var element = new LexWord(tmp, GetDiagnostics());
                    _Elements.Add(element);
                    MoveBehind();
                    return;
                }

                tmp += _Current;
            } while (MoveNext());
        }

        public DiagnosticInfo GetDiagnostics()
        {
            var position = _Index - _LastIndexOfNewLine;
            return new DiagnosticInfo(_CurrentLine, position, _Current);
        }

        private void Error(string s)
        {
            var diag = GetDiagnostics();

            throw new Exception($"{s} at {diag}");
        }

        private void Reset()
        {
            _CurrentLine = 0;
            _LastIndexOfNewLine = 0;
            _Elements = new List<LexElement>();
        }
    }
}

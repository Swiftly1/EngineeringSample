using Common;
using System;
using System.Linq;
using System.Collections.Generic;
using Text2Abstraction.LexicalElements;
using System.Globalization;
using Common.Lexing;

namespace Text2Abstraction
{
    public class TextTransformer : Movable<char>
    {
        private LexingState _State { get; set; }

        private List<LexElement> _Elements = new List<LexElement>();

        private int _CurrentLine { get; set; } = 0;

        private int _LastIndexOfNewLine { get; set; } = 0;

        public Settings Settings { get; set; } = new Settings();

        public TextTransformer(string code, Settings settings = null) : base(code.ToList())
        {
            if (settings != null)
                Settings = settings;

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
                case LexingState.Unknown:
                    throw new Exception("Unrecognized token");
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

                if (ahead.Sucess && ahead.Items[0] == '\n')
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
            var ordered = LexingFacts.CombinedMap.OrderByDescending(x => x.Key.Length).ToList();

            void AddElement(string tmp)
            {
                var first = ordered.First(x => x.Key == tmp);
                _Elements.Add(new LexCharacter(first.Value, diag));
            }

            var tmp = "";
            do
            {
                var lookUpValue = tmp + _Current;
                if (LexingFacts.CombinedMapKeys.Contains(lookUpValue))
                {
                    tmp += _Current;

                    if (IsLast())
                    {
                        AddElement(tmp);
                        return;
                    }
                }
                else
                {
                    AddElement(tmp);
                    MoveBehind();
                    return;
                }
            } while (MoveNext());
        }

        private void HandleNumericalValue()
        {
            void AddElement(string tmp)
            {
                if (!int.TryParse(tmp, out var _) && !double.TryParse(tmp, NumberStyles.Float, CultureInfo.InvariantCulture, out var _))
                    Error($"'{tmp}' is not correct numerical value");

                var element = new LexNumericalLiteral(tmp, GetDiagnostics());
                _Elements.Add(element);
            }

            var tmp = "";
            var legal = new char[] { '.' };

            do
            {
                if (!char.IsNumber(_Current) && !legal.Contains(_Current))
                {
                    AddElement(tmp);
                    MoveBehind();
                    return;
                }
                else if (IsLast())
                {
                    if (IsLast()) tmp += _Current;
                    AddElement(tmp);
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

            void AddElement()
            {
                LexElement element = null;

                if (LanguageFacts.KeywordMapper.TryGetValue(tmp, out var lexingElement))
                {
                    element = new LexKeyword(tmp, lexingElement, GetDiagnostics());
                }
                else
                {
                    element = new LexWord(tmp, GetDiagnostics());
                }

                _Elements.Add(element);
            }

            do
            {
                if (char.IsWhiteSpace(_Current) || IsLast())
                {
                    if (IsLast()) tmp += _Current;

                    AddElement();

                    if (!IsLast())
                        MoveBehind();

                    return;
                }

                if (LexingFacts.OtherTokens.Contains(_Current))
                {
                    AddElement();
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

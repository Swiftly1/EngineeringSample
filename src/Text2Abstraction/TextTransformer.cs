using Common;
using System;
using System.Linq;
using System.Collections.Generic;
using Text2Abstraction.LexicalElements;

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

        public void Walk()
        {
            _Elements = new List<LexElement>();
            var otherSyntax = new List<char> { '.', ',', '(', ')', '{', '}' };

            do
            {
                if (char.IsWhiteSpace(_Current))
                    _State = LexingState.WhiteCharacter;
                else if (char.IsLetter(_Current))
                    _State = LexingState.Word;
                else if (char.IsNumber(_Current))
                    _State = LexingState.NumericalValue;
                else if (otherSyntax.Contains(_Current))
                    _State = LexingState.OtherSyntax;
                else if (_Current == '"')
                    _State = LexingState.String;
                else
                    _State = LexingState.Unknown;

                Handle(_State);

            } while (MoveNext());
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
                case LexingState.WhiteCharacter:
                    break;
            }
        }

        private void HandleNumericalValue()
        {
            throw new NotImplementedException();
        }

        private void HandleString()
        {
            var tmp = "";
            do
            {
                if (_Current == '"' && HasPreviousElement() && ElementAt(_Index - 1) == '\\')
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

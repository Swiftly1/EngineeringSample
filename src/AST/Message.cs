﻿using Common;

namespace AST
{
    public sealed class Message
    {
        private Message(DiagnosticInfo diagnosticInfo, string text, MessageKind kind)
        {
            DiagnosticInfo = diagnosticInfo;
            Text = text;
            Kind = kind;
        }

        public DiagnosticInfo DiagnosticInfo { get; }

        public string Text { get; }

        public MessageKind Kind { get; }

        public static Message CreateInformation(string s, DiagnosticInfo diag) => new Message(diag, s, MessageKind.Information);

        public static Message CreateWarning(string s, DiagnosticInfo diag) => new Message(diag, s, MessageKind.Warning);

        public static Message CreateError(string s, DiagnosticInfo diag) => new Message(diag, s, MessageKind.Error);

        public static Message CreateOther(string s, DiagnosticInfo diag) => new Message(diag, s, MessageKind.Other);
    }
}

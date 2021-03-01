﻿using System;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace AST
{
    public class ErrorHandler
    {
        private List<Message> _Messages { get; } = new List<Message>();

        public IEnumerable<Message> DumpErrors() => _Messages.AsEnumerable();

        private void AddMessage(Message msg)
        {
            if (msg is null || string.IsNullOrWhiteSpace(msg.Text) || msg.DiagnosticInfo is null)
            {
                throw new ArgumentException(nameof(msg)); 
            }

            _Messages.Add(msg);
        }

        public void AddInformation(string s, DiagnosticInfo diag) => AddMessage(Message.CreateInformation(s, diag));

        public void AddWarning(string s, DiagnosticInfo diag) => AddMessage(Message.CreateWarning(s, diag));

        public void AddError(string s, DiagnosticInfo diag) => AddMessage(Message.CreateInformation(s, diag));
    }
}

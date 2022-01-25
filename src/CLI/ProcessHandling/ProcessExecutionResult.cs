namespace CLI.ProcessHandling
{
    internal class ProcessExecutionResult
    {
        public bool Success { get; private set; }

        public string ErrorLog { get; private set; }

        public string OutputLog { get; private set; }

        public static ProcessExecutionResult Ok(string outputLog, string errLog = "")
        {
            return new ProcessExecutionResult
            {
                Success = true,
                OutputLog = outputLog,
                ErrorLog = errLog
            };
        }

        public static ProcessExecutionResult Fail(string outputLog, string errLog)
        {
            return new ProcessExecutionResult
            {
                Success = false,
                OutputLog = outputLog,
                ErrorLog = errLog
            };
        }
    }
}

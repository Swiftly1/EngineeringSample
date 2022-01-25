using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace CLI.ProcessHandling
{
    internal static class ProcessHelper
    {
        private const string EndedCorrectly = "Ended";

        private const string TimeOut = "TimeOut";

        /// <summary>
        /// Executes process with provided arguments with some work directory and returns
        /// Success - True/False - basing on StandardErrorOutput or TimeOut.
        /// StandardOutput Logs
        /// StandardError Logs
        /// </summary>
        /// <param name="path">Path to file that has to be executed</param>
        /// <param name="args">Args e.g "--test"</param>
        /// <param name="workingDirectory">Working Directory. If left as a NULL, then it'll use parent folder of the file.</param>
        /// <param name="timeOutSeconds"></param>
        /// <returns>ProcessExecutionResult</returns>
        public static ProcessExecutionResult ExecuteProgram
        (
            string path,
            string args,
            string workingDirectory = null,
            int timeOutSeconds = 10
        )
        {
            var output = new StringBuilder();
            var error = new StringBuilder();

            using (var process = new Process())
            using (var disposables = new CompositeDisposable())
            {
                process.EnableRaisingEvents = true;

                process.StartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = path,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    WorkingDirectory = workingDirectory is null ? Path.GetDirectoryName(path) : workingDirectory,
                    Arguments = args,
                };

                Observable
                .FromEventPattern<DataReceivedEventArgs>(process, nameof(Process.ErrorDataReceived))
                .Subscribe
                (
                    onNext: x => error.AppendLine(x.EventArgs.Data),
                    onError: x => error.AppendLine(x.ToString())
                );

                Observable
                .FromEventPattern<DataReceivedEventArgs>(process, nameof(Process.OutputDataReceived))
                .Subscribe
                (
                    onNext: x => output.AppendLine(x.EventArgs.Data),
                    onError: x => error.AppendLine(x.ToString())
                );

                var processExited =
                Observable
                .FromEventPattern<EventArgs>(process, nameof(Process.Exited))
                .Select(_ => EndedCorrectly)
                .Timeout(TimeSpan.FromSeconds(timeOutSeconds), Observable.Return(TimeOut))
                .Do(exit_status =>
                {
                    if (exit_status == TimeOut)
                    {
                        try
                        {
                            process.Kill();
                        }
                        catch { }
                    }
                });

                processExited.Subscribe().DisposeWith(disposables);

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                var result = processExited.Take(1).Wait();

                var errors = error.ToString();
                var outputs = output.ToString();

                if (result == TimeOut)
                    errors = $"TimeOut After: {timeOutSeconds}s. " + errors;

                return result == EndedCorrectly && errors.Trim().Length == 0 ?
                ProcessExecutionResult.Ok(outputs, errors) :
                ProcessExecutionResult.Fail(outputs, errors);
            }
        }
    }
}

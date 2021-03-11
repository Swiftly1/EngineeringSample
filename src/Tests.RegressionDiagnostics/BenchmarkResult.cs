namespace Tests.RegressionDiagnostics
{
    public class BenchmarkResult
    {
        public string Method { get; set; }
        public string Job { get; set; }
        public bool AnalyzeLaunchVariance { get; set; }
        public string EvaluateOverhead { get; set; }
        public string MaxAbsoluteError { get; set; }
        public string MaxRelativeError { get; set; }
        public string MinInvokeCount { get; set; }
        public string MinIterationTime { get; set; }
        public string OutlierMode { get; set; }
        public long Affinity { get; set; }
        public string EnvironmentVariables { get; set; }
        public string Jit { get; set; }
        public string Platform { get; set; }
        public string PowerPlanMode { get; set; }
        public string Runtime { get; set; }
        public bool AllowVeryLargeObjects { get; set; }
        public bool Concurrent { get; set; }
        public bool CpuGroups { get; set; }
        public bool Force { get; set; }
        public string HeapAffinitizeMask { get; set; }
        public string HeapCount { get; set; }
        public bool NoAffinitize { get; set; }
        public bool RetainVm { get; set; }
        public bool Server { get; set; }
        public string Arguments { get; set; }
        public string BuildConfiguration { get; set; }
        public string Clock { get; set; }
        public string EngineFactory { get; set; }
        public string NuGetReferences { get; set; }
        public string Toolchain { get; set; }
        public string IsMutator { get; set; }
        public int InvocationCount { get; set; }
        public string IterationCount { get; set; }
        public string IterationTime { get; set; }
        public string LaunchCount { get; set; }
        public string MaxIterationCount { get; set; }
        public string MaxWarmupIterationCount { get; set; }
        public string MinIterationCount { get; set; }
        public string MinWarmupIterationCount { get; set; }
        public string RunStrategy { get; set; }
        public int UnrollFactor { get; set; }
        public string WarmupCount { get; set; }
        public string Mean { get; set; }
        public string Error { get; set; }
        public string StdDev { get; set; }
    }
}
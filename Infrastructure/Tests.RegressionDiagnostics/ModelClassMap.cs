using CsvHelper.Configuration;

namespace Tests.RegressionDiagnostics
{
    public class ModelClassMap : ClassMap<BenchmarkResult>
    {
        public ModelClassMap()
        {
            Map(m => m.Method).Name("Method");
            Map(m => m.Job).Name("Job");
            Map(m => m.AnalyzeLaunchVariance).Name("AnalyzeLaunchVariance");
            Map(m => m.EvaluateOverhead).Name("EvaluateOverhead");
            Map(m => m.MaxAbsoluteError).Name("MaxAbsoluteError");
            Map(m => m.MaxRelativeError).Name("MaxRelativeError");
            Map(m => m.MinInvokeCount).Name("MinInvokeCount");
            Map(m => m.MinIterationTime).Name("MinIterationTime");
            Map(m => m.OutlierMode).Name("OutlierMode");
            Map(m => m.Affinity).Name("Affinity");
            Map(m => m.EnvironmentVariables).Name("EnvironmentVariables");
            Map(m => m.Jit).Name("Jit");
            Map(m => m.Platform).Name("Platform");
            Map(m => m.PowerPlanMode).Name("PowerPlanMode");
            Map(m => m.Runtime).Name("Runtime");
            Map(m => m.AllowVeryLargeObjects).Name("AllowVeryLargeObjects");
            Map(m => m.Concurrent).Name("Concurrent");
            Map(m => m.CpuGroups).Name("CpuGroups");
            Map(m => m.Force).Name("Force");
            Map(m => m.HeapAffinitizeMask).Name("HeapAffinitizeMask");
            Map(m => m.HeapCount).Name("HeapCount");
            Map(m => m.NoAffinitize).Name("NoAffinitize");
            Map(m => m.RetainVm).Name("RetainVm");
            Map(m => m.Server).Name("Server");
            Map(m => m.Arguments).Name("Arguments");
            Map(m => m.BuildConfiguration).Name("BuildConfiguration");
            Map(m => m.Clock).Name("Clock");
            Map(m => m.EngineFactory).Name("EngineFactory");
            Map(m => m.NuGetReferences).Name("NuGetReferences");
            Map(m => m.Toolchain).Name("Toolchain");
            Map(m => m.IsMutator).Name("IsMutator");
            Map(m => m.InvocationCount).Name("InvocationCount");
            Map(m => m.IterationCount).Name("IterationCount");
            Map(m => m.IterationTime).Name("IterationTime");
            Map(m => m.LaunchCount).Name("LaunchCount");
            Map(m => m.MaxIterationCount).Name("MaxIterationCount");
            Map(m => m.MaxWarmupIterationCount).Name("MaxWarmupIterationCount");
            Map(m => m.MinIterationCount).Name("MinIterationCount");
            Map(m => m.MinWarmupIterationCount).Name("MinWarmupIterationCount");
            Map(m => m.RunStrategy).Name("RunStrategy");
            Map(m => m.UnrollFactor).Name("UnrollFactor");
            Map(m => m.WarmupCount).Name("WarmupCount");
            Map(m => m.Mean).Name("Mean");
            Map(m => m.Error).Name("Error");
            Map(m => m.StdDev).Name("StdDev");
        }
    }
}
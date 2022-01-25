using System.Text.Json.Serialization;

namespace CLI
{
    public class RunnerSettings
    {
        [JsonPropertyName("llc")]
        public string LLVM_llc { get; set; }

        [JsonPropertyName("wasm-ld")]
        public string LLVM_wasmld { get; set; }

        [JsonPropertyName("opt")]
        public string LLVM_opt { get; set; }
    }
}

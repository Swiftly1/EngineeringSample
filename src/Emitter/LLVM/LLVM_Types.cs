using Common;

#nullable enable
namespace Emitter.LLVM
{
    public static class LLVM_Types
    {
        public static Result<string> GetLLVMType(TypeInfo type)
        {
            string? llvm_type = type.Name switch
            {
                "int32" => "i32",
                _ => null
            };

            if (llvm_type is null)
            {
                return Result<string>.Error($"Type {type} has not defined translation to LLVM type.");
            }

            return Result<string>.Ok(llvm_type);
        }
    }
}

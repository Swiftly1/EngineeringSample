namespace AST.Trees.Miscs
{
    public class BasicVariableDescription
    {
        public BasicVariableDescription()
        {

        }

        public BasicVariableDescription(string variableName, string typeName)
        {
            VariableName = variableName;
            TypeName = typeName;
        }

        public string VariableName { get; set; }

        public string TypeName { get; set; }
    }
}

namespace AST.Passes.Results
{
    public abstract class PassResult
    {
        protected PassResult(string passName)
        {
            PassName = passName;
        }

        public string PassName { get; }
    }
}

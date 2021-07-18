namespace AST.Passes.Results
{
    public abstract class PassResult
    {
        protected PassResult(string passName)
        {
            PassName = passName;
        }

        public string PassName { get; }

        public bool FullStop { get; set; }

        public ErrorHandler Errors { get; set; } = new();
    }
}

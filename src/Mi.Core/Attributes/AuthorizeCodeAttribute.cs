namespace Mi.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class AuthorizeCodeAttribute : Attribute
    {
        private string code { get; set; }

        public AuthorizeCodeAttribute(string code)
        {
            this.code = code;
        }

        public string Code => code;
    }
}
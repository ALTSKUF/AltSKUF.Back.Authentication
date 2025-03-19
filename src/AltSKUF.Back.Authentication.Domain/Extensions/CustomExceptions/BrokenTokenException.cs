namespace AltSKUF.Back.Authentication.Domain.Extensions.CustomExceptions
{
    public class BrokenTokenException : Exception
    {
        public BrokenTokenException()
            : base("broken_token") { }
    }
}

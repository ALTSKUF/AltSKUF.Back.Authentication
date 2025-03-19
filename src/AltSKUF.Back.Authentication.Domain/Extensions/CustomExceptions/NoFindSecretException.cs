namespace AltSKUF.Back.Authentication.Domain.Extensions.CustomExceptions
{
    internal class NoFindSecretException : Exception
    {
        public NoFindSecretException() 
            : base("no_find_secret")
        {
        }
    }
}

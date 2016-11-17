namespace Material.OAuth.Authentication
{
    public class TokenValidationResult
    {
        public bool IsTokenValid { get; }
        public string Reason { get; }

        public TokenValidationResult(bool isTokenValid) : 
            this(isTokenValid, string.Empty)
        { }

        public TokenValidationResult(
            bool isTokenValid,
            string reason)
        {
            IsTokenValid = isTokenValid;
            Reason = reason;
        }
    }
}

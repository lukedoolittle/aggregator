namespace Material.OAuth
{
    public static class OAuthConfiguration
    {
        /// <summary>
        /// Default timeout for all oauth security parameters (state, nonce, etc)
        /// </summary>
        public static int SecurityParameterTimeoutInMinutes { get; set; } = 2;
    }
}

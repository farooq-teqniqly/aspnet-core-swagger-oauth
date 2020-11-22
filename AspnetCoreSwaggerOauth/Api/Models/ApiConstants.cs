namespace Api.Models
{
    public static class ApiConstants
    {
        public static class AspNetAuth
        {
            public const string ReadWritePolicyName = "ReadWritePolicy";
            public const string ReadOnlyPolicyName = "ReadOnlyPolicy";
            public const string AuthConfigSectionName = "Auth";
        }
        
        public static class OAuth
        {
            public const string ScopeClaimType = "http://schemas.microsoft.com/identity/claims/scope";
            public const string SecurityDefinitionName = "aad-jwt";
            public static readonly string[] SwaggerUIScopes = { "Api.SwaggerUI" };
        }
    }
}

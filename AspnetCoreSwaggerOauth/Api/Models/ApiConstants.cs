namespace Api.Models
{
    public static class ApiConstants
    {
        public static class AspNetAuth
        {
            public const string PolicyName = "AuthPolicy";
            public const string AuthConfigSectionName = "Auth";
        }
        
        public static class OAuth
        {
            public const string ScopeClaimType = "http://schemas.microsoft.com/identity/claims/scope";
            public const string SecurityDefinitionName = "aad-jwt";
            public const string Scope = "Api.SwaggerUI";
        }
    }
}

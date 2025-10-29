namespace Entity.Domain.Config
{
    public class JwtSettings
    {
        public string Key { get; set; } = null!;
        public string Issuer { get; set; } = "WebModelSecurity.API";
        public string Audience { get; set; } = "ModelSecurity.Client";
        public int AccessTokenExpirationMinutes { get; set; } = 15;
        public int RefreshTokenExpirationDays { get; set; } = 7;
    }
}

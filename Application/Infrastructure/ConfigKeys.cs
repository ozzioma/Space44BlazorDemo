namespace Application.Infrastructure
{
    public class ConfigKeys
    {
        public const string WEB_COOKIE_NAME = "space44.demostudent.cookie";

        public const string API_HOST = nameof(API_HOST);
        public const string ENTITY_ODATA_HOST = nameof(ENTITY_ODATA_HOST);
        public const string ODATA_VIEWS_HOST = nameof(ODATA_VIEWS_HOST);

        public const string ACCESS_TOKEN_KEY = nameof(ACCESS_TOKEN_KEY);
        public const string REFRESH_TOKEN_KEY = nameof(REFRESH_TOKEN_KEY);

        public const string AUTH_CLIENT_ID = "AuthServer:ClientId";
        public const string AUTH_CLIENT_SECRET = "AuthServer:ClientSecret";
        public const string AUTH_AUTHORITY = "AuthServer:Authority";
        public const string AUTH_REFRESH_URL = "AuthServer:RefreshUrl";
        public const string AUTH_AUDIENCE = "AuthServer:Audience";

        public const string APP_NAME = nameof(APP_NAME);
    }
}
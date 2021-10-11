using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Infrastructure
{
    public class WebConfigHelper
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public WebConfigHelper(IConfiguration configuration,
            IHttpContextAccessor _httpContextAccessor)
        {
            Configuration = configuration;
            httpContextAccessor = _httpContextAccessor;
            CurrentUser = httpContextAccessor?.HttpContext?.User;
        }

        public IConfiguration Configuration { get; private set; }

        public HttpContext HttpContext => httpContextAccessor?.HttpContext;
        public ClaimsPrincipal CurrentUser { get; private set; }
        public string CurrentUserName => CurrentUser?.Identity?.Name;

        public Claim AccessTokenClaim =>
            CurrentUser?.Claims.Where(r => r.Type == ConfigKeys.ACCESS_TOKEN_KEY).FirstOrDefault();

        public string AccessToken => AccessTokenClaim?.Value;

        public Claim RefreshTokenClaim =>
            CurrentUser?.Claims.Where(r => r.Type == ConfigKeys.REFRESH_TOKEN_KEY).FirstOrDefault();

        public string RefreshToken => RefreshTokenClaim?.Value;

        public string API_HOST => Configuration[ConfigKeys.API_HOST];
        public string ENTITY_ODATA_HOST => Configuration[ConfigKeys.ENTITY_ODATA_HOST];
    }
}
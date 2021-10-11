using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Infrastructure
{
    public class RefitDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<RefitDelegatingHandler> logger;
        private readonly WebConfigHelper webConfigHelper;

        public RefitDelegatingHandler(IHttpContextAccessor _httpContextAccessor,
            WebConfigHelper _webConfigHelper,
            ILogger<RefitDelegatingHandler> _logger)
        {
            httpContextAccessor = _httpContextAccessor;
            logger = _logger;
            webConfigHelper = _webConfigHelper;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage httpResponseMessage;

            try
            {
                //string accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
                string accessToken = webConfigHelper.AccessToken;

                if (string.IsNullOrEmpty(accessToken))
                {
                    throw new Exception($"Access token is missing for the request {request.RequestUri}");
                }

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var headers = httpContextAccessor.HttpContext.Request.Headers;
                if (headers.ContainsKey("X-Correlation-ID") && !string.IsNullOrEmpty(headers["X-Correlation-ID"]))
                {
                    request.Headers.Add("X-Correlation-ID", headers["X-Correlation-ID"].ToString());
                }

                httpResponseMessage = await base.SendAsync(request, cancellationToken);
                //httpResponseMessage.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to run http query {RequestUri}", request.RequestUri);
                throw;
            }

            return httpResponseMessage;
        }
    }
}
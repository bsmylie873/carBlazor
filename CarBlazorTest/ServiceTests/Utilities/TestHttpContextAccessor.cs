using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace CarBlazorTest.ServiceTests.Utilities;

public class TestHttpContextAccessor : IHttpContextAccessor
{
    public HttpContext HttpContext { get; set; } = new DefaultHttpContext();
    public bool SignInCalled { get; private set; }
    public bool SignOutCalled { get; private set; }

    public TestHttpContextAccessor()
    {
        // Set up the authentication service
        HttpContext.RequestServices = new TestServiceProvider(this);
    }

    private class TestServiceProvider : IServiceProvider
    {
        private readonly TestHttpContextAccessor _accessor;

        public TestServiceProvider(TestHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public object? GetService(Type serviceType)
        {
            return serviceType == typeof(IAuthenticationService) ? new TestAuthenticationService(_accessor) : null;
        }
    }

    private class TestAuthenticationService : IAuthenticationService
    {
        private readonly TestHttpContextAccessor _accessor;

        public TestAuthenticationService(TestHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string? scheme)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        public Task ChallengeAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
        {
            return Task.CompletedTask;
        }

        public Task ForbidAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
        {
            return Task.CompletedTask;
        }

        public Task SignInAsync(HttpContext context, string? scheme, ClaimsPrincipal principal,
            AuthenticationProperties? properties)
        {
            _accessor.SignInCalled = true;
            return Task.CompletedTask;
        }

        public Task SignOutAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
        {
            _accessor.SignOutCalled = true;
            return Task.CompletedTask;
        }
    }
}
using Azure.Core;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace AzureStorageWithEntraId;

/// <summary>
/// Makes it possible to use MSAL to obtain Azure.Identity access tokens
/// </summary>
public class AccessTokenProviderTokenCredential(IAccessTokenProvider accessTokenProvider) : TokenCredential
{
    private readonly IAccessTokenProvider AccessTokenProvider = accessTokenProvider ?? throw new ArgumentNullException(nameof(accessTokenProvider));

    public override Azure.Core.AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
    {
        var accessTokenResult = AccessTokenProvider.RequestAccessToken(new AccessTokenRequestOptions
        {
            Scopes = requestContext.Scopes
        })
        .GetAwaiter()
        .GetResult();
        if (accessTokenResult.TryGetToken(out var accessToken))
        {
            var result = new Azure.Core.AccessToken(accessToken.Value, accessToken.Expires);
            return result;
        }
        else
        {
            throw new Exception($"Failed to obtain access token for scopes '{string.Join(",", requestContext.Scopes)}'");
        }
    }

    public async override ValueTask<Azure.Core.AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
    {
        var accessTokenResult = await AccessTokenProvider.RequestAccessToken(new AccessTokenRequestOptions
        {
            Scopes = requestContext.Scopes
        }).ConfigureAwait(false);
        if (accessTokenResult.TryGetToken(out var accessToken))
        {
            var result = new Azure.Core.AccessToken(accessToken.Value, accessToken.Expires);
            return result;
        }
        else
        {
            throw new Exception($"Failed to obtain access token for scopes '{string.Join(",", requestContext.Scopes)}'");
        }
    }
}

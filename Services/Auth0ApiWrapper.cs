using System;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Microsoft.Extensions.Configuration;
using NeedsSoySauce.Entities;
using NeedsSoySauce.Models;
using RestSharp;

namespace NeedsSoySauce.Services
{
    public class Auth0ApiWrapper
    {
        private string _clientId;
        private string _clientSecret;
        private string _domain;
        private string _managementApiAudience;

        private AccessTokenResponse _accessTokenResponse;
        private DateTime _accessTokenExpiryOnUtc;
        private int _skew = 300;

        public Auth0ApiWrapper(string clientId, string clientSecret, string domain, string managementApiAudience)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _domain = domain;
            _managementApiAudience = managementApiAudience;
            _accessTokenResponse = GetManagementApiToken();
            _accessTokenExpiryOnUtc = DateTime.UtcNow.AddSeconds(_accessTokenResponse.ExpiresIn - _skew);
        }

        private void RefreshAccessTokenIfNeeded()
        {
            if (DateTime.UtcNow > _accessTokenExpiryOnUtc || _accessTokenResponse is null)
            {
                _accessTokenResponse = GetManagementApiToken();
                _accessTokenExpiryOnUtc = DateTime.UtcNow.AddSeconds(_accessTokenResponse.ExpiresIn - _skew);
            }
        }

        private AccessTokenResponse GetManagementApiToken()
        {
            var client = new AuthenticationApiClient(_domain);
            var tokenRequest = new ClientCredentialsTokenRequest()
            {
                ClientId = _clientId,
                ClientSecret = _clientSecret,
                Audience = _managementApiAudience
            };

            var task = client.GetTokenAsync(tokenRequest);
            task.Wait();
            return task.Result;
        }

        public User GetUser(string userId)
        {
            RefreshAccessTokenIfNeeded();
            var client = new ManagementApiClient(_accessTokenResponse.AccessToken, _domain);
            var task = client.Users.GetAsync(userId, "user_id,email,username", true);
            task.Wait();
            var user = task.Result;
            return new User(user.UserId, user.UserName ?? user.Email, user.Email);
        }
    }
}
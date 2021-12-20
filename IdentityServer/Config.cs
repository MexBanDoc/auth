﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            { 
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("photos_app", "Web Photos", new []
                {
                    "role", "subscription", "testing"
                })
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[] 
            {
                new ApiResource("api1", "My API")
                {
                    Scopes = { "scope1" }
                },
                new ApiResource("photos_service", "Сервис фотографий")
                {
                    Scopes = { "photos" }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("scope1", "My scope"),
                new ApiScope("photos", "Фотографии")
            };
        
        public static IEnumerable<Client> Clients =>
            new Client[] 
            {
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "scope1" }
                },
                new Client()
                {
                    ClientId = "Photos App By OAuth",
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "photos" }
                },
                new Client
                {
                    ClientId = "Photos App by OIDC",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
        
                    // NOTE: показывать ли пользователю страницу consent со списком запрошенных разрешений
                    RequireConsent = false,

                    // NOTE: куда отправлять после логина
                    RedirectUris = { "https://localhost:5001/signin-passport" },

                    AllowedScopes = new List<string>
                    {
                        // NOTE: Позволяет запрашивать id token
                        IdentityServerConstants.StandardScopes.OpenId,
                        // NOTE: Позволяет запрашивать профиль пользователя через id token
                        IdentityServerConstants.StandardScopes.Profile,
                        // NOTE: Позволяет запрашивать email пользователя через id token
                        IdentityServerConstants.StandardScopes.Email,
                        "photos_app"
                    },

                    // NOTE: Надо ли добавлять информацию о пользователе в id token при запросе одновременно
                    // id token и access token, как это происходит в code flow.
                    // Либо придется ее получать отдельно через user info endpoint.
                    AlwaysIncludeUserClaimsInIdToken = true,
                    
                    PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-passport" },
                }
            };
    }
}
// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Listify.Identity
{
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("roles", "Your role(s)", new [] { JwtClaimTypes.Role })
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("listifyWebAPI", "Listify WebAPI", new []
                {
                    "profile", "roles", "ListifyWebAPI"
                })
                {
                    ApiSecrets =
                    {
                        new Secret("ListifySecretApp".Sha256())
                    }
                }
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // Angular CLI Client
                new Client
                {
                    ClientId = "listify.webapp",
                    ClientName = "The Listify WebApplication",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = { "http://localhost:4200" },
                    PostLogoutRedirectUris = { "http://localhost:4200" },
                    // LogoUri = "",
                    AllowedCorsOrigins = { "http://localhost:4200" },
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "listifyWebAPI"
                    },
                    EnableLocalLogin = true,
                    ClientUri = "http://localhost:4200"
                },
            };
        }
    }
}
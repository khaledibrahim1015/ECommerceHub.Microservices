// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace ECcommerceHub.Identity
{

    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
            new IdentityResources.OpenId(),  // return subjectid 
            new IdentityResources.Profile(), //  return username firstname etc
            };
        //  api scope authenticaion level read or write or readandwrite 
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalogapi"),
                new ApiScope("basketapi")

            };
        //  List of Microservices can go here 
        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                // Catalog.Api
                // audience 
                new ApiResource(name:"Catalog", displayName: "Catalog.API")
                {
                    Scopes= new []{ "catalogapi" }
                },
                // Basket.Api
                new ApiResource(name: "Basket", displayName: "Basket.Api")
                {
                    Scopes={ "basketapi" }
                }

            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
            // Catalog.Api
            // MACHINE TO MACHINE FLOW 
            new Client()
            {
                ClientName ="Catalog Api Client ",
                ClientId= "CatalogApiClient",
                ClientSecrets={new Secret("7f053372-2504-496f-a7b7-b04c3972bd92".Sha256()) },
                AllowedGrantTypes= GrantTypes.ClientCredentials,
                AllowedScopes={ "catalogapi" }
            },
            // Basket.Api
            // machine to machine flow 
            new Client()
            {
                ClientName="Basket Api Client",
                ClientId=  "BasketApiClient",
                AllowedGrantTypes= GrantTypes.ClientCredentials,
                ClientSecrets=new []{new Secret("fa4d1332-0d6b-4a8b-94cc-477a75c1041f".Sha256()) },
                AllowedScopes= { "basketapi" }

            },


            // Or Combine one 
              new Client()
            {
                ClientName ="Catalog Api Client ",
                ClientId= "CatalogApiClientBasketApiClient",
                ClientSecrets={new Secret("7f053372-2504-496f-a7b7-b04c3972bd92".Sha256()) },
                AllowedGrantTypes= GrantTypes.ClientCredentials,
                AllowedScopes={ "catalogapi", "basketapi" }
            },


            };
    }
}
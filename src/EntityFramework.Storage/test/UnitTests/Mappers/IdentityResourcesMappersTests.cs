/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using IdentityServer8.EntityFramework.Mappers;
using IdentityServer8.EntityFramework.Entities;
using IdentityServer8.Models;
using Xunit;
using ModelIdentityResource = IdentityServer8.Models.IdentityResource;

namespace IdentityServer8.EntityFramework.UnitTests.Mappers;

public class IdentityResourcesMappersTests
{
    [Fact]
    public void CanMapIdentityResources()
    {
        var model = new ModelIdentityResource();
        var mappedEntity = model.ToEntity();
        var mappedModel = mappedEntity.ToModel();

        Assert.NotNull(mappedModel);
        Assert.NotNull(mappedEntity);
    }

    [Fact]
    public void ToModel_MapsEntityGraphWithoutThrowing()
    {
        var entity = new IdentityServer8.EntityFramework.Entities.IdentityResource
        {
            Enabled = true,
            Name = "profile",
            DisplayName = "User profile",
            Description = "Non-regression mapper path",
            Required = true,
            Emphasize = true,
            ShowInDiscoveryDocument = true,
            UserClaims = new List<IdentityResourceClaim>
            {
                new() { Type = "name" },
                new() { Type = "email" },
            },
            Properties = new List<IdentityResourceProperty>
            {
                new() { Key = "source", Value = "unit-test" },
            },
        };

        var model = entity.ToModel();

        Assert.NotNull(model);
        Assert.Equal(entity.Name, model.Name);
        Assert.Equal(entity.DisplayName, model.DisplayName);
        Assert.Equal(entity.Description, model.Description);
        Assert.Equal(entity.Required, model.Required);
        Assert.Equal(entity.Emphasize, model.Emphasize);
        Assert.Contains("name", model.UserClaims);
        Assert.Contains("email", model.UserClaims);
        Assert.True(model.Properties.ContainsKey("source"));
        Assert.Equal("unit-test", model.Properties["source"]);
    }

    [Fact]
    public void RoundTrip_PreservesClaimsAndProperties()
    {
        var model = new ModelIdentityResource("openid", "OpenID", new[] { "sub", "name" })
        {
            Description = "Roundtrip",
            Required = true,
            Emphasize = false,
            ShowInDiscoveryDocument = true,
        };
        model.Properties["tier"] = "gold";

        var entity = model.ToEntity();
        var mappedBack = entity.ToModel();

        Assert.NotNull(entity);
        Assert.NotNull(mappedBack);
        Assert.Equal(model.Name, mappedBack.Name);
        Assert.Equal(model.DisplayName, mappedBack.DisplayName);
        Assert.Equal(model.Description, mappedBack.Description);
        Assert.Equal(model.Required, mappedBack.Required);
        Assert.Equal(model.Emphasize, mappedBack.Emphasize);
        Assert.Contains("sub", mappedBack.UserClaims);
        Assert.Contains("name", mappedBack.UserClaims);
        Assert.Equal("gold", mappedBack.Properties["tier"]);
    }
}

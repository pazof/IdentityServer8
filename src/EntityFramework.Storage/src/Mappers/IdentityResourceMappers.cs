/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using IdentityServer8.EntityFramework.Entities;

namespace IdentityServer8.EntityFramework.Mappers
{
    /// <summary>
    /// Extension methods to map to/from entity/model for identity resources.
    /// </summary>
    public static class IdentityResourceMappers
    {
        /// <summary>
        /// Maps an entity to a model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Models.IdentityResource ToModel(this IdentityResource entity)
        {
            if (entity == null)
            {
                return null;
            }

            var model = new Models.IdentityResource
            {
                Enabled = entity.Enabled,
                Name = entity.Name,
                DisplayName = entity.DisplayName,
                Description = entity.Description,
                Required = entity.Required,
                Emphasize = entity.Emphasize,
                ShowInDiscoveryDocument = entity.ShowInDiscoveryDocument,
            };

            if (entity.UserClaims != null)
            {
                foreach (var claim in entity.UserClaims)
                {
                    if (claim?.Type != null)
                    {
                        model.UserClaims.Add(claim.Type);
                    }
                }
            }

            if (entity.Properties != null)
            {
                foreach (var property in entity.Properties)
                {
                    if (property?.Key != null)
                    {
                        model.Properties[property.Key] = property.Value;
                    }
                }
            }

            return model;
        }

        /// <summary>
        /// Maps a model to an entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static IdentityResource ToEntity(this Models.IdentityResource model)
        {
            if (model == null)
            {
                return null;
            }

            var entity = new IdentityResource
            {
                Enabled = model.Enabled,
                Name = model.Name,
                DisplayName = model.DisplayName,
                Description = model.Description,
                Required = model.Required,
                Emphasize = model.Emphasize,
                ShowInDiscoveryDocument = model.ShowInDiscoveryDocument,
                UserClaims = new List<IdentityResourceClaim>(),
                Properties = new List<IdentityResourceProperty>(),
            };

            if (model.UserClaims != null)
            {
                foreach (var claim in model.UserClaims)
                {
                    if (claim != null)
                    {
                        entity.UserClaims.Add(new IdentityResourceClaim { Type = claim });
                    }
                }
            }

            if (model.Properties != null)
            {
                foreach (var property in model.Properties)
                {
                    if (property.Key != null)
                    {
                        entity.Properties.Add(new IdentityResourceProperty
                        {
                            Key = property.Key,
                            Value = property.Value,
                        });
                    }
                }
            }

            return entity;
        }
    }
}

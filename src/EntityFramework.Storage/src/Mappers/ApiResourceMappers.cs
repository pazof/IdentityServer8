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
    /// Extension methods to map to/from entity/model for API resources.
    /// </summary>
    public static class ApiResourceMappers
    {
        /// <summary>
        /// Maps an entity to a model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Models.ApiResource ToModel(this ApiResource entity)
        {
            if (entity == null)
            {
                return null;
            }

            var model = new Models.ApiResource
            {
                Enabled = entity.Enabled,
                Name = entity.Name,
                DisplayName = entity.DisplayName,
                Description = entity.Description,
                ShowInDiscoveryDocument = entity.ShowInDiscoveryDocument,
                AllowedAccessTokenSigningAlgorithms = ParseAllowedSigningAlgorithms(entity.AllowedAccessTokenSigningAlgorithms),
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

            if (entity.Scopes != null)
            {
                foreach (var scope in entity.Scopes)
                {
                    if (scope?.Scope != null)
                    {
                        model.Scopes.Add(scope.Scope);
                    }
                }
            }

            if (entity.Secrets != null)
            {
                foreach (var secret in entity.Secrets)
                {
                    if (secret == null)
                    {
                        continue;
                    }

                    model.ApiSecrets.Add(new Models.Secret
                    {
                        Description = secret.Description,
                        Value = secret.Value,
                        Expiration = secret.Expiration,
                        Type = secret.Type,
                    });
                }
            }

            return model;
        }

        /// <summary>
        /// Maps a model to an entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ApiResource ToEntity(this Models.ApiResource model)
        {
            if (model == null)
            {
                return null;
            }

            var entity = new ApiResource
            {
                Enabled = model.Enabled,
                Name = model.Name,
                DisplayName = model.DisplayName,
                Description = model.Description,
                ShowInDiscoveryDocument = model.ShowInDiscoveryDocument,
                AllowedAccessTokenSigningAlgorithms = JoinAllowedSigningAlgorithms(model.AllowedAccessTokenSigningAlgorithms),
                UserClaims = new List<ApiResourceClaim>(),
                Properties = new List<ApiResourceProperty>(),
                Scopes = new List<ApiResourceScope>(),
                Secrets = new List<ApiResourceSecret>(),
            };

            if (model.UserClaims != null)
            {
                foreach (var claim in model.UserClaims)
                {
                    if (claim != null)
                    {
                        entity.UserClaims.Add(new ApiResourceClaim { Type = claim });
                    }
                }
            }

            if (model.Properties != null)
            {
                foreach (var property in model.Properties)
                {
                    if (property.Key != null)
                    {
                        entity.Properties.Add(new ApiResourceProperty
                        {
                            Key = property.Key,
                            Value = property.Value,
                        });
                    }
                }
            }

            if (model.Scopes != null)
            {
                foreach (var scope in model.Scopes)
                {
                    if (scope != null)
                    {
                        entity.Scopes.Add(new ApiResourceScope { Scope = scope });
                    }
                }
            }

            if (model.ApiSecrets != null)
            {
                foreach (var secret in model.ApiSecrets)
                {
                    if (secret == null)
                    {
                        continue;
                    }

                    entity.Secrets.Add(new ApiResourceSecret
                    {
                        Description = secret.Description,
                        Value = secret.Value,
                        Expiration = secret.Expiration,
                        Type = secret.Type,
                    });
                }
            }

            return entity;
        }

        private static ICollection<string> ParseAllowedSigningAlgorithms(string source)
        {
            var list = new HashSet<string>();
            if (!String.IsNullOrWhiteSpace(source))
            {
                source = source.Trim();
                foreach (var item in source.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct())
                {
                    list.Add(item);
                }
            }

            return list;
        }

        private static string JoinAllowedSigningAlgorithms(ICollection<string> source)
        {
            if (source == null || !source.Any())
            {
                return null;
            }

            return source.Aggregate((x, y) => $"{x},{y}");
        }
    }
}

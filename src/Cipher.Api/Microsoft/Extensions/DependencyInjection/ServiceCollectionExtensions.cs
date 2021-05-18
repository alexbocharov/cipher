// Copyright (c) Alexander Bocharov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Crypton.Api;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCryptonOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CryptonSettings>(configuration);

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v0.1.0", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "Crypton API",
                    Version = "v0.1.0",
                    Description = "The Crypto API service",
                    TermsOfService = "Terms Of Service"
                });
            });

            return services;
        }
    }
}

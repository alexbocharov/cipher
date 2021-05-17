// Copyright (c) Alexander Bocharov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Crypton.Api.Services
{
    public class X509Certificate2Service : IX509Certificate2Service
    {
        private readonly CryptonSettings _settings;
        private readonly ILogger _logger;

        public X509Certificate2Service(IOptionsMonitor<CryptonSettings> settings, ILogger<X509Certificate2Service> logger)
        {
            _settings = settings.CurrentValue;
            _logger = logger;
        }

        public Task<X509Certificate2> GetCertificateAsync()
        {
            X509Certificate2 certificate = null;

            var store = new X509Store(_settings.SigningCertificateStroeName, _settings.SigningCertificateStoreLocation);
            store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);

            try
            {
                certificate = store.Certificates.Find(X509FindType.FindByThumbprint, _settings.SigningCertificateThumbprint, false)[0];
                return Task.FromResult(certificate);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get security certificate (Store: \"{store.Location}\\{store.Name}\" Thumbprint: \"{_settings.SigningCertificateThumbprint}\").");
                _logger.LogTrace(ex, $"Failed to get security certificate (Store: \"{store.Location}\\{store.Name}\" Thumbprint: \"{_settings.SigningCertificateThumbprint}\").");
            }
            finally
            {
                store.Close();
            }

            return Task.FromResult(certificate);
        }
    }
}

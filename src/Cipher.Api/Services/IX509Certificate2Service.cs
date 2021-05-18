// Copyright (c) Alexander Bocharov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Cipher.Api.Services
{
    public interface IX509Certificate2Service
    {
        Task<X509Certificate2> GetCertificateAsync();
    }
}

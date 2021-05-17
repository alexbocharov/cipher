// Copyright (c) Alexander Bocharov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Crypton.Api.Services
{
    public interface IXmlDsigService
    {
        Task<string> SignAsync(string xmlToBase64, X509Certificate2 certificate);
        Task<bool> VerifyAsync(string xmlToBase64, string signToBase64);
    }
}

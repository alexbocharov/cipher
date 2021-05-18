// Copyright (c) Alexander Bocharov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Security.Cryptography.X509Certificates;

namespace Cipher.Api
{
    public class CipherSettings
    {
        public StoreLocation SigningCertificateStoreLocation { get; set; } = StoreLocation.CurrentUser;
        public StoreName SigningCertificateStroeName { get; set; } = StoreName.My;
        public string SigningCertificateThumbprint { get; set; }
    }
}

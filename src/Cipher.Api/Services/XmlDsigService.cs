// Copyright (c) Alexander Bocharov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CryptoPro.Sharpei.Xml;
using Microsoft.Extensions.Logging;

namespace Cipher.Api.Services
{
    public class XmlDsigService : IXmlDsigService
    {
        public XmlDsigService(ILogger<XmlDsigService> logger)
        {
            Logger = logger;
        }

        protected ILogger Logger { get; set; }

        public Task<string> SignAsync(string xmlToBase64, X509Certificate2 certificate)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            var signToBase64 = string.Empty;

            using (var memoryStream = new MemoryStream(Convert.FromBase64String(xmlToBase64)))
            using (var reader = new StreamReader(memoryStream, Encoding.UTF8))
            using (var xmlTextReader = new XmlTextReader(reader))
            {
                var xmlDocument = new XmlDocument { PreserveWhitespace = true };
                xmlDocument.Load(xmlTextReader);

                var signedXml = new SignedXml(xmlDocument) { SigningKey = certificate.PrivateKey };

                var reference = new Reference
                {
                    Uri = string.Empty,
                    DigestMethod = CPSignedXml.XmlDsigGost3411_2012_256Url
                };
                signedXml.AddReference(reference);

                signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;

                var keyInfo = new KeyInfo();
                keyInfo.AddClause(new KeyInfoX509Data(certificate));

                signedXml.KeyInfo = keyInfo;

                signedXml.ComputeSignature();

                var xmlDigitalSignature = signedXml.GetXml();
                signToBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(xmlDigitalSignature.OuterXml));
            }

            return Task.FromResult(signToBase64);
        }

        public Task<bool> VerifyAsync(string xmlToBase64, string signToBase64)
        {
            using (var memoryStream = new MemoryStream(Convert.FromBase64String(xmlToBase64)))
            using (var reader = new StreamReader(memoryStream, Encoding.UTF8))
            using (var xmlTextReader = new XmlTextReader(reader))
            {
                var xmlDocument = new XmlDocument { PreserveWhitespace = true };
                xmlDocument.Load(xmlTextReader);

                using (var memoryStreamSign = new MemoryStream(Convert.FromBase64String(signToBase64)))
                using (var readerSign = new StreamReader(memoryStreamSign, Encoding.UTF8))
                using (var xmlTextReaderSign = new XmlTextReader(readerSign))
                {
                    var xmlDocumentSign = new XmlDocument { PreserveWhitespace = true };
                    xmlDocumentSign.Load(xmlTextReaderSign);

                    var signedXml = new SignedXml(xmlDocument);

                    var signedElement = xmlDocumentSign.GetElementsByTagName("Signature", SignedXml.XmlDsigNamespaceUrl);
                    signedXml.LoadXml((XmlElement)signedElement[0]);

                    if (signedXml.Signature.KeyInfo.OfType<KeyInfoX509Data>().First()?.Certificates[0] is X509Certificate2 certificate)
                    {
                        try
                        {
                            return Task.FromResult(signedXml.CheckSignature(certificate, true));
                        }
                        catch
                        {
                            Logger.LogError("Signature Verification Error.");
                        }
                    }

                    return Task.FromResult(false);
                }
            }
        }
    }
}

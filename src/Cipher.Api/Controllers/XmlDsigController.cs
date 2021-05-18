// Copyright (c) Alexander Bocharov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Net;
using System.Threading.Tasks;
using Cipher.Api.Models;
using Cipher.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cipher.Api.Controllers
{
    [Route("api/[controller]")]
    public class XmlDsigController : Controller
    {
        private readonly IX509Certificate2Service _certificateService;
        private readonly IXmlDsigService _xmlDsigService;
        private readonly ILogger _logger;

        public XmlDsigController(IX509Certificate2Service certificateService, IXmlDsigService xmlDsigService, ILogger<XmlDsigService> logger)
        {
            _certificateService = certificateService;
            _xmlDsigService = xmlDsigService;
            _logger = logger;
        }

        [HttpPost]
        [Route("sign")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SignAsync([FromBody] XmlDsigSignRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var certificate = await _certificateService.GetCertificateAsync();
            if (certificate == null)
            {
                _logger.LogError("Certificate not configured.");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            var signature = await _xmlDsigService.SignAsync(request.Content, certificate);
            return Ok(signature);
        }

        [HttpPost]
        [Route("verify")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> VerifyAsync([FromBody] XmlDsigVerifyRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var verify = await _xmlDsigService.VerifyAsync(request.Content, request.Signature);
            return Ok(verify);
        }
    }
}

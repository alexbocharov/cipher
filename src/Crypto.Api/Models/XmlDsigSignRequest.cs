// Copyright (c) Alexander Bocharov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace Crypton.Api.Models
{
    public class XmlDsigSignRequest
    {
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}

// Copyright (c) Alexander Bocharov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Mvc;

namespace Crypton.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => new RedirectResult("~/swagger");
    }
}

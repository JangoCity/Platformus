﻿// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Platformus.Barebone;
using Platformus.Domain.Data.Entities;
using Platformus.Globalization;

namespace Platformus.Domain.Frontend
{
  public class DefaultRouteHandler : IDefaultRouteHandler
  {
    public IActionResult TryHandle(IRequestHandler requestHandler, string url)
    {
      IMicrocontrollerResolver microcontrollerResolver = requestHandler.HttpContext.RequestServices.GetService<IMicrocontrollerResolver>();
      Microcontroller microcontroller = microcontrollerResolver.GetMicrocontroller(requestHandler, url);

      if (microcontroller == null)
        return null;

      IMicrocontroller microcontrollerInstance = this.GetMicrocontrollerInstance(microcontroller);

      if (microcontrollerInstance == null)
        return null;

      return microcontrollerInstance.Invoke(requestHandler, microcontroller, microcontrollerResolver.GetParameters(microcontroller.UrlTemplate, url));
    }

    private IMicrocontroller GetMicrocontrollerInstance(Microcontroller microcontroller)
    {
      return StringActivator.CreateInstance<IMicrocontroller>(microcontroller.CSharpClassName);
    }
  }
}
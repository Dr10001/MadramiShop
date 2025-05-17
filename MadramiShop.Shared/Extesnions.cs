﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MadramiShop.Shared;

public static class Extesnions
{
    public static int GetUserId(this ClaimsPrincipal principal) =>
    Convert.ToInt32(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
}

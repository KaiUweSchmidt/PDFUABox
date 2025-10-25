// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Identity;
using PDFUABox.WebApp.Areas.Identity.Data;

namespace PDFUABox.WebApp.Extensions;

internal static class UserManagerExtensions
{
    public static string GetUserIdSafe(this UserManager<PDFUABoxUser> userManager, System.Security.Claims.ClaimsPrincipal user)
    {
        ArgumentNullException.ThrowIfNull(nameof(user), "User cannot be null.");
        var userEntity = userManager.GetUserAsync(user).Result; // Get the current user
        if (userEntity == null)
        {
            throw new InvalidOperationException("User not found.");
        }
        return userEntity.Id;
    }
}

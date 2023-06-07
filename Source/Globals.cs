// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aksio.IngressMiddleware;

#pragma warning disable MA0069 // Avoid static globals

/// <summary>
/// Represents global settings for the ingress middleware.
/// </summary>
public static class Globals
{
    /// <summary>
    /// Gets the default <see cref="JsonSerializerOptions"/> to use.
    /// </summary>
    public static JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };
}
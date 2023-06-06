// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;
using Aksio.IngressMiddleware.Security;

namespace Aksio.IngressMiddleware;

/// <summary>
/// Represents a claim.
/// </summary>
public class RawClaim
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Claim"/> class.
    /// </summary>
    public RawClaim()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Claim"/> class.
    /// </summary>
    /// <param name="type">Type of claim.</param>
    /// <param name="value">Value for the claim.</param>
    public RawClaim(string type, string value)
    {
        Type = type;
        Value = value;
    }

    /// <summary>
    /// Gets or sets the type of claim.
    /// </summary>
    [JsonPropertyName("typ")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the claim.
    /// </summary>
    [JsonPropertyName("val")]
    public string Value { get; set; } = string.Empty;
}

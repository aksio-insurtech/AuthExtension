// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;

namespace Aksio.IngressMiddleware.Security;

/// <summary>
/// Represents a claim.
/// </summary>
public class Claim
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Claim"/> class.
    /// </summary>
    public Claim()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Claim"/> class.
    /// </summary>
    /// <param name="type">Type of claim.</param>
    /// <param name="value">Value for the claim.</param>
    public Claim(string type, string value)
    {
        Type = type;
        Value = value;
    }

    /// <summary>
    /// Gets or sets the type of claim.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the claim.
    /// </summary>
    public string Value { get; set; } = string.Empty;
}

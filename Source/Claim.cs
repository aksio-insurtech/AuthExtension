// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;

namespace Aksio.IngressMiddleware;

public class Claim
{
    public Claim()
    {
    }

    public Claim(string type, string value)
    {
        Type = type;
        Value = value;
    }

    [JsonPropertyName("typ")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("val")]
    public string Value { get; set; } = string.Empty;
}
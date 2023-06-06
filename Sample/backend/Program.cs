using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/.aksio/me", (HttpRequest request) => {
    var principal = request.Headers["x-ms-client-principal"].ToString();
    var json = Convert.FromBase64String(principal);
    var jsonText = Encoding.UTF8.GetString(json);

    var jsonObject = (JsonSerializer.Deserialize<JsonObject>(jsonText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }))!;

    var nameClaim = ((jsonObject["claims"] as JsonArray)!
        .Where(_ => _ is JsonObject)
        .Cast<JsonObject>()
        .Single(_ => _["typ"]!.GetValue<string>() == "name"))!;
    var name = nameClaim["val"]!.GetValue<string>()!;

    return $"Hello {name}";
});

app.Run();

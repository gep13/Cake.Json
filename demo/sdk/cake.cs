#!/usr/bin/env dotnet
#:sdk Cake.Sdk@6.2.0
#:project ../../src/Cake.Json/Cake.Json.csproj

// Cake SDK consumer demo for Cake.Json. Runs as a file-based .NET
// program (introduced in .NET 10) using the Cake.Sdk directives.
// The #:project directive above lets the SDK build the addin from
// source rather than referencing a published nupkg.
//
// To run locally:
//   cd demo/sdk
//   dotnet cake.cs
//
// Mirrors the alias surface exercised by demo/script/json.cake and
// demo/frosting/, against a temp working directory under
// demo/sdk/BuildArtifacts/ (gitignored). Each task asserts the
// expected outcome and throws on mismatch — the script fails
// (non-zero exit) if any alias misbehaves.

using Cake.Json;
using Newtonsoft.Json.Linq;

var workDir = Directory("./BuildArtifacts/temp/test-json-sdk");
var sampleFile = workDir + File("sample.json");
var roundtripFile = workDir + File("roundtrip.json");
var prettyFile = workDir + File("pretty.json");

Task("Default")
    .IsDependentOn("Setup")
    .IsDependentOn("Serialize-To-String")
    .IsDependentOn("Deserialize-From-String")
    .IsDependentOn("Roundtrip-File")
    .IsDependentOn("Pretty-Format")
    .IsDependentOn("Parse-JObject-From-String")
    .IsDependentOn("Parse-JObject-From-File")
    .IsDependentOn("Cleanup");

Task("Setup")
    .Does(() =>
{
    if (DirectoryExists(workDir))
    {
        DeleteDirectory(workDir, new DeleteDirectorySettings { Recursive = true });
    }

    EnsureDirectoryExists(workDir);
    System.IO.File.WriteAllText(
        sampleFile.Path.FullPath,
        "{ \"Name\": \"Whiskers\", \"Age\": 7 }");
    Information("Setup complete.");
});

Task("Serialize-To-String")
    .IsDependentOn("Setup")
    .Does(() =>
{
    var pet = new Pet { Name = "Rex", Age = 3 };
    var json = SerializeJson(pet);

    AssertThat(json.Contains("\"Name\":\"Rex\""), "SerializeJson: missing Name");
    AssertThat(json.Contains("\"Age\":3"), "SerializeJson: missing Age");
    Information("SerializeJson OK ({0})", json);
});

Task("Deserialize-From-String")
    .IsDependentOn("Setup")
    .Does(() =>
{
    var pet = DeserializeJson<Pet>("{ \"Name\": \"Mittens\", \"Age\": 5 }");

    AssertThat(pet.Name == "Mittens", "DeserializeJson: Name mismatch");
    AssertThat(pet.Age == 5, "DeserializeJson: Age mismatch");
    Information("DeserializeJson OK ({0}, age {1})", pet.Name, pet.Age);
});

Task("Roundtrip-File")
    .IsDependentOn("Setup")
    .Does(() =>
{
    var pet = new Pet { Name = "Buddy", Age = 2 };
    SerializeJsonToFile(roundtripFile, pet);
    AssertThat(System.IO.File.Exists(roundtripFile.Path.FullPath),
        "SerializeJsonToFile: file not created");

    var loaded = DeserializeJsonFromFile<Pet>(roundtripFile);
    AssertThat(loaded.Name == "Buddy", "Roundtrip: Name mismatch");
    AssertThat(loaded.Age == 2, "Roundtrip: Age mismatch");
    Information("SerializeJsonToFile + DeserializeJsonFromFile OK ({0}, age {1})", loaded.Name, loaded.Age);
});

Task("Pretty-Format")
    .IsDependentOn("Setup")
    .Does(() =>
{
    var pet = new Pet { Name = "Fluffy", Age = 4 };

    var pretty = SerializeJsonPretty(pet);
    AssertThat(pretty.Contains("\n") || pretty.Contains("\r\n"),
        "SerializeJsonPretty: missing newlines (expected indented output)");
    Information("SerializeJsonPretty OK ({0} chars)", pretty.Length);

    SerializeJsonToPrettyFile(prettyFile, pet);
    var diskContent = System.IO.File.ReadAllText(prettyFile.Path.FullPath);
    AssertThat(diskContent.Contains("\n") || diskContent.Contains("\r\n"),
        "SerializeJsonToPrettyFile: missing newlines on disk");
    Information("SerializeJsonToPrettyFile OK ({0} chars on disk)", diskContent.Length);
});

Task("Parse-JObject-From-String")
    .IsDependentOn("Setup")
    .Does(() =>
{
    var jobj = ParseJson("{ \"Name\": \"Spot\", \"Age\": 8 }");

    AssertThat((string)jobj["Name"] == "Spot", "ParseJson: Name mismatch");
    AssertThat((int)jobj["Age"] == 8, "ParseJson: Age mismatch");
    Information("ParseJson OK ({0}, age {1})", jobj["Name"], jobj["Age"]);
});

Task("Parse-JObject-From-File")
    .IsDependentOn("Setup")
    .Does(() =>
{
    var jobj = ParseJsonFromFile(sampleFile);

    AssertThat((string)jobj["Name"] == "Whiskers", "ParseJsonFromFile: Name mismatch");
    AssertThat((int)jobj["Age"] == 7, "ParseJsonFromFile: Age mismatch");
    Information("ParseJsonFromFile OK ({0}, age {1})", jobj["Name"], jobj["Age"]);
});

Task("Cleanup")
    .IsDependentOn("Serialize-To-String")
    .IsDependentOn("Deserialize-From-String")
    .IsDependentOn("Roundtrip-File")
    .IsDependentOn("Pretty-Format")
    .IsDependentOn("Parse-JObject-From-String")
    .IsDependentOn("Parse-JObject-From-File")
    .Does(() =>
{
    if (DirectoryExists(workDir))
    {
        DeleteDirectory(workDir, new DeleteDirectorySettings { Recursive = true });
    }

    Information("Cleanup complete.");
});

RunTarget("Default");

// ----- Helpers (must come AFTER top-level statements per CS8803) -----

static void AssertThat(bool condition, string message)
{
    if (!condition)
    {
        throw new Exception("Assertion failed: " + message);
    }
}

public class Pet
{
    public string Name { get; set; }

    public int Age { get; set; }
}

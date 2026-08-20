using System;
using System.IO;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Frosting;
using Cake.Json;

namespace Build.Tasks
{
    [TaskName("Roundtrip-File")]
    [IsDependentOn(typeof(SetupTask))]
    public sealed class RoundtripFileTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var pet = new Pet { Name = "Buddy", Age = 2 };
            context.SerializeJsonToFile(context.RoundtripFile, pet);
            AssertThat(
                File.Exists(context.MakeAbsolute(context.RoundtripFile).FullPath),
                "SerializeJsonToFile: file not created");

            var loaded = context.DeserializeJsonFromFile<Pet>(context.RoundtripFile);
            AssertThat(loaded.Name == "Buddy", "Roundtrip: Name mismatch");
            AssertThat(loaded.Age == 2, "Roundtrip: Age mismatch");
            context.Information(
                "SerializeJsonToFile + DeserializeJsonFromFile OK ({0}, age {1})",
                loaded.Name,
                loaded.Age);
        }

        private static void AssertThat(bool condition, string message)
        {
            if (!condition)
            {
                throw new Exception("Assertion failed: " + message);
            }
        }
    }
}

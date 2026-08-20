using System;
using System.IO;
using Cake.Common.Diagnostics;
using Cake.Frosting;
using Cake.Json;

namespace Build.Tasks
{
    [TaskName("Pretty-Format")]
    [IsDependentOn(typeof(SetupTask))]
    public sealed class PrettyFormatTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var pet = new Pet { Name = "Fluffy", Age = 4 };

            var pretty = context.SerializeJsonPretty(pet);
            AssertThat(
                pretty.Contains("\n") || pretty.Contains("\r\n"),
                "SerializeJsonPretty: missing newlines (expected indented output)");
            context.Information("SerializeJsonPretty OK ({0} chars)", pretty.Length);

            context.SerializeJsonToPrettyFile(context.PrettyFile, pet);
            var diskContent = File.ReadAllText(context.MakeAbsolute(context.PrettyFile).FullPath);
            AssertThat(
                diskContent.Contains("\n") || diskContent.Contains("\r\n"),
                "SerializeJsonToPrettyFile: missing newlines on disk");
            context.Information("SerializeJsonToPrettyFile OK ({0} chars on disk)", diskContent.Length);
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

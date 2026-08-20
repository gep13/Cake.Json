using System;
using Cake.Common.Diagnostics;
using Cake.Frosting;
using Cake.Json;

namespace Build.Tasks
{
    [TaskName("Parse-JObject-From-File")]
    [IsDependentOn(typeof(SetupTask))]
    public sealed class ParseFromFileTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var jobj = context.ParseJsonFromFile(context.SampleFile);

            AssertThat((string)jobj["Name"] == "Whiskers", "ParseJsonFromFile: Name mismatch");
            AssertThat((int)jobj["Age"] == 7, "ParseJsonFromFile: Age mismatch");
            context.Information("ParseJsonFromFile OK ({0}, age {1})", jobj["Name"], jobj["Age"]);
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

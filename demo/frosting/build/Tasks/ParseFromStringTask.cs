using System;
using Cake.Common.Diagnostics;
using Cake.Frosting;
using Cake.Json;

namespace Build.Tasks
{
    [TaskName("Parse-JObject-From-String")]
    [IsDependentOn(typeof(SetupTask))]
    public sealed class ParseFromStringTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var jobj = context.ParseJson("{ \"Name\": \"Spot\", \"Age\": 8 }");

            AssertThat((string)jobj["Name"] == "Spot", "ParseJson: Name mismatch");
            AssertThat((int)jobj["Age"] == 8, "ParseJson: Age mismatch");
            context.Information("ParseJson OK ({0}, age {1})", jobj["Name"], jobj["Age"]);
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

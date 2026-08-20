using System;
using Cake.Common.Diagnostics;
using Cake.Frosting;
using Cake.Json;

namespace Build.Tasks
{
    [TaskName("Serialize-To-String")]
    [IsDependentOn(typeof(SetupTask))]
    public sealed class SerializeToStringTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var pet = new Pet { Name = "Rex", Age = 3 };
            var json = context.SerializeJson(pet);

            AssertThat(json.Contains("\"Name\":\"Rex\""), "SerializeJson: missing Name");
            AssertThat(json.Contains("\"Age\":3"), "SerializeJson: missing Age");
            context.Information("SerializeJson OK ({0})", json);
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

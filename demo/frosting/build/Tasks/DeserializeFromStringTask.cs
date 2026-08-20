using System;
using Cake.Common.Diagnostics;
using Cake.Frosting;
using Cake.Json;

namespace Build.Tasks
{
    [TaskName("Deserialize-From-String")]
    [IsDependentOn(typeof(SetupTask))]
    public sealed class DeserializeFromStringTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var pet = context.DeserializeJson<Pet>("{ \"Name\": \"Mittens\", \"Age\": 5 }");

            AssertThat(pet.Name == "Mittens", "DeserializeJson: Name mismatch");
            AssertThat(pet.Age == 5, "DeserializeJson: Age mismatch");
            context.Information("DeserializeJson OK ({0}, age {1})", pet.Name, pet.Age);
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

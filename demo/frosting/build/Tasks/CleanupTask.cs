using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Frosting;

namespace Build.Tasks
{
    [TaskName("Cleanup")]
    [IsDependentOn(typeof(SerializeToStringTask))]
    [IsDependentOn(typeof(DeserializeFromStringTask))]
    [IsDependentOn(typeof(RoundtripFileTask))]
    [IsDependentOn(typeof(PrettyFormatTask))]
    [IsDependentOn(typeof(ParseFromStringTask))]
    [IsDependentOn(typeof(ParseFromFileTask))]
    public sealed class CleanupTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            if (context.DirectoryExists(context.WorkDir))
            {
                context.DeleteDirectory(
                    context.WorkDir,
                    new DeleteDirectorySettings { Recursive = true });
            }

            context.Information("Cleanup complete.");
        }
    }
}

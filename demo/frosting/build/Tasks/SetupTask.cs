using System.IO;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Frosting;

namespace Build.Tasks
{
    [TaskName("Setup")]
    public sealed class SetupTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            if (context.DirectoryExists(context.WorkDir))
            {
                context.DeleteDirectory(
                    context.WorkDir,
                    new DeleteDirectorySettings { Recursive = true });
            }

            context.EnsureDirectoryExists(context.WorkDir);

            File.WriteAllText(
                context.MakeAbsolute(context.SampleFile).FullPath,
                "{ \"Name\": \"Whiskers\", \"Age\": 7 }");

            context.Information("Setup complete.");
        }
    }
}

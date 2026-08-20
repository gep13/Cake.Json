using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

namespace Build
{
    public class BuildContext : FrostingContext
    {
        public DirectoryPath WorkDir { get; } = "./BuildArtifacts/temp/test-json-frosting";

        public FilePath SampleFile => WorkDir.CombineWithFilePath("sample.json");

        public FilePath RoundtripFile => WorkDir.CombineWithFilePath("roundtrip.json");

        public FilePath PrettyFile => WorkDir.CombineWithFilePath("pretty.json");

        public BuildContext(ICakeContext context)
            : base(context)
        {
        }
    }
}

using McMaster.Extensions.CommandLineUtils;
using StrawberryShake.CodeGeneration.CSharp;
using StrawberryShake.CodeGeneration.CSharp.Analyzers;
using StrawberryShake.Tools.Configuration;
using static System.Environment;
using static StrawberryShake.Tools.GeneratorHelpers;

namespace StrawberryShake.Tools;

public static class ExportCommand
{
    public static void Build(CommandLineApplication generate)
    {
        generate.Description = "Exports Persisted Queries for Strawberry Shake Clients";

        CommandArgument pathArg = generate.Argument(
            "path",
            "The project directory.");

        CommandOption razorArg = generate.Option(
            "-o|--outputPath",
            "Output Directory.",
            CommandOptionType.SingleValue);

        CommandOption jsonArg = generate.Option(
            "-j|--json",
            "Console output as JSON.",
            CommandOptionType.NoValue);

        generate.OnExecuteAsync(ct =>
        {
            var arguments = new ExportCommandArguments(
                pathArg.Value ?? CurrentDirectory,
                razorArg.Value()!);
            var handler = CommandTools.CreateHandler<ExportCommandHandler>(jsonArg);
            return handler.ExecuteAsync(arguments, ct);
        });
    }

    private sealed class ExportCommandHandler : CommandHandler<ExportCommandArguments>
    {
        public ExportCommandHandler(IConsoleOutput output)
        {
            Output = output;
        }

        public IConsoleOutput Output { get; }

        public override async Task<int> ExecuteAsync(
            ExportCommandArguments arguments,
            CancellationToken cancellationToken)
        {
            using var activity = Output.WriteActivity("Export");
            var generator = new CSharpGeneratorClient(GetCodeGenServerLocation());
            var documents = GetDocuments(arguments.Path);
            var configFiles = GetConfigFiles(arguments.Path);

            foreach (var configFileName in configFiles)
            {
                var config = await LoadConfigAsync(configFileName);

                var persistedDir = configFiles.Length == 1
                    ? arguments.Path
                    : Path.Combine(arguments.Path, config.Extensions.StrawberryShake.Name);

                var request = new GeneratorRequest(
                    configFileName,
                    documents,
                    persistedQueryDirectory: persistedDir,
                    option: RequestOptions.ExportPersistedQueries);

                var response = generator.Execute(request);

                if (response.TryLogErrors(activity))
                {
                    return 1;
                }
            }

            return 0;
        }

        private static async Task<GraphQLConfig> LoadConfigAsync(string configFileName)
        {
            string json = await File.ReadAllTextAsync(configFileName);
            return GraphQLConfig.FromJson(configFileName);
        }
    }

    private sealed class ExportCommandArguments
    {
        public ExportCommandArguments(string path, string outputPath)
        {
            Path = path;
            OutputPath = outputPath;
        }

        public string Path { get; }

        public string OutputPath { get; }
    }
}
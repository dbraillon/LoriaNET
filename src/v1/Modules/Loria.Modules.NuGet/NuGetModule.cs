using Loria.Core;
using NuGet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Loria.Modules.NuGet
{
    public class NuGetModule : Module, IAction
    {
        public override string Name => "NuGet module";
        public override string Description => "It allows me to update my capabilities by downloading new modules";

        public string Command => "nuget";
        public string[] SupportedIntents => new[] { SearchIntent };
        public string[] SupportedEntities => new[] { TextEntity };
        public string[] Samples => throw new NotImplementedException();

        public const string SearchIntent = "search";
        public const string InstallIntent = "install";
        public const string UpdateIntent = "update";
        public const string TextEntity = "text";
        public const string PackageEntity = "package";

        public IPackageRepository Repository { get; set; }
        public PackageManager PackageManager { get; set; }
        public IEnumerable<IPackage> Packages { get; set; }

        public NuGetModule(Engine engine) 
            : base(engine)
        {
        }
        
        public override void Configure()
        {
            var moduleDirPath = Path.Combine(Directory.GetCurrentDirectory(), "modules");

            Repository = PackageRepositoryFactory.Default.CreateRepository("http://nuget.loria.io/nuget");
            PackageManager = new PackageManager(Repository, moduleDirPath);
            Packages = PackageManager.LocalRepository.GetPackages();

            Activate();
        }

        public void Perform(Command command)
        {
            var actionCommand = command.AsActionCommand();

            switch (actionCommand.Intent)
            {
                case SearchIntent:
                    Search(actionCommand);
                    break;

                case InstallIntent:
                    Install(actionCommand);
                    break;

                case UpdateIntent:
                    Update(actionCommand);
                    break;
            }
        }

        public void Search(ActionCommand command)
        {
            var textEntity = command.GetEntity(TextEntity);
            if (textEntity == null) return;

            var text = textEntity.Value;
            var results = Repository.Search(text, allowPrereleaseVersions: false);

            var callbackLines = new List<string>();

            foreach (var result in results)
            {
                callbackLines.Add($"{result.Title} - {result.Description}");
            }

            var callback = callbackLines.Any() ?
                string.Join(Environment.NewLine, callbackLines) :
                "No result found";

            Engine.Propagator.PropagateCallback(new Command("console", callback));
        }

        public void Install(ActionCommand command)
        {
            var packageEntity = command.GetEntity(PackageEntity);
            if (packageEntity == null) return;

            var package = packageEntity.Value;

            PackageManager.InstallPackage(package);
        }

        public void Update(ActionCommand command)
        {
            var packages = PackageManager.SourceRepository.GetUpdates(Packages, includePrerelease: false, includeAllVersions: true);

            foreach (var package in Packages)
            {
                PackageManager.UpdatePackage(package, updateDependencies: false, allowPrereleaseVersions: false);
            }
        }
    }
}

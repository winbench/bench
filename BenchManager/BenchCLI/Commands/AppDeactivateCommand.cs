using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class AppDeactivateCommand : BenchCommand
    {
        private const string POSITIONAL_APP_ID = "App ID";

        public override string Name => "deactivate";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command removes an app from the activation list or marks it as deactivated.")
                .End(BlockType.Paragraph)
                .Begin(BlockType.Paragraph)
                .Text("To actually uninstall the app, you have to run the ").Keyword("setup").Text(" command.")
                .End(BlockType.Paragraph)
                .Begin(BlockType.Paragraph)
                .Text("If the app is currently on the activation list, it is removed from it.").LineBreak()
                .Text("If the app is required by Bench, or as a dependency, it is marked as deactivated.")
                .End(BlockType.Paragraph);

            var positionalAppId = new PositionalArgument(POSITIONAL_APP_ID,
                ArgumentValidation.IsIdString,
                1);
            positionalAppId.Description
                .Text("Specifies the app to deactivate.");
            positionalAppId.PossibleValueInfo
                .Text("An app ID is an alphanumeric string without whitespace.");

            parser.RegisterArguments(
                positionalAppId);
        }

        protected override bool ExecuteCommand(string[] args)
        {
            var appId = Arguments.GetPositionalValue(POSITIONAL_APP_ID);
            var cfg = LoadConfiguration();

            if (!cfg.ContainsGroup(appId))
            {
                WriteError("The app '{0}' was not found.", appId);
                return false;
            }

            var activationFile = cfg.GetStringValue(PropertyKeys.AppActivationFile);
            if (!File.Exists(activationFile))
            {
                WriteError("The activation file for apps was not found.");
                WriteLine("  " + activationFile);
                return false;
            }
            WriteDetail("Found activation file: " + activationFile);
            var deactivationFile = cfg.GetStringValue(PropertyKeys.AppDeactivationFile);
            if (!File.Exists(deactivationFile))
            {
                WriteError("The deactivation file for apps was not found.");
                WriteLine("  " + deactivationFile);
                return false;
            }
            WriteDetail("Found deactivation file: " + deactivationFile);
            var activationList = new ActivationFile(activationFile);
            var deactivationList = new ActivationFile(deactivationFile);

            var app = cfg.Apps[appId];
            WriteDetail("App ID: " + appId);
            if (app.IsDeactivated)
            {
                WriteDetail("The app is allready deactivated.");
                return true;
            }
            if (app.IsActivated)
            {
                WriteDetail("Removing the app from the activation file.");
                activationList.SignOut(appId);
            }
            if (app.IsRequired)
            {
                WriteDetail("The app is required by Bench.");
                if (AskForAssurance("Are you sure you want to deactivate an app, which is required by Bench?"))
                {
                    WriteDetail("Adding the app to the deactivation file.");
                    deactivationList.SignIn(appId);
                    return true;
                }
                else return false;
            }
            if (app.IsDependency)
            {
                WriteDetail("The app is required by the following apps: " + string.Join(", ", app.Responsibilities));
                if (AskForAssurance("Are you sure you want to deactivate an app, which is required by another app?"))
                {
                    WriteDetail("Adding the app to the deactivation file.");
                    deactivationList.SignIn(appId);
                    return true;
                }
                else return false;
            }
            return true;
        }
    }
}

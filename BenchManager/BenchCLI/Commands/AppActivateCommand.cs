using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class AppActivateCommand : BenchCommand
    {
        private const string POSITIONAL_APP_ID = "App ID";

        public override string Name => "activate";

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command marks an app as activated.")
                .End(BlockType.Paragraph)
                .Begin(BlockType.Paragraph)
                .Text("To actually install the app, you have to run the ").Keyword("setup").Text(" command.")
                .End(BlockType.Paragraph)
                .Begin(BlockType.Paragraph)
                .Text("If the app is currently active as a dependency, it is marked as activated anyways.").LineBreak()
                .Text("If the app is required by Bench, it is not marked as activated.").LineBreak()
                .Text("If the app is marked as deactivated, this mark is removed.")
                .End(BlockType.Paragraph);

            var positionalAppId = new PositionalArgument(POSITIONAL_APP_ID,
                ArgumentValidation.IsIdString,
                1);
            positionalAppId.Description
                .Text("Specifies the app to activate.");
            positionalAppId.PossibleValueInfo
                .Text("An app ID is an alphanumeric string without whitespace.");

            parser.RegisterArguments(
                positionalAppId);
        }

        protected override bool ExecuteCommand(string[] args)
        {
            var appId = Arguments.GetPositionalValue(POSITIONAL_APP_ID);
            var cfg = LoadConfiguration();

            if (!cfg.Apps.Exists(appId))
            {
                WriteError("The app '{0}' was not found.", appId);
                return false;
            }

            var activationFile = cfg.GetStringValue(ConfigPropertyKeys.AppActivationFile);
            if (!File.Exists(activationFile))
            {
                WriteError("The activation file for apps was not found.");
                WriteLine("  " + activationFile);
                return false;
            }
            WriteDetail("Found activation file: " + activationFile);
            var deactivationFile = cfg.GetStringValue(ConfigPropertyKeys.AppDeactivationFile);
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
                WriteDetail("Removing the app from the deactivation file.");
                deactivationList.SignOut(appId);
            }
            if (app.IsRequired)
            {
                WriteDetail("The app is required and does not need to be activated.");
                return true;
            }
            if (app.IsActivated)
            {
                WriteDetail("The app is already activated.");
                return true;
            }
            WriteDetail("Adding the app to the activation file.");
            activationList.SignIn(appId);
            if (!app.IsSupported)
            {
                WriteError("The app is activated now, but it is not supported on this system. That is why it will not be installed.");
            }
            return true;
        }
    }
}

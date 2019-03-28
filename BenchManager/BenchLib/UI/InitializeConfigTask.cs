using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Mastersign.Sequence.Sequence;
using Mastersign.Bench.Markdown;

namespace Mastersign.Bench.UI
{
    internal class InitializeConfigTask : WizzardTask
    {
        private ProxyStepControl stepProxy;
        private UserIdentificationStepControl stepUserIdentification;
        private MachineArchitectureStepControl stepMachineArchitecture;
        private ExistingConfigStepControl stepExistingConfig;
        private IsolationStepControl stepIsolation;
        private AppSelectionStepControl stepAppSeletion;
        private AdvancedStepControl stepAdvanced;

        private readonly BenchConfiguration config;

        public InitializeConfigTask(BenchConfiguration config,
            bool initSiteConfig, bool initUserConfig)
        {
            this.config = config;
            InitSiteConfig = initSiteConfig;
            InitUserConfig = initUserConfig;
        }

        public bool InitSiteConfig { get; private set; }

        public bool InitUserConfig { get; private set; }

        public override WizzardStepControl[] StepControls
        {
            get
            {
                var steps = new List<WizzardStepControl>();
                steps.Add(stepProxy);
                steps.Add(stepUserIdentification);
                steps.Add(stepMachineArchitecture);
                steps.Add(stepExistingConfig);
                steps.Add(stepIsolation);
                steps.Add(stepAppSeletion);
                steps.Add(stepAdvanced);
                return steps.ToArray();
            }
        }

        public override bool IsStepVisible(WizzardStepControl wsc)
        {
            if (wsc == stepProxy) return InitSiteConfig;
            if (wsc == stepUserIdentification) return InitSiteConfig;
            if (wsc == stepMachineArchitecture) return InitSiteConfig && Windows.MachineArchitecture.Is64BitOperatingSystem;
            if (wsc == stepExistingConfig) return InitUserConfig;
            if (wsc == stepIsolation) return InitUserConfig && stepExistingConfig.ConfigSource == ExistingConfigStepControl.ConfigSourceType.None;
            if (wsc == stepAppSeletion) return InitUserConfig && stepExistingConfig.ConfigSource == ExistingConfigStepControl.ConfigSourceType.None;
            if (wsc == stepAdvanced) return true;
            return false;
        }

        public override void Before()
        {
            base.Before();

            stepProxy = new ProxyStepControl();
            var proxyInfo = BenchProxyInfo.SystemDefault;
            stepProxy.UseProxy = proxyInfo.UseProxy; // config.GetBooleanValue(PropertyKeys.UseProxy);
            stepProxy.HttpProxy = proxyInfo.HttpProxyAddress; // config.GetStringValue(PropertyKeys.HttpProxy);
            stepProxy.HttpsProxy = proxyInfo.HttpsProxyAddress; //  config.GetStringValue(PropertyKeys.HttpsProxy);
            stepProxy.ProxyBypass = config.GetStringListValue(ConfigPropertyKeys.ProxyBypass);

            stepUserIdentification = new UserIdentificationStepControl();
            stepUserIdentification.UserName = config.GetStringValue(ConfigPropertyKeys.UserName);
            stepUserIdentification.UserEmail = config.GetStringValue(ConfigPropertyKeys.UserEmail);

            stepMachineArchitecture = new MachineArchitectureStepControl();
            stepMachineArchitecture.Allow64Bit = config.GetBooleanValue(ConfigPropertyKeys.Allow64Bit) 
                && Windows.MachineArchitecture.Is64BitOperatingSystem;

            stepExistingConfig = new ExistingConfigStepControl();
            stepExistingConfig.ConfigSource = ExistingConfigStepControl.ConfigSourceType.None;

            stepIsolation = new IsolationStepControl();
            stepIsolation.IntegrateIntoUserProfile = false;

            stepAppSeletion = new AppSelectionStepControl();
            stepAppSeletion.InitializeStepControl(
                Seq(config.GetStringListValue(ConfigPropertyKeys.WizzardApps))
                .Map(ValueParser.ParseKeyValuePair)
                .ToArray());

            stepAdvanced = new AdvancedStepControl();
            stepAdvanced.StartAutoSetup = config.GetBooleanValue(
                ConfigPropertyKeys.WizzardStartAutoSetup, true);
        }

        public override void After()
        {
            if (InitSiteConfig)
            {
                var bypassList = new List<string>();
                foreach (var e in stepProxy.ProxyBypass) bypassList.Add("`" + e + "`");
                var updates = new Dictionary<string, string>
                    {
                        { ConfigPropertyKeys.UserName, stepUserIdentification.UserName },
                        { ConfigPropertyKeys.UserEmail, stepUserIdentification.UserEmail },
                        { ConfigPropertyKeys.UseProxy, stepProxy.UseProxy ? "true" : "false" },
                        { ConfigPropertyKeys.HttpProxy, stepProxy.HttpProxy },
                        { ConfigPropertyKeys.HttpsProxy, stepProxy.HttpsProxy },
                        { ConfigPropertyKeys.ProxyBypass, string.Join(", ", bypassList.ToArray()) },
                        { ConfigPropertyKeys.Allow64Bit, stepMachineArchitecture.Allow64Bit  ? "true" : "false" }
                    };
                var siteConfigTemplateFile = config.GetStringValue(ConfigPropertyKeys.SiteConfigTemplateFile);
                var defaultSiteConfigFile = Path.Combine(config.BenchRootDir,
                    config.GetStringValue(ConfigPropertyKeys.SiteConfigFileName));
                if (File.Exists(defaultSiteConfigFile))
                {
                    var backupFile = defaultSiteConfigFile + ".bak";
                    if (!File.Exists(backupFile))
                    {
                        File.Move(defaultSiteConfigFile, backupFile);
                    }
                }
                File.Copy(siteConfigTemplateFile, defaultSiteConfigFile, false);
                MarkdownPropertyEditor.UpdateFile(defaultSiteConfigFile, updates);
            }
            if (InitUserConfig)
            {
                switch (stepExistingConfig.ConfigSource)
                {
                    case ExistingConfigStepControl.ConfigSourceType.Directory:
                        config.SetValue(ConfigPropertyKeys.UserConfigInitDirectory, stepExistingConfig.ConfigDirectory);
                        break;
                    case ExistingConfigStepControl.ConfigSourceType.ZipFile:
                        config.SetValue(ConfigPropertyKeys.UserConfigInitZipFile, stepExistingConfig.ConfigZipFile);
                        break;
                    case ExistingConfigStepControl.ConfigSourceType.GitRepo:
                        config.SetValue(ConfigPropertyKeys.UserConfigRepository, stepExistingConfig.ConfigGitRepo);
                        break;
                    default:
                        config.SetValue(ConfigPropertyKeys.UserConfigRepository, (object)null);
                        config.SetValue(ConfigPropertyKeys.WizzardIntegrateIntoUserProfile, stepIsolation.IntegrateIntoUserProfile);
                        config.SetValue(ConfigPropertyKeys.WizzardSelectedApps, stepAppSeletion.SelectedApps);
                        break;
                }
            }
            config.SetValue(ConfigPropertyKeys.WizzardStartAutoSetup, stepAdvanced.StartAutoSetup);

            base.After();
        }
    }
}

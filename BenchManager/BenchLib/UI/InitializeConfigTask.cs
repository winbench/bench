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
        private ExistingConfigStepControl stepExistingConfig;
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
                if (InitSiteConfig)
                {
                    steps.Add(stepProxy);
                    steps.Add(stepUserIdentification);
                }
                if (InitUserConfig)
                {
                    steps.Add(stepExistingConfig);
                    steps.Add(stepAppSeletion);
                }
                steps.Add(stepAdvanced);
                return steps.ToArray();
            }
        }

        public override void Before()
        {
            base.Before();

            if (InitSiteConfig)
            {
                stepProxy = new ProxyStepControl();
                var proxyInfo = BenchProxyInfo.SystemDefault;
                stepProxy.UseProxy = proxyInfo.UseProxy; // config.GetBooleanValue(PropertyKeys.UseProxy);
                stepProxy.HttpProxy = proxyInfo.HttpProxyAddress; // config.GetStringValue(PropertyKeys.HttpProxy);
                stepProxy.HttpsProxy = proxyInfo.HttpsProxyAddress; //  config.GetStringValue(PropertyKeys.HttpsProxy);
                stepProxy.ProxyBypass = config.GetStringListValue(ConfigPropertyKeys.ProxyBypass);

                stepUserIdentification = new UserIdentificationStepControl();
                stepUserIdentification.UserName = config.GetStringValue(ConfigPropertyKeys.UserName);
                stepUserIdentification.UserEmail = config.GetStringValue(ConfigPropertyKeys.UserEmail);
            }
            if (InitUserConfig)
            {
                stepExistingConfig = new ExistingConfigStepControl();
                stepExistingConfig.IsConfigGitRepoExisting = false;

                stepAppSeletion = new AppSelectionStepControl();
                stepAppSeletion.InitializeStepControl(
                    Seq(config.GetStringListValue(ConfigPropertyKeys.WizzardApps))
                    .Map(DictionaryValueResolver.ParseKeyValuePair)
                    .ToArray());
            }
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
                if (stepExistingConfig.IsConfigGitRepoExisting)
                {
                    config.SetValue(ConfigPropertyKeys.UserConfigRepository, stepExistingConfig.ConfigGitRepo);
                }
                else
                {
                    config.SetValue(ConfigPropertyKeys.UserConfigRepository, (object)null);
                }
                config.SetValue(ConfigPropertyKeys.WizzardSelectedApps, stepAppSeletion.SelectedApps);
            }
            config.SetValue(ConfigPropertyKeys.WizzardStartAutoSetup, stepAdvanced.StartAutoSetup);

            base.After();
        }
    }
}

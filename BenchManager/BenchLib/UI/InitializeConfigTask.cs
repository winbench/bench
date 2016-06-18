using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mastersign.Bench.Markdown;

namespace Mastersign.Bench.UI
{
    class InitializeConfigTask : WizzardTask
    {
        private ProxyStepControl stepProxy;
        private UserIdentificationStepControl stepUserIdentification;
        private ExistingConfigStepControl stepExistingConfig;
        private AdvancedStepControl stepAdvanced;

        private readonly BenchConfiguration config;

        public InitializeConfigTask(BenchConfiguration config,
            bool initSiteConfig, bool initCustomConfig)
        {
            this.config = config;
            InitSiteConfig = initSiteConfig;
            InitCustomConfig = initCustomConfig;
        }

        public bool InitSiteConfig { get; private set; }

        public bool InitCustomConfig { get; private set; }

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
                if (InitCustomConfig)
                {
                    steps.Add(stepExistingConfig);
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
                stepProxy.ProxyBypass = config.GetStringListValue(PropertyKeys.ProxyBypass);

                stepUserIdentification = new UserIdentificationStepControl();
                stepUserIdentification.UserName = config.GetStringValue(PropertyKeys.UserName);
                stepUserIdentification.UserEmail = config.GetStringValue(PropertyKeys.UserEmail);
            }
            if (InitCustomConfig)
            {
                stepExistingConfig = new ExistingConfigStepControl();
                stepExistingConfig.IsConfigGitRepoExisting = false;
            }
            stepAdvanced = new AdvancedStepControl();
            stepAdvanced.EditCustomConfigBeforeSetup = config.GetBooleanValue(
                PropertyKeys.WizzardEditCustomConfigBeforeSetup, false);
            stepAdvanced.StartAutoSetup = config.GetBooleanValue(
                PropertyKeys.WizzardStartAutoSetup, true);
        }

        public override void After()
        {
            if (InitSiteConfig)
            {
                var bypassList = new List<string>();
                foreach (var e in stepProxy.ProxyBypass) bypassList.Add("`" + e + "`");
                var updates = new Dictionary<string, string>
                    {
                        {PropertyKeys.UserName, stepUserIdentification.UserName },
                        { PropertyKeys.UserEmail, stepUserIdentification.UserEmail },
                        { PropertyKeys.UseProxy, stepProxy.UseProxy ? "true" : "false" },
                        { PropertyKeys.HttpProxy, stepProxy.HttpProxy },
                        { PropertyKeys.HttpsProxy, stepProxy.HttpsProxy },
                        { PropertyKeys.ProxyBypass, string.Join(", ", bypassList.ToArray()) },
                    };
                var siteConfigTemplateFile = config.GetStringValue(PropertyKeys.SiteConfigTemplateFile);
                var defaultSiteConfigFile = Path.Combine(config.BenchRootDir,
                    config.GetStringValue(PropertyKeys.SiteConfigFileName));
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

            if (InitCustomConfig)
            {
                if (stepExistingConfig.IsConfigGitRepoExisting)
                {
                    config.SetValue(PropertyKeys.CustomConfigRepository, stepExistingConfig.ConfigGitRepo);
                }
                else
                {
                    config.SetValue(PropertyKeys.CustomConfigRepository, (object)null);
                }
            }
            config.SetValue(PropertyKeys.WizzardEditCustomConfigBeforeSetup, stepAdvanced.EditCustomConfigBeforeSetup);
            config.SetValue(PropertyKeys.WizzardStartAutoSetup, stepAdvanced.StartAutoSetup);

            base.After();
        }
    }
}

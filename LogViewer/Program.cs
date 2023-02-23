using System.Configuration;

namespace LogViewer
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            SetDefaultConfigEncryption(true);
            Application.Run(new Form_Main());
        }

        private static void SetDefaultConfigEncryption(bool isEncrypt)
        {
            SetConfigSectionEncryption(isEncrypt, "appSettings");
            SetConfigSectionEncryption(isEncrypt, "connectionStrings");
        }

        private static void SetConfigSectionEncryption(bool isEncrypt, string sectionKey)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationSection section = config.GetSection(sectionKey);
            if (section != null)
            {
                if (isEncrypt && !section.SectionInformation.IsProtected)
                {
                    section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                }
                if (!isEncrypt && section.SectionInformation.IsProtected)
                {
                    section.SectionInformation.UnprotectSection();
                }
                section.SectionInformation.ForceSave = true;
                config.Save(ConfigurationSaveMode.Full);
            }
        }
    }
}
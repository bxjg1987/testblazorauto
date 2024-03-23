using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Configuration;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Net.Mail;
using BXJG.Utils.Share;
using ZLJ.Core.Configuration;
using ZLJ.Core.Web;

namespace ZLJ.EntityFrameworkCore.Seed.Host
{
    public class DefaultSettingsCreator
    {
        private readonly ZLJDbContext _context;

        public DefaultSettingsCreator(ZLJDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            int? tenantId = null;

            if (ZLJ.Core.Share.ZLJConsts.MultiTenancyEnabled == false)
            {
                tenantId = MultiTenancyConsts.DefaultTenantId;
            }

            // Emailing
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "admin@mydomain.com", tenantId);
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "mydomain.com mailer", tenantId);

            // Language
            //AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "en", tenantId);

            //设置为默认中文
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "zh-Hans", MultiTenancyConsts.DefaultTenantId);
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "zh-Hans", null);
        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.IgnoreQueryFilters().Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }
            //var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());
            _context.Settings.Add(new Setting(tenantId, null, name, value));
            //_context.Settings.Add(new Setting(default, default, BXJGUtilsConsts.FuwuqiGen, configuration["App:ServerRootAddress"] ));
            _context.SaveChanges();
        }
    }
}

using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Localization.Sources;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections.Immutable;
using BXJG.Utils.RCL;

namespace BXJG.Utils.RCL.Abps
{
    public class LocalizationSource : ILocalizationSource
    {
        AppContainer _appContainer;

        public LocalizationSource(string name)
        {
            _appContainer = AppContainer.App;

        }

        public string Name { get; set; }

        public IReadOnlyList<LocalizedString> GetAllStrings(bool includeDefaults = true)
        {
            return GetAllStrings(CultureInfo.CurrentUICulture, includeDefaults);
        }

        public IReadOnlyList<LocalizedString> GetAllStrings(CultureInfo culture, bool includeDefaults = true)
        {
            Dictionary<string, string> yy;
            string n = default;
            if (_appContainer.AbpUserConfiguration.Localization.Values.TryGetValue(Name, out yy))
            {
                n = Name;
                // return yy.Select(x => new LocalizedString(x.Key, x.Value, new CultureInfo(Name))).ToList();
            }
            else if (includeDefaults)
            {
                var default1 = _appContainer.AbpUserConfiguration.Localization.Languages.Single(c => c.IsDefault);
                yy = _appContainer.AbpUserConfiguration.Localization.Values[default1.Name];
                n = default1.Name;
                //(_appContainer.AbpUserConfiguration.Localization.Values.TryGetValue(_appContainer.AbpUserConfiguration.Localization, out yy))
            }

            return yy.Select(x => new LocalizedString(x.Key, x.Value, culture)).ToImmutableList();
        }

        public string GetString(string name)
        {
            return GetAllStrings().Single(c => c.Name == name).Value;
        }

        public string GetString(string name, CultureInfo culture)
        {
            return GetAllStrings(culture).Single(c => c.Name == name).Value;
        }

        public string? GetStringOrNull(string name, bool tryDefaults = true)
        {
            return GetAllStrings().SingleOrDefault(c => c.Name == name)?.Value;
        }

        public string? GetStringOrNull(string name, CultureInfo culture, bool tryDefaults = true)
        {
            return GetAllStrings(culture).SingleOrDefault(c => c.Name == name)?.Value;
        }

        public List<string> GetStrings(List<string> names)
        {
            return GetAllStrings().Where(c => names.Contains(c.Name)).Select(x => x.Value).ToList();
        }

        public List<string> GetStrings(List<string> names, CultureInfo culture)
        {
            return GetAllStrings(culture).Where(c => names.Contains(c.Name)).Select(x => x.Value).ToList();
        }

        public List<string> GetStringsOrNull(List<string> names, bool tryDefaults = true)
        {
            return GetAllStrings().Where(c => names.Contains(c.Name)).Select(x => x.Value).ToList();
        }

        public List<string> GetStringsOrNull(List<string> names, CultureInfo culture, bool tryDefaults = true)
        {
            throw new NotImplementedException();
        }

        public void Initialize(ILocalizationConfiguration configuration, IIocResolver iocResolver)
        {

            //throw new NotImplementedException();
        }
    }
}

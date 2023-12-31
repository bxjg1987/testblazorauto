using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Localization;
using Abp.Localization.Sources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Web.Blazor.Abp
{
    //在blazor组件中，暂时别用本地化。

    public class Class1 : ILocalizationSource
    {
        AppContainer _appContainer;

        public Class1(AppContainer appContainer)
        {
            _appContainer = appContainer;
        }

        public string Name => throw new NotImplementedException();
        
        public IReadOnlyList<LocalizedString> GetAllStrings(bool includeDefaults = true)
        {
         
            return _appContainer.AbpUserConfiguration.Localization.Values[Name].Select(x => new LocalizedString(x.Key, x.Value, new CultureInfo(Name))).ToList();
        }

        public IReadOnlyList<LocalizedString> GetAllStrings(CultureInfo culture, bool includeDefaults = true)
        {
            throw new NotImplementedException();
        }

        public string GetString(string name)
        {
            throw new NotImplementedException();
        }

        public string GetString(string name, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public string GetStringOrNull(string name, bool tryDefaults = true)
        {
            throw new NotImplementedException();
        }

        public string GetStringOrNull(string name, CultureInfo culture, bool tryDefaults = true)
        {
            throw new NotImplementedException();
        }

        public List<string> GetStrings(List<string> names)
        {
            throw new NotImplementedException();
        }

        public List<string> GetStrings(List<string> names, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public List<string> GetStringsOrNull(List<string> names, bool tryDefaults = true)
        {
            throw new NotImplementedException();
        }

        public List<string> GetStringsOrNull(List<string> names, CultureInfo culture, bool tryDefaults = true)
        {
            throw new NotImplementedException();
        }

        public void Initialize(ILocalizationConfiguration configuration, IIocResolver iocResolver)
        {
            throw new NotImplementedException();
        }
    }

    public class LManager : ILocalizationManager
    {
        public IReadOnlyList<ILocalizationSource> GetAllSources()
        {
            throw new NotImplementedException();
        }

        public ILocalizationSource GetSource(string name)
        {
            throw new NotImplementedException();
        }
    }
}

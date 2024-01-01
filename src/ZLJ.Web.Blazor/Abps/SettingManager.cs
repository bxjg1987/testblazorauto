using Abp;
using Abp.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Web.Blazor.Abps
{
    /// <summary>
    /// 前端实现，仅支持读取，写入blazorserver组件
    /// </summary>
    public class SettingManager : ISettingManager
    {
        AppContainer _appContainer;

        public SettingManager(AppContainer appContainer)
        {
            _appContainer = appContainer;
        }

        public void ChangeSettingForApplication(string name, string value)
        {
            throw new NotImplementedException();
        }

        public Task ChangeSettingForApplicationAsync(string name, string value)
        {
            throw new NotImplementedException();
        }

        public void ChangeSettingForTenant(int tenantId, string name, string value)
        {
            throw new NotImplementedException();
        }

        public Task ChangeSettingForTenantAsync(int tenantId, string name, string value)
        {
            throw new NotImplementedException();
        }

        public void ChangeSettingForUser(UserIdentifier user, string name, string value)
        {
            throw new NotImplementedException();
        }

        public Task ChangeSettingForUserAsync(UserIdentifier user, string name, string value)
        {
            throw new NotImplementedException();
        }
        class SV : ISettingValue {
            public string Name { get; set; }
            public string Value { get; set; }
        }
        public IReadOnlyList<ISettingValue> GetAllSettingValues()
        {
            //var list = new List<ISettingValue>();
            //foreach (var setting in _appContainer.AbpUserConfiguration.Setting)
            //{ 

            //}
           return  _appContainer.AbpUserConfiguration.Setting.Values.Select(x=>new SV { Name=x.Key, Value=x.Value }).ToImmutableList();
        }

        public IReadOnlyList<ISettingValue> GetAllSettingValues(SettingScopes scopes)
        {
            return GetAllSettingValues();
        }

        public Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesAsync()
        {
            return Task.FromResult( GetAllSettingValues());
        }

        public Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesAsync(SettingScopes scopes)
        {
            return GetAllSettingValuesAsync();
        }

        public IReadOnlyList<ISettingValue> GetAllSettingValuesForApplication()
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesForApplicationAsync()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<ISettingValue> GetAllSettingValuesForTenant(int tenantId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesForTenantAsync(int tenantId)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<ISettingValue> GetAllSettingValuesForUser(UserIdentifier user)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesForUserAsync(UserIdentifier user)
        {
            throw new NotImplementedException();
        }

        public string GetSettingValue(string name)
        {
            return _appContainer.AbpUserConfiguration.Setting.Values[name];
        }

        public Task<string> GetSettingValueAsync(string name)
        {
            return Task.FromResult(GetSettingValue(name));
        }

        public string GetSettingValueForApplication(string name)
        {
            throw new NotImplementedException();
        }

        public string GetSettingValueForApplication(string name, bool fallbackToDefault)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSettingValueForApplicationAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSettingValueForApplicationAsync(string name, bool fallbackToDefault)
        {
            throw new NotImplementedException();
        }

        public string GetSettingValueForTenant(string name, int tenantId)
        {
            throw new NotImplementedException();
        }

        public string GetSettingValueForTenant(string name, int tenantId, bool fallbackToDefault)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSettingValueForTenantAsync(string name, int tenantId)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSettingValueForTenantAsync(string name, int tenantId, bool fallbackToDefault)
        {
            throw new NotImplementedException();
        }

        public string GetSettingValueForUser(string name, int? tenantId, long userId)
        {
            throw new NotImplementedException();
        }

        public string GetSettingValueForUser(string name, int? tenantId, long userId, bool fallbackToDefault)
        {
            throw new NotImplementedException();
        }

        public string GetSettingValueForUser(string name, UserIdentifier user)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSettingValueForUserAsync(string name, int? tenantId, long userId)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSettingValueForUserAsync(string name, int? tenantId, long userId, bool fallbackToDefault)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSettingValueForUserAsync(string name, UserIdentifier user)
        {
            throw new NotImplementedException();
        }
    }
}

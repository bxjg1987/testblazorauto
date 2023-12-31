using Abp;
using Abp.Configuration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.App.Common.SettingManager
{
    public class SettingManagerAppService : CommonBaseAppService //: ISettingManagerAppService
    {
        //public void ChangeSettingForApplication([FromQuery] string name, [FromQuery] string value)
        //{
        //    throw new NotImplementedException();
        //}
        ISettingManager settingManager;

        public SettingManagerAppService(ISettingManager settingManager)
        {
            this.settingManager = settingManager;
        }

        public Task ChangeSettingForApplicationAsync([FromForm] string name, [FromForm] string value)
        {
            return settingManager.ChangeSettingForApplicationAsync(name, value);
        }

        //public void ChangeSettingForTenant([FromQuery] int tenantId, [FromQuery] string name, [FromQuery] string value)
        //{
        //    throw new NotImplementedException();
        //}

        public Task ChangeSettingForTenantAsync([FromForm] int tenantId, [FromForm] string name, [FromForm] string value)
        {
            return settingManager.ChangeSettingForTenantAsync(tenantId, name, value);
        }

        //public void ChangeSettingForUser([FromForm] UserIdentifier user, [FromQuery] string name, [FromQuery] string value)
        //{
        //    throw new NotImplementedException();
        //}

        public Task ChangeSettingForUserAsync([FromForm] UserIdentifier user, [FromHeader] string name, [FromHeader] string value)
        {
            return settingManager.ChangeSettingForUserAsync(user, name, value);
        }

        //public IReadOnlyList<ISettingValue> GetAllSettingValues()
        //{
        //    throw new NotImplementedException();
        //}

        //public IReadOnlyList<ISettingValue> GetAllSettingValues([FromForm] SettingScopes scopes)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesAsync()
        //{
        //    throw new NotImplementedException();
        //}

        public Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesAsync(SettingScopes scopes)
        {
            return settingManager.GetAllSettingValuesAsync(scopes);
        }

        //public IReadOnlyList<ISettingValue> GetAllSettingValuesForApplication()
        //{
        //    throw new NotImplementedException();
        //}

        public Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesForApplicationAsync()
        {
            return settingManager.GetAllSettingValuesForApplicationAsync();
        }

        //public IReadOnlyList<ISettingValue> GetAllSettingValuesForTenant([FromQuery] int tenantId)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesForTenantAsync([FromQuery] int tenantId)
        {
            return settingManager.GetAllSettingValuesForTenantAsync(tenantId);
        }

        //public IReadOnlyList<ISettingValue> GetAllSettingValuesForUser([FromForm] UserIdentifier user)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesForUserAsync([FromForm] UserIdentifier user)
        {
            return settingManager.GetAllSettingValuesForUserAsync(user);
        }

        //public string GetSettingValue([FromQuery] string name)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<string> GetSettingValueAsync([FromQuery] string name)
        {
            return settingManager.GetSettingValueAsync(name);
        }

        //public string GetSettingValueForApplication([FromQuery] string name)
        //{
        //    throw new NotImplementedException();
        //}

        //public string GetSettingValueForApplication([FromQuery] string name, [FromQuery] bool fallbackToDefault)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<string> GetSettingValueForApplicationAsync([FromQuery] string name)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<string> GetSettingValueForApplicationAsync([FromQuery] string name, [FromQuery] bool fallbackToDefault)
        {
            return settingManager.GetSettingValueForApplicationAsync(name, fallbackToDefault);
        }

        //public string GetSettingValueForTenant([FromQuery] string name, [FromQuery] int tenantId)
        //{
        //    throw new NotImplementedException();
        //}

        //public string GetSettingValueForTenant([FromQuery] string name, [FromQuery] int tenantId, [FromQuery] bool fallbackToDefault)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<string> GetSettingValueForTenantAsync([FromQuery] string name, [FromQuery] int tenantId)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<string> GetSettingValueForTenantAsync([FromQuery] string name, [FromQuery] int tenantId, [FromQuery] bool fallbackToDefault)
        {
            return settingManager.GetSettingValueForTenantAsync(name, tenantId, fallbackToDefault);
        }

        //public string GetSettingValueForUser([FromQuery] string name, [FromQuery] int? tenantId, [FromQuery] long userId)
        //{
        //    throw new NotImplementedException();
        //}

        //public string GetSettingValueForUser([FromQuery] string name, [FromQuery] int? tenantId, [FromQuery] long userId, [FromQuery] bool fallbackToDefault)
        //{
        //    throw new NotImplementedException();
        //}

        //public string GetSettingValueForUser([FromQuery] string name, [FromForm] UserIdentifier user)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<string> GetSettingValueForUserAsync([FromQuery] string name, [FromQuery] int? tenantId, [FromQuery] long userId)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<string> GetSettingValueForUserAsync([FromQuery] string name, [FromQuery] int? tenantId, [FromQuery] long userId, [FromQuery] bool fallbackToDefault)
        {
            return settingManager.GetSettingValueForUserAsync(name, tenantId, userId, fallbackToDefault);
        }

        //public Task<string> GetSettingValueForUserAsync([FromQuery] string name, [FromForm] UserIdentifier user)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

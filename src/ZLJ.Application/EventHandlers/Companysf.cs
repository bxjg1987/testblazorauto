using Abp.Domain.Repositories;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.Runtime.Session;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Authorization.Users;
using ZLJ.BaseInfo.AssociatedCompany;
using ZLJ.Customer;

namespace ZLJ.App.Admin.EventHandlers
{
    public class Companysf : IAsyncEventHandler<EntityCreatingEventData<AssociatedCompanyEntity>>, ITransientDependency
    {
        private readonly UserManager userManager;
        private readonly IAbpSession AbpSession;
        public Companysf(UserManager userManager, IAbpSession abpSession)
        {
            this.userManager = userManager;
            AbpSession = abpSession;
        }

        public async Task HandleEventAsync(EntityCreatingEventData<AssociatedCompanyEntity> eventData)
        {
            var entity = new CustomerStaffInfoEntity();
            entity.Surname = entity.Name;
            entity.Pinyin = TinyPinyin.PinyinHelper.GetPinyinInitials(entity.Name);
            entity.Id = default;
            entity.IsEmailConfirmed = true;
            entity.IsPhoneNumberConfirmed = true;
            entity.IsLockoutEnabled = true;
            entity.IsActive = false;
            entity.CustomerId = eventData.Entity.Id;
            entity.EmailAddress = Guid.NewGuid().ToString("n") + "@a.com";
            await userManager.InitializeOptionsAsync(AbpSession.TenantId);
            CheckErrors(await userManager.CreateAsync(entity, Guid.NewGuid().ToString("n")));
            //await userManager.SetOrganizationUnitsAsync(entity.Id, new[] { input.OuId });
            // var ou = ouRepository.Get(input.OuId);
            //CurrentUnitOfWork.Items["ous"] = new Dictionary<long, CustomerOUEntity> { { entity.Id, ou } };
            //return MapToEntityDto(entity);
        }

        void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors();
        }
    }
}

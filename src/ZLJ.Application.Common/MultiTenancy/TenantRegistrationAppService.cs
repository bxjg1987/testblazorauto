using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Zero.Configuration;
using Lazy.Captcha.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyPinyin;
using ZLJ.Application.Common.Share.MultiTenancy;
using ZLJ.Core.Editions;
using ZLJ.Core.MultiTenancy;

namespace ZLJ.Application.Common.MultiTenancy
{
    /// <summary>
    /// 租户注册应用服务
    /// </summary>
    public class TenantRegistrationAppService : CommonBaseAppService
    {
        private readonly ICaptcha captcha;
        private readonly IHostEnvironment env;
        private readonly TenantManager _tenantManager;

        public TenantRegistrationAppService(ICaptcha captcha, IHostEnvironment env, TenantManager tenantManager)
        {
            this.captcha = captcha;
            this.env = env;
            _tenantManager = tenantManager;
        }

        public async Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input)
        {
            //抄袭zero的
            //if (input.EditionId.HasValue)
            //{
            //    await CheckEditionSubscriptionAsync(input.EditionId.Value, input.SubscriptionStartType);
            //}
            //else
            //{
            //    await CheckRegistrationWithoutEdition();
            //}
            if (/*env.IsProduction() && */!captcha.Validate(input.YzmKey, input.YzmValue))
                    throw new UserFriendlyException("验证失败！验证码错误或失效。");
            using (CurrentUnitOfWork.SetTenantId(null))
            {
              
                //CheckTenantRegistrationIsEnabled();

                //if (UseCaptchaOnRegistration())
                //{
                //    await _recaptchaValidator.ValidateAsync(input.CaptchaResponse);
                //}

                //Getting host-specific settings
                //var isActive = await IsNewRegisteredTenantActiveByDefault(input.SubscriptionStartType);
                //var isEmailConfirmationRequired = await SettingManager.GetSettingValueForApplicationAsync<bool>(
                //    AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin
                //);

                //DateTime? subscriptionEndDate = null;
                //var isInTrialPeriod = false;

                //if (input.EditionId.HasValue)
                //{
                //    isInTrialPeriod = input.SubscriptionStartType == SubscriptionStartType.Trial;

                //    if (isInTrialPeriod)
                //    {
                //        var edition = (SubscribableEdition)await _editionManager.GetByIdAsync(input.EditionId.Value);
                //        subscriptionEndDate = Clock.Now.AddDays(edition.TrialDayCount ?? 0);
                //    }
                //}

                
                var tenantId = await _tenantManager.CreateWithAdminUserAsync(
                    string.Empty,
                    input.Name,
                    input.AdminPassword,
                    null,
                    input.AdminUserName
                    //PinyinHelper.GetPinyin(input.Name) + "@a.com",//  input.AdminEmailAddress,
                    //null,
                    //true,
                    //shouldChangePasswordOnNextLogin: false,
                    //sendActivationEmail: true,
                    //subscriptionEndDate,
                    //isInTrialPeriod,
                    //AppUrlService.CreateEmailActivationUrlFormat(input.TenancyName),
                    //adminName: input.AdminName,
                    //adminSurname: input.AdminSurname
                );

                var tenant = await TenantManager.GetByIdAsync(tenantId);
                //await _appNotifier.NewTenantRegisteredAsync(tenant);

                return new RegisterTenantOutput
                {
                    TenantId = tenant.Id,
                    TenancyName = tenant.TenancyName,
                    Name = input.Name,
                    UserName = AbpUserBase.AdminUserName,
                    Password= input.AdminPassword,
                    //  EmailAddress = 
                    //EmailAddress ="",// input.AdminEmailAddress,
                    IsActive = tenant.IsActive,
                    IsEmailConfirmationRequired = false,// isEmailConfirmationRequired,
                    IsTenantActive =  tenant.IsActive
                };
            }
        }
    }
}

using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.IdentityFramework;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.Threading;
using BXJG.Common.Contracts;
using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Transactions;
using TinyPinyin;

using ZLJ.Core.Authorization.Users;
using ZLJ.Core.Editions;

namespace ZLJ.Core.MultiTenancy
{
    public class TenantManager : AbpTenantManager<Tenant, User>//,IEventhan
    {
        public ICancellationTokenProvider CancellationTokenProvider { get; set; }
        public IAbpZeroDbMigrator _abpZeroDbMigrator { get; set; }
        public RoleManager _roleManager { get; set; }
        public UserManager _userManager { get; set; }
        public IAbpSession AbpSession { get; set; }
        public IPasswordHasher<User> _passwordHasher { get; set; }
        public TenantManager(
            IRepository<Tenant> tenantRepository,
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository,
            EditionManager editionManager,
            IAbpZeroFeatureValueStore featureValueStore)
            : base(
                tenantRepository,
                tenantFeatureRepository,
                editionManager,
                featureValueStore)
        {
        }

        /// <summary>
        /// 创建租户及其默认管理员
        /// 自己注册 或 后台管理租户时都需要
        /// </summary>
        /// <param name="tenancyName"></param>
        /// <param name="name"></param>
        /// <param name="adminPassword"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public async Task<int> CreateWithAdminUserAsync(
            string tenancyName,
            string name,
            string adminPassword,
            //string adminEmailAddress,
            string connectionString,
            //bool isActive,
            //bool shouldChangePasswordOnNextLogin,
            //bool sendActivationEmail,
            //DateTime? subscriptionEndDate,
            //bool isInTrialPeriod,
            //string emailActivationLink,
            string adminName = null
        //string adminSurname = null
        )
        {
            int newTenantId = default;
            long newAdminId;

            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                if (tenancyName.IsNullOrWhiteSpaceBXJG())
                    tenancyName = PinyinHelper.GetPinyinInitials(name) + Random.Shared.Next(1000);

                var tenant = new Tenant(tenancyName, name)
                {
                    IsActive = true,
                    //EditionId = editionId,
                    //SubscriptionEndDateUtc = subscriptionEndDate?.ToUniversalTime(),
                    //IsInTrialPeriod = isInTrialPeriod,
                    ConnectionString = connectionString.IsNullOrWhiteSpace()
                        ? null
                        : SimpleStringCipher.Instance.Encrypt(connectionString)
                };

                await CreateAsync(tenant);
                await UnitOfWorkManager.Current.SaveChangesAsync(); //To get new tenant's id.

                //Create tenant database
                _abpZeroDbMigrator.CreateOrMigrateForTenant(tenant);

                //We are working entities of new tenant, so changing tenant filter
                using (UnitOfWorkManager.Current.SetTenantId(tenant.Id))
                {
                    //Create static roles for new tenant
                    CheckErrors(await _roleManager.CreateStaticRoles(tenant.Id));
                    await UnitOfWorkManager.Current.SaveChangesAsync(); //To get static role ids

                    //grant all permissions to admin role
                    var adminRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.Admin);
                    await _roleManager.GrantAllPermissionsAsync(adminRole);

                    ////User role should be default
                    //var userRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.User);
                    //userRole.IsDefault = true;
                    //CheckErrors(await _roleManager.UpdateAsync(userRole));

                    var adminEmailAddress = tenancyName + "@a.com";
                    //Create admin user for the tenant
                    var adminUser = User.CreateTenantAdminUser(tenant.Id, adminEmailAddress);
                    //adminUser.ShouldChangePasswordOnNextLogin = shouldChangePasswordOnNextLogin;
                    adminUser.IsActive = true;
                    adminUser.UserName = adminName ?? User.AdminUserName;

                    //if (adminPassword.IsNullOrEmpty())
                    //{
                    //    adminPassword = await _userManager.CreateRandomPassword();
                    //}
                    //else
                    //{
                    await _userManager.InitializeOptionsAsync(tenant.Id);
                    foreach (var validator in _userManager.PasswordValidators)
                    {
                        CheckErrors(await validator.ValidateAsync(_userManager, adminUser, adminPassword));
                    }
                    //}
                    //adminUser.Password = _passwordHasher.HashPassword(adminUser, adminPassword);

                    CheckErrors(await _userManager.CreateAsync(adminUser, adminPassword));
                    await UnitOfWorkManager.Current.SaveChangesAsync(); //To get admin user's id

                    //Assign admin user to admin role!
                    CheckErrors(await _userManager.AddToRoleAsync(adminUser, adminRole.Name));

                    ////Notifications
                    //await _appNotifier.WelcomeToTheApplicationAsync(adminUser);

                    ////Send activation email
                    //if (sendActivationEmail)
                    //{
                    //    adminUser.SetNewEmailConfirmationCode();
                    //    await _userEmailer.SendEmailActivationLinkAsync(adminUser, emailActivationLink, adminPassword);
                    //}

                    await UnitOfWorkManager.Current.SaveChangesAsync();

                    //await _backgroundJobManager.EnqueueAsync<TenantDemoDataBuilderJob, int>(tenant.Id);

                    newTenantId = tenant.Id;
                    newAdminId = adminUser.Id;
                }

                await uow.CompleteAsync();
            }

            return newTenantId;
        }




        public ILogger Logger { get; set; }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
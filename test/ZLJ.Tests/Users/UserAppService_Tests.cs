using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Threading.Tasks;
using Xunit;
using ZLJ.Application.Common.StaffInfo;
using ZLJ.Application.Common.Users;
using ZLJ.Application.StaffInfo;

namespace ZLJ.Tests.Users;

public class UserAppService_Tests : ZLJTestBase
{
    private readonly StaffInfoAppService _userAppService;

    public UserAppService_Tests()
    {
        _userAppService = Resolve<StaffInfoAppService>();
    }

    //[Fact]
    //public async Task GetUsers_Test()
    //{
    //    // Act
    //    var output = await _userAppService.GetListAsync(new GetListInput { MaxResultCount = 20, SkipCount = 0 });

    //    // Assert
    //    output.Items.Count.ShouldBeGreaterThan(0);
    //}

    //[Fact]
    //public async Task CreateUser_Test()
    //{
    //    base.LoginAsDefaultTenantAdmin();
    //    // Act
    //    await _userAppService.CreateAsync(
    //        new CreateUserDto
    //        {
    //            EmailAddress = "john@volosoft.com",
    //            IsActive = true,
    //            Name = "John",
    //           // Surname = "Nash",
    //            Password = "123qwe",
    //            UserName = "john.nash", PhoneNumber="3434555", RoleNames=new string[] { }
    //        });

    //    await UsingDbContextAsync(async context =>
    //    {
    //        var johnNashUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "john.nash");
    //        johnNashUser.ShouldNotBeNull();
    //    });
    //}
}

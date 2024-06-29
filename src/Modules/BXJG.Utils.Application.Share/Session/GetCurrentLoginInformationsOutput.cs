namespace BXJG.Utils.Application.Share.Session
{
    public class GetCurrentLoginInformationsOutput
    {
        public ApplicationInfoDto Application { get; set; }=new ApplicationInfoDto();

        public UserLoginInfoDto User { get; set; }=new UserLoginInfoDto();

        public TenantLoginInfoDto Tenant { get; set; }= new TenantLoginInfoDto();
    }
}

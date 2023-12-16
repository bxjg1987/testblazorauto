namespace ZLJ.Admin.CoreRCL.Auth
{
    public class UserInfo
    {
        public long Id { get; set; }

        public string AccessToken { get; set; }

        //public string Name { get; set; }
        //好像不是很有必要，用到的时候再加吧
       // public int? TenantId { get; set; }
    }
}

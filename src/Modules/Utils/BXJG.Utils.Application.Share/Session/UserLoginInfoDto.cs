using Abp.Application.Services.Dto;


namespace BXJG.Utils.Application.Share.Session
{

    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }
    }
}

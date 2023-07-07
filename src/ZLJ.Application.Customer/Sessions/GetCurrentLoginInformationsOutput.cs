using ZLJ.App.Customer.Sessions;

namespace ZLJ.App.Customer.Sessions
{
    public class GetCurrentLoginInformationsOutput:Common.Sessions.Dto.GetCurrentLoginInformationsOutput
    {
        public CustomerInfo Customer { get; set; }

       
    }
}

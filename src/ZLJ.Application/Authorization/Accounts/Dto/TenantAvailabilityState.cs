namespace ZLJ.Application.Authorization.Accounts.Dto
{
    [Flags]
    public enum TenantAvailabilityState
    {
        Available = 1,
        InActive,
        NotFound
    }
}

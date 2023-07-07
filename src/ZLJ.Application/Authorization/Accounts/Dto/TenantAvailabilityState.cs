namespace ZLJ.App.Admin.Authorization.Accounts.Dto
{
    [Flags]
    public enum TenantAvailabilityState
    {
        Available = 1,
        InActive,
        NotFound
    }
}

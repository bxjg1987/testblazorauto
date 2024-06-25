public class sdfd : IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        return Task.CompletedTask;
        //var policyName = context.Requirements.OfType<PolicyRequirement>().FirstOrDefault()?.PolicyName;
    }
}

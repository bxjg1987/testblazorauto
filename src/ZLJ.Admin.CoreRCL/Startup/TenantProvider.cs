namespace ZLJ.Admin.CoreRCL.Startup
{
    public class TenantProvider
    {
        private readonly PersistentComponentState state;
        PersistingComponentStateSubscription _subscription;
        public TenantProvider(PersistentComponentState persistentComponentState)
        {
            state = persistentComponentState;
            _subscription = persistentComponentState.RegisterOnPersisting(sss);
        }

        private async Task sss() {
            state.PersistAsJson();
        }
    }
}

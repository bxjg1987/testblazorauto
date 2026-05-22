export function onRuntimeConfigLoaded(config) {
    //.net10 bug https://github.com/dotnet/aspnetcore/issues/64009#issuecomment-3534259401
    config.disableNoCacheFetch = true;
    console.log("onRuntimeConfigLoaded22");
}
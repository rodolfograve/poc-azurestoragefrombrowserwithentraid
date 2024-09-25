using AzureStorageWithEntraId;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.AdditionalScopesToConsent.Add("https://storage.azure.com/Microsoft.Storage/storageAccounts/blobServices/containers/blobs/read");
    options.ProviderOptions.AdditionalScopesToConsent.Add("https://storage.azure.com/Microsoft.Storage/storageAccounts/blobServices/containers/blobs/write");
});

await builder.Build().RunAsync();

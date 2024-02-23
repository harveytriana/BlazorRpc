using BlazorRpc.Server.Hubs;
using BlazorRpc.Services;

using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Container Services
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddGrpc();
builder.Services.AddSignalR().AddMessagePackProtocol();
builder.Services.AddResponseCompression(opts => {
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
    app.UseWebAssemblyDebugging();
}
else {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseGrpcWeb();

app.MapRazorPages();
app.MapControllers();
// gRPC
app.MapGrpcService<RFService>().EnableGrpcWeb();
// signalR
app.MapHub<RpcFunctionsHub>("/RpcFunctionsHub");

app.MapFallbackToFile("index.html");

app.Run();

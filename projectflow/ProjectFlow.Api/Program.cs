using ProjectFlow.Api.Auth;
using ProjectFlow.Api.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddProjectFlowSwagger();
builder.Services.AddProjectFlowDatabase(builder.Configuration);
builder.Services.AddProjectFlowAuth(builder.Configuration);

builder.Services.AddProjectFlowApplication();
builder.Services.AddProjectFlowInfrastructure();


WebApplication app = builder.Build();

app.UseProjectFlowSwagger();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();

app.Run();

using ProjectFlow.Api.Auth;
using ProjectFlow.Api.Endpoints;
using ProjectFlow.Api.Extensions;
using ProjectFlow.Application.Projects.Ports;
using ProjectFlow.Application.Projects.Queries.GetProjects;
using ProjectFlow.Infrastructure.Projects;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddProjectFlowSwagger();
builder.Services.AddProjectFlowDatabase(builder.Configuration);
builder.Services.AddProjectFlowAuth(builder.Configuration);

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<GetProjectsHandler>();

builder.Services.AddProjectFlowApplication();
builder.Services.AddProjectFlowInfrastructure();


WebApplication app = builder.Build();

app.UseProjectFlowSwagger();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();

app.MapProjectsEndpoints();


app.Run();

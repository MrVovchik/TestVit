using TestVit.DataAccess;
using TestVit.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// DataAccess
builder.Services.AddSingleton<BlogPostDataAccess>();
builder.Services.AddSingleton<BlogPostTegDataAccess>();
builder.Services.AddSingleton<CommentDataAccess>();
builder.Services.AddSingleton<LikeDataAccess>();
builder.Services.AddSingleton<SessionDataAccess>();
builder.Services.AddSingleton<TagDataAccess>();
builder.Services.AddSingleton<UserDataAccess>();

// Services
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<IStatisticsService, StatisticsService>();
builder.Services.AddSingleton<IPostsLineService, PostsLineService>();
builder.Services.AddSingleton<IBlogPostsService, BlogPostsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using TwilioOpenAppointement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Bind Twilio settings from configuration
builder.Services.Configure<TwilioSettings>(
    builder.Configuration.GetSection("TwilioSettings"));

// Add CORS policy (before Build)
builder.Services.AddCors(options =>
{
    options.AddPolicy("NetlifyCors", policy =>
    {
        policy.WithOrigins(
            "https://your-netlify-site.netlify.app", // replace with your Netlify domain
            "http://localhost:3000" // optional for local dev
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

// Bind to the environment PORT for Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NetlifyCors"); // Must be before UseAuthorization()

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

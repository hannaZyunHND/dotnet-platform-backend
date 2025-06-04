using Amazon.S3;
using Upload_S3_AWS.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new AmazonS3Client(
        config["AWS:AccessKey"],
        config["AWS:SecretKey"],
        Amazon.RegionEndpoint.GetBySystemName(config["AWS:Region"]));
});

builder.Services.AddSingleton<IS3Service, S3Service>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseSwagger();
app.UseSwaggerUI();
    
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(builder => builder
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
app.UseAuthorization();
app.MapControllers();
app.Run();
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using RainfallApi.DTOs;
using RainfallApi.Middleware;
using RainfallApi.Parameters;
using RainfallApi.Services;
using RainfallApi.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<Params>, ParamsValidator>();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(opt =>
    {
        opt.InvalidModelStateResponseFactory = context =>
            {
                var modelStateErrors = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(x => x.ErrorMessage).ToArray()
                    );

                var errorDetailList = modelStateErrors
                    .SelectMany(x => x.Value, (parent, error) => new ErrorDetail
                    {
                        PropertyName = parent.Key,
                        Message = error
                    }).ToList();

                var errorResponse = new Error
                {
                    Message = "One or more validation/s failed",
                    Detail = errorDetailList
                };

                return new BadRequestObjectResult(errorResponse);
            };
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<RainfallServiceHttpClient>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.UseMiddleware<ErrorResponseMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

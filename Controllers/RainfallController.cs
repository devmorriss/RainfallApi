using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RainfallApi.DTOs;
using RainfallApi.Parameters;
using RainfallApi.Services;

namespace RainfallApi.Controllers;

[ApiController]
[Route("rainfall")]
public class RainfallController : ControllerBase
{
    private readonly RainfallServiceHttpClient _rainfallServiceHttpClient;
    private readonly IMapper _mapper;

    public RainfallController(RainfallServiceHttpClient rainfallServiceHttpClient, IMapper mapper)
    {
        _rainfallServiceHttpClient = rainfallServiceHttpClient;
        _mapper = mapper;
    }

    [HttpGet("id/{stationId}/readings")]
    public async Task<IActionResult> GetRainFall(int stationId, [FromQuery] Params param)
    {
        var rainfallData = await _rainfallServiceHttpClient.GetRainfallApiReadings(stationId, param.Count);

        if (rainfallData is null || !rainfallData.Items.Any())
        {
            var response = new Error
            {
                Message = $"No readings found for the specified stationd ID: {stationId}",
                Detail = new List<ErrorDetail>
                {
                    new ErrorDetail
                    {
                        PropertyName = nameof(rainfallData.Items),
                        Message = "No data found."
                    }
                }
            };
            return NotFound(response);
        }


        return Ok(new RainfallReadingResponse
        {
            RainfallReadings = _mapper.Map<List<RainfallReading>>(rainfallData.Items)
        });
    }
}


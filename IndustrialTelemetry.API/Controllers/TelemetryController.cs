using IndustrialTelemetry.Application.DTOs;
using IndustrialTelemetry.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IndustrialTelemetry.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TelemetryController : ControllerBase
{
    private readonly ITelemetryService _telemetryService;

    public TelemetryController(ITelemetryService telemetryService)
    {
        _telemetryService = telemetryService;
    }

    /// <summary>
    /// Принимает телеметрию от датчиков с защитой от дублирования (идемпотентность)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TelemetryResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateTelemetry(
        [FromBody] CreateTelemetryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _telemetryService.CreateTelemetryAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetTelemetryByEquipmentId), new { equipmentId = request.EquipmentId }, result);
    }

    /// <summary>
    /// Получает историю телеметрии для конкретного оборудования
    /// </summary>
    [HttpGet("equipment/{equipmentId}")]
    [ProducesResponseType(typeof(List<TelemetryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTelemetryByEquipmentId(
        Guid equipmentId,
        CancellationToken cancellationToken)
    {
        var result = await _telemetryService.GetTelemetryByEquipmentIdAsync(equipmentId, cancellationToken);
        return Ok(result);
    }
}
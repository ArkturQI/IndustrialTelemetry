using IndustrialTelemetry.Application.DTOs;
using IndustrialTelemetry.Application.Interfaces;
using IndustrialTelemetry.Domain.Models;
using IndustrialTelemetry.Infrastructure;
using IndustrialTelemetry.Infrastructure.DATA;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IndustrialTelemetry.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquipmentController : ControllerBase
{
    private readonly IEquipmentService _equipmentService;
    private readonly TelemetryDbContext _dbContext;

    public EquipmentController(IEquipmentService equipmentService, TelemetryDbContext dbContext)
    {
        _equipmentService = equipmentService;
        _dbContext = dbContext;
    }

    /// <summary>
    /// Создает новое оборудование
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(EquipmentResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateEquipment(
        [FromBody] CreateEquipmentRequest request,
        CancellationToken cancellationToken)
    {
        var equipment = new Equipment(request.Name);

        _dbContext.Equipments.Add(equipment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = new EquipmentResponse
        {
            Id = equipment.Id,
            Name = equipment.Name,
            Status = equipment.Status,
            CreatedAt = equipment.CreatedAt
        };

        return CreatedAtAction(nameof(GetEquipment), new { id = equipment.Id }, response);
    }

    /// <summary>
    /// Получает информацию об оборудовании
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EquipmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEquipment(Guid id, CancellationToken cancellationToken)
    {
        var result = await _equipmentService.GetEquipmentByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Обновляет статус оборудования с защитой от гонок данных (Optimistic Concurrency)
    /// </summary>
    [HttpPut("{id}/status")]
    [ProducesResponseType(typeof(EquipmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateEquipmentStatus(
        Guid id,
        [FromBody] UpdateEquipmentStatusRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _equipmentService.UpdateEquipmentStatusAsync(id, request, cancellationToken);
        return Ok(result);
    }
}
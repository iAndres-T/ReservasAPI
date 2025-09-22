using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ReservasAPI.Models;

public partial class Reserva
{
    [ReadOnly(true)]
    public int IdReserva { get; set; }

    public int IdCliente { get; set; }

    public DateTime FechaReserva { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public int? Cantidad { get; set; }

    public string Estado { get; set; } = null!;

    public string? Observaciones { get; set; }

    public DateTime? FechaCreacion { get; set; }
}

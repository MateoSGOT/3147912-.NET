using Agenda.Models.Agenda;

namespace Agenda.Services
{
    public record CrearCitaRequest(
        int VehiculoId,
        int TipoServicioId,
        int? MecanicoId,
        int? BahiaId,
        DateTime InicioLocal,
        int? DuracionMinutosOverride,
        PrioridadCita Prioridad,
        string? MotivoIngreso,
        string? NotasInternas,
        decimal? ValorEstimado,
        string Usuario
    );

    public record EditarCitaRequest(
        int CitaId,
        int VehiculoId,
        int TipoServicioId,
        int? MecanicoId,
        int? BahiaId,
        DateTime InicioLocal,
        int? DuracionMinutosOverride,
        PrioridadCita Prioridad,
        string? MotivoIngreso,
        string? NotasInternas,
        decimal? ValorEstimado,
        byte[] RowVersion,
        string Usuario
    );

    public record ReprogramarCitaRequest(
        int CitaId,
        DateTime NuevoInicioLocal,
        int? NuevoMecanicoId,
        int? NuevaBahiaId,
        int? DuracionMinutosOverride,
        string Motivo,
        byte[] RowVersion,
        string Usuario
    );

    public record CambiarEstadoRequest(
        int CitaId,
        EstadoCita NuevoEstado,
        string? Nota,
        byte[] RowVersion,
        string Usuario
    );

    public record EliminarLogicoRequest(
        int CitaId,
        string Motivo,
        byte[] RowVersion,
        string Usuario
    );

    public record SlotDisponible(
        DateTime InicioLocal,
        DateTime FinLocal
    );
}

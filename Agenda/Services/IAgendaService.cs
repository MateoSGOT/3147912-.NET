    using Agenda.Models.Agenda;

    namespace Agenda.Services
    {
        public interface IAgendaService
        {
            Task<ServiceResult<int>> CrearCitaAsync(CrearCitaRequest req);
            Task<ServiceResult> EditarCitaAsync(EditarCitaRequest req);
            Task<ServiceResult> ReprogramarAsync(ReprogramarCitaRequest req);
            Task<ServiceResult> CambiarEstadoAsync(CambiarEstadoRequest req);
            Task<ServiceResult> EliminarLogicoAsync(EliminarLogicoRequest req);

            Task<ServiceResult<List<SlotDisponible>>> ObtenerSlotsDisponiblesAsync(
                DateTime diaLocal,
                int? mecanicoId,
                int? bahiaId,
                int tipoServicioId,
                int? duracionOverrideMin
            );
        }
    }

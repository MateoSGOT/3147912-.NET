namespace Agenda.ViewModels.Agenda
{
    public class CitaCreateVm : CitaFormVm
    {
        // Para ayudar a UI: preselecciones o sugerencias
        public int? SuggestedMecanicoId { get; set; }
        public int? SuggestedBahiaId { get; set; }

        // Para que UI muestre disponibilidad sugerida
        public bool MostrarDisponibilidad { get; set; } = true;
    }
}

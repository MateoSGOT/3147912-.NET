using System.ComponentModel.DataAnnotations;

namespace Agenda.ViewModels.Agenda
{
    public class DisponibilidadVm
    {
        [Display(Name = "Día")]
        [Required]
        public string DiaLocal { get; set; } = string.Empty; // yyyy-MM-dd

        [Display(Name = "Mecánico")]
        public int? MecanicoId { get; set; }

        [Display(Name = "Bahía")]
        public int? BahiaId { get; set; }

        [Display(Name = "Tipo de servicio")]
        [Range(1, int.MaxValue)]
        public int TipoServicioId { get; set; }

        [Display(Name = "Duración (min)")]
        [Range(15, 600)]
        public int? DuracionMinutosOverride { get; set; }

        public List<SelectOptionVm> Mecanicos { get; set; } = new();
        public List<SelectOptionVm> Bahias { get; set; } = new();
        public List<SelectOptionVm> TiposServicio { get; set; } = new();

        // Resultado
        public List<SlotVm> Slots { get; set; } = new();

        public string? DomainError { get; set; }
        public string? DomainErrorCode { get; set; }
    }

    public class SlotVm
    {
        public string InicioLocal { get; set; } = string.Empty; // para UI
        public string FinLocal { get; set; } = string.Empty;
    }
}

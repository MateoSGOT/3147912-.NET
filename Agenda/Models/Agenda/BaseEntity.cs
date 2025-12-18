using System.ComponentModel.DataAnnotations;

namespace Agenda.Models.Agenda
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        // Auditoría
        [MaxLength(100)]
        public string? CreadoPor { get; set; }
        public DateTime CreadoEnUtc { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? ActualizadoPor { get; set; }
        public DateTime? ActualizadoEnUtc { get; set; }

        // Soft delete
        public bool Eliminado { get; set; } = false;
        public DateTime? EliminadoEnUtc { get; set; }

        [MaxLength(100)]
        public string? EliminadoPor { get; set; }

        // Concurrencia (anti “dos editan al tiempo”)
        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}

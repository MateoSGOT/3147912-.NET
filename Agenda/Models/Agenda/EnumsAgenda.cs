namespace Agenda.Models.Agenda
{
    public enum EstadoCita
    {
        Reservada = 1,
        Confirmada = 2,
        EnProceso = 3,
        Finalizada = 4,
        Cancelada = 5,
        NoAsistio = 6
    }

    public enum PrioridadCita
    {
        Normal = 1,
        Alta = 2,
        Urgente = 3
    }
}

namespace Agenda.Data
{
    public class AgendaSettings
    {
        public string ZonaHoraria { get; set; } = "SA Pacific Standard Time";
        public int MinutosBufferEntreCitas { get; set; } = 10;
        public bool PermitirSobreAgendar { get; set; } = false;

        // strings para leer fácil desde json (luego los parseas a TimeSpan si quieres)
        public string HoraInicioDefecto { get; set; } = "08:00";
        public string HoraFinDefecto { get; set; } = "18:00";
    }
}

using Agenda.ViewModels.Agenda;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Agenda.Controllers
{
    internal static class ControllerHelpers
    {
        public static string CurrentUserName(this Controller c)
            => c.User?.Identity?.IsAuthenticated == true
                ? (c.User.Identity!.Name ?? "USER")
                : "SYSTEM";

        public static DateTime ParseDateTimeLocal(string value)
        {
            // Para inputs datetime-local: yyyy-MM-ddTHH:mm
            // Si llega con segundos también lo soporta
            if (DateTime.TryParseExact(value,
                    new[] { AgendaConstants.DateTimeLocalFormat, "yyyy-MM-ddTHH:mm:ss" },
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var dt))
            {
                return dt;
            }

            // fallback robusto
            return DateTime.Parse(value, CultureInfo.InvariantCulture);
        }

        public static DateTime ParseDateLocal(string value)
        {
            if (DateTime.TryParseExact(value,
                    new[] { AgendaConstants.DateFormat, "yyyy-MM-dd" },
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var dt))
            {
                return dt.Date;
            }
            return DateTime.Parse(value, CultureInfo.InvariantCulture).Date;
        }
    }
}

namespace Agenda.ViewModels.Agenda
{
    public static class RowVersionHelper
    {
        public static string ToBase64(byte[]? rowVersion)
            => rowVersion is null ? "" : Convert.ToBase64String(rowVersion);

        public static byte[] FromBase64(string base64)
            => string.IsNullOrWhiteSpace(base64) ? Array.Empty<byte>() : Convert.FromBase64String(base64);
    }
}

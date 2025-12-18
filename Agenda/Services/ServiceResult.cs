namespace Agenda.Services
{
    public class ServiceResult<T>
    {
        public bool Ok { get; private set; }
        public string? Error { get; private set; }
        public string? Code { get; private set; }
        public T? Data { get; private set; }

        public static ServiceResult<T> Success(T data) => new() { Ok = true, Data = data };
        public static ServiceResult<T> Fail(string code, string error) => new() { Ok = false, Code = code, Error = error };
    }

    public class ServiceResult
    {
        public bool Ok { get; private set; }
        public string? Error { get; private set; }
        public string? Code { get; private set; }

        public static ServiceResult Success() => new() { Ok = true };
        public static ServiceResult Fail(string code, string error) => new() { Ok = false, Code = code, Error = error };
    }
}

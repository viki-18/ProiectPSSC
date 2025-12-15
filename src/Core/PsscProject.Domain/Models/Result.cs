namespace PsscProject.Domain.Models
{
    // Clasa generică ce poate conține fie o valoare de succes (T), fie o eroare
    public class Result<T>
    {
        // Proprietăți (read-only)
        public T? Value { get; }
        public string? Error { get; }

        // Proprietăți ajutătoare pentru a verifica starea
        public bool IsSuccess => Error == null;
        public bool IsFailure => !IsSuccess;

        // Constructor privat (pentru a forța folosirea metodelor statice Success/Failure)
        private Result(T? value, string? error)
        {
            Value = value;
            Error = error;
        }

        // Metoda statică pentru Succes (returnează un rezultat valid)
        public static Result<T> Success(T value)
        {
            return new Result<T>(value, null);
        }

        // Metoda statică pentru Eșec (returnează doar mesajul de eroare)
        public static Result<T> Failure(string error)
        {
            return new Result<T>(default, error);
        }
    }
}
using System.Text.Json.Serialization;

namespace NLayerApp.Core.DTOs.ResponseDTOs
{
    public class CustomResponseDto<T>
    {
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        [JsonIgnore]    // statusCode yi frontend tarafına göndermeye gerek yok zaten elde ediyor, o yüzden sen bunu json a dönüştürürken ignore et diyoruz.
        public int StatusCode { get; set; }

        // Bu static fonksiyonları ayrı bir class içinde değil de direkt olarak ilgili dönülecek nesnenin classında yazdık. Buna STATIC FACTORY METHOD DESIGN PATTERN denilir.
        public static CustomResponseDto<T> Success(int statusCode, T data) => new() { StatusCode = statusCode, Data = data };
        public static CustomResponseDto<T> Success(int statusCode) => new() { StatusCode = statusCode };
        public static CustomResponseDto<T> Fail(int statusCode, List<string> errors) => new() { StatusCode = statusCode, Errors = errors };
        public static CustomResponseDto<T> Fail(int statusCode, string error) => new() { StatusCode = statusCode, Errors = new List<string> { error } };
        public static CustomResponseDto<T> Fail(int statusCode) => new() { StatusCode = statusCode };

    }
}

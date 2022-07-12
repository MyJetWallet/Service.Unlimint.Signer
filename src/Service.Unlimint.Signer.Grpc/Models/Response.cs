using System.Runtime.Serialization;

namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class Response<T>
    {
        [DataMember(Order = 1)] public T Data { get; set; }
        [DataMember(Order = 2)] public string ErrorMessage { get; set; }
        [DataMember(Order = 3)] public bool IsSuccess { get; set; }

        [DataMember(Order = 4)] public int StatusCode { get; set; }

        public static Response<T> Success(T data)
        {
            return new Response<T>()
            {
                Data = data,
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public static Response<T> Error(string errorMessage, int statusCode = 500)
        {
            return new Response<T>()
            {
                ErrorMessage = errorMessage,
                IsSuccess = false,
                StatusCode = statusCode,
            };
        }
    }
}
using System;
using System.Text;
using System.Threading.Tasks;
using DotNetCoreDecorators;
using Service.Circle.Signer.Utils;
using Service.Unlimint.Signer.Domain.Models;
using Service.Unlimint.Signer.Grpc;
using Service.Unlimint.Signer.Grpc.Models;

namespace Service.Unlimint.Signer.Services
{
    public class CircleEncryptionKeyService : ICircleEncryptionKeyService
    {
        private readonly RsaKeyStorage _rsaKeyStorage;

        public CircleEncryptionKeyService(RsaKeyStorage rsaKeyStorage)
        {
            this._rsaKeyStorage = rsaKeyStorage;
        }

        public async Task<Response<CircleEncryptionKey>> GetCircleEncryptionKey(GetCircleEncryptionKeyRequest request)
        {
            var key = _rsaKeyStorage.GetKey("key1");

            return key != null
                ? Response<CircleEncryptionKey>.Success(new CircleEncryptionKey
                {
                    BrokerId = request.BrokerId,
                    EncryptionKey = key.PubKey,
                    Id = "key1",
                    UpdateDate = key.UpdateDate,
                })
                : Response<CircleEncryptionKey>.Error("Encryption key is not found");
        }

        public Task<Response<string>> CircleEncryptionData(CircleEncryptDataRequest request)
        {
            var result = Encoding.UTF8.GetString(PgpEncryptor.Encrypt(Encoding.UTF8.GetBytes(request.Data),
                Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(Convert.FromBase64String(request.EncryptionKey))
                    .Trim())));

            return Response<string>.Success(Convert.ToBase64String(Encoding.UTF8.GetBytes(result))).AsTask();
        }
    }
}
using System;
using System.Threading.Tasks;
using ProtoBuf.Grpc.Client;
using Service.Unlimint.Signer.Client;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Task.Delay(10);
            GrpcClientFactory.AllowUnencryptedHttp2 = true;

            Console.Write("Press enter to start");
            Console.ReadLine();

            //var url = "http://localhost:80";
            //var url = "http://circle-signer.spot-services.svc.cluster.local:80";
            var url = "http://localhost:5001";
            var fac = new UnlimintPaymentsClientFactory(url);// "http://circle-signer.spot-services.svc.cluster.local:80");//"http://localhost:5001");
            var client = fac.GetUnlimintPaymentsService();


            //var paymentFac = new CirclePaymentsClientFactory(url);

            //var client = paymentFac.GetCirclePaymentsService();
            //client.AddCirclePayment(new Service.Circle.Signer.Grpc.Models.AddPaymentRequest
            //{
            //    Amount = 100,
            //    BrokerId = "jetwallet",
            //    ClientId = ,
            //    Currency =,
            //    Description =,
            //    EncryptedData = ,
            //    IdempotencyKey =,
            //    IpAddress =,
            //    KeyId =,
            //    SessionId =,
            //    SourceId =,
            //    SourceType =,
            //    Verification =,
            //});

            //var acc = await client.AddCircleUsSwiftBankAccount(new()
            //{
            //    BrokerId = "jetwallet",
            //    //AccountNumber = "123456789",
            //    //BankAddressBankName = "",
            //    //BankAddressCity = ,
            //    BankAddress = new MyJetWallet.Circle.Models.WireTransfers.BankAddress
            //    {
            //        Country = "US",
            //        Line2 = "",
            //        //BankName 
            //    },
            //    BillingDetails = new MyJetWallet.Circle.Models.WireTransfers.BillingDetails()
            //    {
            //        City = "Boston",
            //        Country = "US",
            //        District = "MA",
            //        Line1 = "1 Main Street",
            //        Name = "John Smith",
            //        PostalCode = "02201",
            //        Line2 = ""
            //    },
            //    IdempotencyKey = "6ae62bf2-bd71-49ce-a599-165ffcc33680",
            //    //BankAddressDistrict= ,
            //    //BankAddressLine1 = ,
            //    //BankAddressLine2 = ,
            //    //RoutingNumber = "021000021",
            //});

            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
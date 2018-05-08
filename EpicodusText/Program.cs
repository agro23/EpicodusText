using System;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace EpicodusText
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    //Replace the placeholders in double curly brackets in the code with actual values. For instance,
        //    //1 Making a connection with the server where the API is located.
        //    var client = new RestClient("https://api.twilio.com/2010-04-01");

        //    //2 Creating our request, adding the physical path to the specific API controller, and denoting the HTTP method.
        //    var request = new RestRequest("Accounts/AC05672c2102100d67fdbfab0fb8ed293e/Messages", Method.POST);

        //    //3 Adding the necessary parameters to our request.
        //    request.AddParameter("To", "+14153171569");
        //    request.AddParameter("From", "+16504886979");
        //    request.AddParameter("Body", "Hello world!");

        //    //4 Providing the client the appropriate credentials necessary to authenticate the request.
        //    client.Authenticator = new HttpBasicAuthenticator("AC05672c2102100d67fdbfab0fb8ed293e", "bc4530800c9e50944ffcfa876e3b3f26");

        //    //5 Executing the request to the client. Note that the second argument for ExecuteAsync() needs callback
        //    client.ExecuteAsync(request, response =>
        //    {
        //        Console.WriteLine(response);
        //    });
        //    Console.ReadLine();
        //}

        static void Main(string[] args)
        {
            var client = new RestClient("https://api.twilio.com/2010-04-01");
            //1
            var request = new RestRequest("Accounts/AC05672c2102100d67fdbfab0fb8ed293e/Messages.json", Method.GET);
            client.Authenticator = new HttpBasicAuthenticator("AC05672c2102100d67fdbfab0fb8ed293e", "bc4530800c9e50944ffcfa876e3b3f26");
            //2
            var response = new RestResponse();
            //3a
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            //4
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            List<Message> messageList = JsonConvert.DeserializeObject<List<Message>>(jsonResponse["messages"].ToString());
            foreach (Message message in messageList)
            {
                Console.WriteLine("To: {0}", message.To);
                Console.WriteLine("From: {0}", message.From);
                Console.WriteLine("Body: {0}", message.Body);
                Console.WriteLine("Status: {0}", message.Status);
            }
            Console.ReadLine();
        }

        //3b
        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }


    }
}
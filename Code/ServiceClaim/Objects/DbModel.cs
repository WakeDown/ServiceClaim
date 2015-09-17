using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace ServiceClaim.Objects
{
    public class DbModel
    {
        public static string OdataServiceUri = ConfigurationManager.AppSettings["OdataServiceUri"];

        public static HttpClient GetApiClientAsync(string contentType = "application/json")
        {
            X509Certificate2 cert = null;
            X509Store store = null;

            try
            {
                store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
                // You can check certificate here ... 
                // and populate cert variable.. 
            }
            finally
            {
                if (store != null) store.Close();
            }
            var baseUri = new Uri(OdataServiceUri);
            var clientHandler = new WebRequestHandler();
            CredentialCache cc = new CredentialCache();
            cc.Add(baseUri, "NTLM", CredentialCache.DefaultNetworkCredentials);
            clientHandler.Credentials = cc;
            if (cert != null) clientHandler.ClientCertificates.Add(cert);
            
            var client = new HttpClient(clientHandler) { BaseAddress = baseUri };
            return client;
        }

        //public static HttpClient GetApiClient(string url, string contentType = "application/json")
        //{
        //    var uri = new Uri(url);
        //    return GetApiClient(url, contentType);
        //}

        //public static HttpClient GetApiClient(string contentType = "application/json")
        //{
        //    X509Certificate2 cert = null;
        //    X509Store store = null;

        //    try
        //    {
        //        store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
        //        store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
        //        // You can check certificate here ... 
        //        // and populate cert variable.. 
        //    }
        //    finally
        //    {
        //        if (store != null) store.Close();
        //    }
        //    var baseUri = new Uri(OdataServiceUri);
        //    var clientHandler = new WebRequestHandler();
        //    CredentialCache cc = new CredentialCache();
        //    cc.Add(baseUri, "NTLM", CredentialCache.DefaultNetworkCredentials);
        //    clientHandler.Credentials = cc;
        //    if (cert != null) clientHandler.ClientCertificates.Add(cert);

        //    var client = new HttpClient(clientHandler) { BaseAddress = baseUri };
        //    return client;
        //}

        protected static string GetJson(Uri uri)
        {
            string result = String.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            CredentialCache cc = new CredentialCache();
            cc.Add(uri, "NTLM", CredentialCache.DefaultNetworkCredentials);

            request.Credentials = cc;
            request.ContentType = "application/json";

            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    result = reader.ReadToEnd();
                }


            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                String errorText = String.Empty;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    errorText = reader.ReadToEnd();
                }

                throw new Exception(errorText);
            }

            return result;
        }

        protected static string RequestApi(Uri uri, string json, RequestMethod method/*, out HttpStatusCode stCode*/)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            CredentialCache cc = new CredentialCache();
            cc.Add(uri, "NTLM", CredentialCache.DefaultNetworkCredentials);
            request.Credentials = cc;
            request.ContentType = "text/json";

            switch (method)
            {
                case RequestMethod.Get:
                    request.Method = "GET";
                    break;
                case RequestMethod.Head:
                    request.Method = "HEAD";
                    break;
                case RequestMethod.Post:
                    request.Method = "POST";
                    break;
                case RequestMethod.Put:
                    request.Method = "PUT";
                    break;
                case RequestMethod.Delete:
                    request.Method = "DELETE";
                    break;
            }

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            string result = String.Empty;
            var response = (HttpWebResponse)request.GetResponse();
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                result = reader.ReadToEnd();
            }
            //stCode = response.StatusCode;
            return result;
            //return response.StatusCode == HttpStatusCode.Created;
        }

        protected static bool PostJson(Uri uri, string json, out ResponseMessage responseMessage)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            CredentialCache cc = new CredentialCache();
            cc.Add(uri, "NTLM", CredentialCache.DefaultNetworkCredentials);
            request.Credentials = cc;
            //request.Headers.Add("Authorization", AuthorizationHeaderValue);
            request.ContentType = "text/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var response = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                string responseContent = streamReader.ReadToEnd();
                responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(responseContent);
            }

            return response.StatusCode == HttpStatusCode.Created;
        }

        public static ResponseMessage DeserializeResponse(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                string responseContent = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<ResponseMessage>(responseContent);
            }

            //string responseContent = re;
            //return JsonConvert.DeserializeObject<ResponseMessage>(responseContent);
        }
    }
}
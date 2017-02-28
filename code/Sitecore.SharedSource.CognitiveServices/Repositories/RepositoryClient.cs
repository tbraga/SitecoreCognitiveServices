﻿using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Sitecore.SharedSource.CognitiveServices.Models;

namespace Sitecore.SharedSource.CognitiveServices.Repositories
{
    public class RepositoryClient : IRepositoryClient
    {
        public async Task<string> SendPostMultiPartAsync(string apiKey, string url, string data)
        {
            return await SendPostAsync(apiKey, url, data, "multipart/form-data");
        }

        public async Task<string> SendEncodedFormPostAsync(string apiKey, string url, string data)
        {
            return await SendPostAsync(apiKey, url, data, "application/x-www-form-urlencoded");
        }

        public async Task<string> SendTextPostAsync(string apiKey, string url, string data)
        {
            return await SendPostAsync(apiKey, url, data, "text/plain");
        }

        public async Task<string> SendJsonPostAsync(string apiKey, string url, string data)
        {
            return await SendPostAsync(apiKey, url, data, "application/json");
        }

        protected async Task<string> SendPostAsync(string apiKey, string url, string data, string contentType)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("url");
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("ApiKey");
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentException("data");

            byte[] reqData = Encoding.UTF8.GetBytes(data);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Ocp-Apim-Subscription-Key", apiKey);
            request.ContentType = contentType;
            request.Accept = contentType;
            request.ContentLength = (long)reqData.Length;
            request.Method = "POST";

            Stream requestStreamAsync = await request.GetRequestStreamAsync();
            requestStreamAsync.Write(reqData, 0, reqData.Length);
            requestStreamAsync.Close();

            WebResponse responseAsync = await request.GetResponseAsync();
            StreamReader streamReader = new StreamReader(responseAsync.GetResponseStream());
            string end = streamReader.ReadToEnd();
            streamReader.Close();
            responseAsync.Close();

            return end;
        }

        public async Task<string> SendOperationPostAsync(string apiKey, string url, string data)
        {

            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("url");
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("ApiKey");
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentException("data");

            byte[] reqData = Encoding.UTF8.GetBytes(data);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Ocp-Apim-Subscription-Key", apiKey);
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.ContentLength = (long)reqData.Length;
            request.Method = "POST";

            Stream requestStreamAsync = await request.GetRequestStreamAsync();
            requestStreamAsync.Write(reqData, 0, reqData.Length);
            requestStreamAsync.Close();

            HttpWebResponse responseAsync = (HttpWebResponse)await request.GetResponseAsync();
            var opLocation = responseAsync.GetResponseHeader("operation-location");
            responseAsync.Close();

            return opLocation;
        }

        public TokenResponse SendTokenRequest(string apiKey, string clientId)
        {
            byte[] reqData = Encoding.UTF8.GetBytes($"resource=https%3A%2F%2Fapi.contentmoderator.cognitive.microsoft.com%2Freview&client_id={clientId}&client_secret={apiKey}&grant_type=client_credentials");

            WebRequest request = WebRequest.Create("https://login.microsoftonline.com/contentmoderatorprod.onmicrosoft.com/oauth2/token");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = (long)reqData.Length;

            Stream requestStreamAsync = request.GetRequestStream();
            requestStreamAsync.Write(reqData, 0, reqData.Length);
            requestStreamAsync.Close();

            WebResponse responseAsync = request.GetResponse();
            StreamReader streamReader = new StreamReader(responseAsync.GetResponseStream());
            string end = streamReader.ReadToEnd();
            streamReader.Close();
            responseAsync.Close();

            TokenResponse t = new JavaScriptSerializer().Deserialize<TokenResponse>(end);

            return t;
        }

        public string SendTokenPost(string apiKey, string url, string token)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("url");
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("token");

            byte[] reqData = Encoding.UTF8.GetBytes("");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Ocp-Apim-Subscription-Key", apiKey);
            request.Headers.Add("authorization", $"Bearer {token}");
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Method = "POST";
            request.ContentLength = (long)reqData.Length;

            Stream requestStreamAsync = request.GetRequestStream();
            requestStreamAsync.Write(reqData, 0, reqData.Length);
            requestStreamAsync.Close();

            WebResponse responseAsync = request.GetResponse();
            StreamReader streamReader = new StreamReader(responseAsync.GetResponseStream());
            string end = streamReader.ReadToEnd();
            streamReader.Close();
            responseAsync.Close();

            return end;
        }
    }
}
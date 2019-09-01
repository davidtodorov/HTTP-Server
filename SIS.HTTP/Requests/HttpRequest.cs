using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Excceptions;
using SIS.HTTP.Headers;
using SIS.HTTP.Headers.Contracts;
using SIS.HTTP.Requests.Contracts;

namespace SIS.HTTP.Requests
{
    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            CoreValidator.ThrowIfNullOrEmpty(requestString, nameof(requestString));
            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();

            this.ParseRequest(requestString);
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; private set; }

        public Dictionary<string, object> QueryData { get; private set; }

        public IHttpHeaderCollection Headers { get; private set; }

        public HttpRequestMethod RequestMethod { get; private set; }
         
        private void ParseRequest(string requestString)
        {
            var splitRequestContent = requestString
                .Split(new[] { GlobalConstants.HttpNewLine }, StringSplitOptions.None);

            var requestLineParams = splitRequestContent[0].Trim()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValidRequestLine(requestLineParams))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLineParams);
            this.ParseRequestUrl(requestLineParams);
            this.ParseRequestPath(requestLineParams);

            this.ParseRequestHeaders(splitRequestContent.Skip(1).ToArray());
            //this.ParseCookies();

            this.ParseRequestParameters(splitRequestContent[splitRequestContent.Length - 1]);
        }

        private bool IsValidRequestLine(string[] requestLineParams)
        {
            bool isValid = requestLineParams.Length == 3 && requestLineParams[2] == GlobalConstants.HTTpOneProtocolFragment;

            return isValid;
        }

        private void ParseRequestMethod(string[] requestLineParams)
        {
            HttpRequestMethod method;
            var result = HttpRequestMethod.TryParse(requestLineParams[0], true, out method);

            if (!result)
            {
                throw new BadRequestException(String.Format(GlobalConstants.UnsupportedHttpRequestMethodException,
                    requestLineParams[0]));
            }

            this.RequestMethod = method;

        }

        private void ParseRequestUrl(string[] requestLineParams)
        {
            this.Url = requestLineParams[1];
        }

        private void ParseRequestPath(string[] requestLineParams)
        {
            this.Path = requestLineParams[1].Split('?')[0];
        }

        private void ParseRequestHeaders(string[] plainHeaders)
        {
            plainHeaders.Select(ph => ph
                .Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                .ToList()
                .ForEach(phKvp => this.Headers.AddHeader(new HttpHeader(phKvp[0], phKvp[1])));
        }

        private void ParseCookies()
        {

        }

        private void ParseRequestParameters(string requestBody)
        {
            this.ParseRequestQueryParameter();
            this.ParseRequestFormDataParameters(requestBody);
        }

        private void ParseRequestQueryParameter()
        {
            //name="pesho"&id=1
            this.Url.Split(new char[] { '?' }, StringSplitOptions.RemoveEmptyEntries)[1]
                .Split('&')
                .Select(queryParameter => queryParameter.Split('='))
                .ToList()
                .ForEach(queryKvp =>
                {
                    AddQueryParametersToDictionary(queryKvp, this.QueryData);
                });
        }

        private void ParseRequestFormDataParameters(string requestBody)
        {
            //TODO: Parse Multiple Parameters by name
            //cars=Opel&cars=VW  it should be cars collection
            requestBody
                .Split('&')
                .Select(queryParameter => queryParameter.Split('='))
                .ToList()
                .ForEach(queryKvp =>
                {
                    AddQueryParametersToDictionary(queryKvp, this.FormData);
                });
        }

        private void AddQueryParametersToDictionary(string[] queryKvp, Dictionary<string, object> dictWithParams)
        {
            var paramKey = queryKvp[0];
            var paramValue = queryKvp[1];
            if (dictWithParams.ContainsKey(paramKey))
            {
                var formDataValue = dictWithParams[paramKey];
                var paramList = new List<object> { formDataValue, paramValue };
                dictWithParams[paramKey] = paramList;
            }
            else
            {
                dictWithParams.Add(queryKvp[1], queryKvp[2]);
            }
        }

        private bool IsValidRequestQueryString(string queryString, string[] queryParameters)
        {
            return false;
        }
    }
}

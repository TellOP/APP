// <copyright file="OAuth2API.cs" company="University of Murcia">
// Copyright © 2016 University of Murcia
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// <author>Mattia Zago</author>
// <author>Alessandro Menti</author>

namespace TellOP.API
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Xamarin.Auth;

    /// <summary>
    /// Base class for APIs accessed on the TellOP server via OAuth 2.0.
    /// </summary>
    public class OAuth2API
    {
        /// <summary>
        /// The URI for the API call.
        /// </summary>
        private Uri apiURI;

        /// <summary>
        /// The HTTP method the API call should use.
        /// </summary>
        private HttpMethod apiMethod;

        /// <summary>
        /// The OAuth 2.0 account to use to authenticate the request.
        /// </summary>
        private Account oauthAccount;

        /// <summary>
        /// The POST body for the API call.
        /// </summary>
        private string postBody;

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2API"/> class.
        /// </summary>
        /// <param name="baseUri">The base URI for the API call.</param>
        /// <param name="method">The HTTP method the API call should
        /// use.</param>
        /// <param name="account">The OAuth 2.0 account to use for
        /// authenticating API calls.</param>
        /// <exception cref="ArgumentNullException">One of the arguments
        /// (except postBody) is <c>null</c>.</exception>
        public OAuth2API(
            Uri baseUri,
            HttpMethod method,
            Account account)
            : this(baseUri, method, account, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2API"/> class.
        /// </summary>
        /// <param name="baseUri">The base URI for the API call.</param>
        /// <param name="method">The HTTP method the API call should
        /// use.</param>
        /// <param name="account">The OAuth 2.0 account to use for
        /// authenticating API calls.</param>
        /// <param name="postBody">The body to be submitted in POST
        /// requests.</param>
        /// <exception cref="ArgumentNullException">One of the arguments
        /// (except postBody) is <c>null</c>.</exception>
        public OAuth2API(
            Uri baseUri,
            HttpMethod method,
            Account account,
            string postBody)
        {
            this.EndpointUri = baseUri;
            Tools.Logger.Log(this, "EndPointURI Called: " + this.EndpointUri);
            this.EndpointMethod = method;
            this.OAuthAccount = account;
            this.postBody = postBody;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2API"/> class.
        /// </summary>
        /// <param name="baseUri">The base URI for the API call.</param>
        /// <param name="method">The HTTP method the API call should
        /// use.</param>
        /// <param name="account">The OAuth 2.0 account to use for
        /// authenticating API calls.</param>
        /// <exception cref="ArgumentNullException">One of the arguments is
        /// <c>null</c>.</exception>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification="An overload is already provided")]
        public OAuth2API(
            string baseUri,
            HttpMethod method,
            Account account)
            : this(new Uri(baseUri), method, account)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2API"/> class.
        /// </summary>
        /// <param name="baseUri">The base URI for the API call.</param>
        /// <param name="method">The HTTP method the API call should
        /// use.</param>
        /// <param name="account">The OAuth 2.0 account to use for
        /// authenticating API calls.</param>
        /// <param name="postBody">The body to be submitted in POST
        /// requests.</param>
        /// <exception cref="ArgumentNullException">One of the arguments is
        /// <c>null</c>.</exception>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "An overload is already provided")]
        public OAuth2API(
            string baseUri,
            HttpMethod method,
            Account account,
            string postBody)
            : this(new Uri(baseUri), method, account, postBody)
        {
        }

        /// <summary>
        /// Gets or sets the URI for the API endpoint.
        /// </summary>
        /// <exception cref="ArgumentNullException">The API endpoint URI is
        /// <c>null</c>.</exception>
        public Uri EndpointUri
        {
            get
            {
                return this.apiURI;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(
                        "value",
                        "The endpoint URI can not be null");
                }

                this.apiURI = value;
            }
        }

        /// <summary>
        /// Gets or sets the HTTP method to use for the API call.
        /// </summary>
        /// <exception cref="ArgumentNullException">The HTTP method is
        /// <c>null</c>.</exception>
        public HttpMethod EndpointMethod
        {
            get
            {
                return this.apiMethod;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(
                        "value",
                        "The endpoint HTTP method can not be null");
                }

                this.apiMethod = value;
            }
        }

        /// <summary>
        /// Gets or sets the OAuth 2.0 account used to authenticate the
        /// requests.
        /// </summary>
        /// <exception cref="ArgumentNullException">The account is <c>null</c>.
        /// </exception>
        public Account OAuthAccount
        {
            get
            {
                return this.oauthAccount;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(
                        "value",
                        "The account can not be null");
                }

                this.oauthAccount = value;
            }
        }

        /// <summary>
        /// Gets or sets the text sent as the body in a POST API call.
        /// </summary>
        public string PostBody
        {
            get
            {
                return this.postBody;
            }

            set
            {
                this.postBody = value;
            }
        }

        /// <summary>
        /// Call the API endpoint asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task{String}"/> object containing the API
        /// response text.</returns>
        /// <exception cref="UnsuccessfulAPICallException">Thrown if the API
        /// request did not complete successfully.</exception>
        public async Task<string> CallEndpointAsync()
        {
            Uri apiURIWithoutQueryString = new Uri(string.Format("{0}{1}{2}{3}", this.apiURI.Scheme, "://", this.apiURI.Authority, this.apiURI.AbsolutePath));

            Dictionary<string, string> apiURIParameters = new Dictionary<string, string>();
            if (this.apiURI.Query != string.Empty)
            {
                string[] paramPairs = this.apiURI.Query.Split('&');
                for (int i = 0; i < paramPairs.Length; ++i)
                {
                    string[] paramSplit = paramPairs[i].Split('=');

                    // Remove the '?' that separates the query string from the base URI
                    if (i == 0 && paramSplit[0][0] == '?')
                    {
                        paramSplit[0] = paramSplit[0].TrimStart('?');
                    }

                    apiURIParameters.Add(paramSplit[0], (paramSplit.Length == 2) ? paramSplit[1] : null);
                }
            }

            // TODO: check if this is admissible with POST requests.
            // Remember that parameters are passed in the POST body in
            // Xamarin.Auth:
            // <https://github.com/xamarin/Xamarin.Auth/blob/master/src/Xamarin.Auth/Request.cs#L53>
            // <https://github.com/xamarin/Xamarin.Auth/blob/master/src/Xamarin.Auth/Request.cs#L316>
            // even though this means that we can not pass a POST body and a
            // query string in the URL at the same time (!)
            if (this.postBody != null)
            {
                string[] bodyParamPairs = this.postBody.Split('&');
                foreach (string bodyParam in bodyParamPairs)
                {
                    string[] bodyParamSplit = bodyParam.Split('=');
                    apiURIParameters.Add(
                        bodyParamSplit[0],
                        (bodyParamSplit.Length == 2) ? bodyParamSplit[1] : null);
                }
            }

            OAuth2Request apiCall = new OAuth2Request(this.apiMethod.ToString(), apiURIWithoutQueryString, apiURIParameters, this.oauthAccount);

            Response apiResponse = await apiCall.GetResponseAsync()
                .ConfigureAwait(false);

            if (apiResponse != null)
            {
                if (apiResponse.StatusCode == HttpStatusCode.OK)
                {
                    return await apiResponse.GetResponseTextAsync()
                        .ConfigureAwait(false);
                }
                else
                {
                    throw new UnsuccessfulAPICallException(
                        "The API call did not complete successfully (StatusCode: " + apiResponse.StatusCode + ")",
                        null,
                        apiResponse.StatusCode,
                        apiResponse);
                }
            }
            else
            {
                throw new UnsuccessfulAPICallException(
                    "The API call returned a null response");
            }
        }
    }
}

// <copyright file="OAuth2Api.cs" company="University of Murcia">
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

namespace TellOP.Api
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
    public class OAuth2Api
    {
        /// <summary>
        /// The URI for the API call.
        /// </summary>
        private Uri _apiURI;

        /// <summary>
        /// The HTTP method the API call should use.
        /// </summary>
        private HttpMethod _apiMethod;

        /// <summary>
        /// The OAuth 2.0 account to use to authenticate the request.
        /// </summary>
        private Account _oauthAccount;

        /// <summary>
        /// The POST body for the API call.
        /// </summary>
        private string _postBody;

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2Api"/> class.
        /// </summary>
        /// <param name="baseUri">The base URI for the API call.</param>
        /// <param name="method">The HTTP method the API call should use.</param>
        /// <param name="account">The OAuth 2.0 account to use for authenticating API calls.</param>
        /// <exception cref="ArgumentNullException">One of the arguments is <c>null</c>.</exception>
        public OAuth2Api(Uri baseUri, HttpMethod method, Account account)
            : this(baseUri, method, account, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2Api"/> class.
        /// </summary>
        /// <param name="baseUri">The base URI for the API call.</param>
        /// <param name="method">The HTTP method the API call should use.</param>
        /// <param name="account">The OAuth 2.0 account to use for authenticating API calls.</param>
        /// <param name="postBody">The body to be submitted in POST requests.</param>
        /// <exception cref="ArgumentNullException">One of the arguments (except <paramref name="postBody"/>) is
        /// <c>null</c>.</exception>
        public OAuth2Api(Uri baseUri, HttpMethod method, Account account, string postBody)
        {
            this.EndpointUri = baseUri;
            this.EndpointMethod = method;
            this.OAuthAccount = account;
            this._postBody = postBody;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2Api"/> class.
        /// </summary>
        /// <param name="baseUri">The base URI for the API call.</param>
        /// <param name="method">The HTTP method the API call should use.</param>
        /// <param name="account">The OAuth 2.0 account to use for authenticating API calls.</param>
        /// <exception cref="ArgumentNullException">One of the arguments is <c>null</c>.</exception>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification="An overload is already provided")]
        public OAuth2Api(string baseUri, HttpMethod method, Account account)
            : this(new Uri(baseUri), method, account)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2Api"/> class.
        /// </summary>
        /// <param name="baseUri">The base URI for the API call.</param>
        /// <param name="method">The HTTP method the API call should use.</param>
        /// <param name="account">The OAuth 2.0 account to use for authenticating API calls.</param>
        /// <param name="postBody">The body to be submitted in POST requests.</param>
        /// <exception cref="ArgumentNullException">One of the arguments (except <paramref name="postBody"/>) is
        /// <c>null</c>.</exception>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "An overload is already provided")]
        public OAuth2Api(string baseUri, HttpMethod method, Account account, string postBody)
            : this(new Uri(baseUri), method, account, postBody)
        {
        }

        /// <summary>
        /// Gets or sets the URI for the API endpoint.
        /// </summary>
        /// <exception cref="ArgumentNullException">The API endpoint URI is <c>null</c>.</exception>
        public Uri EndpointUri
        {
            get
            {
                return this._apiURI;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this._apiURI = value;
            }
        }

        /// <summary>
        /// Gets or sets the HTTP method to use for the API call.
        /// </summary>
        /// <exception cref="ArgumentNullException">The HTTP method is <c>null</c>.</exception>
        public HttpMethod EndpointMethod
        {
            get
            {
                return this._apiMethod;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this._apiMethod = value;
            }
        }

        /// <summary>
        /// Gets or sets the OAuth 2.0 account used to authenticate the requests.
        /// </summary>
        /// <exception cref="ArgumentNullException">The account is <c>null</c>.</exception>
        public Account OAuthAccount
        {
            get
            {
                return this._oauthAccount;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this._oauthAccount = value;
            }
        }

        /// <summary>
        /// Gets or sets the text sent as the body in a POST API call.
        /// </summary>
        public string PostBody
        {
            get
            {
                return this._postBody;
            }

            set
            {
                this._postBody = value;
            }
        }

        /// <summary>
        /// Call the API endpoint asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task{String}"/> object containing the API response text.</returns>
        /// <exception cref="UnsuccessfulApiCallException">Thrown if the API request did not complete
        /// successfully.</exception>
        public async Task<string> CallEndpointAsync()
        {
            Tools.Logger.Log(this.GetType().ToString(), "Calling the API endpoint \"" + this.EndpointUri + "\" with the " + this._apiMethod.Method + " method and the following POST body: \"" + this._postBody + "\"");

            Uri apiURIWithoutQueryString = new Uri(string.Format("{0}{1}{2}{3}", this._apiURI.Scheme, "://", this._apiURI.Authority, this._apiURI.AbsolutePath));

            Dictionary<string, string> apiURIParameters = new Dictionary<string, string>();
            if (this._apiURI.Query != string.Empty)
            {
                string[] paramPairs = this._apiURI.Query.Split('&');
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
            // Remember that parameters are passed in the POST body in Xamarin.Auth:
            // <https://github.com/xamarin/Xamarin.Auth/blob/master/src/Xamarin.Auth/Request.cs#L53>
            // <https://github.com/xamarin/Xamarin.Auth/blob/master/src/Xamarin.Auth/Request.cs#L316>
            // even though this means that we can not pass a POST body and a query string in the URL at the same time (!)
            if (this._postBody != null)
            {
                string[] bodyParamPairs = this._postBody.Split('&');
                foreach (string bodyParam in bodyParamPairs)
                {
                    string[] bodyParamSplit = bodyParam.Split('=');
                    apiURIParameters.Add(bodyParamSplit[0], (bodyParamSplit.Length == 2) ? bodyParamSplit[1] : null);
                }
            }

            OAuth2Request apiCall = new OAuth2Request(this._apiMethod.ToString(), apiURIWithoutQueryString, apiURIParameters, this._oauthAccount);

            Response apiResponse = await apiCall.GetResponseAsync().ConfigureAwait(false);

            if (apiResponse != null)
            {
                if (apiResponse.StatusCode == HttpStatusCode.OK)
                {
                    Tools.Logger.Log(this.GetType().ToString(), "The API call completed successfully. Getting the response...");
                    string response = await apiResponse.GetResponseTextAsync().ConfigureAwait(false);
                    Tools.Logger.Log(this.GetType().ToString(), "Response: " + response);
                    return response;
                }
                else
                {
                    Tools.Logger.Log(this.GetType().ToString(), "The API call did not complete successfully. Status code: " + apiResponse.StatusCode.ToString());
                    throw new UnsuccessfulApiCallException("The API call did not complete successfully (status code: " + apiResponse.StatusCode + ")", null, apiResponse.StatusCode, apiResponse);
                }
            }
            else
            {
                Tools.Logger.Log(this.GetType().ToString(), "The API call returned a null response");
                throw new UnsuccessfulApiCallException("The API call returned a null response");
            }
        }
    }
}

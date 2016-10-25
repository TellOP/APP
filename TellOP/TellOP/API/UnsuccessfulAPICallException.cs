// <copyright file="UnsuccessfulApiCallException.cs" company="University of Murcia">
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
    using System.Net;
    using Xamarin.Auth;

    /// <summary>
    /// An exception thrown when an API call does not complete successfully.
    /// </summary>
    public class UnsuccessfulApiCallException : Exception
    {
        /// <summary>
        /// The HTTP status code returned by the API call.
        /// </summary>
        private HttpStatusCode _apiStatus;

        /// <summary>
        /// The Web response returned by the API call.
        /// </summary>
        private Response _webResponse;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulApiCallException"/> class.
        /// </summary>
        public UnsuccessfulApiCallException()
            : this("The API call failed.", null, HttpStatusCode.OK, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulApiCallException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public UnsuccessfulApiCallException(string message)
            : this(message, null, HttpStatusCode.OK, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulApiCallException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception, if any.</param>
        public UnsuccessfulApiCallException(string message, Exception innerException)
            : this(message, innerException, HttpStatusCode.OK, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulApiCallException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="status">The HTTP status code returned by the API call.</param>
        public UnsuccessfulApiCallException(string message, HttpStatusCode status)
            : this(message, null, status, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulApiCallException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception, if any.</param>
        /// <param name="status">The HTTP status code returned by the API call.</param>
        /// <param name="response">The Web response returned by the API call.</param>
        public UnsuccessfulApiCallException(string message, Exception innerException, HttpStatusCode status, Response response)
            : base(message, innerException)
        {
            this._apiStatus = status;
            this._webResponse = response;
        }

        /// <summary>
        /// Gets the HTTP status code returned by the API call.
        /// </summary>
        public HttpStatusCode Status
        {
            get
            {
                return this._apiStatus;
            }
        }

        /// <summary>
        /// Gets the Web response returned by the API call.
        /// </summary>
        public Response GetResponse
        {
            get
            {
                return this._webResponse;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            string message = "TellOP.Api.UnsuccessfulApiCallException: " + this.Message + ". HTTP status code: " + this._apiStatus.ToString() + ".";

            if (this._webResponse != null)
            {
                message += " API response: " + this._webResponse.ToString() + ".";
            }

            message += " at " + this.StackTrace.ToString();
            return message;
        }
    }
}

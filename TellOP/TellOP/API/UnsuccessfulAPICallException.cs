// <copyright file="UnsuccessfulAPICallException.cs" company="University of Murcia">
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
    using System.Net;
    using Xamarin.Auth;

    /// <summary>
    /// An exception thrown when an API call does not complete successfully.
    /// </summary>
    public class UnsuccessfulAPICallException : Exception
    {
        /// <summary>
        /// The HTTP status code returned by the API call.
        /// </summary>
        private HttpStatusCode apiStatus;

        /// <summary>
        /// The Web response returned by the API call.
        /// </summary>
        private Response webResponse;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="UnsuccessfulAPICallException"/> class.
        /// </summary>
        public UnsuccessfulAPICallException()
            : this(null, null, HttpStatusCode.OK, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="UnsuccessfulAPICallException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public UnsuccessfulAPICallException(string message)
            : this(message, null, HttpStatusCode.OK, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="UnsuccessfulAPICallException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception, if any.</param>
        public UnsuccessfulAPICallException(
            string message,
            Exception innerException)
            : this(message, innerException, HttpStatusCode.OK, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="UnsuccessfulAPICallException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="status">The HTTP status code returned by the API call.
        /// </param>
        public UnsuccessfulAPICallException(
            string message,
            HttpStatusCode status)
            : this(message, null, status, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="UnsuccessfulAPICallException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception, if any.</param>
        /// <param name="status">The HTTP status code returned by the API call.
        /// </param>
        /// <param name="response">The Web response returned by the API call.
        /// </param>
        public UnsuccessfulAPICallException(
            string message,
            Exception innerException,
            HttpStatusCode status,
            Response response)
            : base(message, innerException)
        {
            this.apiStatus = status;
            this.webResponse = response;
        }

        /// <summary>
        /// Gets the HTTP status code returned by the API call.
        /// </summary>
        public HttpStatusCode Status
        {
            get
            {
                return this.apiStatus;
            }
        }

        /// <summary>
        /// Gets the Web response returned by the API call.
        /// </summary>
        public Response GetResponse
        {
            get
            {
                return this.webResponse;
            }
        }
    }
}

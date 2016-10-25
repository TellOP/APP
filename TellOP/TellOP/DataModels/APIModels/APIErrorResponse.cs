// <copyright file="ApiErrorResponse.cs" company="University of Murcia">
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
// <author>Alessandro Menti</author>

namespace TellOP.DataModels.APIModels
{
    using Newtonsoft.Json;

    /// <summary>
    /// JSON object representing an error response by an API endpoint on the TellOP server.
    /// </summary>
    [JsonObject]
    public class ApiErrorResponse
    {
        /// <summary>
        /// The Web service endpoint was unable to initialize the cURL library.
        /// </summary>
        public const int UnableToInitializeCurl = 1;

        /// <summary>
        /// The Web service endpoint was unable to set a cURL option.
        /// </summary>
        public const int UnableToSetCurlOption = 2;

        /// <summary>
        /// The Web service endpoint was unable to execute the remote cURL request.
        /// </summary>
        public const int UnableToExecuteCurlRequest = 3;

        /// <summary>
        /// The cURL request was completed successfully, but the remote server returned an error.
        /// </summary>
        public const int ErrorInCurlResponse = 4;

        /// <summary>
        /// The provided data does not pass validation.
        /// </summary>
        public const int ValidationError = 5;

        /// <summary>
        /// The Web service is unable to parse the remote response (either due to an internal error or to the response
        /// being malformed).
        /// </summary>
        public const int UnableToParseRemoteResponse = 6;

        /// <summary>
        /// Gets or sets the API error code.
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the API error message.
        /// </summary>
        [JsonProperty("error_message")]
        public string Message { get; set; }
    }
}

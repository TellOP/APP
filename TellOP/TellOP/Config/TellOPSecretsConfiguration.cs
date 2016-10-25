// <copyright file="TellOPSecretsConfiguration.cs" company="University of Murcia">
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

namespace TellOP.Config
{
    /// <summary>
    /// The configuration file containing the base endpoint URL and the OAuth 2.0 credentials.
    /// </summary>
    internal class TellOPSecretsConfiguration
    {
        /// <summary>
        /// The server URL.
        /// </summary>
        public static readonly string ServerBaseUrl = "https://tellop.inf.um.es";

        /// <summary>
        /// The OAuth 2.0 client id.
        /// </summary>
        public static readonly string OAuth2ClientId = "1011a510829210912e6b9c63f4108e5b28fdc110e7dde792dfb1ee45524cc5c1f4a78";

        // TODO: insert the OAuth 2.0 client secret obtained from the Tell-OP server.
        // The field is currently left empty to preserve its secrecy.

        /// <summary>
        /// The OAuth 2.0 client secret.
        /// </summary>
        public static readonly string OAuth2ClientSecret = string.Empty;

        /// <summary>
        /// A space-separated list of OAuth 2.0 scopes this app will access.
        /// </summary>
        public static readonly string OAuth2Scopes = "basic dashboard exercises profile onlineresources";
    }
}

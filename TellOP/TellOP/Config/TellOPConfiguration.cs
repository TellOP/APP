﻿// <copyright file="TellOPConfiguration.cs" company="University of Murcia">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataModels.Enums;

    /// <summary>
    /// A class containing the Tell-OP base URI and endpoint URI building functions.
    /// </summary>
    public sealed class TellOPConfiguration
    {
        /// <summary>
        /// A list of server API names and of the respective relative URIs.
        /// </summary>
        private static Dictionary<string, string> _serverEndpoints = new Dictionary<string, string>()
        {
            { "TellOP.API.Adelex", "/api/v1/resource/adelex" },
            { "TellOP.API.CollinsDictionary." + SupportedLanguage.English.ToLCID(), "/api/v1/resource/collins" },
            { "TellOP.API.CollinsDictionaryGetEntry." + SupportedLanguage.English.ToLCID(), "/api/v1/resource/collinsgetentry" },
            { "TellOP.API.CollinsDictionary." + SupportedLanguage.German.ToLCID(), "/api/v1/resource/collinsde" },
            { "TellOP.API.CollinsDictionaryGetEntry." + SupportedLanguage.German.ToLCID(), "/api/v1/resource/collinsdegetentry" },
            { "TellOP.API.Exercise", "/api/v1/app/exercise" },
            { "TellOP.API.ExerciseFeatured", "/api/v1/app/featured" },
            { "TellOP.API.ExerciseHistory", "/api/v1/app/history" },
            { "TellOP.API.LexTutorAPIController", "/api/v1/resource/lextutor" },
            { "TellOP.API.NetSpeakFollowing", "/api/v1/resource/netspeakfollowing" },
            { "TellOP.API.NetSpeakPreceding", "/api/v1/resource/netspeakpreceding" },
            { "TellOP.API.Stands4Dictionary", "/api/v1/resource/stands4dictionary" },
            { "TellOP.API.StringNet", "/api/v1/resource/stringnet" },
            { "TellOP.API.Tips", "/api/v1/app/tips" },
            { "TellOP.API.UserProfile", "/api/v1/app/profile" },
            { "TellOP.API.SpanishPOSTagger", "/api/v1/resource/estagger" },
            { "TellOP.API.GermanPOSTagger", "/api/v1/resource/detagger" },
            { "TellOP.API.OxfordDictionary.English", "/api/v1/resource/oxford/en" },
            { "TellOP.API.OxfordDictionary.USEnglish", "/api/v1/resource/oxford/en" },
            { "TellOP.API.OxfordDictionary.French", "/api/v1/resource/oxford/fr" },
            { "TellOP.API.OxfordDictionary.German", "/api/v1/resource/oxford/de" },
            { "TellOP.API.OxfordDictionary.Italian", "/api/v1/resource/oxford/it" },
            { "TellOP.API.OxfordDictionary.Spanish", "/api/v1/resource/oxford/es" },
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="TellOPConfiguration"/> class.
        /// </summary>
        private TellOPConfiguration()
        {
        }

        /// <summary>
        /// Gets the base URL of the server as an <see cref="Uri"/>.
        /// </summary>
        public static Uri ServerBaseUrl
        {
            get
            {
                return new Uri(TellOPSecretsConfiguration.ServerBaseUrl);
            }
        }

        // URLs used in the client.

        /// <summary>
        /// Gets the privacy page URL.
        /// </summary>
        public static Uri PrivacyUrl
        {
            get
            {
                return new Uri(TellOPSecretsConfiguration.ServerBaseUrl + "/privacy");
            }
        }

        /// <summary>
        /// Gets the signup page URL.
        /// </summary>
        public static Uri SignUpUrl
        {
            get
            {
                return new Uri(TellOPSecretsConfiguration.ServerBaseUrl + "/register");
            }
        }

        // OAuth 2.0 endpoints.
        // The access token, authorization and redirect URLs are kept internal; since the client ID and secret are
        // internal as well, there's no need to make them public.
        // The other endpoints are public as they might be used by custom, platform-specific renderers (as is the case
        // with the profile endpoint).

        /// <summary>
        /// Gets the URL for the profile endpoint.
        /// </summary>
        public static Uri OAuth2ProfileUrl
        {
            get
            {
                return new Uri(TellOPSecretsConfiguration.ServerBaseUrl + "/api/v1/app/profile");
            }
        }

        /// <summary>
        /// Gets the OAuth 2.0 access token URL.
        /// </summary>
        public static Uri OAuth2AccessTokenUrl
        {
            get
            {
                return new Uri(TellOPSecretsConfiguration.ServerBaseUrl + "/oauth/2/token");
            }
        }

        /// <summary>
        /// Gets the OAuth 2.0 authorization URL.
        /// </summary>
        public static Uri OAuth2AuthorizeUrl
        {
            get
            {
                return new Uri(TellOPSecretsConfiguration.ServerBaseUrl + "/oauth/2/authorize");
            }
        }

        /// <summary>
        /// Gets the OAuth 2.0 redirect URL.
        /// </summary>
        public static Uri OAuth2RedirectUrl
        {
            get
            {
                return new Uri(TellOPSecretsConfiguration.ServerBaseUrl + "/oauth/2/success");
            }
        }

        /// <summary>
        /// Gets the OAuth 2.0 client ID.
        /// </summary>
        public static string OAuth2ClientId
        {
            get
            {
                return TellOPSecretsConfiguration.OAuth2ClientId;
            }
        }

        /// <summary>
        /// Gets the OAuth 2.0 client secret.
        /// </summary>
        public static string OAuth2ClientSecret
        {
            get
            {
                return TellOPSecretsConfiguration.OAuth2ClientSecret;
            }
        }

        /// <summary>
        /// Gets a space-separated list of OAuth 2.0 scopes this app will access.
        /// </summary>
        public static string OAuth2Scopes
        {
            get
            {
                return TellOPSecretsConfiguration.OAuth2Scopes;
            }
        }

        /// <summary>
        /// Retrieves the correct endpoint from the configuration file.
        /// </summary>
        /// <param name="fullyQualifiedApiClassName">The server API name, e.g. <c>TellOP.API.Adelex</c> for the
        /// <see cref="Api.AdelexApi"/> class.</param>
        /// <returns>The endpoint URL corresponding to the server API name.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the key is missing or misspelled.</exception>
        public static string GetEndpoint(string fullyQualifiedApiClassName)
        {
            try
            {
                return TellOPSecretsConfiguration.ServerBaseUrl + _serverEndpoints[fullyQualifiedApiClassName];
            }
            catch (Exception ex)
            {
                Tools.Logger.Log("TellOPConfiguration:GetEndpoint", "You requested '" + fullyQualifiedApiClassName + "', but these are the values:\n" + string.Join("\n", _serverEndpoints.Select(x => x.Key + "\t=> " + x.Value).ToArray()), ex);
                return null;
            }
        }

        /// <summary>
        /// Retrieves the correct endpoint from the configuration file.
        /// </summary>
        /// <param name="fullyQualifiedApiClassName">The server API name, e.g. <c>TellOP.API.Adelex</c> for the
        /// <see cref="Api.AdelexApi"/> class.</param>
        /// <returns>The endpoint URL corresponding to the server API name.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the key is missing or misspelled.</exception>
        public static Uri GetEndpointAsUri(string fullyQualifiedApiClassName)
        {
            return new Uri(GetEndpoint(fullyQualifiedApiClassName));
        }
    }
}

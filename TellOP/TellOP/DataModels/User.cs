// <copyright file="User.cs" company="University of Murcia">
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

namespace TellOP.DataModels
{
    using DataModels.Enums;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// A JSON object representing a user.
    /// </summary>
    [JsonObject]
    public class User
    {
        /// <summary>
        /// Gets or sets the e-mail address of the user.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the language of the user.
        /// </summary>
        [JsonProperty("locale")]
        public string Locale { get; set; }

        /// <summary>
        /// Gets or sets the title of the user.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the name and surname of the user.
        /// </summary>
        [JsonProperty("displayname")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the language level of the user.
        /// </summary>
        /// <seealso cref="LanguageLevelClassification"/>
        [JsonProperty("languagelevel")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LanguageLevelClassification LanguageLevel { get; set; }
    }
}

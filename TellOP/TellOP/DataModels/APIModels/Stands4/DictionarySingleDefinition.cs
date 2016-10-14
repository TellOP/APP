// <copyright file="DictionarySingleDefinition.cs" company="University of Murcia">
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

namespace TellOP.DataModels.APIModels.Stands4
{
    using Newtonsoft.Json;

    /// <summary>
    /// A JSON object representing a single definition returned by the Stands4
    /// Dictionary Definitions endpoint.
    /// </summary>
    [JsonObject]
    public class DictionarySingleDefinition
    {
        /// <summary>
        /// Gets or sets the term that was searched for.
        /// </summary>
        [JsonProperty("term")]
        public string Term { get; set; }

        /// <summary>
        /// Gets or sets the definition of the term.
        /// </summary>
        [JsonProperty("definition")]
        public string Definition { get; set; }

        /// <summary>
        /// Gets or sets the part of speech the term belongs to.
        /// </summary>
        [JsonProperty("partofspeech")]
        [JsonConverter(typeof(PartOfSpeechJSONConverter))]
        public PartOfSpeech PartOfSpeech { get; set; }

        /// <summary>
        /// Gets or sets the usage example for the term.
        /// </summary>
        [JsonProperty("example")]
        public string[] Examples { get; set; } // FIXME
    }
}

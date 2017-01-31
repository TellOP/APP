// <copyright file="OxfordWord.cs" company="University of Murcia">
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

namespace TellOP.DataModels.ApiModels
{
    using System;
    using System.Collections.Generic;
    using Enums;
    using Newtonsoft.Json;
    using Nito.AsyncEx;

    /// <summary>
    /// A JSON object representing a single sense in Oxford Dictionary search result.
    /// endpoints.
    /// </summary>
    [JsonObject]
    public class OxfordWord : IWord
    {
        /// <summary>
        /// Gets or sets the definitions.
        /// </summary>
        [JsonProperty("definitions")]
        public IList<string> Definitions { get; set; }

        /// <summary>
        /// Gets or sets the examples.
        /// </summary>
        [JsonProperty("examples")]
        public IList<string> Examples { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Part of Speech.
        /// </summary>
        [JsonProperty("pos")]
        [JsonConverter(typeof(PartOfSpeechJsonConverter))]
        public PartOfSpeech PartOfSpeech { get; set; }

        /// <summary>
        /// Gets or sets the definitions.
        /// </summary>
        [JsonProperty("term")]
        public string Term { get; set; }

        /// <summary>
        /// Gets the level.
        /// </summary>
        public AsyncLazy<LanguageLevelClassification> Level
        {
            get
            {
                return new AsyncLazy<LanguageLevelClassification>(() => this._level);
            }
        }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        [JsonProperty("level")]
        [JsonConverter(typeof(LanguageLevelClassificationJsonConverter))]
        private LanguageLevelClassification _level { get; set; }
    }
}

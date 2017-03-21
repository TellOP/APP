// <copyright file="EnglishWord.cs" company="University of Murcia">
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

namespace TellOP.DataModels.ApiModels
{
    using Api;
    using DataModels.Enums;
    using Newtonsoft.Json;
    using Nito.AsyncEx;

    /// <summary>
    /// A word returned by the <see cref="EnglishPOSTagger"/> API controller.
    /// </summary>
    [JsonObject]
    public class EnglishWord : IWord
    {
        /// <summary>
        /// Gets or sets the CEFR level of this word.
        /// </summary>
        public AsyncLazy<LanguageLevelClassification> Level { get; set; }

        /// <summary>
        /// Gets or sets the part of speech this term is categorized into.
        /// </summary>
        [JsonProperty("pos")]
        [JsonConverter(typeof(PartOfSpeechJsonConverter))]
        public PartOfSpeech PartOfSpeech { get; set; }

        /// <summary>
        /// Gets or sets the string representation of this word.
        /// </summary>
        [JsonProperty("token")]
        public string Term { get; set; }

        /// <summary>
        /// Gets or sets the definition obtained by the remote API.
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        /// Gets or sets the examples obtained by the remote API.
        /// </summary>
        public string Example { get; set; }

        /// <summary>
        /// Gets or sets the synonyms obtained by the remote API.
        /// </summary>
        public string Synonyms { get; set; }

        /// <summary>
        /// Gets or sets the antonyms obtained by the remote API.
        /// </summary>
        public string Antonyms { get; set; }
    }
}

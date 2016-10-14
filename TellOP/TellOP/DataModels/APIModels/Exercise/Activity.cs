// <copyright file="Activity.cs" company="University of Murcia">
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

namespace TellOP.DataModels.APIModels.Exercise
{
    using Enums;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// The basic JSON object for all possible activities.
    /// </summary>
    [JsonObject]
    public abstract class Activity
    {
        /// <summary>
        /// The JSON representation of the activity type.
        /// </summary>
        public const string ActivityType = "ACTIVITY";

        /// <summary>
        /// Gets or sets the unique ID for this exercise.
        /// </summary>
        [JsonProperty("id")]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the CEFR level for this exercise.
        /// </summary>
        [JsonProperty("level")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LanguageLevelClassification Level { get; set; }

        /// <summary>
        /// Gets or sets the language for this exercise.
        /// </summary>
        [JsonProperty("language")]
        [JsonConverter(typeof(SupportedLanguageJSONConverter))]
        public SupportedLanguage Language { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this exercise is
        /// "featured".
        /// </summary>
        [JsonProperty("featured")]
        public bool Featured { get; set; }
    }
}

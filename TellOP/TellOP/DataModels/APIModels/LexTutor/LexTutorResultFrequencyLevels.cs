// <copyright file="LexTutorResultFrequencyLevels.cs" company="University of Murcia">
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

namespace TellOP.DataModels.APIModels.LexTutor
{
    using Newtonsoft.Json;

    /// <summary>
    /// LexTutor word frequency levels.
    /// </summary>
    [JsonObject]
    public class LexTutorResultFrequencyLevels
    {
        /// <summary>
        /// Gets or sets the frequency details for K0 words.
        /// </summary>
        [JsonProperty("K-0")]
        public LexTutorResultFrequencyDetails K0Words { get; set; }

        /// <summary>
        /// Gets or sets the frequency details for K1 words.
        /// </summary>
        [JsonProperty("K-1")]
        public LexTutorResultFrequencyDetails K1Words { get; set; }

        /// <summary>
        /// Gets or sets the frequency details for K2 words.
        /// </summary>
        [JsonProperty("K-2")]
        public LexTutorResultFrequencyDetails K2Words { get; set; }

        /// <summary>
        /// Gets or sets the frequency details for K3 words.
        /// </summary>
        [JsonProperty("K-3")]
        public LexTutorResultFrequencyDetails K3Words { get; set; }

        /// <summary>
        /// Gets or sets the frequency details for K4 words.
        /// </summary>
        [JsonProperty("K-4")]
        public LexTutorResultFrequencyDetails K4Words { get; set; }

        /// <summary>
        /// Gets or sets the frequency details for K5 words.
        /// </summary>
        [JsonProperty("K-5")]
        public LexTutorResultFrequencyDetails K5Words { get; set; }

        /// <summary>
        /// Gets or sets the frequency details for K6 words.
        /// </summary>
        [JsonProperty("K-6")]
        public LexTutorResultFrequencyDetails K6Words { get; set; }

        /// <summary>
        /// Gets or sets the frequency details for K7 words.
        /// </summary>
        [JsonProperty("K-7")]
        public LexTutorResultFrequencyDetails K7Words { get; set; }

        /// <summary>
        /// Gets or sets the frequency details for K8 words.
        /// </summary>
        [JsonProperty("K-8")]
        public LexTutorResultFrequencyDetails K8Words { get; set; }

        /// <summary>
        /// Gets or sets the frequency details for K9 words.
        /// </summary>
        [JsonProperty("K-9")]
        public LexTutorResultFrequencyDetails K9Words { get; set; }

        /// <summary>
        /// Gets or sets the frequency details for off-list words.
        /// </summary>
        [JsonProperty("offlist")]
        public LexTutorResultFrequencyDetails OffList { get; set; }

        /// <summary>
        /// Gets or sets the frequency details for all words.
        /// </summary>
        [JsonProperty("total")]
        public LexTutorResultFrequencyDetails Total { get; set; }
    }
}

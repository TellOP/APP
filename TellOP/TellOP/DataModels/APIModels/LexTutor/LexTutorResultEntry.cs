// <copyright file="LexTutorResultEntry.cs" company="University of Murcia">
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

namespace TellOP.DataModels.APIModels.LexTutor
{
    using Newtonsoft.Json;

    /// <summary>
    /// A JSON object representing a reply returned by the LexTutor endpoint.
    /// </summary>
    [JsonObject]
    public class LexTutorResultEntry
    {
        /// <summary>
        /// Gets or sets the frequency levels for all word classes.
        /// </summary>
        [JsonProperty("frequencyLevels")]
        public LexTutorResultFrequencyLevels FrequencyLevels { get; set; }

        /// <summary>
        /// Gets or sets the ratios for the analyzed text.
        /// </summary>
        [JsonProperty("ratios")]
        public LexTutorResultRatios Ratios { get; set; }
    }
}

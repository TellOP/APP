// <copyright file="LexTutorResultAbsolutePercentFrequency.cs" company="University of Murcia">
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

namespace TellOP.DataModels.ApiModels.LexTutor
{
    using Newtonsoft.Json;

    /// <summary>
    /// The frequency structure for each frequency detail.
    /// </summary>
    [JsonObject]
    public class LexTutorResultAbsolutePercentFrequency
    {
        /// <summary>
        /// Gets or sets the absolute frequency.
        /// </summary>
        [JsonProperty("number")]
        public int AbsoluteFrequency { get; set; }

        /// <summary>
        /// Gets or sets the percent frequency.
        /// </summary>
        [JsonProperty("percent")]
        public float Percent { get; set; }
    }
}

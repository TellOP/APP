// <copyright file="LexTutorResultFrequencyDetails.cs" company="University of Murcia">
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
    /// A class encapsulating the frequency details returned by the LexTutor endpoint for each word class.
    /// </summary>
    [JsonObject]
    public class LexTutorResultFrequencyDetails
    {
        /// <summary>
        /// Gets or sets the word families.
        /// </summary>
        [JsonProperty("families")]
        public LexTutorResultAbsolutePercentFrequency Families { get; set; }

        /// <summary>
        /// Gets or sets the word types.
        /// </summary>
        [JsonProperty("types")]
        public LexTutorResultAbsolutePercentFrequency Types { get; set; }

        /// <summary>
        /// Gets or sets the word tokens.
        /// </summary>
        [JsonProperty("tokens")]
        public LexTutorResultAbsolutePercentFrequency Tokens { get; set; }

        /// <summary>
        /// Gets or sets the word cumulative token.
        /// </summary>
        [JsonProperty("cumulativeToken")]
        public string CumulativeToken { get; set; }

        /// <summary>
        /// Check if this family is zero.
        /// </summary>
        /// <returns>True if families, tokens and types are zero.</returns>
        public bool IsZero()
        {
            return this.Families.IsZero()
                && this.Types.IsZero()
                && this.Tokens.IsZero();
        }
    }
}

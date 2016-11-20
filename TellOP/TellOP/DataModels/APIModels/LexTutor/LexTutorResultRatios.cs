// <copyright file="LexTutorResultRatios.cs" company="University of Murcia">
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
    /// Ratios and statistics for a given word class.
    /// </summary>
    [JsonObject]
    public class LexTutorResultRatios
    {
        /// <summary>
        /// Gets or sets the number of words appearing in the text for this
        /// word class.
        /// </summary>
        [JsonProperty("wordsInText")]
        public int WordsInText { get; set; }

        /// <summary>
        /// Gets or sets the number of different words for this word class.
        /// </summary>
        [JsonProperty("differentWords")]
        public int DifferentWords { get; set; }

        /// <summary>
        /// Gets or sets the type/token ratio for this word class.
        /// </summary>
        [JsonProperty("typeTokenRatio")]
        public float TypeTokenRatio { get; set; }

        /// <summary>
        /// Gets or sets the tokens per type number for this word class.
        /// </summary>
        [JsonProperty("tokensPerType")]
        public float TokensPerType { get; set; }

        /// <summary>
        /// Gets or sets the number of tokens on the list for this word class.
        /// </summary>
        [JsonProperty("tokensOnList")]
        public int TokensOnList { get; set; }

        /// <summary>
        /// Gets or sets the number of types on the list for this word class.
        /// </summary>
        [JsonProperty("typesOnList")]
        public int TypesOnList { get; set; }

        /// <summary>
        /// Gets or sets the number of families on the list for this word class.
        /// </summary>
        [JsonProperty("familiesOnList")]
        public int FamiliesOnList { get; set; }

        /// <summary>
        /// Gets or sets the number of tokens per family on the list for this word class.
        /// </summary>
        [JsonProperty("tokensPerFamily")]
        public float TokensPerFamily { get; set; }

        /// <summary>
        /// Gets or sets the number of types per family on the list for this word class.
        /// </summary>
        [JsonProperty("typesPerFamily")]
        public float TypesPerFamily { get; set; }

        /// <summary>
        /// Gets or sets the number of cognate tokens for this word class.
        /// </summary>
        [JsonProperty("cognatesTokens")]
        public int CognatesTokens { get; set; }

        /// <summary>
        /// Gets or sets the number of "cognate with French" tokens for this word class.
        /// </summary>
        [JsonProperty("cognatesWithFrench")]
        public int CognatesWithFrench { get; set; }

        /// <summary>
        /// Gets or sets the number of "not cognate with French" tokens for this word class.
        /// </summary>
        [JsonProperty("notCognatesWithFrench")]
        public int NotCognatesWithFrench { get; set; }

        /// <summary>
        /// Gets or sets the cognateness for this word class.
        /// </summary>
        [JsonProperty("cognateness")]
        public float Cognateness { get; set; }

        /// <summary>
        /// Gets or sets the sum of individual frequencies for this word class.
        /// </summary>
        [JsonProperty("sumIndividualFreqs")]
        public float SumIndividualFrequencies { get; set; }

        /// <summary>
        /// Gets or sets the frequencies by rateable tokens for this word class.
        /// </summary>
        [JsonProperty("freqsByRateableTokens")]
        public int FrequenciesByRateableTokens { get; set; }

        /// <summary>
        /// Gets or sets the mean frequency for this class.
        /// </summary>
        [JsonProperty("meanFrequency")]
        public LexTutorResultValueStandardDeviation MeanFrequency { get; set; }

        /// <summary>
        /// Gets or sets the logarithm (in base 10) of the count index for this word class.
        /// </summary>
        [JsonProperty("countIndexLog10")]
        public LexTutorResultValueStandardDeviation CountIndexLog10 { get; set; }
    }
}

// <copyright file="AdelexToken.cs" company="University of Murcia">
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

namespace TellOP.DataModels.APIModels.Adelex
{
    using Newtonsoft.Json;

    /// <summary>
    /// A single token in the text analyzed by the Adelex service.
    /// </summary>
    public class AdelexToken
    {
        /// <summary>
        /// Gets or sets the token type (word).
        /// </summary>
        [JsonProperty("type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Gets or sets the frequency of the token in the text.
        /// </summary>
        [JsonProperty("frequency")]
        public int Frequency { get; set; }

        /// <summary>
        /// Gets or sets the frequency of the token inside the text, expressed
        /// as a percent.
        /// </summary>
        [JsonProperty("percent")]
        public float Percent { get; set; }
    }
}

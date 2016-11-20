// <copyright file="AdelexResultEntry.cs" company="University of Murcia">
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

namespace TellOP.DataModels.ApiModels.Adelex
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;

    /// <summary>
    /// A JSON object representing a reply returned by the LexTutor endpoint.
    /// </summary>
    [JsonObject]
    public class AdelexResultEntry
    {
        /// <summary>
        /// Gets or sets the number of tokens in the text.
        /// </summary>
        [JsonProperty("tokenCount")]
        public int TokenCount { get; set; }

        /// <summary>
        /// Gets or sets the number of types in the text.
        /// </summary>
        [JsonProperty("types")]
        public int Types { get; set; }

        /// <summary>
        /// Gets or sets the type/token ratio.
        /// </summary>
        [JsonProperty("typeTokenRatio")]
        public float TypeTokenRatio { get; set; }

        /// <summary>
        /// Gets or sets the lexical diversity coefficient.
        /// </summary>
        [JsonProperty("lexicalDiversity")]
        public float LexicalDiversity { get; set; }

        /// <summary>
        /// Gets or sets the list of tokens occurring in the text.
        /// </summary>
        [JsonProperty("tokens")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Needed for Newtonsoft.Json deserialization")]
        public IList<AdelexToken> Tokens { get; set; }
    }
}

// <copyright file="CollinsJsonEnglishDictionary.cs" company="University of Murcia">
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

namespace TellOP.DataModels.ApiModels.Collins
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;

    /// <summary>
    /// A JSON object representing a reply returned by the "Collins English Dictionary" search endpoint.
    /// </summary>
    [JsonObject]
    public class CollinsJsonEnglishDictionary
    {
        /// <summary>
        /// Gets or sets the dictionary ID.
        /// </summary>
        [JsonProperty("dictionaryCode")]
        public string DictionaryCode { get; set; }

        /// <summary>
        /// Gets or sets the number of results returned by the query.
        /// </summary>
        [JsonProperty("resultNumber")]
        public int ResultCount { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages containing results.
        /// </summary>
        [JsonProperty("pageNumber")]
        public int LastResultPage { get; set; }

        /// <summary>
        /// Gets or sets the current result page.
        /// </summary>
        [JsonProperty("currentPageIndex")]
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the list of results.
        /// </summary>
        [JsonProperty("results")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Needed for Newtonsoft.Json deserialization")]
        public IList<CollinsJsonEnglishDictionarySingleResult> Results { get; set; }
    }
}

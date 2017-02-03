// <copyright file="CollinsJsonDictionaryEntry.cs" company="University of Murcia">
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
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// An entry in the Collins English Dictionary.
    /// </summary>
    [JsonObject]
    public class CollinsJsonDictionaryEntry
    {
        /// <summary>
        /// Gets or sets the unique ID of the dictionary this entry was
        /// extracted from.
        /// </summary>
        [JsonProperty("dictionaryCode")]
        public string DictionaryCode { get; set; }

        /// <summary>
        /// Gets or sets the unique ID for this entry.
        /// </summary>
        [JsonProperty("entryId")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the label (title) of the dictionary entry.
        /// </summary>
        [JsonProperty("entryLabel")]
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the URL of the dictionary entry.
        /// </summary>
        [JsonProperty("entryUrl")]
        public Uri Url { get; set; }

        // FIXME Topics

        /// <summary>
        /// Gets or sets the content of the dictionary entry.
        /// </summary>
        [JsonProperty("entryContent")]
        public CollinsJsonDictionaryEntryContent Content { get; set; }
    }
}

// <copyright file="CollinsJsonDictionaryEntryContent.cs" company="University of Murcia">
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
    /// The content of an <see cref="CollinsJsonDictionaryEntryContent"/>.
    /// </summary>
    [JsonObject]
    public class CollinsJsonDictionaryEntryContent
    {
        /// <summary>
        /// Gets or sets the list of entries (meanings and related words).
        /// </summary>
        [JsonProperty("entries")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Needed for Newtonsoft.Json deserialization")]
        public IList<CollinsJsonDictionaryEntryContentEntry> Entries { get; set; }

        /// <summary>
        /// Gets or sets the list of related words.
        /// </summary>
        [JsonProperty("related")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Needed for Newtonsoft.Json deserialization")]
        public IList<CollinsJsonDictionaryEntryRelated> Related { get; set; }
    }
}

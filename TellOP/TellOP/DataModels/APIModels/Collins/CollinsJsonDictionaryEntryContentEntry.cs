// <copyright file="CollinsJsonDictionaryEntryContentEntry.cs" company="University of Murcia">
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
    using DataModels.Enums;
    using Newtonsoft.Json;

    /// <summary>
    /// A content entry inside an <see cref="CollinsJsonDictionaryEntryContent"/>.
    /// </summary>
    [JsonObject]
    public class CollinsJsonDictionaryEntryContentEntry
    {
        /// <summary>
        /// Gets or sets the part of speech this content entry belongs to.
        /// </summary>
        [JsonConverter(typeof(CollinsJsonPartOfSpeechJsonConverter))]
        [JsonProperty("partOfSpeech")]
        public PartOfSpeech PartOfSpeech { get; set; }

        /// <summary>
        /// Gets or sets the list of senses (meanings) associated to this
        /// content entry.
        /// </summary>
        [JsonProperty("senses")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Needed for Newtonsoft.Json deserialization")]
        public IList<CollinsWordDefinitionSense> Senses { get; set; }
    }
}

// <copyright file="CollinsWordDefinitionSense.cs" company="University of Murcia">
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

namespace TellOP.DataModels.APIModels.Collins
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;

    /// <summary>
    /// A meaning (sense) of an
    /// <see cref="CollinsJSONEnglishDictionaryEntryContentEntry"/>.
    /// </summary>
    [JsonObject]
    public class CollinsWordDefinitionSense
    {
        // TODO: is the following field needed?

        /// <summary>
        /// Gets the original word
        /// </summary>
        public CollinsWord Word { get; private set; }

        /// <summary>
        /// Gets or sets the definitions associated to this sense.
        /// </summary>
        [JsonProperty("definitions")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Needed for Newtonsoft.Json deserialization")]
        public IList<string> Definitions { get; set; }

        /// <summary>
        /// Gets or sets the usage examples for this word.
        /// </summary>
        [JsonProperty("examples")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Needed for Newtonsoft.Json deserialization")]
        public IList<string> Examples { get; set; }

        /// <summary>
        /// Gets or sets the words linked to this sense.
        /// </summary>
        [JsonProperty("seeAlso")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Needed for Newtonsoft.Json deserialization")]
        public IList<CollinsJSONLinkedWord> SeeAlso { get; set; }

        /// <summary>
        /// Gets or sets the words related to this sense.
        /// </summary>
        [JsonProperty("related")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Needed for Newtonsoft.Json deserialization")]
        public IList<CollinsJSONLinkedWord> Related { get; set; }
    }
}

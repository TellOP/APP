// <copyright file="ActivityEssay.cs" company="University of Murcia">
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

namespace TellOP.DataModels.APIModels.Exercise
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;

    /// <summary>
    /// The JSON object for essays.
    /// </summary>
    [JsonObject]
    public class ActivityEssay : Activity
    {
        /// <summary>
        /// The JSON representation of the activity type.
        /// </summary>
        public new const string ActivityType = "ESSAY";

        /// <summary>
        /// Gets or sets the title of the essay.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description for this essay.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a list of tags for this essay.
        /// </summary>
        [JsonProperty("tags")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Needed for Newtonsoft.Json deserialization")]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the minimum amount of words for this essay.
        /// </summary>
        [JsonProperty("minimumwords")]
        public int MinimumWords { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount of words for this essay.
        /// </summary>
        [JsonProperty("maximumwords")]
        public int MaximumWords { get; set; }

        /// <summary>
        /// Gets or sets the user instructions ("preliminary text") for this essay.
        /// </summary>
        [JsonProperty("text")]
        public string PreliminaryText { get; set; }
    }
}

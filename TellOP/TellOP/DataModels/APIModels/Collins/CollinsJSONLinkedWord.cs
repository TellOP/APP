﻿// <copyright file="CollinsJsonLinkedWord.cs" company="University of Murcia">
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
    using Newtonsoft.Json;

    /// <summary>
    /// A "related" or "see also" word linked from a <see cref="CollinsWordDefinitionSense"/>.
    /// </summary>
    [JsonObject]
    public class CollinsJsonLinkedWord
    {
        /// <summary>
        /// Gets or sets the target unique ID of the linked word.
        /// </summary>
        [JsonProperty("target")]
        public string TargetId { get; set; }

        /// <summary>
        /// Gets or sets the linked word/expression.
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}

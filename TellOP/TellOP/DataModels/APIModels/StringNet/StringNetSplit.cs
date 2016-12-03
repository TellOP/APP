// <copyright file="StringNetSplit.cs" company="University of Murcia">
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

namespace TellOP.DataModels.ApiModels.StringNet
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// A single result returned from the StringNet API.
    /// </summary>
    public class StringNetSplit
    {
        /// <summary>
        /// Gets or sets the list of <see cref="StringNetCollocations"/> that appers before the searched word.
        /// </summary>
        [JsonProperty("before")]
        public IList<StringNetCollocations> Before { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="StringNetCollocations"/> that appers after the searched word.
        /// </summary>
        [JsonProperty("after")]
        public IList<StringNetCollocations> After { get; set; }
    }
}

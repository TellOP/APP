// <copyright file="StringNet.cs" company="University of Murcia">
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

namespace TellOP.DataModels.ApiModels
{
    using Newtonsoft.Json;

    /// <summary>
    /// A single result returned from the StringNet API.
    /// </summary>
    public class StringNet
    {
        /// <summary>
        /// Gets or sets the given collocation.
        /// </summary>
        [JsonProperty("collocation")]
        public string Collocation { get; set; }

        /// <summary>
        /// Gets or sets the frequency for the given collocation.
        /// </summary>
        [JsonProperty("frequency")]
        public int Frequency { get; set; }

        /// <summary>
        /// Gets or sets the sample text for the given collocation.
        /// </summary>
        [JsonProperty("sample")]
        public string Sample { get; set; }
    }
}

// <copyright file="Tip.cs" company="University of Murcia">
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

namespace TellOP.DataModels.ApiModels
{
    using Newtonsoft.Json;

    /// <summary>
    /// A single tip as returned from the API endpoint.
    /// </summary>
    [JsonObject]
    public class Tip
    {
        /// <summary>
        /// Gets or sets the unique ID of the tip.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the text of the tip.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets the XAML name for this tip.
        /// </summary>
        public string XamlName
        {
            get
            {
                return "TIPS" + this.Id;
            }
        }

        /// <summary>
        /// Gets a string representation of the tip.
        /// </summary>
        /// <returns>The text of the tip.</returns>
        public override string ToString()
        {
            return this.Text;
        }
    }
}

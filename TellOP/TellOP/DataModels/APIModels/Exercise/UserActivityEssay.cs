// <copyright file="UserActivityEssay.cs" company="University of Murcia">
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
    using System;
    using Enums;
    using Newtonsoft.Json;

    /// <summary>
    /// An essay submitted by the user to the Tell-OP Web service.
    /// </summary>
    [JsonObject]
    public class UserActivityEssay : UserActivity
    {
        /// <summary>
        /// The JSON representation of the user activity type.
        /// </summary>
        public new const string UserActivityType = "ESSAY";

        /// <summary>
        /// Gets or sets the essay text as submitted by the user.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the time and date the essay was submitted.
        /// </summary>
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the exercise status.
        /// </summary>
        [JsonProperty("passed")]
        [JsonConverter(typeof(ExerciseStatusJsonConverter))]
        public ExerciseStatus Status { get; set; }
    }
}

// <copyright file="UserActivityDictionarySearch.cs" company="University of Murcia">
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
    using Newtonsoft.Json;

    /// <summary>
    /// A dictionary search submitted by the user to the Tell-OP Web service.
    /// </summary>
    [JsonObject]
    public class UserActivityDictionarySearch : UserActivity
    {
        /// <summary>
        /// The JSON representation of the user activity type.
        /// </summary>
        public new const string UserActivityType = "DICT_SEARCH";
    }
}

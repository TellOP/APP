// <copyright file="ActivityConverter.cs" company="University of Murcia">
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

namespace TellOP.DataModels.ApiModels.Exercise
{
    using System;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// A <see cref="JsonCreationConverter{Activity}"/> converting JSON representations of exercises to
    /// <see cref="Activity"/> objects (or one of its subclasses) and vice versa.
    /// </summary>
    public class ActivityConverter : JsonCreationConverter<Activity>
    {
        /// <summary>
        /// Creates an instance of the subclass of <see cref="Activity"/> that is most appropriate for the JSON
        /// representation in <paramref name="jsonObject"/>.
        /// </summary>
        /// <param name="objectType">The object type.</param>
        /// <param name="jsonObject">An instance of <see cref="JObject"/> containing the JSON object to convert.</param>
        /// <returns>A new instance of the subclass of <see cref="Activity"/> that is most appropriate for the JSON
        /// representation in <paramref name="jsonObject"/>.</returns>
        protected override Activity Create(Type objectType, JObject jsonObject)
        {
            if (jsonObject == null)
            {
                throw new ArgumentNullException("jsonObject");
            }

            var typeName = jsonObject["type"].ToString();
            switch (typeName)
            {
                case ActivityEssay.ActivityType:
                    return new ActivityEssay();
                case ActivityDictionarySearch.ActivityType:
                    return new ActivityDictionarySearch();
                default:
                    return null;
            }
        }
    }
}

// <copyright file="ExerciseAPI.cs" company="University of Murcia">
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

namespace TellOP.API
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using DataModels.Activity;
    using DataModels.APIModels.Exercise;
    using Newtonsoft.Json;
    using Xamarin.Auth;

    /// <summary>
    /// A class accessing the exercise API on the TellOP server.
    /// </summary>
    public class ExerciseAPI : OAuth2API
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseAPI"/>
        /// class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/>
        /// class to use to store the OAuth 2.0 account credentials.</param>
        public ExerciseAPI(Account account)
            : base(
                  Config.TellOPConfiguration.GetEndpointAsUri("TellOP.API.Exercise"),
                  HttpMethod.Get,
                  account)
        {
            throw new NotImplementedException("Calling this constructor without passing the ID is not supported");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseAPI"/>
        /// class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/>
        /// class to use to store the OAuth 2.0 account credentials.</param>
        /// <param name="id">The ID of the exercise.</param>
        public ExerciseAPI(Account account, int id)
            : base(
                  Config.TellOPConfiguration.GetEndpointAsUri("TellOP.API.Exercise"),
                  HttpMethod.Get,
                  account,
                  "id=" + Uri.EscapeDataString(id.ToString(System.Globalization.CultureInfo.InvariantCulture)))
        {
        }

        /// <summary>
        /// Converts an <see cref="Activity"/> to the most specific subclass of
        /// <see cref="Exercise"/> and fills in the
        /// relevant details.
        /// </summary>
        /// <param name="activity">An instance of <see cref="Activity"/>
        /// containing the data returned by an API endpoint.</param>
        /// <returns>A subclass of <see cref="Exercise"/>
        /// containing the data converted from <paramref name="activity"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if
        /// <paramref name="activity"/> is <c>null</c>.</exception>
        /// <exception cref="NotImplementedException">Thrown if
        /// <paramref name="activity"/> is an activity type that is not
        /// supported by the app at this time.</exception>
        public static Exercise ConvertActivityToExercise(Activity activity)
        {
            if (activity == null)
            {
                throw new ArgumentNullException("activity");
            }

            if (activity.GetType() == typeof(ActivityEssay))
            {
                ActivityEssay resultEssay = (ActivityEssay)activity;
                EssayExercise essay = new EssayExercise()
                {
                    Title = resultEssay.Title,
                    Description = resultEssay.Description,
                    MinimumWords = resultEssay.MinimumWords,
                    MaximumWords = resultEssay.MaximumWords,
                    Contents = resultEssay.PreliminaryText,
                    Featured = resultEssay.Featured,
                    Uid = resultEssay.ID,
                    Language = resultEssay.Language,
                    Level = resultEssay.Level
                };
                foreach (string tag in resultEssay.Tags)
                {
                    essay.Tags.Add(tag);
                }

                return essay;
            }

            // TODO: support dictionary searches
            throw new NotImplementedException("The returned exercise type is not supported yet");
        }

        /// <summary>
        /// Call the API endpoint and return the object representation of the
        /// API response.
        /// </summary>
        /// <returns>A <see cref="Task{Activity}"/> containing the
        /// object representation of the API response as its result.</returns>
        public async Task<Activity> CallEndpointAsObjectAsync()
        {
            return JsonConvert.DeserializeObject<Activity>(
                await this.CallEndpointAsync().ConfigureAwait(false),
                new ActivityConverter());
        }

        /// <summary>
        /// Call the API endpoint and return it in the form used for internal
        /// representation.
        /// </summary>
        /// <returns>An instance of a subclass of
        /// <see cref="Exercise"/> containing the data
        /// returned by the API.</returns>
        public async Task<Exercise> CallEndpointAsExerciseModel()
        {
            return ConvertActivityToExercise(await this.CallEndpointAsObjectAsync().ConfigureAwait(false));
        }
    }
}

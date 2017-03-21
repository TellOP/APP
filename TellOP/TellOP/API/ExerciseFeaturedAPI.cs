// <copyright file="ExerciseFeaturedApi.cs" company="University of Murcia">
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

namespace TellOP.Api
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Threading.Tasks;
    using DataModels.Activity;
    using DataModels.ApiModels.Exercise;
    using Newtonsoft.Json;
    using Xamarin.Auth;

    /// <summary>
    /// A class accessing the list of featured exercises on the TellOP server.
    /// </summary>
    public class ExerciseFeaturedApi : OAuth2Api
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseFeaturedApi"/> class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/> class to use to store the OAuth 2.0 account
        /// credentials.</param>
        public ExerciseFeaturedApi(Account account)
            : base(Config.TellOPConfiguration.GetEndpointAsUri("TellOP.API.ExerciseFeatured"), HttpMethod.Get, account)
        {
        }

        /// <summary>
        /// Call the API endpoint and return the object representation of the API response.
        /// </summary>
        /// <returns>A <see cref="Task{IList}"/> containing the object representation of the API response as its
        /// result.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list inside a Task")]
        public async Task<IList<Activity>> CallEndpointAsObjectAsync()
        {
            try
            {
                var k = await this.CallEndpointAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<List<Activity>>(k, new ActivityConverter());
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex =>
                {
                    Tools.Logger.Log("Featured:CallEndpointAsObjectAsync", ex);
                    return false;
                });
                throw ae;
            }
        }

        /// <summary>
        /// Call the API endpoint and return it in the form used for internal representation.
        /// </summary>
        /// <returns>A <see cref="Task{IList}"/> containing instances of a subclass of <see cref="Exercise"/> filled in
        /// with the data returned by the API.</returns>
        /// <exception cref="NotImplementedException">Thrown if an activity in the result list provided by the API is
        /// an activity type that is not supported by the app at this time.</exception>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list inside a Task")]
        public async Task<IList<Exercise>> CallEndpointAsExerciseModel()
        {
            try
            {
                IList<Activity> resultList = await this.CallEndpointAsObjectAsync().ConfigureAwait(false);
                List<Exercise> convertedResultList = new List<Exercise>();

                foreach (Activity result in resultList)
                {
                    convertedResultList.Add(ExerciseApi.ConvertActivityToExercise(result));
                }

                return convertedResultList;
            }
            catch (Exception ex)
            {
                Tools.Logger.Log("FeaturedPage:CallEndpointAsExerciseModel", ex);
            }

            return new List<Exercise>();
        }
    }
}

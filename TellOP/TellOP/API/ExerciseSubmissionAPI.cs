// <copyright file="ExerciseSubmissionApi.cs" company="University of Murcia">
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
    using System.Net.Http;
    using DataModels.APIModels.Exercise;
    using Xamarin.Auth;

    /// <summary>
    /// A class that submits exercises to the TellOP server.
    /// </summary>
    public class ExerciseSubmissionApi : OAuth2Api
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseSubmissionApi"/> class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/> class to use to store the OAuth 2.0 account
        /// credentials.</param>
        public ExerciseSubmissionApi(Account account)
            : base(Config.TellOPConfiguration.GetEndpointAsUri("TellOP.API.Exercise"), HttpMethod.Post, account)
        {
            throw new NotImplementedException("Calling this constructor without passing the user activity to submit is not supported");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseSubmissionApi"/> class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/> class to use to store the OAuth 2.0 account
        /// credentials.</param>
        /// <param name="activity">The user activity to submit.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="activity"/> is <c>null</c>.</exception>
        public ExerciseSubmissionApi(Account account, UserActivity activity)
            : base(Config.TellOPConfiguration.GetEndpointAsUri("TellOP.API.Exercise"), HttpMethod.Post, account)
        {
            if (activity == null)
            {
                throw new ArgumentNullException("activity");
            }

            this.PostBody = "id=" + Uri.EscapeDataString(activity.ActivityId.ToString(System.Globalization.CultureInfo.InvariantCulture));

            UserActivityEssay essay = activity as UserActivityEssay;
            if (essay != null)
            {
                this.PostBody += "&type=" + UserActivityEssay.UserActivityType + "&text=" + Uri.EscapeDataString(essay.Text);
            }
            else if (activity is UserActivityDictionarySearch)
            {
                this.PostBody += "&type=" + UserActivityDictionarySearch.UserActivityType;
            }
            else
            {
                throw new ArgumentException("The activity type is not supported at this time", "activity");
            }
        }
    }
}

// <copyright file="UserProfile.cs" company="University of Murcia">
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

namespace TellOP.Api
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using DataModels;
    using Newtonsoft.Json;
    using Xamarin.Auth;

    /// <summary>
    /// A class accessing the "User profile" API on the TellOP server.
    /// </summary>
    public class UserProfile : OAuth2Api
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfile"/> class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/> class to use to store the OAuth 2.0 account
        /// credentials.</param>
        public UserProfile(Account account)
            : base(Config.TellOPConfiguration.GetEndpointAsUri("TellOP.API.UserProfile"), HttpMethod.Get, account)
        {
        }

        /// <summary>
        /// Call the API endpoint and return the object representation of the API response.
        /// </summary>
        /// <returns>A <see cref="Task{User}"/> containing the object representation of the API response as its
        /// result.</returns>
        public async Task<User> CallEndpointAsObjectAsync()
        {
            return JsonConvert.DeserializeObject<User>(await this.CallEndpointAsync().ConfigureAwait(false));
        }
    }
}

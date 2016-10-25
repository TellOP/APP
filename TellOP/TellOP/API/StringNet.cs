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
// <author>Mattia Zago</author>
// <author>Alessandro Menti</author>

namespace TellOP.Api
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Xamarin.Auth;

    /// <summary>
    /// A class accessing the StringNet API on the TellOP server.
    /// </summary>
    public class StringNet : OAuth2Api
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringNet"/> class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/> class to use to store the OAuth 2.0 account
        /// credentials.</param>
        public StringNet(Account account)
            : base(Config.TellOPConfiguration.GetEndpointAsUri("TellOP.API.StringNet"), HttpMethod.Get, account)
        {
            throw new NotImplementedException("Calling this constructor without passing the query is not supported");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringNet"/> class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/> class to use to store the OAuth 2.0 account
        /// credentials.</param>
        /// <param name="query">The query to perform.</param>
        public StringNet(Account account, string query)
            : base(new Uri(Config.TellOPConfiguration.GetEndpoint("TellOP.API.StringNet") + "?q=" + Uri.EscapeDataString(query)), HttpMethod.Get, account)
        {
        }

        /// <summary>
        /// Call the API endpoint and return the object representation of the API response.
        /// </summary>
        /// <returns>A <see cref="Task{IList}"/> containing the object representation of the API response as its
        /// result.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list inside a Task")]
        public async Task<IList<StringNet>> CallEndpointAsObjectAsync()
        {
            return JsonConvert.DeserializeObject<List<StringNet>>(await this.CallEndpointAsync().ConfigureAwait(false));
        }
    }
}

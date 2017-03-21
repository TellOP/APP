// <copyright file="CEFRTagger.cs" company="University of Murcia">
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

namespace TellOP.Api
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Threading.Tasks;
    using DataModels.ApiModels;
    using Newtonsoft.Json;
    using Xamarin.Auth;

    /// <summary>
    /// A class accessing the German tokenizer on the TellOP server.
    /// </summary>
    public class CEFRTagger : OAuth2Api
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CEFRTagger"/> class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/> class to use to store the OAuth 2.0 account
        /// credentials.</param>
        public CEFRTagger(Account account)
            : base(Config.TellOPConfiguration.GetEndpointAsUri("TellOP.API.CEFRTagger"), HttpMethod.Get, account)
        {
            throw new NotImplementedException("Calling this constructor without passing the search term is not supported");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CEFRTagger"/> class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/> class to use to store the OAuth 2.0 account
        /// credentials.</param>
        /// <param name="searchTerm">Term</param>
        public CEFRTagger(Account account, string searchTerm)
            : base(new Uri(Config.TellOPConfiguration.GetEndpoint("TellOP.API.CEFRTagger") + "?q=" + Uri.EscapeDataString(Tools.StringUtils.Base64Encode(searchTerm))), HttpMethod.Get, account)
        {
        }

        /// <summary>
        /// Call the API endpoint and return the object representation of the API response.
        /// </summary>
        /// <returns>A <see cref="Task{IList}"/> containing the object representation of the API response as its
        /// result.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list inside a Task")]
        public async Task<IList<CEFRWord>> CallEndpointAsObjectAsync()
        {
            var r = await this.CallEndpointAsync().ConfigureAwait(false);
            var m = JsonConvert.DeserializeObject<List<CEFRWord>>(r);
            return m;
        }
    }
}

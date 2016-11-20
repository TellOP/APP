// <copyright file="AdelexApi.cs" company="University of Murcia">
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
    using System.Threading.Tasks;
    using DataModels.ApiModels.Adelex;
    using Newtonsoft.Json;
    using Xamarin.Auth;

    /// <summary>
    /// A class accessing the Adelex API on the TellOP server.
    /// </summary>
    public class AdelexApi : OAuth2Api
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdelexApi"/> class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/> class to use to store the OAuth 2.0 account
        /// credentials.</param>
        public AdelexApi(Account account)
            : base(Config.TellOPConfiguration.GetEndpointAsUri("TellOP.API.Adelex"), HttpMethod.Post, account)
        {
            throw new NotImplementedException("Calling this constructor without passing the text and order is not supported");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdelexApi"/> class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/> class to use to store the OAuth 2.0 account
        /// credentials.</param>
        /// <param name="text">The text to analyze.</param>
        /// <param name="order">The order the words in the text should be sorted as.</param>
        public AdelexApi(Account account, string text, AdelexOrder order)
            : base(Config.TellOPConfiguration.GetEndpointAsUri("TellOP.API.Adelex"), HttpMethod.Post, account, "text=" + Uri.EscapeDataString(text) + "&order=" + AdelexOrderExtensions.GetHttpParam(order))
        {
        }

        /// <summary>
        /// Call the API endpoint and return the object representation of the API response.
        /// </summary>
        /// <returns>A <see cref="Task{AdelexResultEntry}"/> containing the object representation of the API response
        /// as its result.</returns>
        public async Task<AdelexResultEntry> CallEndpointAsObjectAsync()
        {
            return JsonConvert.DeserializeObject<AdelexResultEntry>(await this.CallEndpointAsync().ConfigureAwait(false));
        }
    }
}

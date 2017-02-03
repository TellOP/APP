// <copyright file="CollinsDictionary.cs" company="University of Murcia">
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
    using DataModels.ApiModels.Collins;
    using DataModels.Enums;
    using Newtonsoft.Json;
    using Xamarin.Auth;

    /// <summary>
    /// A class accessing the Collins Dictionary API on the TellOP server.
    /// </summary>
    public class CollinsDictionary : OAuth2Api
    {
        /// <summary>
        /// Search language
        /// </summary>
        private SupportedLanguage language;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollinsDictionary"/> class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/> class to use to store the OAuth 2.0 account
        /// credentials.</param>
        public CollinsDictionary(Account account)
            : base(Config.TellOPConfiguration.GetEndpointAsUri("TellOP.API.CollinsDictionary." + SupportedLanguage.English.ToLCID()), HttpMethod.Get, account)
        {
            throw new NotImplementedException("Calling this constructor without passing the search term is not supported");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollinsDictionary"/> class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/> class to use to store the OAuth 2.0 account
        /// credentials.</param>
        /// <param name="searchTerm">The word to search for.</param>
        public CollinsDictionary(Account account, string searchTerm)
            : this(account, searchTerm, SupportedLanguage.English)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollinsDictionary"/> class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/> class to use to store the OAuth 2.0 account
        /// credentials.</param>
        /// <param name="language">Language for the search</param>
        /// <param name="searchTerm">The word to search for.</param>
        public CollinsDictionary(Account account, string searchTerm, SupportedLanguage language)
            : base(new Uri(Config.TellOPConfiguration.GetEndpoint("TellOP.API.CollinsDictionary." + language.ToLCID()) + "?q=" + Uri.EscapeDataString(searchTerm)), HttpMethod.Get, account)
        {
            this.language = language;
        }

        /// <summary>
        /// Call the API endpoint and return the object representation of the API response.
        /// </summary>
        /// <returns>A <see cref="Task{CollinsJSONEnglishDictionary}"/> containing the object representation of the API
        /// response as its result.</returns>
        public async Task<CollinsJsonDictionary> CallEndpointAsObjectAsync()
        {
            try
            {
                var k = await this.CallEndpointAsync().ConfigureAwait(false);
                var m = JsonConvert.DeserializeObject<CollinsJsonDictionary>(k);
                return m;
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex =>
                {
                    Tools.Logger.Log("CollinsJsonDictionary:CallEndpointAsObjectAsync", ex);
                    return true;
                });
                throw ae;
            }
        }

        /// <summary>
        /// Call the API endpoint and return a list of words.
        /// </summary>
        /// <returns>A <see cref="Task"/> object having as result an <see cref="IList{CollinsWord}"/> which, in turn,
        /// contains the list of words found during the search.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list inside a Task")]
        public async Task<IList<CollinsWord>> CallEndpointAsCollinsWord()
        {
            CollinsJsonDictionary apiResult = await this.CallEndpointAsObjectAsync().ConfigureAwait(false);

            // TODO: we assume that the first definition is the right one
            if (apiResult.Results.Count > 0)
            {
                CollinsDictionaryGetEntry entryEndpoint = new CollinsDictionaryGetEntry(this.OAuthAccount, apiResult.Results[0].EntryId, this.language);
                return await entryEndpoint.CallEndpointAsCollinsWord().ConfigureAwait(false);
            }
            else
            {
                return new List<CollinsWord>();
            }
        }
    }
}

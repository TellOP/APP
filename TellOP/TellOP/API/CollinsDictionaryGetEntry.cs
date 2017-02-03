// <copyright file="CollinsDictionaryGetEntry.cs" company="University of Murcia">
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
    /// A class accessing the Collins Dictionary "Get entry" API on the TellOP server.
    /// </summary>
    public class CollinsDictionaryGetEntry : OAuth2Api
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollinsDictionaryGetEntry"/> class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/> class to use to store the OAuth 2.0 account
        /// credentials.</param>
        public CollinsDictionaryGetEntry(Account account)
            : base(Config.TellOPConfiguration.GetEndpointAsUri("TellOP.API.CollinsDictionaryGetEntry." + SupportedLanguage.English.ToLCID()), HttpMethod.Get, account)
        {
            throw new NotImplementedException("Calling this constructor without passing the definition ID is not supported");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollinsDictionaryGetEntry"/> class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/> class to use to store the OAuth 2.0 account
        /// credentials.</param>
        /// <param name="wordId">The word ID of the definition to be retrieved.</param>
        public CollinsDictionaryGetEntry(Account account, string wordId)
            : this(account, wordId, SupportedLanguage.English)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollinsDictionaryGetEntry"/> class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/> class to use to store the OAuth 2.0 account
        /// credentials.</param>
        /// <param name="wordId">The word ID of the definition to be retrieved.</param>
        /// <param name="language">Language Enum</param>
        public CollinsDictionaryGetEntry(Account account, string wordId, SupportedLanguage language)
            : base(new Uri(Config.TellOPConfiguration.GetEndpoint("TellOP.API.CollinsDictionaryGetEntry." + language.ToLCID()) + "?entryId=" + Uri.EscapeDataString(wordId)), HttpMethod.Get, account)
        {
        }

        /// <summary>
        /// Call the API endpoint and return the object representation of the API response.
        /// </summary>
        /// <returns>An instance of <see cref="CollinsJsonDictionaryEntry"/> containing the object representation
        /// of the API response as its result.</returns>
        public async Task<CollinsJsonDictionaryEntry> CallEndpointAsObjectAsync()
        {
            return JsonConvert.DeserializeObject<CollinsJsonDictionaryEntry>(await this.CallEndpointAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Call the API endpoint and return a <see cref="CollinsWord"/> object representing the searched word.
        /// </summary>
        /// <returns>A <see cref="Task{IList}"/> containing the object representation of the API response as its
        /// result.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification="Need to return a list inside a Task")]
        public async Task<IList<CollinsWord>> CallEndpointAsCollinsWord()
        {
            CollinsJsonDictionaryEntry apiResult = await this.CallEndpointAsObjectAsync().ConfigureAwait(false);
            List<CollinsWord> resultList = new List<CollinsWord>();

            foreach (CollinsJsonDictionaryEntryContentEntry entry in apiResult.Content.Entries)
            {
                resultList.Add(new CollinsWord(entry, apiResult.Id, apiResult.Label, apiResult.Url));
            }

            return resultList;
        }
    }
}

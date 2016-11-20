// <copyright file="Tips.cs" company="University of Murcia">
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
    using System.Globalization;
    using System.Net.Http;
    using System.Threading.Tasks;
    using DataModels.ApiModels;
    using DataModels.Enums;
    using Newtonsoft.Json;
    using Xamarin.Auth;

    /// <summary>
    /// A class accessing the Tips API on the TellOP server.
    /// </summary>
    public class Tips : OAuth2Api
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tips"/> class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/> class to use to store the OAuth 2.0 account
        /// credentials.</param>
        public Tips(Account account)
            : base(Config.TellOPConfiguration.GetEndpointAsUri("TellOP.API.Tips"), HttpMethod.Get, account)
        {
            throw new NotImplementedException("Calling this constructor without passing the (optional) maximum number of tips, language and CEFR level is not supported");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tips"/> class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/> class to use to store the OAuth 2.0 account
        /// credentials.</param>
        /// <param name="language">The language for the tips.</param>
        /// <param name="cefrLevel">The CEFR level for the tips.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="cefrLevel"/> is
        /// <see cref="LanguageLevelClassification.Unknown"/>.</exception>
        public Tips(Account account, SupportedLanguage language, LanguageLevelClassification cefrLevel)
            : base(new Uri(Config.TellOPConfiguration.GetEndpoint("TellOP.API.Tips") + "?language=" + new SupportedLanguageToLcidConverter().Convert(language, typeof(string), null, CultureInfo.InvariantCulture) + "&cefrLevel=" + (string)new LanguageLevelClassificationToHtmlParamConverter().Convert(cefrLevel, typeof(string), null, CultureInfo.InvariantCulture)), HttpMethod.Get, account)
        {
            if (cefrLevel == LanguageLevelClassification.Unknown)
            {
                throw new ArgumentOutOfRangeException("cefrLevel", "The CEFR level can not be UNKNOWN");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tips"/> class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/> class to use to store the OAuth 2.0 account
        /// credentials.</param>
        /// <param name="language">The language for the tips.</param>
        /// <param name="cefrLevel">The CEFR level for the tips.</param>
        /// <param name="maxNumber">The maximum number of tips to show.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxNumber"/> is less than zero or
        /// if <paramref name="cefrLevel"/> is <see cref="LanguageLevelClassification.Unknown"/>.</exception>
        public Tips(Account account, SupportedLanguage language, LanguageLevelClassification cefrLevel, int maxNumber)
            : base(new Uri(Config.TellOPConfiguration.GetEndpoint("TellOP.API.Tips") + "?maxNum=" + maxNumber.ToString(CultureInfo.InvariantCulture) + "&language=" + (string)new SupportedLanguageToLcidConverter().Convert(language, typeof(string), null, CultureInfo.InvariantCulture) + "&cefrLevel=" + (string)new LanguageLevelClassificationToHtmlParamConverter().Convert(cefrLevel, typeof(string), null, CultureInfo.InvariantCulture)), HttpMethod.Get, account)
        {
            if (maxNumber < 1)
            {
                throw new ArgumentOutOfRangeException("maxNumber", "The maximum number of tips must be greater than zero");
            }

            if (cefrLevel == LanguageLevelClassification.Unknown)
            {
                throw new ArgumentOutOfRangeException("cefrLevel", "The CEFR level can not be UNKNOWN");
            }
        }

        /// <summary>
        /// Call the API endpoint and return the object representation of the API response.
        /// </summary>
        /// <returns>A <see cref="Task{IList}"/> containing the object representation of the API response as its result.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list inside a Task")]
        public async Task<IList<Tip>> CallEndpointAsObjectAsync()
        {
            return JsonConvert.DeserializeObject<List<Tip>>(await this.CallEndpointAsync().ConfigureAwait(false));
        }
    }
}

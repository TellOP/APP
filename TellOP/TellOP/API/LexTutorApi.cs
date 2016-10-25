// <copyright file="LexTutorApi.cs" company="University of Murcia">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Threading.Tasks;
    using DataModels.APIModels.LexTutor;
    using Newtonsoft.Json;
    using Xamarin.Auth;

    /// <summary>
    /// A class accessing the LexTutor API on the TellOP server.
    /// </summary>
    public class LexTutorApi : OAuth2Api
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LexTutorApi"/> class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/> class to use to store the OAuth 2.0 account
        /// credentials.</param>
        public LexTutorApi(Account account)
            : base(Config.TellOPConfiguration.GetEndpointAsUri("TellOP.API.LexTutorAPIController"), HttpMethod.Post, account)
        {
            throw new NotImplementedException("Calling this constructor without passing the search term is not supported");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LexTutorApi"/> class.
        /// </summary>
        /// <param name="account">The instance of the <see cref="Account"/> class to use to store the OAuth 2.0 account
        /// credentials.</param>
        /// <param name="textName">The name of the text.</param>
        /// <param name="textInput">The text to analyze.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="textName"/> is longer than 40 characters or
        /// <paramref name="textInput"/> is longer than 400000 characters.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "The non-nullness of textName/textInput is checked in the invocation of the base constructor")]
        public LexTutorApi(Account account, string textName, string textInput)
            : base(Config.TellOPConfiguration.GetEndpointAsUri("TellOP.API.LexTutorAPIController"), HttpMethod.Post, account, "text_name=" + Uri.EscapeDataString(Preconditions.CheckNotNull(textName)) + "&text_input=" + Uri.EscapeDataString(Preconditions.CheckNotNull(textInput)))
        {
            if (textName.Length > 40)
            {
                throw new ArgumentException("The text name can not be longer than 40 characters", "textName");
            }

            if (textInput.Length > 400000)
            {
                throw new ArgumentException("The text can not be longer than 400000 characters", "textName");
            }
        }

        /// <summary>
        /// Call the API endpoint and return the object representation of the API response.
        /// </summary>
        /// <returns>A <see cref="Task{LexTutorResultEntry}"/> containing the object representation of the API response
        /// as its result.</returns>
        public async Task<LexTutorResultEntry> CallEndpointAsObjectAsync()
        {
            return JsonConvert.DeserializeObject<LexTutorResultEntry>(await this.CallEndpointAsync().ConfigureAwait(false));
        }
    }
}

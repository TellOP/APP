// <copyright file="TipsDataModel.cs" company="University of Murcia">
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

namespace TellOP.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Api;
    using ApiModels;
    using Enums;

    /// <summary>
    /// Static functions used by data models for application tips management.
    /// </summary>
    public static class TipsDataModel
    {
        /// <summary>
        /// Gets a single tip asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task{Tip}"/> object containing the tip as its result.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Since the operation is asynchronous, using a method is the best way")]
        public static async Task<Tip> GetSingleTipAsync()
        {
            // FIXME: allow choosing the correct language and language level
            Tips tipsEndpoint = new Tips(App.OAuth2Account, SupportedLanguage.English, LanguageLevelClassification.B1);
            IList<Tip> applicationTips = await Task.Run(async () => await tipsEndpoint.CallEndpointAsObjectAsync());

            if (applicationTips.Count == 0)
            {
                return null;
            }

            Random rnd = new Random();
            return applicationTips[rnd.Next(applicationTips.Count - 1)];
        }
    }
}

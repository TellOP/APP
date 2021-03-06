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
    using DataModels.Enums;

    /// <summary>
    /// Static functions used by data models for application tips management.
    /// </summary>
    public static class TipsDataModel
    {
        /// <summary>
        /// Caches the tips in order to avoid redownloading them.
        /// </summary>
        private static IList<Tip> tipsCache = new List<Tip>();

        /// <summary>
        /// Gets a single tip asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task{Tip}"/> object containing the tip as its result.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Since the operation is asynchronous, using a method is the best way")]
        public static async Task<Tip> GetSingleTipAsync()
        {
            if (TipsDataModel.tipsCache.Count == 0)
            {
                // FIXME: allow choosing the correct language and language level
                Tips tipsEndpoint = new Tips(App.OAuth2Account, SupportedLanguage.English, LanguageLevelClassification.B1);
                TipsDataModel.tipsCache = await Task.Run(async () => await tipsEndpoint.CallEndpointAsObjectAsync());
            }

            int choosen = new Random().Next(TipsDataModel.tipsCache.Count - 1);

            if (TipsDataModel.tipsCache.Count == 0)
            {
                Tools.Logger.Log("TipsController", "Empty list, something went wrong.");
                return new Tip()
                {
                    Id = -1,
                    Text = "An error occurred. Please report this message to the dev team.",
                };
            }
            else
            {
                return TipsDataModel.tipsCache[choosen];
            }
        }
    }
}

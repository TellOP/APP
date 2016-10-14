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
// <author>Mattia Zago</author>
// <author>Alessandro Menti</author>

namespace TellOP.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using API;
    using APIModels;
    using Enums;
    using Tools;

    /// <summary>
    /// The viewmodel for the tips tab.
    /// </summary>
    public class TipsDataModel
    {
        /// <summary>
        /// A locally cached list of tips.
        /// </summary>
        private IList<Tip> _appTips = null;

        /// <summary>
        /// Gets a locally cached list of tips.
        /// </summary>
        public IReadOnlyList<Tip> TipList
        {
            get
            {
                return new ReadOnlyCollection<Tip>(this._appTips);
            }
        }

        /// <summary>
        /// Gets a single tip chosen at random.
        /// </summary>
        /// <returns>A single tip chosen at random from all the available ones.</returns>
        public async Task<Tip> GetSingleRandom()
        {
            await this._getTipsIfNeeded();

            if (this._appTips == null)
            {
                this._appTips = new List<Tip>()
                        {
                            { new Tip() { Text = "Unable to retrieve the tips now. Please retry later.", ID = 0 } }
                        };
            }

            Random rnd = new Random();
            return this._appTips[rnd.Next(this._appTips.Count - 1)];
        }

        /// <summary>
        /// Gets several random tips at once.
        /// </summary>
        /// <param name="num">The number of tips to retrieve.</param>
        /// <returns>An <see cref="IReadOnlyList{Tip}"/> containing <paramref name="num"/> tips. If the total number of
        /// available tips is less than or equal to <paramref name="num" />, the returned list will contain all
        /// available tips.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list inside a Task")]
        public async Task<IReadOnlyList<Tip>> GetMultipleRandom(int num)
        {
            await this._getTipsIfNeeded();

            if (num >= this._appTips.Count)
            {
                return new ReadOnlyCollection<Tip>(this._appTips);
            }

            List<Tip> result = new List<Tip>();
            Random rnd = new Random();
            for (int i = 0; i < num; ++i)
            {
                int rndElemIndex;
                do
                {
                    rndElemIndex = rnd.Next(this._appTips.Count - 1);
                }
                while (!result.Contains(this._appTips[rndElemIndex]));
                result.Add(this._appTips[rndElemIndex]);
            }

            return new ReadOnlyCollection<Tip>(result);
        }

        /// <summary>
        /// Populates the tips list if it is empty.
        /// </summary>
        /// <returns>True if the operation is performed correctly.</returns>
        private async Task<bool> _getTipsIfNeeded()
        {
            if (this._appTips == null || this._appTips.Count == 0)
            {
                try
                {
                    // TODO Choose the level and supported language
                    Tips tipsEndpoint = new Tips(App.OAuth2Account, SupportedLanguage.English, LanguageLevelClassification.B1);
                    this._appTips = await tipsEndpoint.CallEndpointAsObjectAsync();
                    return this._appTips != null;
                }
                catch (UnsuccessfulAPICallException ex)
                {
                    Logger.Log(this, "_getTipsIfNeeded method", ex);
                    return false;
                }
                catch (Exception ex)
                {
                    Logger.Log(this, "_getTipsIfNeeded method", ex);
                    return false;
                }
                finally
                {
                    // Hardcoded message for tips
                    if (this._appTips == null)
                    {
                        this._appTips = new List<Tip>()
                        {
                            { new Tip() { Text = "Unable to retrieve the tips now. Please retry later.", ID = 0 } }
                        };
                    }
                }
            }

            return this._appTips != null;
        }
    }
}

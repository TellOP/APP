// <copyright file="CollinsMultipleWordsWrapper.cs" company="University of Murcia">
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

namespace TellOP.ViewModels.Collins
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using API;
    using DataModels.APIModels.Collins;
    using Xamarin.Forms;

    /// <summary>
    /// Render a single Collins entry
    /// </summary>
    public class CollinsMultipleWordsWrapper : StackLayout
    {
        /// <summary>
        /// Dictionary with colors background. Used for separate multiple definitions
        /// </summary>
        public static Dictionary<int, Color> _bkgColors = new Dictionary<int, Color>
        {
            { 0, Color.FromHex("FFE0B2") },
            { 1, Color.FromHex("FFCC80") },
            { 2, Color.FromHex("FFB74D") },
            { 3, Color.FromHex("FFA726") },
            { 4, Color.FromHex("FF9800") },
            { 5, Color.FromHex("FB8C00") },
            { 6, Color.FromHex("F57C00") },
        };

        private string _entryID;

        private bool _needToRetrieveResult = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollinsMultipleWordsWrapper"/> class.
        /// </summary>
        /// <param name="collins_entry_id">Collins Entry_ID param</param>
        public CollinsMultipleWordsWrapper(string collins_entry_id)
            : base()
        {
            this.BackgroundColor = Color.Fuchsia;
            this.Padding = 1;
            this.Margin = 1;
            this._entryID = collins_entry_id;

            this.HorizontalOptions = LayoutOptions.FillAndExpand;
            this.VerticalOptions = LayoutOptions.Start;
        }

        /// <summary>
        /// Populate the stacklayout on the first call.
        /// </summary>
        /// <returns>True if everything was correctly executed.</returns>
        internal async Task<bool> Populate()
        {
            if (this._needToRetrieveResult)
            {
                return await this._retrieveResult();
            }

            return false;
        }

        /// <summary>
        /// Force the retrival of the remote results.
        /// </summary>
        /// <returns>True idd everything works.</returns>
        private async Task<bool> _retrieveResult()
        {
            try
            {
                CollinsEnglishDictionaryGetEntry dictionaryEntryAPI = new CollinsEnglishDictionaryGetEntry(App.OAuth2Account, this._entryID);
                List<CollinsWord> dictionaryEntryResult = await dictionaryEntryAPI.CallEndpointAsCollinsWord();

                int i = 0;
                foreach (CollinsWord word in dictionaryEntryResult)
                {
                    CollinsWordView tmpView = new CollinsWordView(word);
                    tmpView.BackgroundColor = CollinsMultipleWordsWrapper._bkgColors[++i % 6];
                    this.Children.Add(new CollinsWordView(word));
                }

                this._needToRetrieveResult = false;
                return true;
            }
            catch (UnsuccessfulAPICallException ex)
            {
                Tools.Logger.Log(this, "_retrieveResult method", ex);
                return false;
            }
            catch (Exception ex)
            {
                Tools.Logger.Log(this, "_retrieveResult method", ex);
                return false;
            }
        }
    }
}

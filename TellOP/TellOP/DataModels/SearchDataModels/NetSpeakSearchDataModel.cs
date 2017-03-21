// <copyright file="NetSpeakSearchDataModel.cs" company="University of Murcia">
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
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Api;
    using ApiModels;
    using Nito.AsyncEx;
    using System;

    /// <summary>
    /// The data model for featured exercises.
    /// </summary>
    public class NetSpeakSearchDataModel : ISearchDataModel
    {
        /// <summary>
        /// A read-only list of NetSpeak dictionary search results.
        /// </summary>
        private INotifyTaskCompletion<string> _searchResultsNetSpeakPreceding;

        /// <summary>
        /// A read-only list of NetSpeak dictionary search results.
        /// </summary>
        private INotifyTaskCompletion<string> _searchResultsNetSpeakFollowing;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetSpeakSearchDataModel"/> class.
        /// </summary>
        public NetSpeakSearchDataModel()
        {
            this.SearchForWord(string.Empty);
        }

        /// <summary>
        /// Fired when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets a read-only list of NetSpeak dictionary search results for the word typed in the search box.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a collection of IWords")]
        public INotifyTaskCompletion<string> SearchResultsNetSpeakPreceding
        {
            get
            {
                return this._searchResultsNetSpeakPreceding;
            }

            private set
            {
                this._searchResultsNetSpeakPreceding = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SearchResultsNetSpeakPreceding"));
            }
        }

        /// <summary>
        /// Gets a read-only list of NetSpeak dictionary search results for the word typed in the search box.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a collection of IWords")]
        public INotifyTaskCompletion<string> SearchResultsNetSpeakFollowing
        {
            get
            {
                return this._searchResultsNetSpeakFollowing;
            }

            private set
            {
                this._searchResultsNetSpeakFollowing = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SearchResultsNetSpeakFollowing"));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the search bar is running or not.
        /// TODO: test binding
        /// </summary>
        public bool IsSearchEnabled
        {
            get
            {
                return this.SearchResultsNetSpeakFollowing.IsCompleted &&
                       this.SearchResultsNetSpeakPreceding.IsCompleted;
            }
        }

        /// <summary>
        /// Searches for a given word.
        /// </summary>
        /// <param name="word">The word to search for.</param>
        public void SearchForWord(string word)
        {
            // TODO: the dictionary search is recorded in the first call. Perhaps find a better design?
            this.SearchResultsNetSpeakPreceding = NotifyTaskCompletion.Create(SearchForWordNetSpeakPrecedingAsync(word));
            this.SearchResultsNetSpeakFollowing = NotifyTaskCompletion.Create(SearchForWordNetSpeakFollowingAsync(word));
        }

        /// <summary>
        /// Searches for a given word asynchronously in the NetSpeak dictionary.
        /// </summary>
        /// <param name="word">The word to search for.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private static async Task<string> SearchForWordNetSpeakPrecedingAsync(string word)
        {
            try
            {
                Tools.Logger.Log("SearchForWordNetSpeakPrecedingAsync", "Start search");
                if (string.IsNullOrWhiteSpace(word))
                {
                    Tools.Logger.Log("SearchForWordNetSpeakPrecedingAsync", "Null or whitespace. Return an empty list");
                    return string.Empty;
                }
                NetSpeakPreceding netSpeakPrecedingEndpoint = new NetSpeakPreceding(App.OAuth2Account, word, 50);
                IList<NetSpeak> netSpeakResult = await Task.Run(async () => await netSpeakPrecedingEndpoint.CallEndpointAsObjectAsync());

                StringBuilder sb = new StringBuilder();

                // Parse the result - TODO: Find a better way (maybe a linq query?)
                foreach (string x in netSpeakResult.Select<NetSpeak, string>(ns => ns.Word))
                {
                    // This removes all the special characters and join the result in a unique string.
                    // The result may contains more than one word, in this case we want only one record with 2 words.
                    // The ' char is needed in order to include words such as "don't".
                    string parsedResult = string.Join(" ", Regex.Matches(x, "[\\w'-]+").Cast<Match>().Select(m => m.Value));
                    if (!string.IsNullOrEmpty(parsedResult))
                    {
                        sb.AppendLine(" - " + parsedResult);
                    }
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                Tools.Logger.Log("NetSpeakSearchDataModel", "Something happened", ex);
                return "Unknown error @158: " + ex;
            }
        }

        /// <summary>
        /// Searches for a given word asynchronously in the NetSpeak dictionary.
        /// </summary>
        /// <param name="word">The word to search for.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private static async Task<string> SearchForWordNetSpeakFollowingAsync(string word)
        {
            try
            {
                Tools.Logger.Log("SearchForWordNetSpeakFollowingAsync", "Start search");
                if (string.IsNullOrWhiteSpace(word))
                {
                    Tools.Logger.Log("SearchForWordNetSpeakFollowingAsync", "Null or whitespace. Return an empty string");
                    return string.Empty;
                }

                NetSpeakFollowing netSpeakFollowingEndpoint = new NetSpeakFollowing(App.OAuth2Account, word, 50);
                IList<NetSpeak> netSpeakResult = await Task.Run(async () => await netSpeakFollowingEndpoint.CallEndpointAsObjectAsync());

                StringBuilder sb = new StringBuilder();

                // Parse the result - TODO: Find a better way (maybe a linq query?)
                foreach (string x in netSpeakResult.Select<NetSpeak, string>(ns => ns.Word))
                {
                    // This removes all the special characters and join the result in a unique string.
                    // The result may contains more than one word, in this case we want only one record with 2 words.
                    // The ' char is needed in order to include words such as "don't".
                    string parsedResult = string.Join(" ", Regex.Matches(x, "[\\w'-]+").Cast<Match>().Select(m => m.Value));
                    if (!string.IsNullOrEmpty(parsedResult))
                    {
                        sb.AppendLine(" - " + parsedResult);
                    }
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                Tools.Logger.Log("NetSpeakSearchDataModel", "Something happened", ex);
                return "Unknown error @201: " + ex;
            }
        }
    }
}

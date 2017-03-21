// <copyright file="StringNetSearchDataModel.cs" company="University of Murcia">
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
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Api;
    using ApiModels.StringNet;
    using Nito.AsyncEx;

    /// <summary>
    /// The data model for featured exercises.
    /// </summary>
    public class StringNetSearchDataModel : ISearchDataModel
    {
        /// <summary>
        /// A read-only list of StringNet dictionary search results.
        /// </summary>
        private INotifyTaskCompletion<StringNetSplit> _searchResultsStringNet;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringNetSearchDataModel"/> class.
        /// </summary>
        public StringNetSearchDataModel()
        {
            this.SearchForWord(string.Empty);
        }

        /// <summary>
        /// Fired when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets a read-only list of StringNet collocation search results for the word typed in the search box.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a collection of IWords")]
        public INotifyTaskCompletion<StringNetSplit> SearchResultsStringNet
        {
            get
            {
                return this._searchResultsStringNet;
            }

            private set
            {
                this._searchResultsStringNet = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SearchResultsStringNet"));
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
                return this.SearchResultsStringNet.IsCompleted;
            }
        }

        /// <summary>
        /// Searches for a given word.
        /// </summary>
        /// <param name="word">The word to search for.</param>
        public void SearchForWord(string word)
        {
            // TODO: the dictionary search is recorded in the first call. Perhaps find a better design?
            this.SearchResultsStringNet = NotifyTaskCompletion.Create(SearchForWordStringNetAsync(word));
        }

        /// <summary>
        /// Searches for a given word asynchronously in the Stringnet database.
        /// </summary>
        /// <param name="word">The word to search for.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private static async Task<StringNetSplit> SearchForWordStringNetAsync(string word)
        {
            Tools.Logger.Log("SearchForWordStringNetAsync", "Start search");
            if (string.IsNullOrWhiteSpace(word))
            {
                Tools.Logger.Log("SearchForWordStringNetAsync", "Null or whitespace. Return an empty list");
                return null;
            }

            StringNetApi snapi = new StringNetApi(App.OAuth2Account, word);
            StringNetSplit result = await Task.Run(async () => await snapi.CallEndpointAsObjectAsync());
            Tools.Logger.Log("SearchForWordStringNetAsync", "Data received");
            return result;
        }
    }
}

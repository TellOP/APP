// <copyright file="CollinsSearchDataModel.cs" company="University of Murcia">
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
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Api;
    using ApiModels.Collins;
    using Nito.AsyncEx;

    /// <summary>
    /// The data model for featured exercises.
    /// </summary>
    public class CollinsSearchDataModel : ISearchDataModel
    {
        /// <summary>
        /// The database ID of the British English "dictionary search" activity.
        /// </summary>
        private const int BritishEnglishDictionarySearchID = 90;

        /// <summary>
        /// A read-only list of Collins dictionary search results.
        /// </summary>
        private INotifyTaskCompletion<ReadOnlyObservableCollection<IWord>> _searchResultsCollins;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollinsSearchDataModel"/> class.
        /// </summary>
        public CollinsSearchDataModel()
        {
            this.SearchForWord(string.Empty);
        }

        /// <summary>
        /// Fired when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets a read-only list of Collins dictionary search results for the word typed in the search box.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a collection of IWords")]
        public INotifyTaskCompletion<ReadOnlyObservableCollection<IWord>> SearchResultsCollins
        {
            get
            {
                return this._searchResultsCollins;
            }

            private set
            {
                this._searchResultsCollins = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SearchResultsCollins"));
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
                return this.SearchResultsCollins.IsCompleted;
            }
        }

        /// <summary>
        /// Searches for a given word.
        /// </summary>
        /// <param name="word">The word to search for.</param>
        public void SearchForWord(string word)
        {
            // TODO: the dictionary search is recorded in the first call. Perhaps find a better design?
            this.SearchResultsCollins = NotifyTaskCompletion.Create(SearchForWordCollinsAsync(word));
        }

        /// <summary>
        /// Searches for a given word asynchronously in the Collins dictionary.
        /// </summary>
        /// <param name="word">The word to search for.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private static async Task<ReadOnlyObservableCollection<IWord>> SearchForWordCollinsAsync(string word)
        {
            Tools.Logger.Log("SearchForWordCollinsAsync", "Start search");
            if (string.IsNullOrWhiteSpace(word))
            {
                Tools.Logger.Log("SearchForWordCollinsAsync", "Null or whitespace. Return an empty list");
                return new ReadOnlyObservableCollection<IWord>(new ObservableCollection<IWord>());
            }

            CollinsDictionary collinsEndpoint = new CollinsDictionary(App.OAuth2Account, word);
            IList<CollinsWord> collinsResult = await Task.Run(async () => await collinsEndpoint.CallEndpointAsCollinsWord());
            Tools.Logger.Log("SearchForWordCollinsAsync", "Data received");
            return new ReadOnlyObservableCollection<IWord>(new ObservableCollection<IWord>(collinsResult));
        }
    }
}

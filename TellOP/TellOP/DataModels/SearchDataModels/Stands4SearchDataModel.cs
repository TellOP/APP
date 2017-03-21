// <copyright file="Stands4SearchDataModel.cs" company="University of Murcia">
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
    using ApiModels.Stands4;
    using Nito.AsyncEx;

    /// <summary>
    /// The data model for featured exercises.
    /// </summary>
    public class Stands4SearchDataModel : ISearchDataModel
    {
        /// <summary>
        /// A read-only list of Stands4 dictionary search results.
        /// </summary>
        private INotifyTaskCompletion<ReadOnlyObservableCollection<IWord>> _searchResultsStands4;

        /// <summary>
        /// Initializes a new instance of the <see cref="Stands4SearchDataModel"/> class.
        /// </summary>
        public Stands4SearchDataModel()
        {
            this.SearchForWord(string.Empty);
        }

        /// <summary>
        /// Fired when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets a read-only list of Stands4 dictionary search results for the word typed in the search box.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a collection of IWords")]
        public INotifyTaskCompletion<ReadOnlyObservableCollection<IWord>> SearchResultsStands4
        {
            get
            {
                return this._searchResultsStands4;
            }

            private set
            {
                this._searchResultsStands4 = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SearchResultsStands4"));
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
                return this.SearchResultsStands4.IsCompleted;
            }
        }

        /// <summary>
        /// Searches for a given word.
        /// </summary>
        /// <param name="word">The word to search for.</param>
        public void SearchForWord(string word)
        {
            // TODO: the dictionary search is recorded in the first call. Perhaps find a better design?
            this.SearchResultsStands4 = NotifyTaskCompletion.Create(SearchForWordStands4Async(word));
        }

        /// <summary>
        /// Records the dictionary search, then searches for a given word asynchronously in the Stands4 dictionary.
        /// </summary>
        /// <param name="word">The word to search for.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private static async Task<ReadOnlyObservableCollection<IWord>> SearchForWordStands4Async(string word)
        {
            Tools.Logger.Log("SearchForWordStands4Async", "Start search");
            if (string.IsNullOrWhiteSpace(word))
            {
                Tools.Logger.Log("SearchForWordStands4Async", "Null or whitespace. Return an empty list");
                return new ReadOnlyObservableCollection<IWord>(new ObservableCollection<IWord>());
            }

            Stands4Dictionary stands4Endpoint = new Stands4Dictionary(App.OAuth2Account, word);
            IList<Stands4Word> stands4Result = await Task.Run(async () => await stands4Endpoint.CallEndpointAsStands4Word());
            Tools.Logger.Log("SearchForWordStands4Async", "Data received");

            return new ReadOnlyObservableCollection<IWord>(new ObservableCollection<IWord>(stands4Result));
        }
    }
}

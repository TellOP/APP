// <copyright file="OxfordSearchDataModel.cs" company="University of Murcia">
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
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Api;
    using ApiModels;
    using Nito.AsyncEx;

    /// <summary>
    /// The data model for featured exercises.
    /// </summary>
    public class OxfordSearchDataModel : ISearchDataModel
    {
        /// <summary>
        /// A read-only list of Oxford dictionary search results.
        /// </summary>
        private INotifyTaskCompletion<ReadOnlyObservableCollection<IWord>> _searchResultsOxford;

        /// <summary>
        /// Initializes a new instance of the <see cref="OxfordSearchDataModel"/> class.
        /// </summary>
        public OxfordSearchDataModel()
        {
            this.SearchForWord(string.Empty);
        }

        /// <summary>
        /// Fired when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets a read-only list of Oxford dictionary search results for the word typed in the search box.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a collection of IWords")]
        public INotifyTaskCompletion<ReadOnlyObservableCollection<IWord>> SearchResultsOxford
        {
            get
            {
                return this._searchResultsOxford;
            }

            private set
            {
                this._searchResultsOxford = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SearchResultsOxford"));
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
                return this.SearchResultsOxford.IsCompleted;
            }
        }

        /// <summary>
        /// Searches for a given word.
        /// </summary>
        /// <param name="word">The word to search for.</param>
        public void SearchForWord(string word)
        {
            // TODO: the dictionary search is recorded in the first call. Perhaps find a better design?
            this.SearchResultsOxford = NotifyTaskCompletion.Create(SearchForWordOxfordAsync(word));
        }

        /// <summary>
        /// Records the dictionary search, then searches for a given word asynchronously in the Oxford dictionary.
        /// </summary>
        /// <param name="word">The word to search for.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private static async Task<ReadOnlyObservableCollection<IWord>> SearchForWordOxfordAsync(string word)
        {
            Tools.Logger.Log("SearchForWordOxfordAsync", "Start search '" + word + "'");
            if (string.IsNullOrWhiteSpace(word))
            {
                Tools.Logger.Log("SearchForWordOxfordAsync", "Null or whitespace. Return an empty list");
                return new ReadOnlyObservableCollection<IWord>(new ObservableCollection<IWord>());
            }

            try
            {
                Tools.Logger.Log("SearchForWordOxfordAsync", "Initialize API");
                OxfordDictionaryAPI oxfordEndpoint = new OxfordDictionaryAPI(App.OAuth2Account, word, Enums.SupportedLanguage.Spanish);

                Tools.Logger.Log("SearchForWordOxfordAsync", "Calling Endpoint");
                IList<OxfordWord> oxfordResult = await Task.Run(async () => await oxfordEndpoint.CallEndpointAsObjectAsync());
                Tools.Logger.Log("SearchForWordOxfordAsync", "Data received (" + oxfordResult.Count + ")");

                return new ReadOnlyObservableCollection<IWord>(new ObservableCollection<IWord>(oxfordResult));
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex =>
                    {
                        Tools.Logger.Log("SearchForWordOxfordAsync", "Inner Exception", ex);
                        return true;
                    });
                Tools.Logger.Log("SearchForWordOxfordAsync", "Returning empty list due to exceptions.");
                return new ReadOnlyObservableCollection<IWord>(new ObservableCollection<IWord>());
            }
        }
    }
}

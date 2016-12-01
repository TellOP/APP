// <copyright file="SearchDataModel.cs" company="University of Murcia">
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
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Api;
    using ApiModels;
    using ApiModels.Collins;
    using ApiModels.Exercise;
    using ApiModels.Stands4;
    using Nito.AsyncEx;

    /// <summary>
    /// The data model for featured exercises.
    /// </summary>
    public class SearchDataModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The database ID of the U.S. English "dictionary search" activity.
        /// </summary>
        private const int USEnglishDictionarySearchID = 21;

        /// <summary>
        /// A read-only list of Stands4 dictionary search results.
        /// </summary>
        private INotifyTaskCompletion<ReadOnlyObservableCollection<IWord>> _searchResultsStands4;

        /// <summary>
        /// A read-only list of Collins dictionary search results.
        /// </summary>
        private INotifyTaskCompletion<ReadOnlyObservableCollection<IWord>> _searchResultsCollins;

        /// <summary>
        /// A read-only list of NetSpeak dictionary search results.
        /// </summary>
        private INotifyTaskCompletion<string> _searchResultsNetSpeakPreceding;

        /// <summary>
        /// A read-only list of NetSpeak dictionary search results.
        /// </summary>
        private INotifyTaskCompletion<string> _searchResultsNetSpeakFollowing;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchDataModel"/> class.
        /// </summary>
        public SearchDataModel()
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
                return this.SearchResultsCollins.IsCompleted &&
                       this.SearchResultsStands4.IsCompleted &&
                       this.SearchResultsNetSpeakFollowing.IsCompleted &&
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
            this.SearchResultsStands4 = NotifyTaskCompletion.Create(SearchForWordStands4Async(word));
            this.SearchResultsCollins = NotifyTaskCompletion.Create(SearchForWordCollinsAsync(word));
            this.SearchResultsNetSpeakPreceding = NotifyTaskCompletion.Create(SearchForWordNetSpeakPrecedingAsync(word));
            this.SearchResultsNetSpeakFollowing = NotifyTaskCompletion.Create(SearchForWordNetSpeakFollowingAsync(word));
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

            UserActivityDictionarySearch dictSearch = new UserActivityDictionarySearch()
            {
                ActivityId = USEnglishDictionarySearchID,
                Word = word
            };
            ExerciseSubmissionApi dictSearchSubmissionEndpoint = new ExerciseSubmissionApi(App.OAuth2Account, dictSearch);
            Task dictSearchSubmissionTask = Task.Run(async () => await dictSearchSubmissionEndpoint.CallEndpointAsync());
            await dictSearchSubmissionTask;
            if (dictSearchSubmissionTask.IsFaulted)
            {
                // Prevent the inner exception from terminating the program.
                foreach (Exception ex in dictSearchSubmissionTask.Exception.InnerExceptions)
                {
                    Tools.Logger.Log(typeof(SearchDataModel).ToString(), "Inner task exception while submitting the search to the dictionary search endpoint. Ignoring.", ex);
                }

                Tools.Logger.Log(typeof(SearchDataModel).ToString(), "Task exception while submitting the search to the dictionary search endpoint. Ignoring.", dictSearchSubmissionTask.Exception);
            }

            Stands4Dictionary stands4Endpoint = new Stands4Dictionary(App.OAuth2Account, word);
            IList<Stands4Word> stands4Result = await Task.Run(async () => await stands4Endpoint.CallEndpointAsStands4Word());
            Tools.Logger.Log("SearchForWordStands4Async", "Data received");

            return new ReadOnlyObservableCollection<IWord>(new ObservableCollection<IWord>(stands4Result));
        }

        /// <summary>
        /// Searches for a given word asynchronously in the NetSpeak dictionary.
        /// </summary>
        /// <param name="word">The word to search for.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private static async Task<string> SearchForWordNetSpeakPrecedingAsync(string word)
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

        /// <summary>
        /// Searches for a given word asynchronously in the NetSpeak dictionary.
        /// </summary>
        /// <param name="word">The word to search for.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private static async Task<string> SearchForWordNetSpeakFollowingAsync(string word)
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

            CollinsEnglishDictionary collinsEndpoint = new CollinsEnglishDictionary(App.OAuth2Account, word);
            IList<CollinsWord> collinsResult = await Task.Run(async () => await collinsEndpoint.CallEndpointAsCollinsWord());
            Tools.Logger.Log("SearchForWordCollinsAsync", "Data received");
            return new ReadOnlyObservableCollection<IWord>(new ObservableCollection<IWord>(collinsResult));
        }
    }
}

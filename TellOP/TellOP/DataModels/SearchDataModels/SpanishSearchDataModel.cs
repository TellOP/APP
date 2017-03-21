// <copyright file="SpanishSearchDataModel.cs" company="University of Murcia">
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
    using ApiModels.Exercise;
    using ApiModels.StringNet;
    using Nito.AsyncEx;

    /// <summary>
    /// The data model for featured exercises.
    /// </summary>
    public class SpanishSearchDataModel : ISearchDataModel
    {
        /// <summary>
        /// The database ID of the British English "dictionary search" activity.
        /// </summary>
        private const int SpanishEnglishDictionarySearchID = 91;

        /// <summary>
        /// A read-only list of Oxford dictionary search results.
        /// </summary>
        private INotifyTaskCompletion<ReadOnlyObservableCollection<IWord>> _searchResultsOxford;

        /// <summary>
        /// A read-only list of NetSpeak dictionary search results.
        /// </summary>
        private INotifyTaskCompletion<string> _searchResultsNetSpeakPreceding;

        /// <summary>
        /// A read-only list of NetSpeak dictionary search results.
        /// </summary>
        private INotifyTaskCompletion<string> _searchResultsNetSpeakFollowing;

        /// <summary>
        /// A read-only list of StringNet dictionary search results.
        /// </summary>
        private INotifyTaskCompletion<StringNetSplit> _searchResultsStringNet;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpanishSearchDataModel"/> class.
        /// </summary>
        public SpanishSearchDataModel()
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
                return this.SearchResultsOxford.IsCompleted &&
                       this.SearchResultsNetSpeakFollowing.IsCompleted &&
                       this.SearchResultsNetSpeakPreceding.IsCompleted &&
                       this.SearchResultsStringNet.IsCompleted;
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
            this.SearchResultsNetSpeakPreceding = NotifyTaskCompletion.Create(SearchForWordNetSpeakPrecedingAsync(word));
            this.SearchResultsNetSpeakFollowing = NotifyTaskCompletion.Create(SearchForWordNetSpeakFollowingAsync(word));
            this.SearchResultsStringNet = NotifyTaskCompletion.Create(SearchForWordStringNetAsync(word));

            Task.Run(async () =>
            {
                try
                {
                    await SaveDictionarySearchAsync(word);
                }
                catch (AggregateException ae)
                {
                    ae.Handle(ex =>
                    {
                        Tools.Logger.Log("SaveDictionaryExternalHandler", ex);
                        return true; // Handled
                    });
                }
            });
        }

        /// <summary>
        /// Save the current search online.
        /// </summary>
        /// <param name="word">String searched</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private static async Task SaveDictionarySearchAsync(string word)
        {
            UserActivityDictionarySearch dictSearch = new UserActivityDictionarySearch()
            {
                ActivityId = SpanishEnglishDictionarySearchID,
                Word = word
            };
            Tools.Logger.Log("DictionaryLog", "Start logging procedure (ID: " + dictSearch.ActivityId + ", Word: " + dictSearch.Word + ")");
            ExerciseSubmissionApi dictSearchSubmissionEndpoint = new ExerciseSubmissionApi(App.OAuth2Account, dictSearch);

            Tools.Logger.Log("DictionaryLog", "Send data to the server");
            Task dictSearchSubmissionTask = Task.Run(async () => await dictSearchSubmissionEndpoint.CallEndpointAsync());
            await dictSearchSubmissionTask;
            if (dictSearchSubmissionTask.IsFaulted)
            {
                Tools.Logger.Log("DictionaryLog", "Exception", dictSearchSubmissionTask.Exception);

                // Prevent the inner exception from terminating the program.
                foreach (Exception ex in dictSearchSubmissionTask.Exception.InnerExceptions)
                {
                    Tools.Logger.Log("DictionaryLog", "Inner exception", ex);
                }
            }
            else
            {
                Tools.Logger.Log("DictionaryLog", "Data received. Everything is OK!");
            }
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

        /// <summary>
        /// Searches for a given word asynchronously in the NetSpeak dictionary.
        /// </summary>
        /// <param name="word">The word to search for.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private static async Task<string> SearchForWordNetSpeakPrecedingAsync(string word)
        {try
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
                Tools.Logger.Log("SpanishSearchDataModel", "Something happened", ex);
                return "Unknown error @303: " + ex;
            }
        }

        /// <summary>
        /// Searches for a given word asynchronously in the NetSpeak dictionary.
        /// </summary>
        /// <param name="word">The word to search for.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private static async Task<string> SearchForWordNetSpeakFollowingAsync(string word)
        {try
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
                Tools.Logger.Log("SpanishSearchDataModel", "Something happened", ex);
                return "Unknown error @345: " + ex;
            }
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

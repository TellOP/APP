// <copyright file="HistoryDataModel.cs" company="University of Murcia">
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
    using System.Linq;
    using System.Threading.Tasks;
    using Activity;
    using Api;
    using ApiModels;
    using ApiModels.Exercise;
    using Nito.AsyncEx;

    /// <summary>
    /// The data model for the exercise history.
    /// </summary>
    public class HistoryDataModel : INotifyPropertyChanged
    {
        /// <summary>
        /// A read-only list containing the exercise history.
        /// </summary>
        private INotifyTaskCompletion<ReadOnlyObservableCollection<Exercise>> _exerciseHistory;

        /// <summary>
        /// A single random tip.
        /// </summary>
        private INotifyTaskCompletion<Tip> _appTip;

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryDataModel"/> class.
        /// </summary>
        public HistoryDataModel()
        {
            this.RefreshHistory();
        }

        /// <summary>
        /// Fired when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets a read-only list containing the exercise history.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a grouping as it is needed by the ListView")]
        public INotifyTaskCompletion<ReadOnlyObservableCollection<Exercise>> ExerciseHistory
        {
            get
            {
                return this._exerciseHistory;
            }

            private set
            {
                this._exerciseHistory = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ExerciseHistory"));
            }
        }

        /// <summary>
        /// Gets a single random tip.
        /// </summary>
        public INotifyTaskCompletion<Tip> AppTip
        {
            get
            {
                return this._appTip;
            }

            private set
            {
                this._appTip = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AppTip"));
            }
        }

        /// <summary>
        /// Return true iff the task are completed.
        /// </summary>
        /// <returns>True iff the task are completed.</returns>
        public bool CanRefresh()
        {
            return this.ExerciseHistory.IsCompleted && this.AppTip.IsCompleted;
        }

        /// <summary>
        /// Refreshes the exercise history.
        /// </summary>
        public void RefreshHistory()
        {
            this.ExerciseHistory = NotifyTaskCompletion.Create(GetHistoryAsync());
            this.AppTip = NotifyTaskCompletion.Create(TipsDataModel.GetSingleTipAsync());
        }

        /// <summary>
        /// Refreshes the exercise history asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private static async Task<ReadOnlyObservableCollection<Exercise>> GetHistoryAsync()
        {
            List<Exercise> historyWithResults = new List<Exercise>();
            ExerciseHistoryApi historyEndpoint = new ExerciseHistoryApi(App.OAuth2Account);

            IEnumerable<UserActivity> history = new List<UserActivity>();
            history = await Task.Run(async () => await historyEndpoint.CallEndpointAsObjectAsync());

            // Remove dictionary searches (they are not meant to be displayed in the list).
            history = history.Where(x => x.GetType() != typeof(DictionarySearchExercise));

            // Preload all exercises in the list to improve performance.
            ExerciseApi exerciseEndpoint;
            foreach (UserActivity histAct in history)
            {
                exerciseEndpoint = new ExerciseApi(App.OAuth2Account, histAct.ActivityId);
                historyWithResults.Add(await Task.Run(async () => await exerciseEndpoint.CallEndpointAsExerciseModel()));
            }

            return new ReadOnlyObservableCollection<Exercise>(new ObservableCollection<Exercise>(historyWithResults));
        }
    }
}

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

namespace TellOP.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Activity;
    using API;
    using APIModels.Exercise;

    /// <summary>
    /// The data model for the exercise history.
    /// </summary>
    public class HistoryDataModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The exercise history.
        /// </summary>
        private ReadOnlyObservableCollection<Exercise> _exerciseHistory;

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryDataModel"/> class.
        /// </summary>
        public HistoryDataModel()
        {
        }

        /// <summary>
        /// Fired when a property of this model changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets a read-only list containing the exercise history.
        /// </summary>
        public ReadOnlyObservableCollection<Exercise> History
        {
            get
            {
                return this._exerciseHistory;
            }
        }

        /// <summary>
        /// Refreshes the list containing the exercise history.
        /// </summary>
        /// <returns>True if everything was completed correctly</returns>
        /// <exception cref="UnsuccessfulAPICallException">Thrown if <see cref="ExerciseHistoryAPI.CallEndpointAsObjectAsync()"/> fails.</exception>
        /// <exception cref="Exception">Thrown if something else fails.</exception>
        public async Task RefreshExerciseHistory()
        {
            List<Exercise> historyWithResults = new List<Exercise>();
            ExerciseHistoryAPI historyEndpoint = new ExerciseHistoryAPI(App.OAuth2Account);

            // These exceptions are handled in the view directly.
            IList<UserActivity> history = new List<UserActivity>();
            history = await historyEndpoint.CallEndpointAsObjectAsync();

            // Performance
            ExerciseAPI exerciseEndpoint;
            Exercise histEx;
            UserActivityEssay histActEssay;
            EssayExercise histExEssay;

            foreach (UserActivity histAct in history)
            {
                try
                {
                    exerciseEndpoint = new ExerciseAPI(App.OAuth2Account, histAct.ActivityID);
                    histEx = await exerciseEndpoint.CallEndpointAsExerciseModel();
                    if (histAct is UserActivityEssay && histEx is EssayExercise)
                    {
                        histActEssay = (UserActivityEssay)histAct;
                        histExEssay = (EssayExercise)histEx;
                        histExEssay.Contents = histActEssay.Text;
                        histExEssay.Status = histActEssay.Status;
                        histExEssay.Timestamp = histActEssay.Timestamp;
                    }
                    else
                    {
                        historyWithResults.Add(histEx);
                    }
                }
                catch (UnsuccessfulAPICallException ex)
                {
                    Tools.Logger.Log(this, "RefreshExerciseHistory method - Internal UserActivity loop", ex);

                    // TODO: Add activity indicator
                    // this.SwitchActivityIndicator(false);
                }
                catch (Exception ex)
                {
                    Tools.Logger.Log(this, "RefreshExerciseHistory method - Internal UserActivity loop", ex);

                    // TODO: Add activity indicator
                    // this.SwitchActivityIndicator(false);
                }
            }

            this._exerciseHistory = new ReadOnlyObservableCollection<Exercise>(new ObservableCollection<Exercise>(historyWithResults));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("History"));
        }
    }
}

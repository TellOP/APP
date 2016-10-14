// <copyright file="FeaturedDataModel.cs" company="University of Murcia">
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
    using System.Linq;
    using Activity;
    using API;
    using Enums;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// The data model for featured exercises.
    /// </summary>
    public class FeaturedDataModel : INotifyPropertyChanged
    {
        /// <summary>
        /// A list of featured exercises.
        /// </summary>
        private ReadOnlyObservableCollection<ExerciseGroup> _featuredExercises;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeaturedDataModel"/> class.
        /// </summary>
        public FeaturedDataModel()
        {
        }

        /// <summary>
        /// Fired when a property of this model changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets a read-only list of featured exercises.
        /// </summary>
        public ReadOnlyObservableCollection<ExerciseGroup> FeaturedExercises
        {
            get
            {
                return this._featuredExercises;
            }
        }

        /// <summary>
        /// Refreshes the list of featured exercises.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="UnsuccessfulAPICallException">Thrown if the API didn't exit correctly</exception>
        public async Task RefreshFeaturedExercises()
        {
            ExerciseFeaturedAPI featuredEndpoint = new ExerciseFeaturedAPI(App.OAuth2Account);

            // These exceptions are handled in the view
            IList<Exercise> featuredExercises = await featuredEndpoint.CallEndpointAsExerciseModel();

            try
            {
                // Group the exercises by their CEFR level.
                IEnumerable<ExerciseGroup> featuredByGroup = from ex in featuredExercises
                                                             group ex by ex.Level into exSameLevel
                                                             select new ExerciseGroup(
                                                                 LanguageLevelClassificationExtension.LevelToLongDescription(exSameLevel.Key),
                                                                 LanguageLevelClassificationExtension.LevelToShortTitle(exSameLevel.Key),
                                                                 exSameLevel.ToList());

                this._featuredExercises = new ReadOnlyObservableCollection<ExerciseGroup>(new ObservableCollection<ExerciseGroup>(featuredByGroup));
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FeaturedExercises"));
                return;
            }
            catch (Exception ex)
            {
                Tools.Logger.Log(this, "RefreshFeaturedExercises method - Exercise Group", ex);

                // TODO: Add activity indicator
                // this.SwitchActivityIndicator(false);
            }
        }
    }
}

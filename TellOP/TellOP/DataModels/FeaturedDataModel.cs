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
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Activity;
    using Api;
    using ApiModels;
    using Enums;
    using Nito.AsyncEx;

    /// <summary>
    /// The data model for featured exercises.
    /// </summary>
    public class FeaturedDataModel : INotifyPropertyChanged
    {
        /// <summary>
        /// A read-only list of featured exercises.
        /// </summary>
        private INotifyTaskCompletion<ReadOnlyObservableCollection<Grouping<Exercise>>> _featuredExercises;

        /// <summary>
        /// A single random tip.
        /// </summary>
        private INotifyTaskCompletion<Tip> _appTip;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeaturedDataModel"/> class.
        /// </summary>
        public FeaturedDataModel()
        {
            this.RefreshExercises();
        }

        /// <summary>
        /// Fired when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets a read-only list of featured exercises.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a grouping as it is needed by the ListView")]
        public INotifyTaskCompletion<ReadOnlyObservableCollection<Grouping<Exercise>>> FeaturedExercises
        {
            get
            {
                return this._featuredExercises;
            }

            private set
            {
                this._featuredExercises = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FeaturedExercises"));
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
        /// Refreshes the list of featured exercises and the list of tips.
        /// </summary>
        public void RefreshExercises()
        {
            this.FeaturedExercises = NotifyTaskCompletion.Create(GetFeaturedExercisesAsync());
            this.AppTip = NotifyTaskCompletion.Create(TipsDataModel.GetSingleTipAsync());
        }

        /// <summary>
        /// Refreshes the list of featured exercises asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private static async Task<ReadOnlyObservableCollection<Grouping<Exercise>>> GetFeaturedExercisesAsync()
        {
            ExerciseFeaturedApi featuredEndpoint = new ExerciseFeaturedApi(App.OAuth2Account);
            IList<Exercise> featuredExercises = await Task.Run(async () => await featuredEndpoint.CallEndpointAsExerciseModel());

            // Group the exercises by their CEFR level.
            LanguageLevelClassificationToLongDescriptionConverter longDescConverter = new LanguageLevelClassificationToLongDescriptionConverter();
            LanguageLevelClassificationToHtmlParamConverter htmlParamConverter = new LanguageLevelClassificationToHtmlParamConverter();
            IEnumerable<Grouping<Exercise>> featuredByGroup = from ex in featuredExercises group ex by ex.Level into exSameLevel select new Grouping<Exercise>((string)longDescConverter.Convert(exSameLevel.Key, typeof(string), null, CultureInfo.CurrentCulture), (string)htmlParamConverter.Convert(exSameLevel.Key, typeof(string), null, CultureInfo.CurrentCulture), exSameLevel.ToList());

            return new ReadOnlyObservableCollection<Grouping<Exercise>>(new ObservableCollection<Grouping<Exercise>>(featuredByGroup));
        }
    }
}

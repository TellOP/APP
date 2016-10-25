// <copyright file="DashboardTabFeatured.xaml.cs" company="University of Murcia">
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

namespace TellOP
{
    using DataModels;
    using DataModels.Activity;
    using Xamarin.Forms;

    /// <summary>
    /// Featured tab on the Dashboard page.
    /// </summary>
    public partial class DashboardTabFeatured : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardTabFeatured"/> class.
        /// </summary>
        public DashboardTabFeatured()
        {
            this.BindingContext = new FeaturedDataModel();
            this.InitializeComponent();
        }

        /// <summary>
        /// Refreshes the exercise list.
        /// </summary>
        public void Refresh()
        {
            ((FeaturedDataModel)this.BindingContext).RefreshExercises();
        }

        /// <summary>
        /// Called when an item in the exercise list is selected.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private async void ExerciseList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // If this handler is called when an item is deselected, bail out
            if (e.SelectedItem == null)
            {
                return;
            }

            // TODO: support dictionary searches
            await this.Navigation.PushAsync(new EssayExerciseView((EssayExercise)e.SelectedItem));
        }
    }
}

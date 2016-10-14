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

namespace TellOP
{
    using API;
    using DataModels;
    using DataModels.Activity;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    /// <summary>
    /// Featured tab on the Dashboard page.
    /// </summary>
    public partial class DashboardTabFeatured : ContentPage
    {
        private FeaturedDataModel _fdm;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DashboardTabFeatured"/> class.
        /// </summary>
        public DashboardTabFeatured()
        {
            this.InitializeComponent();
            this.Refresh();
        }

        /// <summary>
        /// Force the refresh of all the displayed data
        /// </summary>
        public void Refresh()
        {
            this.PopulateExercises();
            this.PopulateTips();
        }

        /// <summary>
        /// Populate the ex list
        /// </summary>
        private async void PopulateExercises()
        {
            try
            {
                if (this._fdm == null)
                {
                    this._fdm = new FeaturedDataModel();
                    await this._fdm.RefreshFeaturedExercises();
                }

                if (this._fdm.FeaturedExercises == null)
                {
                    await this._fdm.RefreshFeaturedExercises();
                }

                this.ExList.ItemsSource = this._fdm.FeaturedExercises;
            }
            catch (UnsuccessfulAPICallException ex)
            {
                Tools.Logger.Log(this, "PopulateExercises", ex);

                // TODO: Add activity indicator
                // this.SwitchActivityIndicator(false);
            }
            catch (System.Exception ex)
            {
                Tools.Logger.Log(this, "PopulateExercises", ex);

                // TODO: Add activity indicator
                // this.SwitchActivityIndicator(false);
            }
        }

        /// <summary>
        /// Populate the tips panel
        /// </summary>
        private async void PopulateTips()
        {
            try
            {
                TipsDataModel currentTip = new TipsDataModel();
                this.TipTitle.Text = (await currentTip.GetSingleRandom()).Text;
            }
            catch (UnsuccessfulAPICallException ex)
            {
                Tools.Logger.Log(this, "PopulateTips method", ex);

                // TODO: Add activity indicator
                // this.SwitchActivityIndicator(false);
            }
            catch (System.Exception ex)
            {
                Tools.Logger.Log(this, "PopulateTips method", ex);

                // TODO: Add activity indicator
                // this.SwitchActivityIndicator(false);
            }
        }

        private async void OnSelection(
            object sender,
            SelectedItemChangedEventArgs e)
        {
            // If this handler is called when an item is deselected, bail out
            if (e.SelectedItem == null)
            {
                return;
            }

            await this.Navigation.PushAsync(new EssayExerciseView(
                (EssayExercise)e.SelectedItem));
        }
    }
}

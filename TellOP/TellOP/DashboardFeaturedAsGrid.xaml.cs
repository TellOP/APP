// <copyright file="DashboardFeaturedAsGrid.xaml.cs" company="University of Murcia">
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
    using System.Threading.Tasks;
    using Api;
    using DataModels;
    using DataModels.Activity;
    using Tools;
    using Xamarin.Forms;

    /// <summary>
    /// Featured tab on the Dashboard page.
    /// </summary>
    public partial class DashboardFeaturedAsGrid : StackLayout
    {
        /// <summary>
        /// Page container
        /// </summary>
        private Page page;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardFeaturedAsGrid"/> class.
        /// </summary>
        /// <param name="parent">Page container</param>
        public DashboardFeaturedAsGrid(Page parent)
        {
            this.BindingContext = new FeaturedDataModel();
            this.InitializeComponent();
            this.page = parent;
        }

        /// <summary>
        /// Retrieve the binding context.
        /// </summary>
        /// <returns><see cref="FeaturedDataModel"/> Binding Context</returns>
        public FeaturedDataModel GetBindingContext()
        {
            return (FeaturedDataModel)this.BindingContext;
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
        private void ExerciseList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        /// <summary>
        /// Called when an item in the exercise list is tapped.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private async void ExerciseList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (await ConnectivityCheck.AskToEnableConnectivity(this.page))
            {
                // TODO: support other exercise types
                try
                {
                    ExerciseApi exerciseEndpoint = new ExerciseApi(App.OAuth2Account, ((EssayExercise)e.Item).Uid);
                    EssayExercise essay = await Task.Run(async () => await exerciseEndpoint.CallEndpointAsExerciseModel()) as EssayExercise;
                    if (essay == null)
                    {
                        await this.page.DisplayAlert(Properties.Resources.Error, Properties.Resources.Exercise_UnableToDisplay, Properties.Resources.ButtonOK);
                    }
                    else
                    {
                        await this.page.Navigation.PushAsync(new EssayExerciseView(essay));
                    }
                }
                catch (UnsuccessfulApiCallException ex)
                {
                    Tools.Logger.Log("ExerciseList_ItemTapped", ex);
                    await this.page.DisplayAlert(Properties.Resources.Error, Properties.Resources.Exercise_UnableToDisplay, Properties.Resources.ButtonOK);
                }
            }
        }
    }
}

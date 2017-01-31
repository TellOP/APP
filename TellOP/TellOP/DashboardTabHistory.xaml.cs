// <copyright file="DashboardTabHistory.xaml.cs" company="University of Murcia">
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
    using System.Diagnostics.CodeAnalysis;
    using DataModels;
    using DataModels.Activity;
    using Tools;
    using Xamarin.Forms;

    /// <summary>
    /// History tab in the Dashboard page.
    /// </summary>
    public partial class DashboardTabHistory : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardTabHistory"/> class.
        /// </summary>
        public DashboardTabHistory()
        {
            this.BindingContext = new HistoryDataModel();
            this.InitializeComponent();
        }

        /// <summary>
        /// Refreshes the exercise history list.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CS4014:Await.Warning", Justification = "Need to run the code async without awaiting the result.")]
        public void Refresh()
        {
            if (((HistoryDataModel)this.BindingContext).CanRefresh())
            {
                ((HistoryDataModel)this.BindingContext).RefreshHistory();
            }
            else
            {
                Logger.LogWithErrorMessage(this, "Cannot refresh history while another operation is running.", new System.Exception("Cannot refresh history."));
            }
        }

        /// <summary>
        /// Called when an item in the exercise list is selected.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private void HistoryList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        /// <summary>
        /// Called when an item in the exercise list is tapped.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private async void HistoryList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // TODO: support other exercise types
            if (await ConnectivityCheck.AskToEnableConnectivity(this))
            {
                await this.Navigation.PushAsync(new EssayExerciseView((EssayExercise)e.Item));
            }
        }
    }
}

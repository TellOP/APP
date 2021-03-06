// <copyright file="DashboardHistoryAsGrid.xaml.cs" company="University of Murcia">
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
    using Tools;
    using Xamarin.Forms;

    /// <summary>
    /// History tab in the Dashboard page.
    /// </summary>
    public partial class DashboardHistoryAsGrid : StackLayout
    {
        /// <summary>
        /// Container page
        /// </summary>
        private Page page;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardHistoryAsGrid"/> class.
        /// </summary>
        /// <param name="parent">Container page.</param>
        public DashboardHistoryAsGrid(Page parent)
        {
            this.page = parent;
            this.BindingContext = new HistoryDataModel();
            this.InitializeComponent();
        }

        /// <summary>
        /// Retrieve the binding context
        /// </summary>
        /// <returns><see cref="HistoryDataModel"/> Binding Context</returns>
        public HistoryDataModel GetBindingContext()
        {
            return (HistoryDataModel)this.BindingContext;
        }

        /// <summary>
        /// Refreshes the exercise history list.
        /// </summary>
        public void Refresh()
        {
            ((HistoryDataModel)this.BindingContext).RefreshHistory();
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
            if (await ConnectivityCheck.AskToEnableConnectivity(this.page))
            {
                await this.Navigation.PushAsync(new EssayExerciseView((EssayExercise)e.Item));
            }
        }
    }
}

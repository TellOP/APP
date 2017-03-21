﻿// <copyright file="SearchCollinsTab.xaml.cs" company="University of Murcia">
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
    using DataModels;
    using DataModels.Enums;
    using ViewModels;
    using Xamarin.Forms;

    /// <summary>
    /// Search tab for collins dictionary
    /// </summary>
    public partial class SearchCollinsTab : ContentPage
    {
        /// <summary>
        /// Parent page
        /// </summary>
        private MainSearch parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchCollinsTab"/> class.
        /// </summary>
        /// <param name="parent">Parent page</param>
        public SearchCollinsTab(MainSearch parent)
        {
            this.Icon = "TAB_Collins.png";
            this.Title = "Collins Definitions";
            this.parent = parent;
            this.PreInitialize();
            this.InitializeComponent();

            this.SearchListCollins.ItemTemplate = new DataTemplate(typeof(CollinsViewCell));
        }

        /// <summary>
        /// Gets the search bar.
        /// </summary>
        public SearchBar Search
        {
            get
            {
                return this.SearchBar;
            }
        }

        /// <summary>
        /// Gets the binding context as <see cref="ISearchDataModel"/>.
        /// </summary>
        public ISearchDataModel SearchModel
        {
            get
            {
                return (ISearchDataModel)this.BindingContext;
            }
        }

        /// <summary>
        /// Pre-initialization process.
        /// </summary>
        private void PreInitialize()
        {
            // this.BindingContext = this.parent.BindingContext;
            switch (App.ActiveSearchLanguage)
            {
                case SupportedLanguage.English:
                    this.BindingContext = new CollinsSearchDataModel();
                    break;
                case SupportedLanguage.German:
                    this.BindingContext = new GermanSearchDataModel();
                    break;
            }
        }

        /// <summary>
        /// Called when the user taps on a result in one of the search result lists.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private void SearchList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (((ISearchDataModel)this.BindingContext).IsSearchEnabled)
            {
                ((ListView)sender).SelectedItem = null;
                ((ListView)sender).BeginRefresh();
                ((ListView)sender).EndRefresh();
            }
            else
            {
                // TODO: display alert?
            }
        }

        /// <summary>
        /// Called when the user taps on "Search" in the search bar.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private async void SearchBar_SearchButtonPressed(object sender, System.EventArgs e)
        {
            await this.parent.SearchBar_SearchButtonPressed(sender, e);
        }
    }
}

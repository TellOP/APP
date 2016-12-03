// <copyright file="StringNetCollocationsViewCell.xaml.cs" company="University of Murcia">
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

namespace TellOP.ViewModels
{
    using System;
    using DataModels.ApiModels.Stands4;
    using Xamarin.Forms;

    /// <summary>
    /// A <see cref="ViewCell"/> representing a <see cref="Stands4Word"/>.
    /// </summary>
    public partial class StringNetCollocationsViewCell : ViewCell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringNetCollocationsViewCell"/> class.
        /// </summary>
        public StringNetCollocationsViewCell()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Called when the details panel is expanded or collapsed.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private void InvertDetailsPanel(object sender, EventArgs e)
        {
            bool show = this.SpoilerExampleLabel.IsVisible;

            this.SpoilerExampleLabel.IsVisible = !show;
            this.SpoilerExampleText.IsVisible = !show;
            this.SpoilerIndicator.Text = show ? Properties.Resources.StringNetViewCell_Contracted : Properties.Resources.StringNetViewCell_Expanded;
        }
    }
}

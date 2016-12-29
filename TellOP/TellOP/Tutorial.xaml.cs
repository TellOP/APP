// <copyright file="Tutorial.xaml.cs" company="University of Murcia">
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
    using System;
    using Xamarin.Forms;

    /// <summary>
    /// Tutorial Class.
    /// </summary>
    public partial class Tutorial : ContentPage
    {
        /// <summary>
        /// Current image status.
        /// </summary>
        private int imgCounter = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tutorial"/> class.
        /// </summary>
        public Tutorial()
        {
            this.InitializeComponent();
            this.img.Source = "tutorial01.png";
            this.Progress.Source = "bullet1.png";

            this.PrevButton.Text = "Skip";
        }

        /// <summary>
        /// Show the following image in the tutorial.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arg object.</param>
        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            if (((Button)sender).Text == "Close")
            {
                await this.Navigation.PopModalAsync();
                return;
            }

            this.img.Source = "tutorial0" + (++this.imgCounter) + ".jpg";
            this.Progress.Source = "bullet" + this.imgCounter + ".jpg";

            if (this.imgCounter == 1)
            {
                this.PrevButton.Text = "Skip";
            }
            else if (this.imgCounter == 2)
            {
                this.PrevButton.Text = "Previous";
            }
            else if (this.imgCounter == 3)
            {
                this.PrevButton.Text = "Previous";
            }
            else if (this.imgCounter == 4)
            {
                this.PrevButton.Text = "Previous";
            }
            else if (this.imgCounter == 5)
            {
                this.NextButton.Text = "Next";
            }
            else if (this.imgCounter == 6)
            {
                this.NextButton.Text = "Close";
            }
        }

        /// <summary>
        /// Show the previous image in the tutorial.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arg</param>
        private async void PrevButton_Clicked(object sender, EventArgs e)
        {
            if (((Button)sender).Text == "Skip")
            {
                await this.Navigation.PopModalAsync();
                return;
            }

            this.img.Source = "tutorial0" + (--this.imgCounter) + ".png";
            this.Progress.Source = "bullet" + this.imgCounter + ".png";

            if (this.imgCounter == 1)
            {
                this.PrevButton.Text = "Skip";
            }
            else if (this.imgCounter == 2)
            {
                this.PrevButton.Text = "Previous";
            }
            else if (this.imgCounter == 5)
            {
                this.NextButton.Text = "Next";
            }
            else if (this.imgCounter == 6)
            {
                this.NextButton.Text = "Close";
            }
        }
    }
}

// <copyright file="WordDataTemplateSelector.cs" company="University of Murcia">
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
// <author>Alessandro Menti</author>

namespace TellOP.ViewModels
{
    using DataModels;
    using DataModels.ApiModels.Collins;
    using DataModels.ApiModels.Stands4;
    using Xamarin.Forms;

    /// <summary>
    /// Given an object instance implementing <see cref="IWord"/>, selects an appropriate view model to display it in a
    /// <see cref="ListView"/>.
    /// </summary>
    public class WordDataTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// The data template for Stands4 words.
        /// </summary>
        private readonly DataTemplate _stands4DataTemplate;

        /// <summary>
        /// The data template for Collins words.
        /// </summary>
        private readonly DataTemplate _collinsDataTemplate;

        /// <summary>
        /// Initializes a new instance of the <see cref="WordDataTemplateSelector"/> class.
        /// </summary>
        public WordDataTemplateSelector()
        {
            this._stands4DataTemplate = new DataTemplate(typeof(Stands4ViewCell));
            this._collinsDataTemplate = new DataTemplate(typeof(CollinsViewCell));
        }

        /// <summary>
        /// Called when a template should be chosen.
        /// </summary>
        /// <param name="item">The item for which the template should be chosen.</param>
        /// <param name="container">The containing list.</param>
        /// <returns>The instance of <see cref="DataTemplate"/> that should be used to represent the object.</returns>
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is Stands4Word)
            {
                return this._stands4DataTemplate;
            }
            else if (item is CollinsWord)
            {
                return this._collinsDataTemplate;
            }

            return null;
        }
    }
}

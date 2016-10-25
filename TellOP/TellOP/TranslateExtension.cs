// <copyright file="TranslateExtension.cs" company="University of Murcia">
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

namespace TellOP
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Resources;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    // Exclude the 'Extension' suffix when using this element in XAML markup.

    /// <summary>
    /// XAML localization extension.
    /// </summary>
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        /// <summary>
        /// Resource ID.
        /// </summary>
        private const string ResourceId = "TellOP.Properties.Resources";

        /// <summary>
        /// Culture info object used for localization.
        /// </summary>
        private readonly CultureInfo ci;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateExtension"/> class.
        /// </summary>
        public TranslateExtension()
        {
            this.ci = DependencyService.Get<ILocalize>().CurrentCultureInfo;
        }

        /// <summary>
        /// Gets or sets the text of a localized string.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Provides a translated value for a given text string.
        /// </summary>
        /// <param name="serviceProvider">XAML service provider.</param>
        /// <returns>The translated value, or the string key in case no translation is found (in release
        /// builds).</returns>
        /// <exception cref="Exception">Thrown in debug builds if no translation is found.</exception>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (this.Text == null)
            {
                return string.Empty;
            }

            ResourceManager resmgr = new ResourceManager(
                ResourceId,
                typeof(TranslateExtension).GetTypeInfo().Assembly);

            var translation = resmgr.GetString(this.Text, this.ci);

            if (translation == null)
            {
#if DEBUG
                throw new Exception(string.Format(CultureInfo.InvariantCulture, "Key '{0}' was not found in resources '{1}' for culture '{2}'.", this.Text, ResourceId, this.ci.Name));
#else
                // HACK: returns the key, which GETS DISPLAYED TO THE USER
                translation = Text;
#endif
            }

            return translation;
        }
    }
}

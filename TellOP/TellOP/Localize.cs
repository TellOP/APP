// <copyright file="Localize.cs" company="University of Murcia">
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
    using System.Globalization;
    using System.Reflection;
    using System.Resources;
    using Xamarin.Forms;

    /// <summary>
    /// Localization support class.
    /// </summary>
    public sealed class Localize
    {
        /// <summary>
        /// Current culture information.
        /// </summary>
        private static readonly CultureInfo CurrentCulture = DependencyService.Get<ILocalize>().CurrentCultureInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="Localize"/> class.
        /// </summary>
        private Localize()
        {
        }

        /// <summary>
        /// Gets a localized string.
        /// </summary>
        /// <param name="key">String key.</param>
        /// <param name="comment">String comment.</param>
        /// <returns>The localized version of the string having <paramref name="key"/> as its key.</returns>
        public static string GetString(string key, string comment)
        {
            ResourceManager temp = new ResourceManager("TellOP.Properties.Resources", typeof(Localize).GetTypeInfo().Assembly);

            string result = temp.GetString(key, CurrentCulture);
            return result;
        }
    }
}

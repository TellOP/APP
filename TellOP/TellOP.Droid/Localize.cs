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

using Xamarin.Forms;

[assembly: Dependency(typeof(TellOP.Android.Localize))]

namespace TellOP.Android
{
    using System.Globalization;
    using System.Threading;

    /// <summary>
    /// Localizer implementation for Android.
    /// </summary>
    public class Localize : TellOP.ILocalize
    {
        /// <summary>
        /// Gets the current culture information.
        /// </summary>
        public CultureInfo CurrentCultureInfo
        {
            get
            {
                Java.Util.Locale androidLocale = Java.Util.Locale.Default;
                return new CultureInfo(androidLocale.ToString().Replace("_", "-"));
            }
        }

        /// <summary>
        /// Sets the locale that is currently in use.
        /// </summary>
        public void SetLocale()
        {
            var androidLocale = Java.Util.Locale.Default; // user's preferred locale
            var netLocale = androidLocale.ToString().Replace("_", "-");
            var ci = new CultureInfo(netLocale);

            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
    }
}

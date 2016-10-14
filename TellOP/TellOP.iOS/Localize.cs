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

using System.Globalization;
using System.Security;
using System.Threading;
using Foundation;

[assembly: Xamarin.Forms.Dependency(typeof(TellOP.iOS.Localize))]

#pragma warning disable SA1300
namespace TellOP.iOS
#pragma warning restore SA1300
{
    /// <summary>
    /// Localization support class.
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
                string netLanguage = "en";
                string prefLanguageOnly = "en";
                if (NSLocale.PreferredLanguages.Length > 0)
                {
                    string pref = NSLocale.PreferredLanguages[0];

                    // HACK: Apple treats portuguese fallbacks in a strange way:
                    // https://developer.apple.com/library/ios/documentation/MacOSX/Conceptual/BPInternational/LocalizingYourApp/LocalizingYourApp.html
                    // "For example, use pt as the language ID for Portuguese as it
                    // is used in Brazil and pt-PT as the language ID for
                    // Portuguese as it is used in Portugal"
                    prefLanguageOnly = pref.Substring(0, 2);
                    if (prefLanguageOnly == "pt")
                    {
                        if (pref == "pt")
                        {
                            // Get the correct Brazilian language strings from the
                            // PCL RESX (note the local iOS folder is still "pt")
                            pref = "pt-BR";
                        }
                        else
                        {
                            // Portugal
                            pref = "pt-PT";
                        }
                    }

                    netLanguage = pref.Replace("_", "-");
                }

                CultureInfo ci = null;
                try
                {
                    ci = new CultureInfo(netLanguage);
                }
                catch
                {
                    // The iOS locale is not a valid .NET culture (e.g. "en-ES": English in
                    // Spain): fallback to the first characters, in this case "en"
                    ci = new CultureInfo(prefLanguageOnly);
                }

                return ci;
            }
        }

        /// <summary>
        /// Sets the locale that is currently in use.
        /// </summary>
        [SecurityCritical]
        public void SetLocale()
        {
            var iosLocaleAuto = NSLocale.AutoUpdatingCurrentLocale.LocaleIdentifier;
            var netLocale = iosLocaleAuto.Replace("_", "-");
            CultureInfo ci;
            try
            {
                ci = new CultureInfo(netLocale);
            }
            catch
            {
                ci = this.CurrentCultureInfo;
            }

            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
    }
}

// <copyright file="ThirdPartyLicenses.xaml.cs" company="University of Murcia">
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
    using System.IO;
    using System.Reflection;
    using Xamarin.Forms;

    /// <summary>
    /// The "third party licenses" about window.
    /// </summary>
    public partial class ThirdPartyLicenses : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyLicenses"/> class.
        /// </summary>
        public ThirdPartyLicenses()
        {
            this.InitializeComponent();

            string noticeText = string.Empty;
            try
            {
                Stream noticeStream = typeof(About).GetTypeInfo().Assembly.GetManifestResourceStream("TellOP.NOTICE.html");
                using (var reader = new StreamReader(noticeStream))
                {
                    noticeText = reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                noticeText = Properties.Resources.About_ThirdPartyLicenses_UnableToLoad;
            }

            HtmlWebViewSource noticeHTML = new HtmlWebViewSource();
            noticeHTML.Html = noticeText;
            this.ThirdPartyLicensesView.Source = noticeHTML;
        }
    }
}

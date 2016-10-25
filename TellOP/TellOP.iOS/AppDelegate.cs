// <copyright file="AppDelegate.cs" company="University of Murcia">
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

#pragma warning disable SA1300
namespace TellOP.iOS
#pragma warning restore SA1300
{
    using Foundation;
    using HockeyApp.iOS;
    using UIKit;

    /// <summary>
    /// The UIApplicationDelegate for the application. This class is responsible for launching the User Interface of
    /// the application, as well as listening (and optionally responding) to application events from iOS.
    /// </summary>
    [Register("AppDelegate")]
    public partial class AppDelegate
        : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        /// <summary>
        /// This method is invoked when the application has loaded and is ready to run.
        /// </summary>
        /// <param name="app">An <see cref="UIApplication" /> object referencing the application.</param>
        /// <param name="options">An <see cref="NSDictionary" /> object containing the application options.</param>
        /// <returns><c>true</c> if the application has finished launching, <c>false</c> otherwise.</returns>
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            BITHockeyManager manager = BITHockeyManager.SharedHockeyManager;
            manager.Configure(TellOPiOSConfiguration.HockeyAppId);
            manager.StartManager();
            manager.Authenticator.AuthenticateInstallation();

            global::Xamarin.Forms.Forms.Init();
            this.LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}

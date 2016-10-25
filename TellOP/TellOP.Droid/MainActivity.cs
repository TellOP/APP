// <copyright file="MainActivity.cs" company="University of Murcia">
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

namespace TellOP.Droid
{
    using global::Android.App;
    using global::Android.Content.PM;
    using global::Android.OS;
    using HockeyApp.Android;
    using HockeyApp.Android.Metrics;

    /// <summary>
    /// Creates the main activity for the application.
    /// </summary>
    [Activity(Label = "Tell-OP", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        /// <summary>
        /// Called when the main activity is created.
        /// </summary>
        /// <param name="bundle">The app bundle.</param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            CrashManager.Register(this, TellOPDroidConfiguration.HockeyAppId);

            MetricsManager.Register(this.Application, TellOPDroidConfiguration.HockeyAppId);

            // Check for updates (debug builds only)
#if DEBUG
            UpdateManager.Register(this, TellOPDroidConfiguration.HockeyAppId);
#endif

            global::Xamarin.Forms.Forms.Init(this, bundle);
            this.LoadApplication(new App());
        }

        /// <inheritdoc/>
        protected override void OnPause()
        {
            base.OnPause();
#if DEBUG
            UpdateManager.Unregister();
#endif
        }

        /// <inheritdoc/>
        protected override void OnDestroy()
        {
            base.OnDestroy();
#if DEBUG
            UpdateManager.Unregister();
#endif
        }
    }
}

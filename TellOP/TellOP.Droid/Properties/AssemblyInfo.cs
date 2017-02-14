// <copyright file="AssemblyInfo.cs" company="University of Murcia">
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

using System.Reflection;
using System.Runtime.InteropServices;
using Android.App;
using TellOP.Droid;
using Xamarin.Forms.Xaml;

[assembly: AssemblyTitle("Tell-OP for Android")]
[assembly: AssemblyDescription("The Tell-OP mobile app for Android")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("University of Murcia")]
[assembly: AssemblyProduct("Tell-OP")]
[assembly: AssemblyCopyright("Copyright © 2016 University of Murcia")]
[assembly: AssemblyTrademark("All trademarks belong to their respective owners.")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("0.7.*")]
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

// Disable debugging on release builds
#if DEBUG
[assembly: Application(Debuggable = true)]
#else
[assembly: Application(Debuggable = false)]
#endif

// Android permissions
[assembly: UsesPermission(Android.Manifest.Permission.AccessNetworkState)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessWifiState)]
[assembly: UsesPermission(Android.Manifest.Permission.Internet)]
[assembly: UsesFeature("android.hardware.wifi", Required = false)]

// The HockeyApp API 2 key
[assembly: MetaData("net.hockeyapp.android.appIdentifier", Value = TellOPDroidConfiguration.HockeyAppSecret)]

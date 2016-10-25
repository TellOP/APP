// <copyright file="Logger.cs" company="University of Murcia">
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

namespace TellOP.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    /// <summary>
    /// A class used to log debug errors.
    /// </summary>
    public sealed class Logger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        private Logger()
        {
        }

        /// <summary>
        /// Logs an error to the debug console and records it in HockeyApp if tracking is enabled.
        /// </summary>
        /// <param name="caller">A string identifying the logger.</param>
        /// <param name="message">The message to log.</param>
        public static void Log(string caller, string message)
        {
            Log(caller, message, null);
        }

        /// <summary>
        /// Logs an error to the debug console and records it in HockeyApp if tracking is enabled.
        /// </summary>
        /// <param name="caller">A string identifying the logger.</param>
        /// <param name="ex">The exception to log.</param>
        public static void Log(string caller, Exception ex)
        {
            Log(caller, "An exception occurred.", ex);
        }

        /// <summary>
        /// Logs an error to the debug console and records it in HockeyApp if tracking is enabled.
        /// </summary>
        /// <param name="caller">A string identifying the logger.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="ex">The exception associated to the message.</param>
        public static void Log(string caller, string message, Exception ex)
        {
            if (caller == null || string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message))
            {
                // Do not throw an ArgumentNullException (the logger will probably be used in "difficult" situations
                // where an exception has already occurred)
                return;
            }

            if (message.Contains("\n"))
            {
                Debug.WriteLine("[" + caller + "] --------------------------------");
                Debug.WriteLine(message);

                try
                {
                    Debug.WriteLineIf(ex != null, "Exception: " + ex.ToString());
                }
                catch (Exception)
                {
                }

                Debug.WriteLine("[End " + caller + "] --------------------------------");
            }
            else
            {
                Debug.WriteLine("[" + caller + "] " + message);

                try
                {
                    Debug.WriteLineIf(ex != null, "Exception: " + ex.ToString());
                }
                catch (Exception)
                {
                }
            }

            // Track the event in HockeyApp on supported platforms
            Action trackInHockeyApp = new Action(() =>
            {
                if (!HockeyApp.MetricsManager.Disabled)
                {
                    HockeyApp.MetricsManager.TrackEvent("logMessage", new Dictionary<string, string> { { "caller", caller }, { "message", message }, { "exception", (ex != null ? ex.ToString() : string.Empty) } }, new Dictionary<string, double>());
                }
            });
            Device.OnPlatform(trackInHockeyApp, trackInHockeyApp, null, null);
        }

        /// <summary>
        /// Logs an error to the debug console, records it in HockeyApp (if tracking is enabled) and displays it to the
        /// user.
        /// </summary>
        /// <param name="caller">An instance of <see cref="Page"/> containing the page this subroutine is called
        /// from.</param>
        /// <param name="message">The message to log and display.</param>
        /// <param name="ex">The exception associated to the message.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task LogWithErrorMessage(Page caller, string message, Exception ex)
        {
            Log(caller.GetType().ToString(), message, ex);
            await caller.DisplayAlert(Properties.Resources.Error, message, Properties.Resources.ButtonOK);
        }
    }
}

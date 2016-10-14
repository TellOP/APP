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

namespace TellOP.Tools
{
    using System;

    /// <summary>
    /// Debugger class
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
        /// Logs an error to the debug console.
        /// </summary>
        /// <param name="who">A string identifying the caller.</param>
        /// <param name="customMessage">A custom message.</param>
        public static void Log(string who, string customMessage)
        {
            if (who == null || customMessage == null)
            {
                return;
            }

            if (customMessage.Contains("\n"))
            {
                System.Diagnostics.Debug.WriteLine("[" + who + " Log]--------------------------------");
                System.Diagnostics.Debug.WriteLine("Message: " + customMessage);
                System.Diagnostics.Debug.WriteLine("[End " + who + " Log]--------------------------------");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[" + who + " Log]: " + customMessage);
            }
        }

        /// <summary>
        /// Log to the debug console the error.
        /// </summary>
        /// <param name="who">Identify the caller</param>
        /// <param name="customMessage">Custom message</param>
        public static void Log(object who, string customMessage)
        {
            if (who == null || customMessage == null)
            {
                return;
            }

            Log(who.GetType().Name, customMessage);
        }

        /// <summary>
        /// Log to the debug console the error.
        /// </summary>
        /// <param name="who">Identify the caller</param>
        /// <param name="customMessage">Custom message</param>
        /// <param name="ex">Optional exception to be logged</param>
        public static void Log(string who, string customMessage, API.UnsuccessfulAPICallException ex)
        {
            if (who == null || customMessage == null || ex == null)
            {
                return;
            }

            System.Diagnostics.Debug.WriteLine("[" + who + " Log]--------------------------------");
            System.Diagnostics.Debug.WriteLine("Message: " + customMessage);
            System.Diagnostics.Debug.WriteLine("API Response: " + ex.GetResponse);
            System.Diagnostics.Debug.WriteLine("Exception Stack Trace: ");
            System.Diagnostics.Debug.WriteLine(ex);
            System.Diagnostics.Debug.WriteLine("[End " + who + " Log]--------------------------------");
        }

        /// <summary>
        /// Log to the debug console the error.
        /// </summary>
        /// <param name="who">Identify the caller</param>
        /// <param name="ex">Optional exception to be logged</param>
        public static void Log(string who, Exception ex)
        {
            if (who == null || ex == null)
            {
                return;
            }

            System.Diagnostics.Debug.WriteLine("[" + who + " Log]--------------------------------");
            System.Diagnostics.Debug.WriteLine("Exception Stack Trace: ");
            System.Diagnostics.Debug.WriteLine(ex);
            System.Diagnostics.Debug.WriteLine("[End " + who + " Log]--------------------------------");
        }

        /// <summary>
        /// Log to the debug console the error.
        /// </summary>
        /// <param name="who">Identify the caller</param>
        /// <param name="ex">Optional exception to be logged</param>
        public static void Log(string who, API.UnsuccessfulAPICallException ex)
        {
            if (who == null || ex == null)
            {
                return;
            }

            System.Diagnostics.Debug.WriteLine("[" + who + " Log]--------------------------------");
            System.Diagnostics.Debug.WriteLine("API Response: " + ex.GetResponse);
            System.Diagnostics.Debug.WriteLine("Exception Stack Trace: ");
            System.Diagnostics.Debug.WriteLine(ex);
            System.Diagnostics.Debug.WriteLine("[End " + who + " Log]--------------------------------");
        }

        /// <summary>
        /// Log to the debug console the error.
        /// </summary>
        /// <param name="who">Identify the caller</param>
        /// <param name="customMessage">Custom message</param>
        /// <param name="ex">Optional exception to be logged</param>
        public static void Log(string who, string customMessage, Exception ex)
        {
            if (who == null || customMessage == null || ex == null)
            {
                return;
            }

            System.Diagnostics.Debug.WriteLine("[" + who + " Log]--------------------------------");
            System.Diagnostics.Debug.WriteLine("Message: " + customMessage);
            System.Diagnostics.Debug.WriteLine("Exception Stack Trace: ");
            System.Diagnostics.Debug.WriteLine(ex);
            System.Diagnostics.Debug.WriteLine("[End " + who + " Log]--------------------------------");
        }

        /// <summary>
        /// Log to the debug console the error.
        /// </summary>
        /// <param name="who">Identify the caller</param>
        /// <param name="customMessage">Custom message</param>
        /// <param name="ex">Optional exception to be logged</param>
        public static void Log(object who, string customMessage, Exception ex)
        {
            if (who == null || customMessage == null || ex == null)
            {
                return;
            }

            Log(who.GetType().Name, customMessage, ex);
        }

        /// <summary>
        /// Log to the debug console the error.
        /// </summary>
        /// <param name="who">Identify the caller</param>
        /// <param name="ex">Optional exception to be logged</param>
        public static void Log(object who, Exception ex)
        {
            if (who == null || ex == null)
            {
                return;
            }

            Log(who.GetType().Name, ex);
        }

        /// <summary>
        /// Log to the debug console and warn the user
        /// </summary>
        /// <param name="currentPage">Current page</param>
        /// <param name="customMessage">Custom message</param>
        /// <returns>>A <see cref="System.Threading.Tasks.Task"/> representing the asynchronous operation.</returns>
        public static async System.Threading.Tasks.Task LogWithErrorMessage(Xamarin.Forms.Page currentPage, string customMessage)
        {
            Log(currentPage.GetType().Name, customMessage);
            await currentPage.DisplayAlert("[Error] " + currentPage.GetType().Name, customMessage, "OK");
        }

        /// <summary>
        /// Log to the debug console and warn the user
        /// </summary>
        /// <param name="currentPage">Current page</param>
        /// <param name="customMessage">Custom message</param>
        /// <param name="ex">Optional exception to be logged</param>
        /// <returns>>A <see cref="System.Threading.Tasks.Task"/> representing the asynchronous operation.</returns>
        public static async System.Threading.Tasks.Task LogWithErrorMessage(Xamarin.Forms.Page currentPage, string customMessage, System.Exception ex)
        {
            Log(currentPage.GetType().Name, customMessage, ex);
            await currentPage.DisplayAlert("[Error] " + currentPage.GetType().Name, customMessage + "\nException Message: " + ex.Message, "OK");
        }
    }
}

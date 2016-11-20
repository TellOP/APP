// <copyright file="ConnectivityCheck.cs" company="University of Murcia">
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

namespace TellOP.Tools
{
    using System.Threading.Tasks;
    using Plugin.Connectivity;
    using Xamarin.Forms;

    /// <summary>
    /// A static class containing connectivity check methods.
    /// </summary>
    public static class ConnectivityCheck
    {
        /// <summary>
        /// Asks the user to enable connectivity on their phone if needed.
        /// </summary>
        /// <param name="callingPage">The <see cref="Page"/> calling this routine.</param>
        /// <returns><c>true</c> if connectivity is enabled, <c>false</c> otherwise.</returns>
        public static async Task<bool> AskToEnableConnectivity(Page callingPage)
        {
            while (!CrossConnectivity.Current.IsConnected)
            {
                Logger.Log(callingPage.GetType().ToString(), "Device is not connected, showing an error message");
                if (!await callingPage.DisplayAlert(Properties.Resources.ConnectivityMissing_Title, Properties.Resources.ConnectivityMissing_Text, Properties.Resources.ButtonRetry, Properties.Resources.ButtonCancel))
                {
                    return false;
                }
            }

            Logger.Log(callingPage.GetType().ToString(), "Showing the Authentication window");
            return true;
        }
    }
}

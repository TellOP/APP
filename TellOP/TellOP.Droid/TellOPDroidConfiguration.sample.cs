// <copyright file="TellOPDroidConfiguration.cs" company="University of Murcia">
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

namespace TellOP.Droid
{
    /// <summary>
    /// A class containing IDs and secrets specific to TellOP.Droid.
    /// </summary>
    internal sealed class TellOPDroidConfiguration
    {
        /// <summary>
        /// The application ID for the HockeyApp service.
        /// </summary>
        internal const string HockeyAppId = string.Empty;

        // TODO: fill in the following field with the HockeyApp secret.

        /// <summary>
        /// The secret for the HockeyApp service.
        /// </summary>
        internal const string HockeyAppSecret = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="TellOPDroidConfiguration"/> class.
        /// </summary>
        private TellOPDroidConfiguration()
        {
        }
    }
}

// <copyright file="ILocalize.cs" company="University of Murcia">
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
    using System.Globalization;

    /// <summary>
    /// Interface for cross-platform localization services.
    /// </summary>
    public interface ILocalize
    {
        /// <summary>
        /// Gets the current culture information.
        /// </summary>
        /// <returns>A <see cref="CultureInfo"/> object containing the current culture information.</returns>
        CultureInfo CurrentCultureInfo { get; }

        /// <summary>
        /// Sets the locale that is currently in use.
        /// </summary>
        void SetLocale();
    }
}

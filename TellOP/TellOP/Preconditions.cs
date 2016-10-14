// <copyright file="Preconditions.cs" company="University of Murcia">
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

    /// <summary>
    /// A class containing argument precondition checks.
    /// </summary>
    internal static class Preconditions
    {
        /// <summary>
        /// Checks if a value is null (throwing an exception in that case).
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to check.</param>
        /// <returns><paramref name="value"/></returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        public static T CheckNotNull<T>(T value)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            return value;
        }
    }
}

// <copyright file="AdelexOrderExtensions.cs" company="University of Murcia">
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

namespace TellOP.Api
{
    using System;

    /// <summary>
    /// Extensions to the <see cref="AdelexOrder"/> enumeration.
    /// </summary>
    public sealed class AdelexOrderExtensions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdelexOrderExtensions"/> class.
        /// </summary>
        private AdelexOrderExtensions()
        {
        }

        /// <summary>
        /// Gets the HTTP parameter corresponding to the specified order value.
        /// </summary>
        /// <param name="order">The order value.</param>
        /// <returns>The HTTP parameter to be used in the <see cref="AdelexApi"/> API controller.</returns>
        public static string GetHttpParam(AdelexOrder order)
        {
            switch (order)
            {
                case AdelexOrder.Alphabetical:
                    return "alphabetical";
                case AdelexOrder.Frequency:
                    return "frequency";
                default:
                    throw new ArgumentException("The order parameter is not a member of AdelexOrder", "order");
            }
        }
    }
}

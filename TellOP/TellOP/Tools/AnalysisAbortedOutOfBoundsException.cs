// <copyright file="AnalysisAbortedOutOfBoundsException.cs" company="University of Murcia">
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
    /// Exception class needed in order to distinguish exception in the HockeyApp event log.
    /// </summary>
    public class AnalysisAbortedOutOfBoundsException : OperationCanceledException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnalysisAbortedOutOfBoundsException"/> class.
        /// </summary>
        /// <param name="message">See <see cref="OperationCanceledException.OperationCanceledException(string)"/></param>
        public AnalysisAbortedOutOfBoundsException(string message)
            : base(message)
        {
        }
    }
}

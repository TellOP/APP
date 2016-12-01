﻿// <copyright file="DictionarySearchExercise.cs" company="University of Murcia">
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

namespace TellOP.DataModels.Activity
{
    /// <summary>
    /// A dictionary search fictitious exercise (so that the existing exercise
    /// infrastructure can be kept and used effectively).
    /// </summary>
    public class DictionarySearchExercise : Exercise
    {
        /// <summary>
        /// Gets or sets the word that was searched for.
        /// </summary>
        public string Word { get; set; }
    }
}

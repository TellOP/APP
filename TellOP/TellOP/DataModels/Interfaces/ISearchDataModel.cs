// <copyright file="ISearchDataModel.cs" company="University of Murcia">
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

namespace TellOP.DataModels
{
    using System.ComponentModel;

    /// <summary>
    /// Interface for any search data model.
    /// </summary>
    public interface ISearchDataModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets a value indicating whether the search is enabled or not.
        /// </summary>
        bool IsSearchEnabled { get; }

        /// <summary>
        /// Search for a word
        /// </summary>
        /// <param name="word">Term to be searched.</param>
        void SearchForWord(string word);
    }
}

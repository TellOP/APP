// <copyright file="Grouping.cs" company="University of Murcia">
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

namespace TellOP.DataModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using DataModels;

    /// <summary>
    /// A class used to enable groups in ListView (XAML).
    /// </summary>
    /// <remarks>It is essential that each group directly derives from a collection to which individual item access is
    /// possible. Collections in which each item can be accessed only via a property are unsupported, as the ListView
    /// in the XAML markup won't find the items.</remarks>
    /// <typeparam name="T">The value type.</typeparam>
    public class Grouping<T> : ObservableCollection<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Grouping{T}"/> class.
        /// </summary>
        /// <param name="groupName">The name of the group.</param>
        /// <param name="groupShortName">The short name of the group.</param>
        /// <param name="collection">An <see cref="IEnumerable{Exercise}"/> containing the exercises in the
        /// group.</param>
        public Grouping(string groupName, string groupShortName, IEnumerable<T> collection)
            : base(collection)
        {
            this.GroupName = groupName;
            this.GroupShortName = groupShortName;
        }

        /// <summary>
        /// Gets the group name.
        /// </summary>
        /// <see href="https://forums.xamarin.com/discussion/17976/listview-grouping-example-please">See this example
        /// on the Xamarin Forums.</see>
        public string GroupName { get; private set; }

        /// <summary>
        /// Gets the short name of the group.
        /// </summary>
        /// <see href="https://forums.xamarin.com/discussion/17976/listview-grouping-example-please">See this example
        /// on the Xamarin Forums.</see>
        public string GroupShortName { get; private set; }
    }
}

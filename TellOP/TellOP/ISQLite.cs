﻿// <copyright file="ISQLite.cs" company="University of Murcia">
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
    /// <summary>
    /// An interface to get a SQLite connection string given the database name.
    /// </summary>
    public interface ISQLite
    {
        /// <summary>
        /// Gets a SQLite database connection given a database name. The database will be loaded from the application's
        /// assets (and copied in an accessible position if necessary).
        /// </summary>
        /// <param name="databaseName">The database file name.</param>
        /// <returns>The required connection string.</returns>
        string GetConnectionString(string databaseName);
    }
}

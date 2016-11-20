// <copyright file="SQLiteImplementation.cs" company="University of Murcia">
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

[assembly: Xamarin.Forms.Dependency(typeof(TellOP.iOS.SQLiteImplementation))]

#pragma warning disable SA1300
namespace TellOP.iOS
#pragma warning restore SA1300
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Security.Cryptography;
    using Foundation;

    /// <summary>
    /// An iOS implementation of the <see cref="ISQLite"/> interface.
    /// </summary>
    public class SQLiteImplementation : ISQLite
    {
        /// <summary>
        /// Gets a SQLite database connection given a database name. The database will be loaded from the application's
        /// assets (and copied in an accessible position if necessary).
        /// </summary>
        /// <param name="databaseName">The database file name.</param>
        /// <returns>The required connection string.</returns>
        [SuppressMessage("Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes", Justification = "The using directives take care of this")]
        public string GetConnectionString(string databaseName)
        {
            // Copy the database to the "Local application data" folder. (This is required because we can not load a
            // SQLite database directly from the iOS package; SQLite requires R/W access and we would like to avoid
            // memory backing).
            // TODO: do we need to add "Tell-OP" at the end? See
            // https://developer.xamarin.com/guides/ios/application_fundamentals/working_with_the_file_system/
            string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", "Application Support");
            string databasePath = Path.Combine(directoryPath, databaseName);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Check if the database is not present or if a new revision is available in the app package. Note that
            // asset streams do not support seeking, so we have to open the file twice in case we need to copy it.
            char[] splitChars = { '.' };
            string[] splitDBName = databaseName.Split(splitChars);
            string splitName;
            string splitExt;

            if (splitChars.GetLength(0) == 1)
            {
                splitName = splitDBName[0];
                splitExt = string.Empty;
            }
            else
            {
                splitName = splitDBName[0];
                splitExt = splitDBName[1];
            }

            string assetPath = NSBundle.MainBundle.PathForResource(splitName, splitExt);

            bool needsCopy = false;
            if (!File.Exists(databasePath))
            {
                needsCopy = true;
            }
            else
            {
                byte[] appDbHash;
                byte[] localDbHash;

                using (Stream appDb = new FileStream(assetPath, FileMode.Open))
                {
                    appDbHash = this.Sha256Stream(appDb);
                    appDb.Close();
                }

                using (Stream localDb = new FileStream(databasePath, FileMode.Open))
                {
                    localDbHash = this.Sha256Stream(localDb);
                    localDb.Close();
                }

                needsCopy = !appDbHash.Equals(localDbHash);
            }

            if (needsCopy)
            {
                File.Copy(assetPath, databasePath);
            }

            return databasePath;
        }

        /// <summary>
        /// Gets the SHA256 hash of a stream.
        /// </summary>
        /// <param name="input">The stream.</param>
        /// <returns>The SHA256 hash of the contents of <paramref name="input"/>, in array form.</returns>
        private byte[] Sha256Stream(Stream input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return sha256Hash.ComputeHash(input);
            }
        }
    }
}

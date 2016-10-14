// <copyright file="WordSearchUtilities.cs" company="University of Murcia">
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

namespace TellOP.DataModels
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A class containing some utilities for managing word search results.
    /// </summary>
    public sealed class WordSearchUtilities
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WordSearchUtilities"/> class.
        /// </summary>
        private WordSearchUtilities()
        {
        }

        /// <summary>
        /// Given a list of words, returns the one that is the most probable
        /// match.
        /// </summary>
        /// <param name="wordList">An <see cref="IList{IWord}"/> object
        /// containing the list of possible words.</param>
        /// <returns>An <see cref="IWord"/> object containing the most probable
        /// match.</returns>
        /// <exception cref="ArgumentNullException">Thrown in case
        /// <paramref name="wordList"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown in case
        /// <paramref name="wordList"/> is empty.</exception>
        public static IWord GetMostProbable(IList<IWord> wordList)
        {
            if (wordList == null)
            {
                throw new ArgumentNullException("wordList");
            }

            foreach (IWord w in wordList)
            {
                // TODO: use a better algorithm (e.g. frequency analysis).
                // Just return the first word for now.
                Tools.Logger.Log("WordSearchUtilities", "Choosen word:\t\t(" + w.Term + " as " + w.PartOfSpeech + ")");
                return w;
            }

            throw new ArgumentException("The word list can not be empty", "wordList");
        }

        /// <summary>
        /// Given a list of words, returns the one that is the most probable
        /// match.
        /// </summary>
        /// <param name="wordList">An <see cref="IEnumerable{IWord}"/> object
        /// containing the list of possible words.</param>
        /// <returns>An <see cref="IWord"/> object containing the most probable
        /// match.</returns>
        /// <exception cref="ArgumentNullException">Thrown in case
        /// <paramref name="wordList"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown in case
        /// <paramref name="wordList"/> is empty.</exception>
        public static IWord GetMostProbable(IEnumerable<IWord> wordList)
        {
            if (wordList == null)
            {
                throw new ArgumentNullException("wordList");
            }

            foreach (IWord w in wordList)
            {
                // TODO: use a better algorithm (e.g. frequency analysis).
                // Just return the first word for now.
                return w;
            }

            throw new ArgumentException("The word list can not be empty", "wordList");
        }
    }
}

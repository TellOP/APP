// <copyright file="OAuthAccountStoreFactory.cs" company="University of Murcia">
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
    using Xamarin.Auth;

    /// <summary>
    /// A factory for <see cref="AccountStore"/> (overrides account store
    /// creation on UWP, which is only partially supported by
    /// <see cref="Xamarin.Auth"/> at this time).
    /// </summary>
    public sealed class OAuthAccountStoreFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthAccountStoreFactory"/> class.
        /// </summary>
        private OAuthAccountStoreFactory()
        {
        }

        /// <summary>
        /// Gets or sets the functor that creates a new instance of
        /// <see cref="AccountStore"/>.
        /// </summary>
        public static Func<AccountStore> Create { get; set; }
            = () => AccountStore.Create();
    }
}

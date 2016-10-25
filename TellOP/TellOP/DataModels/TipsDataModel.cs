// <copyright file="TipsDataModel.cs" company="University of Murcia">
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
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Api;
    using APIModels;
    using Enums;

    /// <summary>
    /// The data model for application tips.
    /// </summary>
    public class TipsDataModel : INotifyPropertyChanged
    {
        // Note: we follow the NotifyTaskCompletion sketch given at https://msdn.microsoft.com/en-us/magazine/dn605875.aspx
        // because we have two properties (ApplicationTips and SingleRandomTip) that depend on the result of the tip
        // retrieval asynchronous task (so we can not just implement INotifyTaskCompletion, SingleRandomTip would
        // need to wait on the result or at least listen to the PropertyChanged event).

        /// <summary>
        /// The task used to retrieve tips from the server.
        /// </summary>
        private Task<IList<Tip>> _tipsTask;

        /// <summary>
        /// A list of all available application tips.
        /// </summary>
        private ReadOnlyObservableCollection<Tip> _applicationTips;

        /// <summary>
        /// A lock for the application tips object.
        /// </summary>
        private object _appLock;

        /// <summary>
        /// Initializes a new instance of the <see cref="TipsDataModel"/> class.
        /// </summary>
        public TipsDataModel()
        {
            this._applicationTips = new ReadOnlyObservableCollection<Tip>(new ObservableCollection<Tip>(new List<Tip>()));
            this._appLock = new object();
            var dummy = this.RefreshTipsAsync();
        }

        /// <summary>
        /// Fired when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets a read-only list of application tips.
        /// </summary>
        public ReadOnlyObservableCollection<Tip> ApplicationTips
        {
            get
            {
                lock (this._appLock)
                {
                    return this._applicationTips;
                }
            }

            private set
            {
                lock (this._appLock)
                {
                    this._applicationTips = value;
                }

                this.OnPropertyChanged("ApplicationTips");
            }
        }

        /// <summary>
        /// Gets a single random tip from the list of all available tips.
        /// </summary>
        public Tip SingleRandomTip
        {
            get
            {
                lock (this._appLock)
                {
                    if (this._applicationTips.Count == 0)
                    {
                        return null;
                    }

                    Random rnd = new Random();
                    return this._applicationTips[rnd.Next(this._applicationTips.Count - 1)];
                }
            }
        }

        /// <summary>
        /// Gets the error message associated with the tip retrieval task.
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return this._tipsTask.Exception.InnerException != null ? this._tipsTask.Exception.InnerException.Message : null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the tip retrieval operation is faulted.
        /// </summary>
        public bool IsFaulted
        {
            get
            {
                return this._tipsTask.IsFaulted;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the tip retrieval operation is not complete.
        /// </summary>
        public bool IsNotCompleted
        {
            get
            {
                return !this._tipsTask.IsCompleted;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the tip retrieval operation was completed successfully.
        /// </summary>
        public bool IsSuccessfullyCompleted
        {
            get
            {
                return this._tipsTask.Status == TaskStatus.RanToCompletion;
            }
        }

        /// <summary>
        /// Fired when a property of this class is changed.
        /// </summary>
        /// <param name="propertyName">The property that was changed,</param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Refreshes the list of available tips.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. </returns>
        private async Task RefreshTipsAsync()
        {
            try
            {
                // TODO: allow selecting the language and level classification
                Tips tipsEndpoint = new Tips(App.OAuth2Account, SupportedLanguage.English, LanguageLevelClassification.B1);
                this._tipsTask = Task.Run(async () => await tipsEndpoint.CallEndpointAsObjectAsync());
                await this._tipsTask;
            }
            catch
            {
            }

            this.OnPropertyChanged("IsNotCompleted");

            if (this._tipsTask.IsFaulted)
            {
                this.OnPropertyChanged("IsFaulted");
                this.OnPropertyChanged("ErrorMessage");
            }

            if (this._tipsTask.IsCanceled || this._tipsTask.IsFaulted)
            {
                this.ApplicationTips = new ReadOnlyObservableCollection<Tip>(new ObservableCollection<Tip>(new List<Tip>()));
            }
            else
            {
                this.ApplicationTips = new ReadOnlyObservableCollection<Tip>(new ObservableCollection<Tip>(this._tipsTask.Result));
                this.OnPropertyChanged("IsSuccessfullyCompleted");
            }

            this.OnPropertyChanged("SingleRandomTip");
        }
    }
}

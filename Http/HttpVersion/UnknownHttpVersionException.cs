#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using System;

namespace Http.Version
{
    /// <summary>
    /// This class represents an exception used to indicate that unknown HTTP version has been requested.
    /// </summary>
    [Serializable]
    public class UnknownHttpVersionException : Exception
    {
        /// <summary>
        /// This is the default constructor used to initialize an instance of this class.
        /// </summary>
        public UnknownHttpVersionException() : base() { }

        /// <summary>
        /// This constructor is used to set the exception message to the given <paramref name="message" />.
        /// </summary>
        /// <param name="message">
        /// This string represents the message that will be used as the exception message.
        /// </param>
        public UnknownHttpVersionException(string message) : base(message) { }
    }
}

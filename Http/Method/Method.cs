#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

namespace Http.Method
{
    /// <summary>
    /// This class is used to represent HTTP methods used inside HTTP request messages.
    /// </summary>
    public class Method
    {
        /// <summary>
        /// This property holds the value which identifies the HTTP method represented by this instance.
        /// </summary>
        public MethodType Type { get; }

        /// <summary>
        /// This constructor is used to set the <see cref="Type" /> property to the given
        /// <paramref name="type" />.
        /// </summary>
        /// <param name="type">
        /// This value represents a HTTP method.
        /// </param>
        internal Method(MethodType type)
        {
            Type = type;
        }
    }
}

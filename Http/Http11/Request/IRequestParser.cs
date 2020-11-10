#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using System.Collections.Generic;

namespace Http.Http11.Request
{
    /// <summary>
    /// This interface can be used to parse an HTTP request into an object of type <see cref="IRequest" />.
    /// </summary>
    public interface IRequestParser
    {
        /// <summary>
        /// This property returns the current status of the parser.
        /// </summary>
        /// <value>
        /// The value of this property can indicate whether or not the parser is ready to create an
        /// <see cref="IRequest" /> object, or if an error occurred.
        /// </value>
        ParserStatus Status { get; }

        /// <summary>
        /// This property contains the error message with the information about an error that occurred during request
        /// parsing.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// If the value of <see cref="Status" /> is not <see cref="ParserStatus.Error" /> then any operation on this
        /// property will result in an exception.
        /// </exception>
        string Error { get; }

        /// <summary>
        /// This method feeds the parser with the given chunk of data.
        /// </summary>
        /// <param name="data">
        /// This is the chunk of data that will be fed to the parser.
        /// </param>
        void FeedData(IList<byte> data);

        /// <summary>
        /// This method clears the parser from all data and sets the object into its initial state.
        /// </summary>
        void Clear();
    }
}

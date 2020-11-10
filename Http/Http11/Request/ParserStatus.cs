#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

namespace Http.Http11.Request
{
    /// <summary>
    /// This enum defines the statuses of <see cref="IRequestParser">the HTTP request parser</see>.
    /// </summary>
    public enum ParserStatus
    {
        /// <summary>
        /// This value indicates that the parser does not contain any data.
        /// </summary>
        Empty,

        /// <summary>
        /// This value indicates that the parser contains some data, but it cannot be parsed into a request. More data
        /// required.
        /// </summary>
        Unsatisfied,

        /// <summary>
        /// This value indicates that the parser contains a full request and it's ready to create one. It possible that
        /// the parser also contains the data of another request.
        /// </summary>
        Ready,

        /// <summary>
        /// This value indicates that the parser received an invalid data and it's not possible to parse a request out
        /// of it.
        /// </summary>
        Error,
    }
}

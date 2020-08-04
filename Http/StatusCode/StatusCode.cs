#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

namespace Http.StatusCode
{
    /// <summary>
    /// This class represents status-codes used by the HTTP Server to inform the peers about the status of the requested
    /// resource.
    /// </summary>
    /// <seealso href="https://tools.ietf.org/html/rfc7230#section-3.1.2">RFC 7230 - 3.1.2 Status Line</seealso>
    public class StatusCode
    {
        /// <summary>
        /// This property contains the number representing HTTP status-code of this instance.
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// This constructor is used to set <see cref="Value" /> property based on the given <paramref name="value" />.
        /// </summary>
        /// <param name="value">
        /// This value represents a valid HTTP status-code.
        /// </param>
        internal StatusCode(int value)
        {
            Value = value;
        }
    }
}

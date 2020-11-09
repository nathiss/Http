#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using System.Collections.Generic;
using System.Text;

namespace Http.Common.MessageBody
{
    /// <summary>
    /// This interface proides the functionality to access message body of a HTTP request or a HTTP response.
    /// </summary>
    public interface IMessageBody
    {
        /// <summary>
        /// This method returns the content of the message body represented as a sequence of bytes.
        /// </summary>
        /// <returns>
        /// The content of the message body represented as a sequence of bytesis returned.
        /// </returns>
        List<byte> GetContent();

        /// <summary>
        /// This method returns the content of the message body represented as a string encoded with the default
        /// encoding.
        /// </summary>
        /// <returns>
        /// The content of the message body represented as a string encoded with the default encoding is returned.
        /// </returns>
        /// <remarks>
        /// This method uses the default platform encoding provided by this .NET implementation.
        /// <see cref="Encoding.Default" />
        /// </remarks>
        /// <seealso cref="GetContentString(Encoding)" />
        string GetContentString();

        /// <summary>
        /// This method returns the content of the message body represented as a string encoded with the given encoding.
        /// </summary>
        /// <param name="encoding">The encoding used to encode the message body as a string.</param>
        /// <returns>
        /// The content of the message body represented as a string encoded with the given encoding is returned.
        /// </returns>
        string GetContentString(Encoding encoding);
    }
}

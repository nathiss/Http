#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace Http.Common.MessageBody
{
    /// <summary>
    /// This class implements the message body of a HTTP request or a HTTP response.
    /// </summary>
    public class HttpMessageBody : IMessageBody
    {
        /// <summary>
        /// This method sets the message body to the given <paramref name="content" />.
        /// </summary>
        /// <param name="content">
        /// This is the new message body content.
        /// </param>
        public void SetContent(List<byte> content)
        {
            _content = content;
        }

        /// <summary>
        /// This method sets the message body to the given <paramref name="content" />, serialized to byte sequence
        /// using the default encoding.
        /// </summary>
        /// <param name="content">
        /// This is the message body content encoded as string.
        /// </param>
        /// <remarks>
        /// This method uses the default platform encoding provided by this .NET implementation.
        /// <see cref="Encoding.Default" />
        /// </remarks>
        /// <seealso cref="GetContentString(Encoding)" />
        public void SetContent(string content)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            SetContent(content, Encoding.Default);
        }

        /// <summary>
        /// This method sets the message body to the given <paramref name="content" />, serialized to byte sequence
        /// using <paramref name="encoding" />.
        /// </summary>
        /// <param name="content">
        /// This is the message body content encoded as string.
        /// </param>
        /// <param name="encoding">
        /// This is the encoding of the given <paramref name="content" />.
        /// </param>
        public void SetContent(string content, Encoding encoding)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (encoding is null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            var byteArray = encoding.GetBytes(content);
            _content = new List<byte>(byteArray);
        }

        /// <inheritdoc />
        public bool HasBody => _content == null;

        /// <inheritdoc />
        public int Count
        {
            get => _content?.Count ?? 0;
        }

        /// <inheritdoc />
        public List<byte> GetContent()
        {
            return _content;
        }

        /// <inheritdoc />
        public string GetContentString()
        {
            return GetContentString(Encoding.Default);
        }

        /// <inheritdoc />
        public string GetContentString(Encoding encoding)
        {
            if (encoding is null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            return encoding.GetString(_content.ToArray());
        }

        /// <summary>
        /// This is the message body content represented as a sequence of bytes.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc7230#section-3.3">RFC 7230 (Section 3.3)</seealso>
        private List<byte> _content;
    }
}

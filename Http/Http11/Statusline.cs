#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using System;
using Http.Common.StatusCode;
using Http.Common.Version;

namespace Http.Http11
{
    /// <summary>
    /// This class provides the functionality to operate on the objects representing HTTP status-lines.
    /// </summary>
    public class Statusline : IStatusLine
    {
        /// <inheritdoc />
        public HttpVersion HttpVersion
        {
            get => _httpVersion;
            set => _httpVersion = value ?? throw new ArgumentNullException(nameof(HttpVersion));
        }

        /// <inheritdoc />
        public HttpStatusCode StatusCode
        {
            get => _httpStatusCode;
            set => _httpStatusCode = value ?? throw new ArgumentNullException(nameof(StatusCode));
        }

        /// <inheritdoc />
        public string ReasonPhrase => reasonphraseRepository.GetReasonPhrase(StatusCode);

        /// <summary>
        /// This field contains the HTTP version of the status line.
        /// </summary>
        private HttpVersion _httpVersion;

        /// <summary>
        /// This field contains the status code of the status line.
        /// </summary>
        private HttpStatusCode _httpStatusCode;

        /// <summary>
        /// This field is used to translate <see cref="StatusCode" /> into a string representing reason-phrase.
        /// </summary>
        // TODO: Use inversion of control?
        private static readonly IReasonPhraseRepository reasonphraseRepository = new ReasonPhraseRepository();
    }
}

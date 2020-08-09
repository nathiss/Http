#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using Http.StatusCode;
using Http.Version;

namespace Http.Http11
{
    /// <summary>
    /// This class provides the functionality to operate on the objects representing HTTP status-lines.
    /// </summary>
    public class Statusline : IStatusLine
    {
        /// <inheritdoc />
        public HttpVersion HttpVersion { get; set; }

        /// <inheritdoc />
        public HttpStatusCode StatusCode { get; set; }

        /// <inheritdoc />
        public string ReasonPhrase => reasonphraseRepository.GetReasonPhrase(StatusCode);

        /// <summary>
        /// This field is used to translate <see cref="StatusCode" /> into a string representing reason-phrase.
        /// </summary>
        // TODO: Use inversion of control?
        private static readonly IReasonPhraseRepository reasonphraseRepository = new ReasonPhraseRepository();
    }
}

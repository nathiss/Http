#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using Http.Common.StatusCode;

namespace Http.Http11
{
    /// <summary>
    /// This interface provides the functionality to map <see cref="HttpStatusCode" /> objects into string representing
    /// reason-phrase.
    /// </summary>
    public interface IReasonPhraseRepository
    {
        /// <summary>
        /// This method returns a string representing reason-phrase matched from the given
        /// <paramref name="statusCode" /> object.
        /// </summary>
        /// <param name="statusCode">
        /// This is the status-code which will be translated into reason-phrase.
        /// </param>
        /// <returns>
        /// A string representing reason-phrase matched from the given <paramref name="statusCode" /> object is
        /// returned.
        /// </returns>
        string GetReasonPhrase(HttpStatusCode statusCode);
    }
}

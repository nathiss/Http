#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

namespace Http.HttpVersion
{
    /// <summary>
    /// This interface defines the functionality to get access to <see cref="HttpVersion" /> objects.
    /// </summary>
    public interface IHttpVersionRepository
    {
        /// <summary>
        /// This method returns a <see cref="HttpVersion" /> object which matches the given <paramref name="version" />.
        /// </summary>
        /// <param name="version">
        /// This is the HTTP-version used to match <see cref="HttpVersion" /> object.
        /// </param>
        /// <returns>
        /// A <see cref="HttpVersion" /> object which matches the given <paramref name="version" /> is returned.
        /// </returns>
        /// <exception cref="UnknownHttpVersion">
        /// An exception of this type is thrown when the given <paramref name="version" /> is unrecognized.
        /// </exception>
        HttpVersion Get(HttpVersionType version);
    }
}

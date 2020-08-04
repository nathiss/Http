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
    /// This interface defines the functionality to access <see cref="StatusCode" /> which are known.
    /// </summary>
    public interface IStatusCodeRepository
    {
        /// <summary>
        /// This method returns a <see cref="StatusCode" /> object which matches the given
        /// <paramref name="statusCodeValue" />.
        /// </summary>
        /// <param name="statusCodeValue">
        /// This is the number which will be used to get the right <see cref="StatusCode" /> object.
        /// </param>
        /// <returns>
        /// A <see cref="StatusCode" /> object which matches the given <paramref name="statusCodeValue" /> is returned.
        /// </returns>
        /// <exception cref="UnknownStatusCodeException">
        /// An exception of this type is thrown when the given <paramref name="statusCodeValue" /> is unrecognized.
        /// </exception>
        StatusCode GetStatusCode(int statusCodeValue);
    }
}

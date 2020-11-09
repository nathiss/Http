#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

namespace Http.Common.Headers
{
    /// <summary>
    /// This interface defines the functionality used to normalize header-fields' field-names case.
    /// </summary>
    /// <remarks>
    /// For HTTP/1.1 see: <see href="https://tools.ietf.org/html/rfc7231">RFC 7231</see>
    /// </remarks>
    public interface IFieldNameNormalizer
    {
        /// <summary>
        /// This method returns a string which is the normalized version of the given <paramref name="fieldName" />.
        /// </summary>
        /// <param name="fieldName">
        /// This is the header-field's field-name which will be normalized.
        /// </param>
        /// <returns>
        /// A string which is the normalized version of the given <paramref name="fieldName" /> is returned.
        /// </returns>
        string Normalize(string fieldName);
    }
}

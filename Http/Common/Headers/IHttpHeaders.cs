#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using System.Collections.Generic;

namespace Http.Common.Headers
{
    /// <summary>
    /// This interface implements the functionality to access the header section of HTTP requests and HTTP responses.
    /// </summary>
    /// <seealso cref="HeaderField" />
    public interface IHttpHeaders
    {
        /// <summary>
        /// This indexer is used to:
        ///     <list type="bullet">
        ///         <item>
        ///             Get field-value of a header-field which field-name is equal to the given
        ///             <paramref name="fieldName" /> (case-insensitive).
        ///         </item>
        ///         <item>
        ///             Set field-value for a header-field of which field-name is equal to the given
        ///             <paramref name="fieldName" />. Case will be formalized and therefore not-preserved.
        ///         </item>
        ///     </list>
        /// </summary>
        /// <exception type="UnknownHeaderFieldException">
        /// An exception of this type is thrown then the user tried to get a field-value of non-existing header-field.
        /// </exception>
        /// <remarks>
        /// It not possible to set multiple headers with the same field-name. Setting the second field will result in
        /// overwritting of the first one. As mentioned in
        /// <see href="https://tools.ietf.org/html/rfc7230#section-3.2.2">RFC 7230 (Section 3.2.2)</see> "Set-Cookie"
        /// header is well-known exception, altough this library does not support it. Thus any server using this library
        /// must combine "Set-Cookie" headers in order to set this header-field.
        /// </remarks>
        /// <seealso href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Set-Cookie#Syntax" />
        string this[string fieldName] { get; set; }

        /// <summary>
        /// This method returns an indication of whether or not the header exists in this container.
        /// </summary>
        /// <param name="fieldName">
        /// The field-name of the header.
        /// </param>
        /// <returns>
        /// An indication of whether or not the header exists in this container is returned.
        /// </returns>
        bool HasHeader(string fieldName);

        /// <summary>
        /// This property returns the amount of headers stored in this collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// This method clears the headers list.
        /// </summary>
        void Clear();

        /// <summary>
        /// This methods a collection of <see cref="HeaderField" /> each representing one HTTP header-field.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="HeaderField" /> each representing one HTTP header-field is returned.
        /// </returns>
        IList<HeaderField> ToList();
    }
}

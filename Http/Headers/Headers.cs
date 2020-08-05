#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace Http.Headers
{
    /// <summary>
    /// This class represents the headers section of HTTP requests and HTTP responses.
    /// </summary>
    public class Headers : IHeaders
    {
        /// <inheritdoc />
        public string this[string fieldName]
        {
            get
            {
                try
                {
                    return GetHeaderFieldByName(fieldName).Value;
                }
                catch (InvalidOperationException)
                {
                    throw new UnknownHeaderFieldException("A header with field-name \"{fieldName}\" does not exist.");
                }
            }
            set
            {
                try
                {
                    var header = GetHeaderFieldByName(fieldName);
                    header.Value = value;
                }
                catch (InvalidOperationException)
                {
                    headers.Add(HeaderField.Create(fieldName, value));
                }
            }
        }

        /// <inheritdoc />
        public int Count => headers.Count;

        /// <summary>
        /// This method returns a header-field which field-name is equal to the given <paramref name="fieldName" />
        /// (case-insensitive).
        /// </summary>
        /// <param name="fieldName">
        /// This is the field-name of the header-field that we-re looking for.
        /// </param>
        /// <returns>
        ///  A header-field which field-name is equal to the given <paramref name="fieldName" /> (case-insensitive) is
        /// returned.
        /// </returns>
        /// <exception type="InvalidOperationException">
        /// An exception of this type is thrown by methods which are called by this method if the header-field cannot
        /// be found.
        /// </exception>
        private HeaderField GetHeaderFieldByName(string fieldName)
        {
            return headers.First(
                headerField => string.Compare(fieldName, headerField.Name, StringComparison.OrdinalIgnoreCase) == 0
            );
        }

        /// <summary>
        /// This collection contains all header-field of this instance of <see cref="Headers" />.
        /// </summary>
        /// <typeparam name="HeaderField">
        /// This type represents a single header-field.
        /// </typeparam>
        private readonly IList<HeaderField> headers = new List<HeaderField>();
    }
}

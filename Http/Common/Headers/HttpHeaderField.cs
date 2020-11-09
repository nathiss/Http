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
    /// This class represents HTTP header-field which are used inside HTTP requests and HTTP responses.
    /// </summary>
    /// <remarks>
    ///     <list type="bullet">
    ///         <item>
    ///             For HTTP/1.1 see:
    ///             <see href="https://tools.ietf.org/html/rfc7230#section-3.2">RFC (Section 3.2. - Header Fields)</see>
    ///         </item>
    ///         <item>
    ///             For HTTP/2 see:
    ///             TODO: Link HTTP/2 documentation about Header Fields.
    ///         </item>
    ///     </list>
    /// </remarks>
    public class HeaderField
    {
        /// <summary>
        /// This property represents the field-name of this header-field.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// This property represents the field-value of this header-field.
        /// </summary>
        public string Value { get; internal set; }

        /// <summary>
        /// This constructor sets the values of <see cref="Name" /> and <see cref="Value" /> properties.
        /// </summary>
        /// <param name="name">
        /// This string represents the field-name of this header-field.
        /// </param>
        /// <param name="value">
        /// This string represents the field-value of this header-field.
        /// </param>
        private HeaderField(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// This method creates a new instance of <see cref="HeaderField" /> and sets its <see cref="Name" /> and
        /// <see cref="Value" /> properties to the given <paramref name="name" /> and <paramref name="value" />
        /// respectively.
        /// </summary>
        /// <param name="name">
        /// This string represents the field-name of this header-field.
        /// </param>
        /// <param name="value">
        /// This string represents the field-value of this header-field.
        /// </param>
        /// <returns>
        /// A new instance of <see cref="HeaderField" /> and sets its <see cref="Name" /> and <see cref="Value" />
        /// properties to the given <paramref name="name" /> and <paramref name="value" /> respectively is returned.
        /// </returns>
        internal static HeaderField Create(string name, string value)
        {
            return new HeaderField(name, value);
        }
    }
}

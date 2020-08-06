#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using System;
using System.Linq;

namespace Http.Extentions
{
    /// <summary>
    /// This class provides the extension method used inside Http project.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// This method return a string created from transformed <paramref name="str" /> so that the first character is
        /// uppercase and all other characters are lowercase.
        /// </summary>
        /// <param name="str">
        /// This is the string which will be transformed.
        /// </param>
        /// <returns>
        /// A string created from transformed <paramref name="str" /> so that the first character is uppercase and all
        /// other characters are lowercase is returned.
        /// </returns>
        internal static string FirstCharToUpper(this string str)
        {
            return str switch
            {
                null => throw new ArgumentNullException(nameof(str)),
                "" => throw new ArgumentException($"{nameof(str)} cannot be empty", nameof(str)),
                _ => str.First().ToString().ToUpper() + str.Substring(1).ToLower(),
            };
        }
    }
}

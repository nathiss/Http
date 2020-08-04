#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

namespace Http.Method
{
    /// <summary>
    /// This interface defines the functionality to get access to <see cref="Method" /> objects.
    /// </summary>
    public interface IMethodRepository
    {
        /// <summary>
        /// This method returns a <see cref="Method" /> object which matches the given <paramref name="type" />.
        /// </summary>
        /// <param name="version">
        /// This is the method used to match <see cref="Method" /> object.
        /// </param>
        /// <returns>
        /// A <see cref="Method" /> object which matches the given <paramref name="type" /> is returned.
        /// </returns>
        /// <exception cref="UnknownMethodException">
        /// An exception of this type is thrown when the given <paramref name="type" /> is unrecognized.
        /// </exception>
        Method Get(MethodType type);
    }
}

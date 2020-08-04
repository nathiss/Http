#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using System.Collections.Generic;

namespace Http.Method
{
    /// <summary>
    /// This class implements the functionality used to access <see cref="Method" /> objects.
    /// </summary>
    public class MethodRepository : IMethodRepository
    {
        /// <inheritdoc />
        public Method GetMethod(MethodType type)
        {
            try
            {
                return KnownMethods[type];
            }
            catch (KeyNotFoundException)
            {
                throw new UnknownMethodException($"Unknown HTTP method: {type}.");
            }
        }

        /// <summary>
        /// This collection contains all known HTTP methods in the system.
        /// </summary>
        /// <value></value>
        private static readonly IDictionary<MethodType, Method> KnownMethods = new Dictionary<MethodType, Method>
        {
            { MethodType.Get, new Method(MethodType.Get) },
            { MethodType.Head, new Method(MethodType.Head) },
            { MethodType.Post, new Method(MethodType.Post) },
            { MethodType.Put, new Method(MethodType.Put) },
            { MethodType.Delete, new Method(MethodType.Delete) },
            { MethodType.Trace, new Method(MethodType.Trace) },
            { MethodType.Connect, new Method(MethodType.Connect) },
            { MethodType.Options, new Method(MethodType.Options) },
        };
    }
}

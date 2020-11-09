#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using System.Collections.Generic;

namespace Http.Common.Method
{
    /// <summary>
    /// This class implements the functionality used to access <see cref="HttpMethod" /> objects.
    /// </summary>
    public class HttpMethodRepository : IHttpMethodRepository
    {
        /// <inheritdoc />
        public HttpMethod GetMethod(HttpMethodType type)
        {
            try
            {
                return KnownMethods[type];
            }
            catch (KeyNotFoundException)
            {
                throw new UnknownHttpMethodException($"Unknown HTTP method: {type}.");
            }
        }

        /// <summary>
        /// This collection contains all known HTTP methods in the system.
        /// </summary>
        /// <value></value>
        private static readonly IDictionary<HttpMethodType, HttpMethod> KnownMethods = new Dictionary<HttpMethodType, HttpMethod>
        {
            { HttpMethodType.Get, new HttpMethod(HttpMethodType.Get) },
            { HttpMethodType.Head, new HttpMethod(HttpMethodType.Head) },
            { HttpMethodType.Post, new HttpMethod(HttpMethodType.Post) },
            { HttpMethodType.Put, new HttpMethod(HttpMethodType.Put) },
            { HttpMethodType.Delete, new HttpMethod(HttpMethodType.Delete) },
            { HttpMethodType.Trace, new HttpMethod(HttpMethodType.Trace) },
            { HttpMethodType.Connect, new HttpMethod(HttpMethodType.Connect) },
            { HttpMethodType.Options, new HttpMethod(HttpMethodType.Options) },
        };
    }
}

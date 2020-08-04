#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using System.Collections.Generic;

namespace Http.HttpVersion
{
    /// <summary>
    /// This class implements the functionality used to access <see cref="HttpVersion" /> objects.
    /// </summary>
    public class HttpVersionRepository : IHttpVersionRepository
    {
        /// <inheritdoc />
        public HttpVersion Get(HttpVersionType version)
        {
            try
            {
                return KnownHttpVersions[version];
            }
            catch (KeyNotFoundException)
            {
                throw new UnknownHttpVersion($"Unknown HTTP protocol version: {version}");
            }
        }

        /// <summary>
        /// This collection contains all knows version of the HTTP protocol known to this program.
        /// </summary>
        private static readonly IDictionary<HttpVersionType, HttpVersion> KnownHttpVersions =
        new Dictionary<HttpVersionType, HttpVersion>
        {
            { HttpVersionType.Http1_0, new HttpVersion(HttpVersionType.Http1_0) },
            { HttpVersionType.Http1_1, new HttpVersion(HttpVersionType.Http1_1) },
            { HttpVersionType.Http2_0, new HttpVersion(HttpVersionType.Http2_0) },
        };
    }
}

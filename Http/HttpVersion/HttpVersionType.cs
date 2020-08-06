#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion


namespace Http.Version
{
    /// <summary>
    /// This enum is used by <see cref="HttpVersion" /> to distinguish HTTP protocol versions.
    /// </summary>
    public enum HttpVersionType
    {
        /// <summary>
        /// This value represents HTTP/1.0.
        /// </summary>
        Http1_0,

        /// <summary>
        /// This value represents HTTP/1.1.
        /// </summary>
        Http1_1,

        /// <summary>
        /// This value represents HTTP/2.0.
        /// </summary>
        Http2_0,
    }
}

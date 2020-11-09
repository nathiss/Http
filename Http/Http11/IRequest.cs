#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using Http.Common.MessageBody;
using Http.Common.Method;
using Http.Common.Version;
using Uri;

namespace Http.Http11
{
    /// <summary>
    /// This interface provides the functionality to read a HTTP request.
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// This property contains the HTTP method used in the request.
        /// </summary>
        HttpMethodType Method { get; }

        /// <summary>
        /// This property contains the target URI of the requested resource.
        /// </summary>
        UniformResourceIdentifier Target { get; }

        /// <summary>
        /// This property contains the protocol version used in the request.
        /// </summary>
        HttpVersionType HttpVersion { get; }

        /// <summary>
        /// This method returns the value of a HTTP header identified by the given <paramref name="fieldName" />.
        /// </summary>
        /// <param name="fieldName">
        /// This is the name of the requested HTTP header.
        /// </param>
        /// <returns>
        /// The value of a HTTP header identified by the given <paramref name="fieldName" /> is returned. If the header
        /// does not exist in the request, this method returns null.
        /// </returns>
        string GetHeader(string fieldName);

        /// <summary>
        /// This property contains the message body of the request.
        /// </summary>
        IMessageBody MessageBody { get; }
    }
}

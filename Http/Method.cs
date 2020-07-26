﻿#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using System;
using System.Collections.Generic;

namespace Http
{
    /// <summary>
    /// This class represents the HTTP methods used for the client-server communication.
    /// </summary>
    /// <seealso cref="Type" />
    /// <seealso href="https://www.ietf.org/rfc/rfc2616.txt">RFC 2616</seealso>
    public class Method
    {
        /// <summary>
        /// This enum is used to distinguish <see cref="Schematics.HttpMethod">HttpMethod</see> objects.
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// The GET method means retrieve whatever information (in the form of an entity)
            /// is identified by the Request-URI.
            /// </summary>
            /// <remarks>
            /// If the Request-URI refers to a data-producing process, it is the produced
            /// data which shall be returned as the entity in the response and not the
            /// source text of the process, unless that text happens to be the output of
            /// the process.
            /// </remarks>
            Get,

            /// <summary>
            /// The HEAD method is identical to GET except that the server MUST NOT
            /// return a message-body in the response. The metainformation contained
            /// in the HTTP headers in response to a HEAD request SHOULD be identical
            /// to the information sent in response to a GET request.
            /// </summary>
            Head,

            /// <summary>
            /// The POST method is used to request that the origin server accept the
            /// entity enclosed in the request as a new subordinate of the resource
            /// identified by the Request-URI in the Request-Line.
            /// </summary>
            /// <remarks>
            ///     POST is designed to allow a uniform method to cover the following functions:
            ///     <list type="bullet">
            ///         <item>Annotation of existing resources;</item>
            ///         <item>
            ///             Posting a message to a bulletin board, newsgroup, mailing list,
            ///             or similar group of articles;
            ///         </item>
            ///         <item>
            ///             Providing a block of data, such as the result of submitting a
            ///             form, to a data-handling process;
            ///         </item>
            ///         <item>
            ///             Extending a database through an append operation.
            ///         </item>
            ///     </list>
            /// </remarks>
            Post,

            /// <summary>
            /// The PUT method requests that the enclosed entity be stored under the
            /// supplied Request-URI. If the Request-URI refers to an already
            /// existing resource, the enclosed entity SHOULD be considered as a
            /// modified version of the one residing on the origin server.
            /// </summary>
            Put,

            /// <summary>
            /// The DELETE method requests that the origin server delete the resource
            /// identified by the Request-URI. This method MAY be overridden by human
            /// intervention (or other means) on the origin server.
            /// </summary>
            /// <remarks>
            /// The client cannot be guaranteed that the operation has been carried out,
            /// even if the  status code returned from the origin server indicates that
            /// the action has been completed successfully. However, the server SHOULD
            /// NOT indicate success unless, at the time the response is given, it
            /// intends to delete the resource or move it to an inaccessible location.
            /// </remarks>
            Delete,

            /// <summary>
            /// The TRACE method is used to invoke a remote, application-layer loop-
            /// back of the request message. The final recipient of the request
            /// SHOULD reflect the message received back to the client as the
            /// entity-body of a 200 (OK) response.
            /// </summary>
            Trace,

            /// <summary>
            /// This specification reserves the method name CONNECT for use with a
            /// proxy that can dynamically switch to being a tunnel (e.g. SSL tunneling).
            /// </summary>
            Connect,

            /// <summary>
            /// The OPTIONS method represents a request for information about the
            /// communication options available on the request/response chain
            /// identified by the Request-URI. This method allows the client to
            /// determine the options and/or requirements associated with a resource,
            /// or the capabilities of a server, without implying a resource action
            /// or initiating a resource retrieval.
            /// </summary>
            /// <remarks>
            /// Responses to this method are not cacheable.
            /// </remarks>
            Options,
        }

        /// <summary>
        /// This method converts the given <paramref name="encodedHttpMethod"/> into a HttpMethod object.
        /// If the <paramref name="encodedHttpMethod"/> contains an invalid method, this method will
        /// return null.
        /// </summary>
        /// <param name="encodedHttpMethod">This is the encoded HTTP method.</param>
        /// <returns>
        /// A parsed HttpMethod object is returned. If the given <paramref name="encodedHttpMethod"/> contains
        /// and invalid HTTP method, the method will return null.
        /// </returns>
        public static Method FromString(string encodedHttpMethod)
        {
            if (string.IsNullOrWhiteSpace(encodedHttpMethod))
            {
                return null;
            }

            try
            {
                encodedHttpMethod = encodedHttpMethod.Trim().ToUpper();
                return ValidHttpMethodsDictionary[encodedHttpMethod];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// This property returns the HttpMethod object encoded into a string.
        /// </summary>
        public string EncodedMethod { get; }

        /// <summary>
        /// This property value represents the Method object.
        /// </summary>
        public Type MethodType { get; }

        /// <summary>
        /// This constructor creates an object from the given <paramref name="encodedMethod"/>.
        /// </summary>
        /// <param name="encodedMethod">
        /// This is the encoded HTTP Method. The method must be a valid HTTP Method and
        /// must be capitalized.
        /// </param>
        private Method(string encodedMethod)
        {
            EncodedMethod = encodedMethod;
            MethodType = Enum.Parse<Type>(encodedMethod, true);
        }

        /// <summary>
        /// This dictionary contains a collection of all valid HTTP methods.
        /// </summary>
        private static readonly IDictionary<string, Method> ValidHttpMethodsDictionary = new Dictionary<string, Method>
        {
            {"GET", new Method("GET")},
            {"HEAD", new Method("HEAD")},
            {"POST", new Method("POST")},
            {"PUT", new Method("PUT")},
            {"DELETE", new Method("DELETE")},
            {"TRACE", new Method("TRACE")},
            {"CONNECT", new Method("CONNECT")},
            {"OPTIONS", new Method("OPTIONS")},
        };
    }
}

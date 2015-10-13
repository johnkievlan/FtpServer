﻿//-----------------------------------------------------------------------
// <copyright file="FtpConnectionData.cs" company="Fubar Development Junker">
//     Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>
// <author>Mark Junker</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

using FubarDev.FtpServer.FileSystem;

using JetBrains.Annotations;

using Sockets.Plugin.Abstractions;

namespace FubarDev.FtpServer
{
    /// <summary>
    /// Common data for a <see cref="FtpConnection"/>
    /// </summary>
    public sealed class FtpConnectionData : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FtpConnectionData"/> class.
        /// </summary>
        /// <param name="connection">The <see cref="FtpConnection"/> to create the data for</param>
        internal FtpConnectionData([NotNull] FtpConnection connection)
        {
            UserData = new ExpandoObject();
            TransferMode = new FtpTransferMode(FtpFileType.Ascii);
            BackgroundCommandHandler = new BackgroundCommandHandler(connection);
            Path = new Stack<IUnixDirectoryEntry>();
        }

        /// <summary>
        /// Gets or sets the current user name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user with the <see cref="UserName"/>
        /// is logged in.
        /// </summary>
        public bool IsLoggedIn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user is anonymous.
        /// </summary>
        public bool IsAnonymous { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Encoding"/> for the <code>NLST</code> command.
        /// </summary>
        public Encoding NlstEncoding { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IUnixFileSystem"/> to use for the user.
        /// </summary>
        public IUnixFileSystem FileSystem { get; set; }

        /// <summary>
        /// Gets or sets the current path into the <see cref="FileSystem"/>
        /// </summary>
        [NotNull, ItemNotNull]
        public Stack<IUnixDirectoryEntry> Path { get; set; }

        /// <summary>
        /// Gets the current <see cref="IUnixDirectoryEntry"/> of the current <see cref="Path"/>
        /// </summary>
        public IUnixDirectoryEntry CurrentDirectory
        {
            get
            {
                if (Path.Count == 0)
                    return FileSystem.Root;
                return Path.Peek();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="FtpTransferMode"/>
        /// </summary>
        public FtpTransferMode TransferMode { get; set; }

        /// <summary>
        /// Gets or sets the address to use for an active data connection.
        /// </summary>
        public Uri PortAddress { get; set; }

        /// <summary>
        /// Gets or sets the data connection for a passive data transfer
        /// </summary>
        public ITcpSocketClient PassiveSocketClient { get; set; }

        /// <summary>
        /// Gets the <see cref="BackgroundCommandHandler"/> that's required for the <code>ABOR</code> command.
        /// </summary>
        public BackgroundCommandHandler BackgroundCommandHandler { get; }

        /// <summary>
        /// Gets the last used transfer type command.
        /// </summary>
        /// <remarks>
        /// It's not allowed to use PASV when PORT was used previously - and vice versa.
        /// </remarks>
        public string TransferTypeCommandUsed { get; set; }

        /// <summary>
        /// Gets or sets the restart position for appending data to a file.
        /// </summary>
        public long? RestartPosition { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IUnixFileEntry"/> to use for a <code>RNTO</code> operation.
        /// </summary>
        public SearchResult<IUnixFileEntry> RenameFrom { get; set; }

        /// <summary>
        /// Gets or sets user data as <code>dynamic</code> object
        /// </summary>
        public dynamic UserData { get; set; }

        /// <inheritdoc/>
        public void Dispose()
        {
            BackgroundCommandHandler.Dispose();
            PassiveSocketClient?.Dispose();
            FileSystem?.Dispose();
            PassiveSocketClient = null;
            FileSystem = null;
        }
    }
}

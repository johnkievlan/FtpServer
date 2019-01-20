// <copyright file="BadParameterException.cs" company="40three GmbH">
// Copyright (c) 40three GmbH. All rights reserved.
// </copyright>

namespace FubarDev.FtpServer.FileSystem.Error
{
    /// <summary>
    /// Command not implemented for that parameter.
    /// </summary>
    public class BadParameterException : FileSystemException
    {
        /// <inheritdoc />
        public override int FtpErrorCode { get; } = 504;
    }
}

﻿using System;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Harry.Extensions.Configuration.Yaml.Test
{
    /// <summary>
    ///     Adapted from: https://github.com/aspnet/Configuration/blob/b2609af11b7e1083d9bc4e6e21178297efcf798c/test/Microsoft.Extensions.Configuration.Test.Common/TestStreamHelpers.cs
    /// </summary>
    internal static class TestStreamHelpers
    {
        public static readonly string ArbitraryFilePath = "Unit tests do not touch file system";

        public static IFileProvider StringToFileProvider(string str)
            => new TestFileProvider(str);

        private class TestFile : IFileInfo
        {
            private readonly string _data;

            public TestFile(string str) => _data = str;

            public bool Exists => true;
            public bool IsDirectory => false;
            public DateTimeOffset LastModified => throw new NotImplementedException();
            public long Length => throw new NotImplementedException();
            public string Name => throw new NotImplementedException();
            public string PhysicalPath => throw new NotImplementedException();
            public Stream CreateReadStream() => StringToStream(_data);
        }

        private class TestFileProvider : IFileProvider
        {
            private string _data;
            
            public TestFileProvider(string str) => _data = str;

            public IDirectoryContents GetDirectoryContents(string subpath)
                => throw new NotImplementedException();

            public IFileInfo GetFileInfo(string subpath)
                => new TestFile(_data);

            public IChangeToken Watch(string filter)
                => throw new NotImplementedException();
        }

        public static Stream StringToStream(string str)
        {
            var memStream = new MemoryStream();
            var textWriter = new StreamWriter(memStream);
            textWriter.Write(str);
            textWriter.Flush();
            memStream.Seek(0, SeekOrigin.Begin);

            return memStream;
        }

        public static string StreamToString(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}

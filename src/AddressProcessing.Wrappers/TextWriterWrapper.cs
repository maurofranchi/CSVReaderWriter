using System;
using System.IO;
using AddressProcessing.Contracts;

namespace AddressProcessing.Wrappers
{
    public class TextWriterWrapper : ITextWriter
    {
        private StreamWriter _writer;

        public TextWriterWrapper(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            _writer = fileInfo.CreateText();
        }
        public void Dispose()
        {
            _writer?.Close();
            _writer = null;
        }

        public void WriteLine(string line)
        {
            if (_writer == null) throw new NullReferenceException("The writer is not initializated.");
            _writer.WriteLine(line);
        }
    }
}
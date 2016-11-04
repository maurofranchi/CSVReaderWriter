using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using AddressProcessing.Contracts;

namespace AddressProcessing.Wrappers
{
    public class TextReaderWrapper : ITextReader
    {
        private StreamReader _reader;

        public TextReaderWrapper(string fileName)
        {
            _reader = File.OpenText(fileName);
        }
        public void Dispose()
        {
            _reader?.Close();
            _reader = null;
        }

        public string ReadLine()
        {
            if (_reader == null) throw new NullReferenceException("The reader is not initializated.");
            // Reads the line only if there are still characters to read. Returns null otherwise
            return _reader.Peek() >= 0 ? _reader.ReadLine() : null;
        }
    }
}

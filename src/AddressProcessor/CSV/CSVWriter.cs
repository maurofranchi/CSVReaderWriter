using System;
using AddressProcessing.Contracts;

namespace AddressProcessing.CSV
{
    public class CSVWriter : ICSVWriter
    {
        private ITextWriter _writer;

        public CSVWriter(ITextWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer), "Please provide a valid text writer.");
            _writer = writer;
        }

        public void Dispose()
        {
            _writer?.Dispose();
            _writer = null;
        }

        public void Write(params string[] columns)
        {
            if (_writer == null) throw new NullReferenceException("The writer cannot be used anymore. Pleaser reinitializate it.");
            if (columns == null) throw new ArgumentNullException(nameof(columns), "Please provide at least 1 column");
            var outPut = string.Join("\t", columns);
            _writer.WriteLine(outPut);
        }

        public void Close()
        {
            Dispose();
        }
    }
}
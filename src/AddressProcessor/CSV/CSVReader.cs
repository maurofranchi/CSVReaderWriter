using System;
using System.IO;
using AddressProcessing.Contracts;
using AddressProcessing.Wrappers;

namespace AddressProcessing.CSV
{
    public class CSVReader : ICSVReader
    {
        private ITextReader _reader;

        public CSVReader(ITextReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader), "Please provide a valid text reader.");
            _reader = reader;
        }

        public void Dispose()
        {
            _reader?.Dispose();
            _reader = null;
        }

        public bool Read(out string column1, out string column2)
        {
            if (_reader == null) throw new NullReferenceException("The reader cannot be used anymore. Pleaser reinitializate it.");

            const int firstColumn = 0;
            const int secondColumn = 1;

            var line = _reader.ReadLine();

            //Default values
            column1 = null;
            column2 = null;

            if (line == null) return false;

            var columns = line.Split('\t');

            //There is always at least 1 element in the array
            column1 = columns[firstColumn];
            if (columns.Length > 1)
                column2 = columns[secondColumn];

            return true;
        }

        public void Close()
        {
            Dispose();
        }
    }
}
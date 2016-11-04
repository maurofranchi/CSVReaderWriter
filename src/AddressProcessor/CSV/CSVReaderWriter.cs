using System;
using System.IO;
using AddressProcessing.Contracts;
using AddressProcessing.Wrappers;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.

        Changes made:
        1) Implemented IDisposable in order to be able to dispose the streamers.
        2) rafactored the Read(string column1, string column2) metodo in order to use the other Read method 
        considering that they execute the same exact code. I kept the method for backwards compatibility, even 
        though it seems not to be used anywhere in this Solution (but it could be in other part of the production
        solution).
        3) Internal use of the ICSVReader and ICSVWriter in order to separate the  

    */

    /// <summary>
    /// Honestly I would like to get rid of this class and use the right ICSV* Classes instead.
    /// This would decouple the application and apply avoid bracking the SRP, but given the back compatibility requirement we have to keepit.
    /// This could be part of the discussion on how to refactor the whole app in order to have a proper decoupled architecture.
    /// </summary>
    public class CSVReaderWriter : IDisposable
    {
        protected ICSVReader _csvReader = null;
        protected ICSVWriter _csvWriter = null;

        [Flags] //Even though I said that we could remove it, I keep it for back compatibility
        public enum Mode { Read = 1, Write = 2 };

        public CSVReaderWriter()
        {
            // just here for retro-compatibility
        }

        // This is actually the way I would like to use the classes I implemented and avoid the use of the "open" method.
        public CSVReaderWriter(ICSVReader reader, ICSVWriter writer)
        {
            _csvReader = reader;
            _csvWriter = writer;
        }

        public void Open(string fileName, Mode mode)
        {
            if (mode == Mode.Read)
            {
                //Given the above statement 
                _csvReader = new CSVReader(new TextReaderWrapper(fileName));
            }
            else if (mode == Mode.Write)
            {
                _csvWriter = new CSVWriter(new TextWriterWrapper(fileName));
            }
            else
            {
                throw new Exception("Unknown file mode for " + fileName);
            }
        }

        public void Write(params string[] columns)
        {
            if (_csvWriter == null) throw new NullReferenceException("The file writer is closed. Please reopen it.");
            _csvWriter.Write(columns);
        }

        public bool Read(string column1, string column2)
        {
            string c1, c2;
            return Read(out c1, out c2);
        }

        public bool Read(out string column1, out string column2)
        {
            if (_csvReader == null) throw new NullReferenceException("The file reader is closed. Please reopen it.");
            return _csvReader.Read(out column1, out column2);
        }
        

        public void Close()
        {
            _csvWriter?.Close();
            _csvWriter = null;
            _csvReader?.Close();
            _csvReader = null;
        }

        public void Dispose()
        {
            Close();
        }
    }
}

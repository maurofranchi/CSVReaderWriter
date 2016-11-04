using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AddressProcessing.Contracts;
using AddressProcessing.CSV;
using Moq;
using NUnit.Framework;

namespace Csv.Tests
{
    [TestFixture]
    public class CSVReaderWriterTests
    {
        [Test()]
        public void Open_FlagNotSupported_ThrowException()
        {
            var fileName = @"C:\whatever.csv";
            var ex = Assert.Throws<Exception>(() => new CSVReaderWriter().Open(fileName, CSVReaderWriter.Mode.Read & CSVReaderWriter.Mode.Write));
            Assert.AreEqual("Unknown file mode for " + fileName, ex.Message);
        }

        [Test()]
        public void Open_WriteFlag_CSVWriterIsInstanciated()
        {
            var fileName = @".\whatever.csv";
            var fake = new FakeCSVReaderWriter();
            Assert.DoesNotThrow(() => fake.Open(fileName, CSVReaderWriter.Mode.Write));
            Assert.IsNotNull(fake.Writer);
        }

        [Test()]
        public void Open_ReadFlag_CSVReaderIsInstanciated()
        {
            var fileName = @"test_data\contacts.csv";
            var fake = new FakeCSVReaderWriter();
            Assert.DoesNotThrow(() => fake.Open(fileName, CSVReaderWriter.Mode.Read));
            Assert.IsNotNull(fake.Reader);
        }

        [Test]
        public void Read_ReaderIsDisposed_ThrowException()
        {
            var mockReader = new Mock<ICSVReader>();
            var csv = new CSVReaderWriter(mockReader.Object, It.IsAny<ICSVWriter>());
            csv.Close();

            string c1;
            string c2;
            var ex = Assert.Throws<NullReferenceException>(() => csv.Read(out c1, out c2));
            Assert.AreEqual("The file reader is closed. Please reopen it.", ex.Message);
        }

        // There is no need to do more tests for the Read method with valid input as it has been tested already throow ICSVReader

        [Test]
        public void Write_WriterIsDisposed_ThrowException()
        {
            var csv = new CSVReaderWriter(It.IsAny<ICSVReader>(), null);
            csv.Close();

            var ex = Assert.Throws<NullReferenceException>(() => csv.Write(It.IsAny<string[]>()));
            Assert.AreEqual("The file writer is closed. Please reopen it.", ex.Message);
        }

        // There is no need to do more tests for the Write method with valid input as it has been tested already throow ICSVWriter

    }

    internal class FakeCSVReaderWriter : CSVReaderWriter
    {
        public ICSVWriter Writer => _csvWriter;
        public ICSVReader Reader => _csvReader;
    }

    
}

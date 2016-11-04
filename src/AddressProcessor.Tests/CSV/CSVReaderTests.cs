using NUnit.Framework;
using AddressProcessing.CSV;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressProcessing.Contracts;
using Moq;

namespace Csv.Tests
{
    [TestFixture()]
    public class CSVReaderTests
    {
        [Test()]
        public void Ctor_PassNullreader_ThrowArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new CSVReader(null));
            Assert.AreEqual("Please provide a valid text reader.\r\nParameter name: reader", ex.Message);
        }

        [Test]
        public void Ctor_PassValidReader_ExpectNoException()
        {
            var mockReader = new Mock<ITextReader>();
            Assert.DoesNotThrow(() => new CSVReader(mockReader.Object));
        }

        [Test]
        public void Read_ReaderIsDisposed_ThrowException()
        {
            var mockReader = new Mock<ITextReader>();
            var csvReader = new CSVReader(mockReader.Object);
            csvReader.Dispose();

            string c1;
            string c2;
            var ex = Assert.Throws<NullReferenceException>(() => csvReader.Read(out c1, out c2));
            Assert.AreEqual("The reader cannot be used anymore. Pleaser reinitializate it.", ex.Message);
        }

        [Test]
        [TestCase("", "", null, true)]
        [TestCase(null, null, null, false)]
        [TestCase("\t", "", "", true)]
        [TestCase("abc\tdef", "abc", "def", true)]
        [TestCase("\tdef\tghi", "", "def", true)]
        [TestCase("abc\t\tghi", "abc", "", true)]
        public void Read_ReadFRomTheFile_ReturnsExpectedResult(string line, string expectedC1, string expectedC2, bool expectedResult)
        {
            var mockReader = new Mock<ITextReader>();
            mockReader.Setup(m => m.ReadLine()).Returns(line);
            var csvReader = new CSVReader(mockReader.Object);
            
            string c1;
            string c2;
            var result = csvReader.Read(out c1, out c2);
            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedC1, c1);
            Assert.AreEqual(expectedC2, c2);
        }
    }
}
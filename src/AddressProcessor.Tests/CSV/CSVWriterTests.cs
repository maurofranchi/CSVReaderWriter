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
    public class CSVWriterTests
    {
        [Test()]
        public void Ctor_PassNullwriter_ThrowArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new CSVWriter(null));
            Assert.AreEqual("Please provide a valid text writer.\r\nParameter name: writer", ex.Message);
        }

        [Test]
        public void Ctor_PassValidWriter_ExpectNoException()
        {
            var mockWriter = new Mock<ITextWriter>();
            Assert.DoesNotThrow(() => new CSVWriter(mockWriter.Object));
        }

        [Test]
        public void Write_WriterIsDisposed_ThrowException()
        {
            var mockWriter = new Mock<ITextWriter>();
            var csvWriter = new CSVWriter(mockWriter.Object);
            csvWriter.Dispose();

            var ex = Assert.Throws<NullReferenceException>(() => csvWriter.Write(It.IsAny<string[]>()));
            Assert.AreEqual("The writer cannot be used anymore. Pleaser reinitializate it.", ex.Message);
        }

        [Test]
        public void Write_NoColumns_ThrowException()
        {
            var csvWriter = new CSVWriter(new FakeWriter());

            var ex = Assert.Throws<ArgumentNullException>(() => csvWriter.Write(null));
            Assert.AreEqual("Please provide at least 1 column\r\nParameter name: columns", ex.Message);
        }

        [Test]
        [TestCase(new string[] {}, "")]
        [TestCase(new[] { "" }, "")]
        [TestCase(new[] { "abc", "def" }, "abc\tdef")]
        [TestCase(new[] { "abc", "", "", "lmn" }, "abc\t\t\tlmn")]
        [TestCase(new[] { "abc", null, "ghi", "" }, "abc\t\tghi\t")]
        public void Writee_ValidInput_ReturnsExpectedResult(string[] columns, string expectedLine)
        {
            var fakekWriter = new FakeWriter();
            var csvWriter = new CSVWriter(fakekWriter);
            
            csvWriter.Write(columns);
            Assert.AreEqual(expectedLine, fakekWriter.Line);
        }
    }

    internal class FakeWriter : ITextWriter
    {
        public string Line { get; private set; }
        public void Dispose()
        {
            Line = null;
        }

        public void WriteLine(string line)
        {
            Line = line;
        }
    }
}
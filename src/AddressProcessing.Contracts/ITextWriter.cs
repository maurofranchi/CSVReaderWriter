using System;

namespace AddressProcessing.Contracts
{
    //This interface will help me to decouple the CSVReader from the StreamWriter implementation.
    //In this way I can mock it and focus on the logic of the CSVWriter.
    public interface ITextWriter : IDisposable
    {
        void WriteLine(string line);
    }
}
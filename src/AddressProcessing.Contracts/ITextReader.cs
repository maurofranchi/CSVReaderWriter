using System;

namespace AddressProcessing.Contracts
{
    //This interface will help me to decouple the CSVReader from the StreamReader implementation.
    //In this way I can mocke it and focus on the logic of the CSVReader.
    public interface ITextReader : IDisposable
    {
        string ReadLine();
    }
}
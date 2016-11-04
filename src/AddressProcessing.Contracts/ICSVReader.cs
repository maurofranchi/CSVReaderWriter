using System;

namespace AddressProcessing.Contracts
{
    public interface ICSVReader : IDisposable
    {
        bool Read(out string column1, out string column2);
        void Close();
    }
}
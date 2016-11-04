using System;

namespace AddressProcessing.Contracts
{
    public interface ICSVWriter : IDisposable
    {
        void Write(params string[] columns);
        void Close();
    }
}
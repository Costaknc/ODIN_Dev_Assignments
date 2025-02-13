using System;

namespace IoT_Backend_Assignment
{
    public class IPServiceNotAvailableException : Exception
    {
        public int StatusCode { get; }
        public string Response { get; }

        public IPServiceNotAvailableException(string message, int status, string response) : base(message)
        {
            StatusCode = status;
            Response = response;
        }
    }
}

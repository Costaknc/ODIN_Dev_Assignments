using System;

namespace IoT_Backend_Assignment
{
    public interface IIPInfoProvider
    {
        IIpDetails GetDetails(string ip);
    }

    public interface IIpDetails
    {
        String City { get; set; }

        String Country { get; set; }

        String Continent { get; set; }

        double Latitude { get; set; }

        double Longitude { get; set; }

    }
}

using IoT_Backend_Assignment;
using static Batches;

namespace WebApi {
    public class DbHandler
    {
        public static void InsertNewRow(string ip, IIpDetails details)
        {
            using (var context = new DbEntity())
            {
                var newIpRow = new DbEntity.IpRow
                {
                    IP = ip,
                    City = details.City,
                    Country = details.Country,
                    Continent = details.Continent,
                    Lat = details.Latitude,
                    Lon = details.Longitude
                };

                context.IpDetailsTable.Add(newIpRow);
                try
                {
                    context.SaveChanges();

                    Console.WriteLine("New row added successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        public static DbEntity.IpRow? FetchRow(string ip)
        {
            using (var context = new DbEntity())
            {
                try
                {
                    var ipRow = context.IpDetailsTable.FirstOrDefault(i => i.IP == ip);

                    return ipRow;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error", ex.Message);
                    return null;
                }
            }
        }

        public static void UpdateDatabase(updateRequest request)
        {
            using (var context = new DbEntity())
            {
                var ipRow = context.IpDetailsTable.FirstOrDefault(row => row.IP == request.IP);

                if (ipRow == null)
                {
                    Console.WriteLine($"IP {request.IP} not found in the database. Skipping update.");
                    return;
                }

                switch (request.field.ToLower())
                {
                    case "city":
                        ipRow.City = request.value;
                        break;
                    case "country":
                        ipRow.Country = request.value;
                        break;
                    case "continent":
                        ipRow.Continent = request.value;
                        break;
                    case "lat":
                        if (double.TryParse(request.value, out var lat))
                        {
                            ipRow.Lat = lat;
                        }
                        else
                        {
                            Console.WriteLine($"Invalid latitude value: {request.value}. Skipping update.");
                            return;
                        }
                        break;
                    case "lon":
                        if (double.TryParse(request.value, out var lon))
                        {
                            ipRow.Lon = lon;
                        }
                        else
                        {
                            Console.WriteLine($"Invalid longitude value: {request.value}. Skipping update.");
                            return;
                        }
                        break;
                    default:
                        Console.WriteLine($"Invalid field name: {request.field}. Skipping update.");
                        return;
                }

                context.SaveChanges();
                Console.WriteLine($"Updated IP {request.IP}: {request.field} = {request.value}");
            }
        }
    }
}
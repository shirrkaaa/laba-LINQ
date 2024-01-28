using ConsoleApp.Model;
using ConsoleApp.Model.Enum;
using ConsoleApp.OutputTypes;

namespace ConsoleApp;

public class QueryHelper : IQueryHelper
{
    /// <summary>
    /// Get Deliveries that has payed
    /// </summary>
    public IEnumerable<Delivery> Paid(IEnumerable<Delivery> deliveries) => 
        deliveries.Where(delivery => delivery.Status == DeliveryStatus.Confirmed); // Завдання 1

    /// <summary>
    /// Get Deliveries that now processing by system (not Canceled or Done)
    /// </summary>
   public IEnumerable<Delivery> NotFinished(IEnumerable<Delivery> deliveries) =>
        deliveries.Where(delivery => delivery.Status != DeliveryStatus.Canceled && delivery.Status != DeliveryStatus.Done); // Завдання 2
    
    /// <summary>
    /// Get DeliveriesShortInfo from deliveries of specified client
    /// </summary>
    public IEnumerable<DeliveryShortInfo> DeliveryInfosByClient(IEnumerable<Delivery> deliveries, string clientId) =>
        deliveries
            .Where(delivery => delivery.ClientId == clientId)
            .Select(delivery => new DeliveryShortInfo
            {
                Id = delivery.Id,
                StartCity = delivery.Direction.Origin,
                EndCity = delivery.Direction.Destination,
                ClientId = delivery.ClientId,
                Type = delivery.Type,
                LoadingPeriod = delivery.LoadingPeriod,
                ArrivalPeriod = delivery.ArrivalPeriod,
                CargoType = delivery.CargoType
                Status = delivery.Status,
                ClientName = d.ClientName,
            }); // Завдання 3
    
    /// <summary>
    /// Get first ten Deliveries that starts at specified city and have specified type
    /// </summary>
    public IEnumerable<Delivery> DeliveriesByCityAndType(IEnumerable<Delivery> deliveries, string cityName, DeliveryType type) =>
        deliveries
            .Where(delivery => delivery.Direction.Origin.City == cityName && delivery.Type == type)
            .Take(10); //Завдання 4
    
    /// <summary>
    /// Order deliveries by status, then by start of loading period
    /// </summary>
    public IEnumerable<Delivery> OrderByStatusThenByStartLoading(IEnumerable<Delivery> deliveries) =>
        deliveries
            .OrderBy(delivery => delivery.Status)
            .ThenBy(delivery => delivery.StartLoadingPeriod); // Завдання 5

    /// <summary>
    /// Count unique cargo types
    /// </summary>
    public int CountUniqCargoTypes(IEnumerable<Delivery> deliveries) =>
        deliveries.Select(delivery => delivery.CargoType).Distinct().Count(); // Завдання 6
    
    /// <summary>
    /// Group deliveries by status and count deliveries in each group
    /// </summary>
    public Dictionary<DeliveryStatus, int> CountsByDeliveryStatus(IEnumerable<Delivery> deliveries) =>
        deliveries
            .GroupBy(delivery => delivery.Status)
            .ToDictionary(group => group.Key, group => group.Count()); // Завдання 7
    
    /// <summary>
    /// Group deliveries by start-end city pairs and calculate average gap between end of loading period and start of arrival period (calculate in minutes)
    /// </summary>
    public IEnumerable<AverageGapsInfo> AverageTravelTimePerDirection(IEnumerable<Delivery> deliveries) =>
        deliveries.GroupBy(delivery => new { StartCity = delivery.Direction.Origin, EndCity = delivery.Direction.Destination })
            .Select(group => new AverageGapsInfo
            {
                StartCity = group.Key.Origin,
                EndCity = group.Key.Destination,
                AverageGap = group.Average(delivery => 
            {
                DateTime start = delivery.LoadingPeriod.Start.GetValueOrDefault();
                DateTime end = delivery.ArrivalPeriod.End.GetValueOrDefault();

                if (start != default && end != default)
                {
                    return (end - start).TotalMinutes;
                }

                return 0.0;
            });
        }); // Завдання 8

    /// <summary>
    /// Paging helper
    /// </summary>
    public IEnumerable<TElement> Paging<TElement, TOrderingKey>(IEnumerable<TElement> elements,
        Func<TElement, TOrderingKey> ordering,
        Func<TElement, bool>? filter = null,
        int countOnPage = 100,
        int pageNumber = 1) => new List<TElement>(); //TODO: Завдання 9 
}

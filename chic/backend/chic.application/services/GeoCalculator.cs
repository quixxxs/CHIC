// chic.application/services/GeoCalculator.cs
namespace chic.application.services;

public static class GeoCalculator
{
    private const double RadioTierraMetros = 6_371_000;

    // Fórmula de Haversine: distancia en metros entre dos coordenadas
    public static double DistanciaEnMetros(double lat1, double lon1, double lat2, double lon2)
    {
        var dLat = ARadianes(lat2 - lat1);
        var dLon = ARadianes(lon2 - lon1);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ARadianes(lat1)) * Math.Cos(ARadianes(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return RadioTierraMetros * c;
    }

    private static double ARadianes(double grados) => grados * Math.PI / 180;
}
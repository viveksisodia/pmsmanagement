namespace Construction.Domain.Areas;

/// <summary>Areas are stored in square metres. Conversions live here only.</summary>
public static class AreaUnits
{
    public const string SquareMetre = "sqm";
    public const string SquareFoot = "sqft";
    public const string Acre = "acre";
    public const string Hectare = "hectare";

    private const decimal SqmPerSqft = 0.09290304m;      // exact (1 ft = 0.3048 m)
    private const decimal SqmPerAcre = 4046.8564224m;    // exact (international acre)
    private const decimal SqmPerHectare = 10000m;

    public static readonly IReadOnlyList<string> LandUnits = [SquareMetre, SquareFoot, Acre, Hectare];
    public static readonly IReadOnlyList<string> ProjectUnits = [SquareMetre, SquareFoot];

    public static decimal ToSquareMetres(decimal value, string unit) => Math.Round(value * Factor(unit), 4, MidpointRounding.AwayFromZero);

    public static decimal FromSquareMetres(decimal sqm, string unit) => Math.Round(sqm / Factor(unit), 2, MidpointRounding.AwayFromZero);

    private static decimal Factor(string unit) => unit switch
    {
        SquareMetre => 1m,
        SquareFoot => SqmPerSqft,
        Acre => SqmPerAcre,
        Hectare => SqmPerHectare,
        _ => throw new DomainException($"Unknown area unit '{unit}'.")
    };

    /// <summary>Carpet ≤ built-up ≤ super when the values are given.</summary>
    public static bool AreInOrder(decimal? carpet, decimal? builtUp, decimal? super)
        => (carpet is null || builtUp is null || carpet <= builtUp) && (builtUp is null || super is null || builtUp <= super);
}

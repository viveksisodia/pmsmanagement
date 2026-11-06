namespace Construction.Domain.Structure;

/// <summary>Options for the floor generator. Level type ids come from the tenant's level type master.</summary>
public sealed record FloorPlanOptions(
    int Basements,
    int BasementLevelTypeId,
    int? GroundLevelTypeId,
    int Podiums,
    int PodiumLevelTypeId,
    int TypicalFrom,
    int TypicalTo,
    int TypicalLevelTypeId,
    IReadOnlyCollection<int> SkipNumbers,
    int? TerraceLevelTypeId,
    decimal? HeightM);

public sealed record PlannedFloor(string Code, string Name, int LevelTypeId, int SortOrder, decimal? HeightM);

/// <summary>Builds the floor list of a tower or block, lowest level first.</summary>
public static class FloorPlanner
{
    public const int MaxFloors = 200;

    public static IReadOnlyList<PlannedFloor> Plan(FloorPlanOptions o)
    {
        if (o.Basements is < 0 or > 10) throw new DomainException("Basements must be between 0 and 10.");
        if (o.Podiums is < 0 or > 10) throw new DomainException("Podiums must be between 0 and 10.");
        if (o.TypicalFrom < 1) throw new DomainException("Typical floors start at 1 or higher.");
        if (o.TypicalTo < o.TypicalFrom - 1) throw new DomainException("\"Typical floors to\" must be at least \"from\" (or one less for none).");
        if (o.HeightM is <= 0) throw new DomainException("Floor height must be greater than zero.");

        var list = new List<PlannedFloor>();
        for (var b = o.Basements; b >= 1; b--) list.Add(new($"B{b}", $"Basement {b}", o.BasementLevelTypeId, 0, o.HeightM));
        if (o.GroundLevelTypeId is int g) list.Add(new("G", "Ground", g, 0, o.HeightM));
        for (var p = 1; p <= o.Podiums; p++) list.Add(new($"P{p}", $"Podium {p}", o.PodiumLevelTypeId, 0, o.HeightM));
        var skip = o.SkipNumbers.ToHashSet();
        for (var f = o.TypicalFrom; f <= o.TypicalTo; f++)
        {
            if (skip.Contains(f)) continue;
            list.Add(new(TypicalCode(f), $"Floor {f}", o.TypicalLevelTypeId, 0, o.HeightM));
        }
        if (o.TerraceLevelTypeId is int t) list.Add(new("T", "Terrace", t, 0, null));
        if (list.Count == 0) throw new DomainException("The options produce no floors.");
        if (list.Count > MaxFloors) throw new DomainException($"A building can have at most {MaxFloors} floors.");
        return list.Select((x, i) => x with { SortOrder = (i + 1) * 10 }).ToList();
    }

    /// <summary>Typical floors are two digits (01…99), then plain numbers.</summary>
    public static string TypicalCode(int floor) => floor < 100 ? floor.ToString("00") : floor.ToString();

    public static IReadOnlyCollection<int> ParseSkipList(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return [];
        var result = new HashSet<int>();
        foreach (var part in text.Split([',', ';', ' '], StringSplitOptions.RemoveEmptyEntries))
        {
            if (!int.TryParse(part, out var n) || n < 1) throw new DomainException($"\"{part}\" isn't a valid floor number to skip.");
            result.Add(n);
        }
        return result;
    }
}

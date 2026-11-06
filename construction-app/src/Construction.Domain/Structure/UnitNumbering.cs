namespace Construction.Domain.Structure;

/// <summary>
/// Unit number patterns. Tokens: {P} phase code, {T} building code, {FF} floor code, {NN} position (two digits).
/// Example: "{T}-{FF}{NN}" gives A1-0502 for Tower A1, floor 05, position 2.
/// </summary>
public static class UnitNumbering
{
    public const string DefaultPattern = "{T}-{FF}{NN}";
    public const int MaxLength = 40;

    public static void Validate(string? pattern, bool usesFloors)
    {
        if (string.IsNullOrWhiteSpace(pattern)) throw new DomainException("Enter a unit number pattern.");
        if (!pattern.Contains("{NN}")) throw new DomainException("The unit number pattern must contain {NN} (position).");
        if (usesFloors && !pattern.Contains("{FF}")) throw new DomainException("For towers and blocks the pattern must contain {FF} (floor code).");
        var stripped = pattern.Replace("{P}", "").Replace("{T}", "").Replace("{FF}", "").Replace("{NN}", "");
        if (stripped.Contains('{') || stripped.Contains('}')) throw new DomainException("The pattern has an unknown token. Use {P}, {T}, {FF} and {NN}.");
    }

    public static string Format(string pattern, string? phaseCode, string buildingCode, string? floorCode, int position)
    {
        var no = pattern
            .Replace("{P}", phaseCode ?? "")
            .Replace("{T}", buildingCode)
            .Replace("{FF}", floorCode ?? "")
            .Replace("{NN}", position.ToString("00"))
            .Trim();
        if (no.Length == 0) throw new DomainException("The pattern produced an empty unit number.");
        if (no.Length > MaxLength) throw new DomainException($"Unit number {no} is longer than {MaxLength} characters.");
        return no;
    }
}

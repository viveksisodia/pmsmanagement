using Construction.Domain.Areas;

namespace Construction.Domain.Structure;

/// <summary>A floor the generator may place units on.</summary>
public sealed record FloorSlot(int FloorId, string Code, int SortOrder, bool IsHabitable);

/// <summary>One position on every chosen floor (or one villa / plot when there are no floors).</summary>
public sealed record UnitPosition(int Position, int UnitTypeId, string? Facing, decimal? CarpetSqm, decimal? BuiltUpSqm,
                                  decimal? SuperSqm, decimal? BalconySqm, decimal? PlotSqm);

public sealed record PlannedUnit(string UnitNo, int? FloorId, string? FloorCode, int Position, int UnitTypeId, string? Facing,
                                 decimal? CarpetSqm, decimal? BuiltUpSqm, decimal? SuperSqm, decimal? BalconySqm, decimal? PlotSqm,
                                 bool AlreadyExists);

public static class UnitPlanner
{
    public const int MaxUnitsPerRun = 5000;

    /// <summary>Units for a range of floors of a tower or block. Non-habitable floors in the range are skipped.</summary>
    public static IReadOnlyList<PlannedUnit> PlanOnFloors(string pattern, string? phaseCode, string buildingCode,
        IReadOnlyList<FloorSlot> floors, int fromFloorId, int toFloorId, IReadOnlyList<UnitPosition> positions,
        IReadOnlySet<string> existingNumbers)
    {
        UnitNumbering.Validate(pattern, usesFloors: true);
        CheckPositions(positions);
        var ordered = floors.OrderBy(f => f.SortOrder).ToList();
        var from = ordered.FindIndex(f => f.FloorId == fromFloorId);
        var to = ordered.FindIndex(f => f.FloorId == toFloorId);
        if (from < 0 || to < 0) throw new DomainException("Choose the first and last floor from this building.");
        if (to < from) throw new DomainException("The last floor must be at or above the first floor.");
        var chosen = ordered.Skip(from).Take(to - from + 1).Where(f => f.IsHabitable).ToList();
        if (chosen.Count == 0) throw new DomainException("There are no habitable floors in the chosen range.");

        var units = new List<PlannedUnit>();
        foreach (var f in chosen)
            foreach (var p in positions)
            {
                var no = UnitNumbering.Format(pattern, phaseCode, buildingCode, f.Code, p.Position);
                units.Add(new(no, f.FloorId, f.Code, p.Position, p.UnitTypeId, p.Facing, p.CarpetSqm, p.BuiltUpSqm, p.SuperSqm,
                              p.BalconySqm, p.PlotSqm, existingNumbers.Contains(no)));
            }
        return Finish(units);
    }

    /// <summary>Villas or plots directly under a cluster or sector: one unit per position, no floor.</summary>
    public static IReadOnlyList<PlannedUnit> PlanWithoutFloors(string pattern, string? phaseCode, string buildingCode,
        IReadOnlyList<UnitPosition> positions, IReadOnlySet<string> existingNumbers)
    {
        UnitNumbering.Validate(pattern, usesFloors: false);
        CheckPositions(positions);
        var units = positions.Select(p =>
        {
            var no = UnitNumbering.Format(pattern, phaseCode, buildingCode, null, p.Position);
            return new PlannedUnit(no, null, null, p.Position, p.UnitTypeId, p.Facing, p.CarpetSqm, p.BuiltUpSqm, p.SuperSqm,
                                   p.BalconySqm, p.PlotSqm, existingNumbers.Contains(no));
        }).ToList();
        return Finish(units);
    }

    private static void CheckPositions(IReadOnlyList<UnitPosition> positions)
    {
        if (positions.Count == 0) throw new DomainException("Add at least one position.");
        if (positions.Any(p => p.Position < 1 || p.Position > 999)) throw new DomainException("Positions must be between 1 and 999.");
        if (positions.GroupBy(p => p.Position).Any(g => g.Count() > 1)) throw new DomainException("Each position number can be used once.");
        foreach (var p in positions)
        {
            if (!AreaUnits.AreInOrder(p.CarpetSqm, p.BuiltUpSqm, p.SuperSqm))
                throw new DomainException($"Position {p.Position}: areas must follow carpet ≤ built-up ≤ super.");
            if (new[] { p.CarpetSqm, p.BuiltUpSqm, p.SuperSqm, p.BalconySqm, p.PlotSqm }.Any(a => a is < 0))
                throw new DomainException($"Position {p.Position}: areas can't be negative.");
        }
    }

    private static IReadOnlyList<PlannedUnit> Finish(List<PlannedUnit> units)
    {
        if (units.Count > MaxUnitsPerRun) throw new DomainException($"One run can create at most {MaxUnitsPerRun:N0} units. Pick fewer floors.");
        var dup = units.GroupBy(u => u.UnitNo, StringComparer.OrdinalIgnoreCase).FirstOrDefault(g => g.Count() > 1);
        if (dup is not null)
            throw new DomainException($"The numbering pattern gives the same number ({dup.Key}) to more than one unit. Add {{FF}} or {{NN}} to the pattern.");
        return units;
    }
}

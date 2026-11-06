using Construction.Domain;
using Construction.Domain.Areas;
using Construction.Domain.Structure;
using Construction.Domain.Units;

namespace Construction.Domain.Tests;

public class AreaUnitsTests
{
    [Theory]
    [InlineData(1, "sqm", 1)]
    [InlineData(1000, "sqft", 92.9030)]
    [InlineData(1, "acre", 4046.8564)]
    [InlineData(2.5, "hectare", 25000)]
    public void Converts_to_square_metres(decimal value, string unit, decimal expected)
        => Assert.Equal(expected, AreaUnits.ToSquareMetres(value, unit));

    [Fact]
    public void Round_trip_square_feet() => Assert.Equal(1150m, AreaUnits.FromSquareMetres(AreaUnits.ToSquareMetres(1150m, "sqft"), "sqft"));

    [Fact]
    public void Unknown_unit_is_rejected() => Assert.Throws<DomainException>(() => AreaUnits.ToSquareMetres(1, "yard"));

    [Fact] public void Areas_in_order() => Assert.True(AreaUnits.AreInOrder(70m, 80m, 100m));
    [Fact] public void Carpet_above_built_up() => Assert.False(AreaUnits.AreInOrder(90m, 80m, 100m));
    [Fact] public void Built_up_above_super() => Assert.False(AreaUnits.AreInOrder(70m, 110m, 100m));
    [Fact] public void Missing_areas_are_fine() => Assert.True(AreaUnits.AreInOrder(null, null, null));
}

public class FloorPlannerTests
{
    private static FloorPlanOptions Options(int basements = 1, int? ground = 2, int podiums = 0, int from = 1, int to = 3,
                                            string? skip = null, int? terrace = 8)
        => new(basements, 1, ground, podiums, 4, from, to, 5, FloorPlanner.ParseSkipList(skip), terrace, 3.0m);

    [Fact]
    public void Plans_lowest_level_first_with_codes()
    {
        var floors = FloorPlanner.Plan(Options(basements: 2, podiums: 1));
        Assert.Equal(["B2", "B1", "G", "P1", "01", "02", "03", "T"], floors.Select(f => f.Code));
        Assert.Equal(floors.OrderBy(f => f.SortOrder).Select(f => f.Code), floors.Select(f => f.Code));
        Assert.Null(floors.Last().HeightM);
    }

    [Fact]
    public void Skips_floor_numbers() =>
        Assert.DoesNotContain(FloorPlanner.Plan(Options(to: 15, skip: "13, 4")), f => f.Code is "13" or "04");

    [Fact]
    public void Cleo_tower_gf_to_27_with_terrace_has_29_levels() =>
        Assert.Equal(29, FloorPlanner.Plan(Options(basements: 0, to: 27)).Count);

    [Fact]
    public void Floor_100_and_above_use_plain_numbers() => Assert.Equal("120", FloorPlanner.TypicalCode(120));

    [Theory]
    [InlineData("13,x")]
    [InlineData("-1")]
    public void Bad_skip_list_is_rejected(string skip) => Assert.Throws<DomainException>(() => FloorPlanner.ParseSkipList(skip));

    [Fact]
    public void Too_many_floors_is_rejected() => Assert.Throws<DomainException>(() => FloorPlanner.Plan(Options(to: 250)));
}

public class UnitNumberingTests
{
    [Fact]
    public void Formats_default_pattern() => Assert.Equal("A1-0502", UnitNumbering.Format(UnitNumbering.DefaultPattern, null, "A1", "05", 2));

    [Fact]
    public void Formats_phase_token() => Assert.Equal("H2/B3/G01", UnitNumbering.Format("{P}/{T}/{FF}{NN}", "H2", "B3", "G", 1));

    [Theory]
    [InlineData("{T}-{FF}")]       // no position
    [InlineData("{T}-{NN}")]       // no floor in a tower
    [InlineData("{T}-{FF}{NN}{X}")] // unknown token
    public void Invalid_tower_patterns(string pattern) => Assert.Throws<DomainException>(() => UnitNumbering.Validate(pattern, usesFloors: true));

    [Fact]
    public void Villa_pattern_without_floor_is_valid() => UnitNumbering.Validate("V-{NN}", usesFloors: false);
}

public class UnitPlannerTests
{
    private static readonly FloorSlot[] Floors =
    [
        new(10, "B1", 10, false), new(11, "G", 20, true), new(12, "01", 30, true), new(13, "02", 40, true), new(14, "T", 50, false)
    ];
    private static readonly UnitPosition[] Positions =
    [
        new(1, 7, "East", 72.12m, null, null, null, null), new(2, 7, "West", 72.12m, null, null, null, null)
    ];

    [Fact]
    public void Plans_units_on_habitable_floors_only()
    {
        var units = UnitPlanner.PlanOnFloors("{T}-{FF}{NN}", null, "A1", Floors, 10, 14, Positions, new HashSet<string>());
        Assert.Equal(["A1-G01", "A1-G02", "A1-0101", "A1-0102", "A1-0201", "A1-0202"], units.Select(u => u.UnitNo));
        Assert.All(units, u => Assert.NotEqual(10, u.FloorId));
    }

    [Fact]
    public void Marks_existing_numbers()
    {
        var units = UnitPlanner.PlanOnFloors("{T}-{FF}{NN}", null, "A1", Floors, 12, 12, Positions, new HashSet<string> { "A1-0101" });
        Assert.True(units.Single(u => u.UnitNo == "A1-0101").AlreadyExists);
        Assert.False(units.Single(u => u.UnitNo == "A1-0102").AlreadyExists);
    }

    [Fact]
    public void Reversed_range_is_rejected() =>
        Assert.Throws<DomainException>(() => UnitPlanner.PlanOnFloors("{T}-{FF}{NN}", null, "A1", Floors, 13, 12, Positions, new HashSet<string>()));

    [Fact]
    public void Range_with_no_habitable_floor_is_rejected() =>
        Assert.Throws<DomainException>(() => UnitPlanner.PlanOnFloors("{T}-{FF}{NN}", null, "A1", Floors, 14, 14, Positions, new HashSet<string>()));

    [Fact]
    public void Duplicate_positions_are_rejected() =>
        Assert.Throws<DomainException>(() => UnitPlanner.PlanOnFloors("{T}-{FF}{NN}", null, "A1", Floors, 12, 12,
            [Positions[0], Positions[0]], new HashSet<string>()));

    [Fact]
    public void Areas_out_of_order_are_rejected() =>
        Assert.Throws<DomainException>(() => UnitPlanner.PlanOnFloors("{T}-{FF}{NN}", null, "A1", Floors, 12, 12,
            [new UnitPosition(1, 7, null, 100m, 90m, null, null, null)], new HashSet<string>()));

    [Fact]
    public void Villas_have_no_floor()
    {
        var units = UnitPlanner.PlanWithoutFloors("V-{NN}", null, "VC1", [new(1, 3, null, null, null, null, null, 240m), new(2, 3, null, null, null, null, null, 240m)], new HashSet<string>());
        Assert.Equal(["V-01", "V-02"], units.Select(u => u.UnitNo));
        Assert.All(units, u => Assert.Null(u.FloorId));
    }
}

public class AvailabilityTests
{
    [Theory]
    [InlineData("Available", "Booked", true)]
    [InlineData("Booked", "Sold", true)]
    [InlineData("Sold", "Registered", true)]
    [InlineData("Registered", "Handed over", true)]
    [InlineData("Available", "Handed over", false)]
    [InlineData("Handed over", "Available", false)]
    public void Transitions(string from, string to, bool allowed) => Assert.Equal(allowed, Availability.CanMove(from, to));
}

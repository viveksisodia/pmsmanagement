namespace Construction.Domain.Units;

/// <summary>Sales availability of a unit. Kept separate from construction progress and compliance.</summary>
public static class Availability
{
    public const string Available = "Available";
    public const string Blocked = "Blocked";
    public const string Booked = "Booked";
    public const string Sold = "Sold";
    public const string Registered = "Registered";
    public const string HandedOver = "Handed over";

    public static readonly IReadOnlyList<string> All = [Available, Blocked, Booked, Sold, Registered, HandedOver];

    /// <summary>Same list as table cpm_AvailabilityTransition, which the database enforces.</summary>
    private static readonly IReadOnlyDictionary<string, string[]> Next = new Dictionary<string, string[]>
    {
        [Available] = [Blocked, Booked],
        [Blocked] = [Available, Booked],
        [Booked] = [Available, Sold],
        [Sold] = [Available, Registered],
        [Registered] = [HandedOver],
        [HandedOver] = []
    };

    public static IReadOnlyList<string> AllowedFrom(string status) => Next.TryGetValue(status, out var n) ? n : [];

    public static bool CanMove(string from, string to) => AllowedFrom(from).Contains(to);
}

using System.Diagnostics;
using PatternsLab.Problems.Builder;
using PatternsLab.Problems.Prototype;
using PatternsLab.Problems.Singleton;

Console.WriteLine("=== SINGLETON: AFTER ===");
var configs = new AppConfig[100];
Parallel.For(0, configs.Length, i => configs[i] = AppConfig.GetInstance());
foreach (AppConfig config in configs)
    Check(ReferenceEquals(config, AppConfig.GetInstance()), "Concurrent access returns the same config", false);
Check(AppConfig.LoadCount == 1, "100 concurrent accesses still use one load");

var db = new DatabaseService();
var ui = new UiService();
db.Config.Theme = "Dark";
ui.Render();
Check(ReferenceEquals(db.Config, ui.Config), "All services share the same config");
Check(ui.Config.Theme == "Dark", "Theme change is visible to the UI");
Check(AppConfig.LoadCount == 1, "Configuration loaded once");
Check(typeof(AppConfig).GetConstructors().Length == 0, "No public AppConfig constructor");

Console.WriteLine("\n=== PROTOTYPE: AFTER ===");
Enemy original = new Orc();
original.Name = "Boss Orc";
var stopwatch = Stopwatch.StartNew();
var army = new List<Enemy>();
for (int i = 1; i <= 5; i++)
{
    Enemy enemy = original.Clone();
    enemy.Name = $"Orc-{i}";
    army.Add(enemy);
}
stopwatch.Stop();
Console.WriteLine($"Cloned five orcs in {stopwatch.ElapsedMilliseconds} ms (no model loading).");
Enemy copy = EnemyCopyHelper.CopyEnemy(original);
Check(!ReferenceEquals(copy, original), "Clone is a new object");
Check(copy.ModelId == original.ModelId, "Private model ID is preserved");
Check(copy.GetType() == original.GetType(), "Runtime enemy type is preserved");
copy.Weapon.Name = "Magic Axe";
copy.Weapon.Damage = 999;
copy.Abilities.Add("Fire Breath");
Check(original.Weapon.Name == "Axe" && original.Weapon.Damage == 25, "Weapon is independent");
Check(original.Abilities.Count == 1, "Abilities are independent");
Enemy elf = new Elf();
Enemy elfCopy = elf.Clone();
Check(elfCopy.ModelId == elf.ModelId && elfCopy.GetType() == elf.GetType(), "Clone also works through Enemy for an Elf");
elfCopy.Abilities[0] = "Changed";
Check(elf.Abilities[0] == "Stealth", "Existing ability entries are independent");

Console.WriteLine("\n=== BUILDER: AFTER ===");
var live = RegistrationCallSites.CreateLiveStudent();
var videos = RegistrationCallSites.CreateVideosOnly();
Console.WriteLine(live);
Console.WriteLine(videos);
Check(live.GroupCode == "G1" && live.SendWhatsApp && live.SendEmailWelcome, "Live registration keeps its original settings");
Check(live.DiscountCode == "EARLY10" && live.MentorNote == "Needs evening slot" &&
    live.PreferredStart == new DateOnly(2026, 10, 1), "All optional values are preserved");
Check(videos.GroupCode == null && !videos.SendWhatsApp && videos.SendEmailWelcome, "Videos registration keeps its original settings");
ExpectFailure(new CourseRegistration.Builder("a@mail.com", "C1", "LiveGroup"), "Missing live group");
ExpectFailure(new CourseRegistration.Builder("a@mail.com", "C1", "VideosOnly").WithGroup("G1"), "Videos with a group");
ExpectFailure(new CourseRegistration.Builder("", "C1", "VideosOnly"), "Missing email");
ExpectFailure(new CourseRegistration.Builder("a@mail.com", "", "VideosOnly"), "Missing course");
ExpectFailure(new CourseRegistration.Builder("a@mail.com", "C1", "Unknown"), "Invalid mode");
var builder = new CourseRegistration.Builder("a@mail.com", "C1", "VideosOnly").WithDiscount("FIRST");
var first = builder.Build();
var second = builder.WithDiscount("SECOND").Build();
Check(first.DiscountCode == "FIRST" && second.DiscountCode == "SECOND", "Builder reuse does not change an existing registration");
Console.WriteLine("All pattern checks passed.");

static void Check(bool condition, string message, bool print = true)
{
    if (!condition)
        throw new Exception("FAIL: " + message);
    if (print)
        Console.WriteLine("PASS: " + message);
}

static void ExpectFailure(CourseRegistration.Builder builder, string message)
{
    
    bool rejected = false;
    try
    {
        builder.Build();
    }
    catch (ArgumentException ex)
    {
        rejected = true;
        Console.WriteLine(message + ": " + ex.Message);
    }
    catch (InvalidOperationException ex)
    {
        rejected = true;
        Console.WriteLine(message + ": " + ex.Message);
    }
    Check(rejected, message + " rejected at Build()");
}

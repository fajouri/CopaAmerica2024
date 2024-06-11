public class Team
{
	public string Name { get; set; }
	public double EloRating { get; set; }
}

public class Program
{
	private static readonly Dictionary<string, double> EloRatings = new()
	{
		{ "Argentina", 2143 },
		{ "Bolivia", 1600 },
		{ "Brazil", 2032 },
		{ "Chile", 1713 },
		{ "Colombia", 2012 },
		{ "Ecuador", 1869 },
		{ "Paraguay", 1709 },
		{ "Peru", 1742 },
		{ "Uruguay", 1992 },
		{ "Venezuela", 1744 },
		{ "Mexico", 1791 },
		{ "USA", 1786 },
		{ "Panama", 1712 },
		{ "Jamaica", 1642 },
		{ "Canada", 1721 },
		{ "Costa Rica", 1620 }
	};

	private static Random rand = new();

	public static void Main()
	{
		int numSimulations = 10000;
		Dictionary<string, int> winCounts = EloRatings.Keys.ToDictionary(team => team, team => 0);

		for (int i = 0; i < numSimulations; i++)
		{
			string winner = SimulateTournament();
			winCounts[winner]++;
		}

		foreach (var team in winCounts)
		{
			Console.WriteLine($"{team.Key}: {(double)team.Value / numSimulations:P2}");
		}
	}

	private static string SimulateTournament()
	{
		var groups = CreateGroups();
		var groupWinners = SimulateGroupStage(groups);
		var knockoutStage = SimulateKnockoutStage(groupWinners);
		return knockoutStage;
	}

	private static List<List<string>> CreateGroups()
	{
		var pot1 = new List<string> { "Argentina", "Mexico", "USA", "Brazil" };
		var pot2 = new List<string> { "Uruguay", "Colombia", "Ecuador", "Peru" };
		var pot3 = new List<string> { "Chile", "Panama", "Venezuela", "Paraguay" };
		var pot4 = new List<string> { "Jamaica", "Bolivia", "Canada", "Costa Rica" };

		var groups = new List<List<string>>();

		for (int i = 0; i < 4; i++)
		{
			groups.Add(new List<string> { pot1[i], pot2[i], pot3[i], pot4[i] });
		}

		return groups;
	}

	private static List<string> SimulateGroupStage(List<List<string>> groups)
	{
		var advancingTeams = new List<string>();

		foreach (var group in groups)
		{
			var groupStandings = new Dictionary<string, int>();

			foreach (var team in group)
			{
				groupStandings[team] = 0;
			}

			for (int i = 0; i < group.Count; i++)
			{
				for (int j = i + 1; j < group.Count; j++)
				{
					var winner = SimulateMatch(group[i], group[j]);
					groupStandings[winner]++;
				}
			}

			var sortedStandings = groupStandings.OrderByDescending(x => x.Value).ToList();
			advancingTeams.Add(sortedStandings[0].Key);
			advancingTeams.Add(sortedStandings[1].Key);
		}

		return advancingTeams;
	}

	private static string SimulateKnockoutStage(List<string> advancingTeams)
	{
		var quarterFinals = new List<(string, string)>
			{
				(advancingTeams[0], advancingTeams[5]),
				(advancingTeams[4], advancingTeams[1]),
				(advancingTeams[2], advancingTeams[7]),
				(advancingTeams[6], advancingTeams[3])
			};

		var semiFinals = new List<string>();

		foreach (var match in quarterFinals)
		{
			semiFinals.Add(SimulateMatch(match.Item1, match.Item2));
		}

		var final = SimulateMatch(semiFinals[0], semiFinals[1]);
		var thirdPlace = SimulateMatch(semiFinals[2], semiFinals[3]);

		return SimulateMatch(final, thirdPlace);
	}

	private static string SimulateMatch(string teamA, string teamB)
	{
		double eloA = EloRatings[teamA];
		double eloB = EloRatings[teamB];
		double probA = 1 / (1 + Math.Pow(10, (eloB - eloA) / 400));

		return rand.NextDouble() < probA ? teamA : teamB;
	}
}

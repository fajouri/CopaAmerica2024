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
		List<string> remainingTeams = [.. EloRatings.Keys];

		while (remainingTeams.Count > 1)
		{
			List<string> nextRoundTeams = [];
			for (int i = 0; i < remainingTeams.Count; i += 2)
			{
				string winner = SimulateMatch(remainingTeams[i], remainingTeams[i + 1]);
				nextRoundTeams.Add(winner);
			}
			remainingTeams = nextRoundTeams;
		}

		return remainingTeams[0];
	}

	private static string SimulateMatch(string teamA, string teamB)
	{
		double eloA = EloRatings[teamA];
		double eloB = EloRatings[teamB];
		double probA = 1 / (1 + Math.Pow(10, (eloB - eloA) / 400));

		return rand.NextDouble() < probA ? teamA : teamB;
	}
}

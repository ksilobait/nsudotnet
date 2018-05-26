using System;

namespace Izhitsky.Nsudotnet.NumberGuesser
{
	class Program
	{
		private static readonly string[] Inspirations =
		{
			"{0}, I thought people were smarter than AI. I've never been that wrong in my entire life",
			"You're going in the wrong direction, {0}",
			"Did you hear about {0}? My statistic say he is the worst in this game",
			"You're wasting your time, {0}",
			"{0}, did you know that Google recruits people based on your achievements?. Well, you're already out of their list",
			"After your excellent progress in this game, I'm wondering how have you launched it in the first place"
		};

		static void Main()
		{
			var initTime = DateTime.Now;

			Console.WriteLine("Type in your name:");
			var username = Console.ReadLine();
			var theNumber = new Random().Next(100 + 1);
			Console.WriteLine("Guess the number between 0 and 100 (or type q to quit):");

			var attempts = 0;
			var history = new bool[1000]; //true - greater
			while (true)
			{
				var input = Console.ReadLine();
				if (input == "q")
				{
					Console.WriteLine("The number was " + theNumber);
					return;
				}

				if (!int.TryParse(input, out var guessedNumber))
				{
					Console.WriteLine("That was not a number");
					continue;
				}

				attempts++;
				if (guessedNumber == theNumber)
				{
					var endTime = DateTime.Now;
					var delta = endTime - initTime;
					Console.WriteLine("\nCongratulations!");
					Console.WriteLine("Total attempts: " + attempts);
					Console.WriteLine("hours:mins:seconds : " + delta.ToString(@"hh\:mm\:ss"));
					Console.WriteLine("History:");
					for (var i = 0; i < Math.Min(attempts, 1000); i++)
					{
						Console.WriteLine(history[i] ? "greater" : "less");
					}

					return;
				}

				if (guessedNumber < theNumber)
				{
					Console.WriteLine("greater");
					if (attempts <= 1000)
					{
						history[attempts - 1] = true;
					}
				}
				else
				{
					Console.WriteLine("less");
					if (attempts <= 1000)
					{
						history[attempts - 1] = false;
					}
				}

				if (attempts % 4 != 0) continue;
				var phrase = new Random().Next(Inspirations.Length);
				Console.WriteLine(Inspirations[phrase], username);
			}
		}
	}
}
using System;
using System.Globalization;

namespace Izhitsky.Nsudotnet.Calendar
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Type in the date (e.g. MM DD YYYY)");
			if (!DateTime.TryParse(Console.ReadLine(), out var dateValue))
			{
				Console.WriteLine("ERROR: wrong date format");
				return;
			}

			Console.WriteLine("Red - weekdays, blue - input date, gray - today date");
			PrintDate(dateValue);
		}

		private static void PrintDate(DateTime dateValue)
		{
			// header
			var mondayToSunday = new DateTime(2018, 01, 01);
			for (var i = 0; i < 7; i++)
			{
				Console.Write(" {0}", DateTimeFormatInfo.CurrentInfo?.GetShortestDayName(mondayToSunday.DayOfWeek));
				mondayToSunday = mondayToSunday.AddDays(1);
			}

			Console.WriteLine();

			// skip till first day
			var currentTime = new DateTime(dateValue.Year, dateValue.Month, 1);
			var x = (int) currentTime.DayOfWeek; // 0 - Su, 1 - Mo, ..., 6 - Sa
			if (x == 0)
			{
				x = 7;
			}

			while (x > 1)
			{
				Console.Write("   ");
				x--;
			}

			var workingDays = 0;
			// print all days
			while (currentTime.Month == dateValue.Month)
			{
				if (currentTime.DayOfWeek != DayOfWeek.Saturday && currentTime.DayOfWeek != DayOfWeek.Sunday)
				{
					Console.BackgroundColor = ConsoleColor.Black;
					workingDays++;
				}
				else
				{
					Console.BackgroundColor = ConsoleColor.Red;
				}

				if (currentTime.Day == dateValue.Day)
				{
					Console.BackgroundColor = ConsoleColor.Blue;
				}

				if (currentTime == DateTime.Today)
				{
					Console.BackgroundColor = ConsoleColor.Gray;
					Console.ForegroundColor = ConsoleColor.Black;
				}

				Console.Write(" {0, 2}", currentTime.Day);
				if (currentTime.DayOfWeek == DayOfWeek.Sunday)
				{
					Console.WriteLine();
				}

				currentTime = currentTime.AddDays(1);
				Console.ForegroundColor = ConsoleColor.Gray;
			}

			Console.BackgroundColor = ConsoleColor.Black;
			Console.WriteLine();
			Console.WriteLine("{0} working days", workingDays);
		}
	}
}
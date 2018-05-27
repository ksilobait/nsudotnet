using System;
using System.IO;

namespace Izhitsky.Nsudotnet.LinesCounter
{
	class Program
	{
		private static int _totalLines;
		private static bool _lineContainsValuableSymbols = false;

		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine("pass the file pattern as parameter in the command line");
				return;
			}

			var pattern = args[0];
			var files = Directory.GetFiles(Directory.GetCurrentDirectory(), pattern,
				SearchOption.AllDirectories); // bin/debug by default
			foreach (var file in files)
			{
				using (var fileReader = new StreamReader(file))
				{
					ProcessFile(fileReader);
				}
			}

			Console.WriteLine("{0} lines found in files {1} from {2}", _totalLines, pattern, Directory.GetCurrentDirectory());
		}

		private static void ProcessFile(TextReader fileReader)
		{
			var inSinglelineComment = false;
			var inMultilineComment = false;

			int someSymbol;
			while ((someSymbol = fileReader.Read()) != -1)
			{
				var current = (char) someSymbol;
				var peeked = (char) fileReader.Peek();
				if (current == ' ' || current == '\t')
				{
					continue;
				}

				if (inSinglelineComment)
				{
					if (current == '\n')
					{
						inSinglelineComment = false;
					}
				}
				else if (inMultilineComment)
				{
					if (current == '\n')
					{
						LineEnd();
					}
					else if (current == '*' && peeked == '/')
					{
						fileReader.Read(); //forward
						inMultilineComment = false;
					}
				}
				else //no comment section
				{
					if (current == '\n')
					{
						LineEnd();
					}
					else if (current == '/' && peeked == '/')
					{
						fileReader.Read(); //forward
						inSinglelineComment = true;
						LineEnd();
					}
					else if (current == '/' && peeked == '*')
					{
						fileReader.Read(); //forward
						inMultilineComment = true;
					}
					else //usual symbol
					{
						_lineContainsValuableSymbols = true;
					}
				}
			}
		}

		private static void LineEnd()
		{
			if (_lineContainsValuableSymbols)
			{
				_lineContainsValuableSymbols = false;
				_totalLines++;
			}
		}
	}
}
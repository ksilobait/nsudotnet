using System;
using System.IO;

namespace Izhitsky.Nsudotnet.LinesCounter
{
	class Program
	{
		private static int _totalLines;
		private static bool _inMultilineComment; // false by default

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
			_inMultilineComment = false;
			string line;
			while ((line = fileReader.ReadLine()) != null)
			{
				line = line.Trim(); //TODO
				if (line.Length == 0)
				{
					continue;
				}

				var firstOnelineComment = line.IndexOf("//", StringComparison.Ordinal);
				if (!_inMultilineComment)
				{
					if (firstOnelineComment == 0)
					{
						continue;
					}

					SkipComments(line);
				}
				else // multiline comment started somewhere upper
				{
					var firstMultilineCommentEnd = line.IndexOf("*/", StringComparison.Ordinal);
					if (firstMultilineCommentEnd == -1) // no closing comment
					{
						continue;
					}

					_inMultilineComment = false;
					if (firstMultilineCommentEnd + 2 == line.Length)
					{
						continue;
					}

					SkipComments(line);
				}
			}
		}

		private static void SkipComments(string line)
		{
			var firstMultilineComment = line.IndexOf("/*", StringComparison.Ordinal);
			var firstMultilineCommentEnd = line.IndexOf("*/", StringComparison.Ordinal);
			while (firstMultilineComment == 0)
			{
				if (firstMultilineCommentEnd == -1) // comment goes down
				{
					_inMultilineComment = true;
					break;
				}

				// something after /**/ comment
				line = line.Substring(firstMultilineCommentEnd + 2); //TODO
				firstMultilineComment = line.IndexOf("/*", StringComparison.Ordinal);
				firstMultilineCommentEnd = line.IndexOf("*/", StringComparison.Ordinal);
			}

			var firstOnelineComment = line.IndexOf("//", StringComparison.Ordinal);
			if (_inMultilineComment || firstOnelineComment == 0)
			{
				return; //line consists only of comments
			}

			_totalLines++;
		}
	}
}
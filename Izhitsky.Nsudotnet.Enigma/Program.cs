using System.Security.Cryptography;

namespace Izhitsky.Nsudotnet.Enigma
{
	class Program
	{
		static void Main(string[] args)
		{
			var aCrypter = Parser.Parse(args);
			aCrypter.Run();
		}
	}
}

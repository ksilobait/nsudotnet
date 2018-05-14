using System;
using System.Security.Cryptography;

namespace Izhitsky.Nsudotnet.Enigma
{
	internal static class Parser
	{
		//encrypt file.txt rc2 output.bin
		//decrypt output.bin rc2 file.key.txt file.txt

		public static Crypter Parse(string[] args)
		{
			if (args.Length < 1)
			{
				PrintUsageAndTerminate();
			}

			switch (args[0])
			{
				case "encrypt":
				{
					if (args.Length != 4)
					{
						PrintUsageAndTerminate();
					}

					return new Encrypter(args[1], ParseMethod(args[2]), args[3]);
				}
				case "decrypt":
				{
					if (args.Length != 5)
					{
						PrintUsageAndTerminate();
					}

					return new Decrypter(args[1], ParseMethod(args[2]), args[3], args[4]);
				}
				default:
				{
					PrintUsageAndTerminate();
					throw new UnreachableException();
				}
			}
		}

		private static SymmetricAlgorithm ParseMethod(string method)
		{
			switch (method.ToLower())
			{
				case "aes":
				{
					return Aes.Create();
				}
				case "des":
				{
					return DES.Create();
				}
				case "rc2":
				{
					return RC2.Create();
				}
				case "rijndael":
				{
					return Rijndael.Create();
				}
				default:
				{
					PrintUsageAndTerminate();
					throw new UnreachableException();
				}
			}
		}

		private static void PrintUsageAndTerminate()
		{
			Console.WriteLine("Usage: exec encrypt source_file.txt [aes|des|rc2|rijndael] encripted_file.txt");
			Console.WriteLine("Alternative usage: exec decrypt encripted_file.txt [aes|des|rc2|rijndael] key_file.key.txt source_destination.txt");
			Environment.Exit(1);
		}
	}
}

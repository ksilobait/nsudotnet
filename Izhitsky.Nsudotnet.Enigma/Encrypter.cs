using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Izhitsky.Nsudotnet.Enigma
{
	internal class Encrypter : Crypter
	{
		private readonly string _inputFileName;
		private readonly Crypter.EncryptionMethod _cryptMethod;
		private readonly string _outputFileName;
		
		//file.txt rc2 output.bin
		public Encrypter(string inputFileName, Crypter.EncryptionMethod cryptMethod, string outputFileName)
		{
			_inputFileName = inputFileName;
			_cryptMethod = cryptMethod;
			_outputFileName = outputFileName;
		}

		internal override void Run()
		{
			switch (_cryptMethod)
			{
				case EncryptionMethod.Aes:
				{
					using (var aes = Aes.Create())
					{
						DumpKeys(aes, _inputFileName);
						DumpEncodedFile(aes, _inputFileName, _outputFileName);
					}
					break;
				}
				case EncryptionMethod.Des:
				{
					using (var des = DES.Create())
					{
						DumpKeys(des, _inputFileName);
						DumpEncodedFile(des, _inputFileName, _outputFileName);
					}
					break;
				}
				case EncryptionMethod.Rc2:
				{
					using (var rc2 = RC2.Create())
					{
						DumpKeys(rc2, _inputFileName);
						DumpEncodedFile(rc2, _inputFileName, _outputFileName);
					}
					break;
				}
				case EncryptionMethod.Rijndael:
				{
					using (var rj = Rijndael.Create())
					{
						DumpKeys(rj, _inputFileName);
						DumpEncodedFile(rj, _inputFileName, _outputFileName);
					}
					break;
				}
				default:
				{
					throw new UnreachableException();
				}
			}
		}

		private static void DumpKeys(SymmetricAlgorithm algorithm, string inputFileName)
		{
			var initializationVector = Encoding.UTF8.GetBytes(Convert.ToBase64String(algorithm.IV));
			var breakLine = Encoding.UTF8.GetBytes("\n");
			var key = Encoding.UTF8.GetBytes(Convert.ToBase64String(algorithm.Key));

			var keyFileName = inputFileName.Replace(".txt", ".key.txt");
			using (var keyFileStream = new FileStream(keyFileName, FileMode.Create))
			{
				keyFileStream.Write(initializationVector, 0, initializationVector.Length);
				keyFileStream.Write(breakLine, 0, breakLine.Length);
				keyFileStream.Write(key, 0, key.Length);
			}
		}

		private static void DumpEncodedFile(SymmetricAlgorithm algorithm, string inputFileName, string outputFileName)
		{
			using (var inputFileStream = new FileStream(inputFileName, FileMode.Open))
			using (var outputFileStream = new FileStream(outputFileName, FileMode.Create))
			{
				var encrypter = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV);
				using (var cryptoStream = new CryptoStream(outputFileStream, encrypter, CryptoStreamMode.Write))
				{
					inputFileStream.CopyTo(cryptoStream);
				}
			}
		}
	}
}

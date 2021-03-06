﻿using System;
using System.IO;
using System.Security.Cryptography;

namespace Izhitsky.Nsudotnet.Enigma
{
	internal class Decrypter : Crypter
	{
		private readonly string _inputFileName;
		private readonly Crypter.EncryptionMethod _cryptMethod;
		private readonly string _keyFileName;
		private readonly string _outputFileName;
		
		//output.bin rc2 file.key.txt file.txt
		public Decrypter(string inputFileName, Crypter.EncryptionMethod cryptMethod, string keyFileName, string outputFileName)
		{
			_inputFileName = inputFileName;
			_cryptMethod = cryptMethod;
			_keyFileName = keyFileName;
			_outputFileName = outputFileName;
		}

		internal override void Run()
		{
			string anInitializationVector;
			string aKey;
			using (var keyFileStreamReader = new StreamReader(new FileStream(_keyFileName, FileMode.Open)))
			{
				anInitializationVector = keyFileStreamReader.ReadLine();
				aKey = keyFileStreamReader.ReadLine();
			}

			if (anInitializationVector == null || aKey == null)
			{
				throw new Exception("Invalid key file");
			}

			switch (_cryptMethod)
			{
				case EncryptionMethod.Aes:
				{
					using (var algorithm = Aes.Create())
					{
						Decrypt(anInitializationVector, algorithm, aKey);
					}
					break;
				}
				case EncryptionMethod.Des:
				{
					using (var algorithm = DES.Create())
					{
						Decrypt(anInitializationVector, algorithm, aKey);
					}
					break;
				}
				case EncryptionMethod.Rc2:
				{
					using (var algorithm = RC2.Create())
					{
						Decrypt(anInitializationVector, algorithm, aKey);

					}
					break;
				}
				case EncryptionMethod.Rijndael:
				{
					using (var algorithm = Rijndael.Create())
					{
						Decrypt(anInitializationVector, algorithm, aKey);
					}
					break;
				}
				default:
				{
					throw new UnreachableException();
				}
			}
		}

		private void Decrypt(string anInitializationVector, SymmetricAlgorithm algorithm, string aKey)
		{
			var initializationVector = Convert.FromBase64String(anInitializationVector);
			algorithm.IV = initializationVector;
			var key = Convert.FromBase64String(aKey);
			algorithm.Key = key;

			var decrypter = algorithm.CreateDecryptor();
			using (var inputFileStream = new FileStream(_inputFileName, FileMode.Open))
			using (var outputFileStream = new FileStream(_outputFileName, FileMode.Create))
			using (var cryptoStream = new CryptoStream(inputFileStream, decrypter, CryptoStreamMode.Read))
			{
				cryptoStream.CopyTo(outputFileStream);
			}
		}
	}
}

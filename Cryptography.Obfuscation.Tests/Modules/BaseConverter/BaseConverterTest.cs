﻿using Cryptography.Obfuscation.Modules;
using Cryptography.Obfuscation.Tests.Factory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text;

namespace Cryptography.Obfuscation.Tests.Modules
{
    [TestClass]
    public class BaseConverterTest
    {
        [TestMethod]
        public void TestAddDummyCharactersConstant()
        {
            var validCharacters = Settings.ValidCharacterSet;
            var dummyCharacters = Settings.DummyCharacterSet;

            // Generate valid input by taking first 3 characters of validCharacters.
            string validInput = new string(validCharacters.Dictionary.Select(x => x.Value).Take(3).ToArray());

            /* Perform operation under test with constant strategy, */
            var obfuscatorUnderTest = ObfuscatorFactory.NewInstance;
            int seed = obfuscatorUnderTest.Seed;
            var testResult = Settings.AddDummyCharacters(validInput, ObfuscationStrategy.Constant, seed);

            // Remove all valid characters from result.
            var dummyResult = testResult.ToCharArray().ToList();
            dummyResult.RemoveAll(x => validCharacters.ContainsValue(x));

            // Ensure result only contains dummy characters.
            Assert.IsTrue(dummyResult.All(x => dummyCharacters.Contains(x)));
        }

        [TestMethod]
        public void TestAddDummyCharactersRandomized()
        {
            var validCharacters = Settings.ValidCharacterSet;
            var dummyCharacters = Settings.DummyCharacterSet;

            // Generate valid input by taking first 3 characters of validCharacters.
            string validInput = new string(validCharacters.Dictionary.Select(x => x.Value).Take(3).ToArray());

            /* Perform operation under test with randomized strategy, */
            var obfuscatorUnderTest = ObfuscatorFactory.NewInstance;
            int seed = obfuscatorUnderTest.Seed;
            var testResult = Settings.AddDummyCharacters(validInput, ObfuscationStrategy.Randomize, seed);

            // Remove all valid characters from result.
            var dummyResult = testResult.ToCharArray().ToList();
            dummyResult.RemoveAll(x => validCharacters.ContainsValue(x));

            // Ensure result only contains dummy characters.
            Assert.IsTrue(dummyResult.All(x => dummyCharacters.Contains(x)));
        }

        [TestMethod]
        public void TestRemoveDummyCharacters()
        {
            var validCharacterSet = Settings.ValidCharacterSet;
            var dummyCharacters = Settings.DummyCharacterSet;

            // Combine input values, which should contain both valid/invalid characters.
            int minLength = Math.Min(dummyCharacters.Length, validCharacterSet.Count);

            var sb = new StringBuilder();
            for (int i = 0; i < minLength; i++) {
                // Append valid/invalid characters in alternating sequence.
                sb.AppendFormat("{0}{1}", validCharacterSet.GetFromKey(i), dummyCharacters[i]);
            }

            string input = sb.ToString();

            // Perform operation under test.
            var obfuscatorUnderTest = ObfuscatorFactory.NewInstance;
            var testResult = Settings.RemoveDummyCharacters(input);

            // Ensure resulting string doesn't contain any dummy characters.
            Assert.IsTrue(testResult.All(x => !dummyCharacters.Contains(x)));
        }
    }
}

using ConsoleApp1;
using PalindromeChecker.Models;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

class Program
{

    private static readonly FileManager<SortedPalindromeResult> _fileManager = new(@"C:\projects\personal\DotnetConcurrency\ConsoleApp1\Assets\palidroms.json");
    private static readonly PalindromChecker _palindromeChecker = new();

    static void Main(string[] args)
    {
        bool anyPalindromesLeftToCheck = true;

        while (anyPalindromesLeftToCheck)
        {
            var existingPalidromsResultOrNull = _fileManager.ReadFromFileAsync().Result;
            var existingPalidromsResults = existingPalidromsResultOrNull == null ? new PalindromeResult() : new PalindromeResult(existingPalidromsResultOrNull);
            var palidromesLeftToCheck = PalindromUtils.GetPalindromesLeftToCheck(existingPalidromsResults);

            if ( !palidromesLeftToCheck.Any())
            {
                break;
            }

            var result = _palindromeChecker.CheckPalindromes(palidromesLeftToCheck.Take(50)).Result;
            var sortedResult = new SortedPalindromeResult(result);

            var mergedPalindromResult = PalindromUtils.MergePalidromResults(new SortedPalindromeResult(existingPalidromsResults), sortedResult);

            _fileManager.SaveToFileAsync(mergedPalindromResult).Wait();
            Console.WriteLine("Finished checking 50 palindromes, sleeping for 5 seconds before next iteration");
            Thread.Sleep(5000); // let the DMV website rest a bit
            
        }
    }
}

using ConsoleApp1;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

class Program
{

    private static readonly FileManager<SortedPalindromeResult> _fileManager = new(@"C:\projects\personal\DotnetConcurrency\ConsoleApp1\palidroms.json");
    private static readonly PalindromChecker _palindromeChecker = new();

    static void Main(string[] args)
    {
        var palidromesLeftToCheck = GetPalindromesLeftToCheck();
        var existingPalidromsResults = _fileManager.ReadFromFileAsync().Result;

        var result = _palindromeChecker.CheckPalindromes(palidromesLeftToCheck.Take(50)).Result;
        var sortedResult = new SortedPalindromeResult(result);

        var mergedPalindromResult = MergePalidromResults(existingPalidromsResults, sortedResult);

        _fileManager.SaveToFileAsync(mergedPalindromResult).Wait();
    }

    private static HashSet<string> GetPalindromesLeftToCheck()
    {
        var allPossiblePalindromes = GetAllPossiblePalidromes().ShouldTryAgain.ToHashSet(); //only needed at first run

        var existingPalidromsResults = _fileManager.ReadFromFileAsync().Result;

        existingPalidromsResults.ShouldTryAgain.ToHashSet();
        var palidromesLeftToCheck = new HashSet<string>(allPossiblePalindromes);
        palidromesLeftToCheck.ExceptWith(existingPalidromsResults.ExistingPalindromes);
        palidromesLeftToCheck.ExceptWith(existingPalidromsResults.NonExistingPalindromes);

        return palidromesLeftToCheck;
    }

    private static SortedPalindromeResult MergePalidromResults(SortedPalindromeResult existingPalidromResults, SortedPalindromeResult newPalindromeResult)
    {
        SortedPalindromeResult sortedPalindromeResult = new();

        foreach (var item in existingPalidromResults.ExistingPalindromes)
        {
            sortedPalindromeResult.ExistingPalindromes.Add(item);
        }
        foreach(var item in existingPalidromResults.NonExistingPalindromes)
        {
            sortedPalindromeResult.NonExistingPalindromes.Add(item);
        }

        foreach(var item in existingPalidromResults.ShouldTryAgain)
        {
            sortedPalindromeResult.ShouldTryAgain.Add(item);
        }

        foreach (var item in newPalindromeResult.ExistingPalindromes)
        {
            sortedPalindromeResult.ExistingPalindromes.Add(item);
            existingPalidromResults.ShouldTryAgain.Remove(item);
        }
        foreach (var item in newPalindromeResult.NonExistingPalindromes)
        {
            sortedPalindromeResult.NonExistingPalindromes.Add(item);
            existingPalidromResults.ShouldTryAgain.Remove(item);
        }

        sortedPalindromeResult.ShouldTryAgain = existingPalidromResults.ShouldTryAgain;

        return sortedPalindromeResult;
    }

    private static PalindromeResult GetAllPossiblePalidromes()
    {
        var all7DigPalindromes = GetAll7DigPalindromesToCheck();
        var all8DigPalindromes = GetAll8DigPalindromesToCheck();


        var existingPalidromResults = _fileManager.ReadFromFileAsync().Result;
        PalindromeResult palidromResult = new();

        foreach (var p in all7DigPalindromes)
        {
            palidromResult.ShouldTryAgain.Add(p);
        }

        foreach (var p in all8DigPalindromes)
        {
            palidromResult.ShouldTryAgain.Add(p);
        }

        return palidromResult;
        //await (new FileManager<PalindromeResult>(@"C:\projects\personal\DotnetConcurrency\ConsoleApp1\palidroms2.json").SaveToFileAsync(palidromResult));
        //_fileManager.SaveToFileAsync(new SortedPalindromeResult(palidromResult)).Wait();
    }

    private static List<string> GetPalindromesToCheck()
    {
        HashSet<string> palindromesToCheck = new();

        int start = 62_000;


        var range = Enumerable.Range(0, 1000);

        foreach (var i in range)
        {
            var palToCheck = (start + i).ToString() + "26";

            bool isPalindrome = true;
            var l = 0;
            var r = palToCheck.Length - 1;

            while (l < r)
            {
                if (palToCheck[l] != palToCheck[r])
                {
                    isPalindrome = false;
                    break;
                }
                l++;
                r--;
            }

            if (isPalindrome)
            {
                palindromesToCheck.Add(palToCheck);
                Console.WriteLine($"Added {palToCheck}");
            }
        }

        return palindromesToCheck.ToList();
    }

    private static List<string> GetAll7DigPalindromesToCheck()
    {
        int start = 1_00_000_00;
        var range = Enumerable.Range(0, 99_999_99);
        var palindromesToCheck = GetPalindromesInRange(start, range);

        return palindromesToCheck.ToList();
    }

    private static List<string> GetAll8DigPalindromesToCheck()
    {
        int start = 1_100_00_000;
        var range = Enumerable.Range(0, 999_999);
        var palindromesEndingWith01 = GetPalindromesInRange(start, range);

        start = 1_200_00_000;
        var palindromesEndingWith02 = GetPalindromesInRange(start, range);
        start = 1_300_00_000;
        var palindromesEndingWith03 = GetPalindromesInRange(start, range);
        start = 1_400_00_000;
        var palindromesEndingWith04 = GetPalindromesInRange(start, range);

        return palindromesEndingWith01.
            Union(palindromesEndingWith02).
            Union(palindromesEndingWith03).
            Union(palindromesEndingWith04).
            ToList();
    }

    private static HashSet<string> GetPalindromesInRange(int start, IEnumerable<int> range)
    {
        HashSet<string> palindromesToCheck = new();
        foreach (var i in range)
        {
            var palToCheck = (start + i).ToString().Substring(1);

            bool isPalindrome = true;
            var l = 0;
            var r = palToCheck.Length - 1;

            while (l < r)
            {
                if (palToCheck[l] != palToCheck[r])
                {
                    isPalindrome = false;
                    break;
                }
                l++;
                r--;
            }

            if (isPalindrome)
            {
                palindromesToCheck.Add(palToCheck);
            }
        }
        return palindromesToCheck;
    }
}

using System.Collections.Concurrent;

namespace PalindromeChecker.Models
{
    public class PalindromeResult
    {
        public ConcurrentBag<string> ExistingPalindromes { get; set; } = new();
        public ConcurrentBag<string> NonExistingPalindromes { get; set; } = new();
        public ConcurrentBag<string> ShouldTryAgain { get; set; } = new();

        public PalindromeResult()
        {

        }

        public PalindromeResult(SortedPalindromeResult sortedPalindromeResult)
        {
            ExistingPalindromes = new(sortedPalindromeResult.ExistingPalindromes);
            NonExistingPalindromes = new(sortedPalindromeResult.NonExistingPalindromes);
            ShouldTryAgain = new(sortedPalindromeResult.ShouldTryAgain);
        }
    }
}

using System.Collections.Concurrent;

namespace ConsoleApp1
{
    public class SortedPalindromeResult
    {
        public IList<string> ExistingPalindromes { get; set; } = new List<string>();
        public IList<string> NonExistingPalindromes { get; set; } = new List<string>();
        public IList<string> ShouldTryAgain { get; set; } = new List<string>();

        public SortedPalindromeResult()
        {
        }
        public SortedPalindromeResult(PalindromeResult palindromeResult)
        {

            ExistingPalindromes = palindromeResult.ExistingPalindromes.ToList();
            ((List<string>)ExistingPalindromes).Sort();

            NonExistingPalindromes = palindromeResult.NonExistingPalindromes.ToList();
            ((List<string>)NonExistingPalindromes).Sort();

            ShouldTryAgain = palindromeResult.ShouldTryAgain.ToList();
            ((List<string>)ShouldTryAgain).Sort();
        }
    }
}

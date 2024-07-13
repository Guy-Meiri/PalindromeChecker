using System.Collections.Concurrent;

namespace ConsoleApp1
{
    public class PalindromeResult
    {
        public ConcurrentBag<string> ExistingPalindromes { get; set; } = new();
        public ConcurrentBag<string> NonExistingPalindromes { get; set; } = new();
        public ConcurrentBag<string> ShouldTryAgain { get; set; } = new();

        public PalindromeResult()
        {

        }
    }
}

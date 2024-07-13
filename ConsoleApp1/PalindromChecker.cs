using HtmlAgilityPack;

namespace ConsoleApp1
{
    public class PalindromChecker
    {
        public async Task<PalindromeResult> CheckPalindromes(IEnumerable<string> palindromes)
        {
            PalindromeResult result = new PalindromeResult();
            ParallelOptions parallelOptions = new()
            {
                MaxDegreeOfParallelism = 20
            };

            await Parallel.ForEachAsync(palindromes, parallelOptions, async (id, _cancellationToken) =>
            {
                await CheckIfCarExists(id, result);
            });

            return result;

        }

        private async Task CheckIfCarExists(string id, PalindromeResult result)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(20000);

            Console.WriteLine($"Checking {id}");
            using (var client = new HttpClient())
            {
                try
                {
                    string url = $"https://www.find-car.co.il/report?PlateNumber={id}";
                    var response = await client.GetAsync(url, cts.Token);
                    string content = await response.Content.ReadAsStringAsync();
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(content);

                    var mainTitleDiv = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'main_title')]");
                    if (mainTitleDiv != null)
                    {
                        var h1Tag = mainTitleDiv.SelectSingleNode(".//h1");
                        if (h1Tag != null && h1Tag.InnerText.Contains(" אופס לא מצאנו את הרכב שחיפשת"))
                        {
                            Console.WriteLine($"didnot find {id}");
                            result.NonExistingPalindromes.Add(id);
                            return;
                        }

                        var h2Tag = mainTitleDiv.SelectSingleNode(".//h2[contains(@class, 'title')]");
                        if (h2Tag != null)
                        {
                            result.ExistingPalindromes.Add(id);
                            Console.WriteLine($"found {id}");
                            return;
                        }
                    }
                    Console.WriteLine($"try again for {id}");
                    result.ShouldTryAgain.Add(id);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                    Console.WriteLine($"Exception for {id}");
                    result.ShouldTryAgain.Add(id);
                    return;

                }

                Console.WriteLine($"Finished checking {id}");
            }
        }
    }
}

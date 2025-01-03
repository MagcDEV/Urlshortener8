using Microsoft.EntityFrameworkCore;

namespace Urlshortener.Api.Services;

public class UrlShorteningService(ApplicationDbContext dbContext)
{
    public const int NumberOfCharsInShortLink = 7;
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    
    private readonly Random _random = new();

    public async Task<string> GenerateUniqueCode()
    {
        var codeChars = new char[NumberOfCharsInShortLink];

        while (true)
        {
            for (var i = 0; i < NumberOfCharsInShortLink; i++)
            {
                var randomIndex = _random.Next(Alphabet.Length - 1);
                codeChars[i] = Alphabet[randomIndex];
            }

            var code = new string(codeChars);

            if (!await dbContext.ShortenedUrls.AnyAsync(x => x.Code == code))
            {
                return code;
            }
        }
    }

    public async Task<string?> GetUrlByCode(string code)
    {
        var shortenedUrl = await dbContext.ShortenedUrls.FirstOrDefaultAsync(x => x.Code == code);
        return shortenedUrl?.LongUrl.ToString() ?? null;
    }
}
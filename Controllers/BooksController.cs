using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using BookRadar.Data;
using BookRadar.Models;
using Microsoft.Extensions.Caching.Memory;

namespace BookRadar.Controllers;

public class BooksController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _cache;

    public BooksController(ApplicationDbContext db, IHttpClientFactory httpClientFactory, IMemoryCache cache)
    {
        _db = db;
        _httpClientFactory = httpClientFactory;
        _cache = cache;
    }

    public async Task<IActionResult> Index(string? author)
    {
        var history = await _db.HistorialBusquedas
            .OrderByDescending(h => h.FechaConsulta)
            .Take(50)
            .ToListAsync();

        if (string.IsNullOrWhiteSpace(author))
        {
            ViewBag.Results = new List<BookResult>();
            return View(new BooksIndexViewModel { Author = string.Empty, History = history });
        }

        var results = await SearchOpenLibraryAsync(author);

        var now = DateTime.UtcNow;
        foreach (var r in results)
        {
            var cacheKey = $"{author}|{r.Title}";
            if (!_cache.TryGetValue(cacheKey, out _))
            {
                // Guardar en DB
                _db.HistorialBusquedas.Add(new SearchHistory
                {
                    Autor = author,
                    Titulo = r.Title,
                    AnioPublicacion = r.FirstPublishYear,
                    Editorial = r.Publisher,
                    FechaConsulta = now
                });

                // Guardar en cache por 1 minuto
                _cache.Set(cacheKey, true, TimeSpan.FromMinutes(1));
            }
        }

        await _db.SaveChangesAsync();

        ViewBag.Results = results;
        var vm = new BooksIndexViewModel { Author = author, History = history };
        return View(vm);
    }

    public async Task<IActionResult> History()
    {
        var history = await _db.HistorialBusquedas.OrderByDescending(h => h.FechaConsulta).ToListAsync();
        return View(history);
    }

    private async Task<List<BookResult>> SearchOpenLibraryAsync(string author)
    {
        var client = _httpClientFactory.CreateClient();
        var encoded = System.Net.WebUtility.UrlEncode(author);
        var url = $"https://openlibrary.org/search.json?author={encoded}";

        using var resp = await client.GetAsync(url);
        if (!resp.IsSuccessStatusCode)
            return new List<BookResult>();

        var json = await resp.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<OpenLibraryResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result?.Docs?.Select(d => new BookResult
        {
            Title = d.Title ?? string.Empty,
            FirstPublishYear = d.FirstPublishYear,
            Publisher = d.Publisher?.FirstOrDefault()
        }).ToList() ?? new List<BookResult>();
    }
}

public class OpenLibraryResponse
{
    public List<OpenLibraryDoc>? Docs { get; set; }
}

public class OpenLibraryDoc
{
    public string? Title { get; set; }
    public int? FirstPublishYear { get; set; }
    public List<string>? Publisher { get; set; }
}

public class BooksIndexViewModel
{
    public string Author { get; set; } = string.Empty;
    public List<SearchHistory> History { get; set; } = new();
}
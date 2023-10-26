using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fall2023_Assignment3_jschmith.Data;
using Fall2023_Assignment3_jschmith.Models;
using Azure.AI.OpenAI;
using Azure;
using OpenAI_API;
using OpenAI_API.Completions;

namespace Fall2023_Assignment3_jschmith.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;
        private object _config;
        private readonly IConfiguration _configuration;

        public MovieController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Movie
        public async Task<IActionResult> Index()
        {
              return _context.Movie != null ? 
                          View(await _context.Movie.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Movie'  is null.");
        }

        // GET: Movie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }
            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            var reviews = GetReviews(movie);
            
            
            return View(movie);
        }

        // GET: Movie/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Genre,Year,Imdb")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Genre,Year,Imdb")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Movie'  is null.");
            }
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //ChatGPT reviews
        [HttpPost("getreviews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetReviews(Movie movie)
        {
            string key = _configuration["apiKey"];

            string movieName = movie.Name;

            var openai = new OpenAIAPI(key);
            CompletionRequest completion = new CompletionRequest();
            completion.Prompt = $"Write 10 reviews for the movie {movieName}";
            completion.Model = OpenAI_API.Models.Model.DavinciText;
            completion.MaxTokens = 1000;

            var result = openai.Completions.CreateCompletionAsync(completion);
            var reviews = new List<string>();
            foreach(var item in result.Result.Completions)
            {
                reviews.Add(item.Text);
            }

            var movieEntity = _context.Movie.FirstOrDefault(m => m.Id == movie.Id);
            if (movieEntity != null)
            {
                foreach (var reviewText in reviews)
                {
                    var review = new Review
                    {
                        MovieId = movieEntity.Id,
                        Text = reviewText
                    };
                    _context.Review.Add(review);
                }
                _context.SaveChanges();
            }
            return Ok(reviews);
        }

        public List<Review> GetMovieReviews(int movieId)
        {
           return _context.Review.Where(r => r.MovieId == movieId).ToList();
        }

        private bool MovieExists(int id)
        {
          return (_context.Movie?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

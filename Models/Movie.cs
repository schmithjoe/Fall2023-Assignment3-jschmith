using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fall2023_Assignment3_jschmith.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string? Genre { get; set; }

        public int Year { get; set; }

        public string? Imdb { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}

namespace Fall2023_Assignment3_jschmith.Models
{
    public class Actor
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Gender { get; set; }

        public int Age { get; set; }

        public string? Imdb { get; set; }
    }
}

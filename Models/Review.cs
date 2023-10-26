namespace Fall2023_Assignment3_jschmith.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}

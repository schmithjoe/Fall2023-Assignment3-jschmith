using System.ComponentModel.DataAnnotations.Schema;

namespace Fall2023_Assignment3_jschmith.Models
{
    public class MovieActor
    {
        public int Id { get; set; }

        [ForeignKey("Actor")]
        public int ActorId { get; set; }

        public Actor? Actor { get; set; }

        [ForeignKey("Movie")]
        public int MovieId { get; set; }

        public Movie? Movie { get; set; }
    }
}

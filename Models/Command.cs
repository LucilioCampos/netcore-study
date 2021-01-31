using System.ComponentModel.DataAnnotations;

namespace Commander.Models
{
    public class Command
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string HowTo { get; set; }
        [Required]
        public string Line { get; set; }
        [Required]
        public string Plataform { get; set; }

        public Command() {}

         public Command(string HowTo, string Line, string Plataform)
        {
            this.HowTo = HowTo;
            this.Line = Line;
            this.Plataform = Plataform;
        }

        public Command(int Id, string HowTo, string Line, string Plataform)
        {
            this.Id = Id;
            this.HowTo = HowTo;
            this.Line = Line;
            this.Plataform = Plataform;
        }

    }


}
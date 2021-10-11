using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Student
{
    public class StudentDeleteCommand
    {
        [Required]
        public int Id { get; set; }
    }
}
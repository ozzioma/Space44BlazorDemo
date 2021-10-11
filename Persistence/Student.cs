using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class Student
    {
        public int Id { get; set; }
        
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public string Career { get; set; }
    }
}
using System.ComponentModel.DataAnnotations.Schema;

namespace activeOrNotActive.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Salary { get; set; }
        public string imgPath { get; set; }
        public string imgname { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("Department")]
        public int DpartmentId { get; set; }
        public Department? Department { get; set; }

    }
}

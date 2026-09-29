using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskLec25.Models
{
        //[PrimaryKey(nameof(TaskId))]
    public class MyTask
    {
        public int MyTaskId { set; get; }

        [Required]
        public string Name { set; get; } = string.Empty;


        [Required]
        public string Description { set; get; } = string.Empty;


        public string FileName { set; get; } = string.Empty;

        public DateTime Deadline { set; get; }

        public int UserId { set; get; }

        [ForeignKey(nameof(UserId))]
        public User User { set; get; } = null;
    }
}

namespace TaskLec25.Models
{
    public class User
    {
        public int Id { set; get; }
        public string Name { set; get; } = string.Empty;

        public ICollection<MyTask> Tasks { get; set; } = new List<MyTask>();

    }
}

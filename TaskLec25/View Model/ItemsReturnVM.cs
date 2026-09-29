namespace TaskLec25.View_Model
{
    public class ItemsReturnVM
    {
        public IEnumerable<MyTask> Tasks { set; get; } = [];
        public string name { set; get; } = string.Empty;
        public int  Id { set; get; }
    }
}

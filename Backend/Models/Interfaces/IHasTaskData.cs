namespace Backend.Models.Interfaces {
    public interface IHasTaskData {
        public string Name        { get; set; }
        public string Description { get; set; }
        public int    StatusId    { get; set; }
    }
}

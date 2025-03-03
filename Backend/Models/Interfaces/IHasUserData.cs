namespace Backend.Models.Interfaces {
    public interface IHasUserData {
        public string     Login    { get; set; }
        public string     Password { get; set; }
        public string     Name     { get; set; }
        public string     Surname  { get; set; }
        public string     Email    { get; set; }
        public UserRights Rights   { get; set; } 
    }
}

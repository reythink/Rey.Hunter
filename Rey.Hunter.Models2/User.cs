namespace Rey.Hunter.Models2 {
    public class User : AccountModel {
        public string Email { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; } = true;
        public string PortraitUrl { get; set; } = "/img/avatar.png";
        public string Position { get; set; }
    }
}


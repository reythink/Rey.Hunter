namespace Rey.Hunter.Models2 {
    public class Role : AccountModel {
        public string Name { get; set; }
        public bool Enabled { get; set; } = true;
    }
}


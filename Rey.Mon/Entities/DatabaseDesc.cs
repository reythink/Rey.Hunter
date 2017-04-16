namespace Rey.Mon.Entities {
    public class DatabaseDesc : Entity, IDatabaseDesc {
        public string Name { get; set; }
        public double SizeOnDisk { get; set; }
        public bool Empty { get; set; }
    }
}

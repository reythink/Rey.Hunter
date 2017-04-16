namespace Rey.Mon.Entities {
    public interface IDatabaseDesc {
        string Name { get; }
        double SizeOnDisk { get; }
        bool Empty { get; }
    }
}

namespace HD.Security
{
    public interface ISesion
    {
        public string? usuario();
        public string? origen();
        public bool generarLog();
    }
}

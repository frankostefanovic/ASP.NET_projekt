namespace Lab2.RezervacijeProstora.Dtos
{
    public class ProstorDatotekaReadDto
    {
        public int Id { get; set; }
        public int ProstorZaProbuId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

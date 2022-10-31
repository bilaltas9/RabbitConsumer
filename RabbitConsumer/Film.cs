namespace RabbitConsumer
{
    internal class Film
    {
        public int Id { get; set; }
        public string? PosterPath { get; set; }
        public bool Adult { get; set; }
        public string? Overview { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? OriginalTitle { get; set; }
        public string? OriginalLanguage { get; set; }
        public string? Title { get; set; }
        public int VoteCount { get; set; }
        public int VoteAverage { get; set; }
        public int Point { get; set; }
        public string? Note { get; set; }
    }
}

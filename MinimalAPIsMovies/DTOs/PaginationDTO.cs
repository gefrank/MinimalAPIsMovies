namespace MinimalAPIsMovies.DTOs
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;
        private int recordsPerPage = 10;
        private readonly int maxRecordsPerPage = 50;
        public int RecordsPerPage
        {
            get => recordsPerPage;
            set
            {
                recordsPerPage = (value > maxRecordsPerPage) ? maxRecordsPerPage : value;
            }
        }
    }
}

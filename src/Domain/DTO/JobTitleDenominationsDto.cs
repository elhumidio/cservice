
namespace Domain.DTO
{
    public class JobTitleDenominationsDto
    {
        public int Id { get; set; }
        public string Denomination { get; set; } = null!;
        public int FkJobTitle { get; set; }

    }
}

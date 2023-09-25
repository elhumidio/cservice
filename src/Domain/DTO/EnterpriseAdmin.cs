
namespace Domain.DTO
{
    public class EnterpriseAdmin
    {
        public int EnterpriseID { get; set; }
        public int UserId { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsManager { get; set; }
    }
}

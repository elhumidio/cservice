namespace Domain.DTO.Distribution
{
    public class EnterpriseUserJobVacDto
    {
        public int IdenterpriseUser { get; set; }
        public int IdjobVacType { get; set; }
        public int MaxJobVacancies { get; set; }
        public int JobVacUsed { get; set; }
        public int Idproduct { get; set; }
        public int Idcontract { get; set; }
        public int? IdjobVacTypeComp { get; set; }
        public DateTime StartDate { get; set; }
    }
}

namespace Domain.Entities
{
    public partial class TsturijobsLang
    {
        public TsturijobsLang()
        {
            Tareas = new HashSet<Area>();
            TjobExpYears = new HashSet<JobExpYear>();
            TjobVacTypes = new HashSet<JobVacType>();
            Tproducts = new HashSet<Product>();
            TsalaryTypes = new HashSet<SalaryType>();
        }

        public int IdsturiJobsLang { get; set; }
        public int Idsite { get; set; }
        public string LangName { get; set; } = null!;
        public int? SysMsgLangId { get; set; }

        public virtual ICollection<Area> Tareas { get; set; }
        public virtual ICollection<JobExpYear> TjobExpYears { get; set; }
        public virtual ICollection<JobVacType> TjobVacTypes { get; set; }
        public virtual ICollection<Product> Tproducts { get; set; }
        public virtual ICollection<SalaryType> TsalaryTypes { get; set; }
    }
}

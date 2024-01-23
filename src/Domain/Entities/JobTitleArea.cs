
namespace Domain.Entities
{
    public class JobTitleArea
    {
        public int FK_JobTitleID {  get; set; }
        public int FK_AreaID { get; set; }
        public int FK_IDSite { get; set; }
        public int FK_IDSLanguage { get; set; }
    }
}

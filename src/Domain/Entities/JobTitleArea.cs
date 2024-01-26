using System.Threading;

namespace Domain.Entities
{
    public class JobTitleArea
    {
        public int FkJobTitleId { get; set; }
        public int FkAreaId { get; set; }
        public int FkIdsite { get; set; }
        public int FkIdslanguage { get; set; }

        public virtual Area Fk { get; set; } = null!;
        public virtual JobTitle FkJobTitle { get; set; } = null!;
    }
}

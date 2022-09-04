using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum ServiceTypes
    {
        ArchivedLimit = 1,
        ManualJobRefresh = 2,
        ManageScreeningQuestionnaires = 4,
        AutomaticClassification = 5,
        ManageMailings = 6,
        ColouredJobPostingCredits = 7,
        LogoEmployerPage = 17,
        LogoFeaturedInChannel = 19,
        JobExternalRedirect = 27
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApisClient.DTO
{
    public class QuestionDTO
    {
        public int IDQuestion { get; set; }
        public int IDQuestionType { get; set; }
        public string Description { get; set; }
        public bool? ConfirmChanges { get; set; } /// Indica si el procés de gravació de la pregunta es contra la base de dades (true) o encara és a memòria (false)
        public int? IDQuest { get; set; }
        public bool chkActive { get; set; }
        public int OrderPlace { get; set; }
        
        public List<QuestionAnswerDTO> ListAnswers { get; set; }


        public QuestionDTO()
        {
            ListAnswers = new List<QuestionAnswerDTO>();
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApisClient.DTO
{
    public class QuestDTO
    {
        public int QuestID { get; set; }
        public int EnterpriseID { get; set; }
        public DateTime CreationDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxScore { get; set; }
        public int MinScore { get; set; }
        public int MaxPoints { get; set; }
        public bool ChkVisible { get; set; }
        public bool ChkAutoClassification { get; set; }
        public decimal MaxPointsDiv2 { get; set; }


        public List<QuestionDTO> list_question { get; set; }
        public bool isEditing { get; set; }

        public QuestDTO()
        {
            list_question = new List<QuestionDTO>();
        }
    }
}

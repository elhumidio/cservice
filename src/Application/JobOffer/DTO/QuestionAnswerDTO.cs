using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApisClient.DTO
{
    public class QuestionAnswerDTO
    {
        public int QuestionID { get; set; }
        public int AnswerID { get; set; }
        public string BaseName { get; set; }
        public int Score { get; set; }
        public bool ChkActive { get; set; }
    }
}

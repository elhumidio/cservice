using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("WP_CategoryOfferRelation")]
    // RelationTypeId: 1 = Area, 2 = Industry.
    public class WP_CategoryOfferRelation
    {
        public string CategoryId { get; set; }
        public int RelationId { get; set; }
        public int RelationTypeId { get; set; }
    }
}

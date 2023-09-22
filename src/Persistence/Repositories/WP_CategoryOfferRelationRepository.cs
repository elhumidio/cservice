using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class WP_CategoryOfferRelationRepository : IWP_CategoryOfferRelationRepository
    {
        private DataContext _dataContext;

        public WP_CategoryOfferRelationRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public int GetRelationIdByCategoryId(string categoryId)
        {
            if (string.IsNullOrEmpty(categoryId))
            {
                return 0;
            }
            WP_CategoryOfferRelation relation = _dataContext.WP_CategoryOfferRelations.FirstOrDefault(x => x.CategoryId.ToUpper() == categoryId.ToUpper());
            if (relation == null)
            {
                return 0;
            }
            return relation.RelationId;
        }
    }
}

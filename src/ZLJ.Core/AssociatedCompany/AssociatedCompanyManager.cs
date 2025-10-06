using Abp.Domain.Repositories;
using ZLJ.Core.AssociatedCompany;

namespace ZLJ.Core.BaseInfo.AssociatedCompany
{
    public class AssociatedCompanyManager : BXJGBaseInfoDomainServiceBase
    {
        private readonly IRepository<AssociatedCompanyEntity, long> Repository;

        public AssociatedCompanyManager(IRepository<AssociatedCompanyEntity, long> associatedCompanyRepository)
        {
            this.Repository = associatedCompanyRepository;
        }

        public async Task Add(AssociatedCompanyEntity entity) {
            var existsQuery = Repository.GetAll().Where(x => x.Name == entity.Name);
            if (await AsyncQueryableExecuter.AnyAsync(existsQuery))
                throw new UserFriendlyException("Ãû³ÆÒÑ´æÔÚ");

            await Repository.InsertAsync(entity);
        }
    }
}
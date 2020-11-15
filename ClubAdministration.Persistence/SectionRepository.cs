using ClubAdministration.Core.Contracts;
using ClubAdministration.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ClubAdministration.Persistence
{
    public class SectionRepository : ISectionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SectionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Section[]> GetAllAsync()
            => await _dbContext.Sections
                                .OrderBy(s => s.Name)
                                .ToArrayAsync();
        public async Task<Section[]> GetAllWithMemberSectionsAndMembersAsync()
            => await _dbContext.Sections
                                .Include(s => s.MemberSections)
                                .ThenInclude(s => s.Member)
                                .OrderBy(s => s.Name)
                                .ToArrayAsync();

    }
}
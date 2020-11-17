using ClubAdministration.Core.Contracts;
using ClubAdministration.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ClubAdministration.Persistence
{
    public class MemberRepository : IMemberRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MemberRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Member> GetMemberByNameAsync(string firstName, string lastName)
            => await _dbContext.Members
                                .Where(m => m.FirstName == firstName && m.LastName == lastName)
                                .FirstOrDefaultAsync();

        public async Task<Member[]> GetAllAsync()
            => await _dbContext.Members
                                .OrderBy(m => m.LastName)
                                .ToArrayAsync();

        public async Task<Member[]> GetAllWithMemberSectionsAsync()
            => await _dbContext.Members
                                .Include(m => m.MemberSections)
                                .OrderBy(m => m.LastName)
                                .ToArrayAsync();

        public void UpdateMember(Member member)
            => _dbContext.Members.Update(member);

        public bool CheckIfMemberExistsSync(int id, string firstName, string lastName)
        {
            var memberInDb = _dbContext.Members.Where(m => m.Id != id && m.FirstName == firstName && m.LastName == lastName)
                                .FirstOrDefault();

            if(memberInDb != null)
            {
                return true;
            }

            return false;
        }

        public async Task<string[]> GetAllMemberNamesAsync()
            => await _dbContext.Members.OrderBy(m => m.LastName)
                                        .Select(m => m.FullName)
                                        .ToArrayAsync();
    }
}
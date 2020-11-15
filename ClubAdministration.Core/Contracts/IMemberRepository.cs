using ClubAdministration.Core.Entities;
using System.Threading.Tasks;

namespace ClubAdministration.Core.Contracts
{
    public interface IMemberRepository
    {
        Task<Member> GetMemberByNameAsync(string firstName, string lastName);
        Task<Member[]> GetAllAsync();
        Task<Member[]> GetAllWithMemberSectionsAsync();
        void UpdateMember(Member member);
        bool CheckIfMemberExistsSync(int id, string firstName, string lastName);
            
    }
}

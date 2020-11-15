﻿using ClubAdministration.Core.Contracts;
using ClubAdministration.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubAdministration.Persistence
{
    public class MemberSectionRepository : IMemberSectionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MemberSectionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRangeAsync(IEnumerable<MemberSection> memberSections)
          => await _dbContext.AddRangeAsync(memberSections);

        public async Task<MemberSection[]> GetAllWithMembersAndSectionsAsync()
            => await _dbContext.MemberSections
                                .Include(ms => ms.Member)
                                .Include(ms => ms.Section)
                                .OrderBy(ms => ms.Section.Name)
                                .ToArrayAsync();
    }
}
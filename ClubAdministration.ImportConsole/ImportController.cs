using ClubAdministration.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace ClubAdministration.ImportConsole
{
    public class ImportController
    {
        const string FileName = "members.csv";
        const int LASTNAME_IDX = 0;
        const int FIRSTNAME_IDX = 1;
        const int SECTION_IDX = 2;

        public static async Task<MemberSection[]> ReadFromCsvAsync()
        {
            var data = await MyFile.ReadStringMatrixFromCsvAsync(FileName, false);

            var sections = data.GroupBy(d => d[SECTION_IDX])
                                .Select(d => new Section()
                                {
                                    Name = d.Key,
                                    MemberSections = new List<MemberSection>()
                                })
                                .ToDictionary(x => x.Name);

            var members = data.GroupBy(data => data[LASTNAME_IDX] + "_" + data[FIRSTNAME_IDX])
                                .Select(d => new Member()
                                {
                                    LastName = d.Key.Split("_")[LASTNAME_IDX],
                                    FirstName = d.Key.Split("_")[FIRSTNAME_IDX],
                                    MemberSections = new List<MemberSection>()
                                })
                                .ToDictionary(x => x.LastName + "_" + x.FirstName);

            var memberSections = data.Select(d => new MemberSection()
            {
                Member = members[d[LASTNAME_IDX] + "_" + d[FIRSTNAME_IDX]],
                Section = sections[d[SECTION_IDX]]
            }).ToArray();

            return memberSections;    
        }
    }
}

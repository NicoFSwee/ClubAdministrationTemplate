using ClubAdministration.Core.Contracts;
using ClubAdministration.Core.DataTransferObjects;
using ClubAdministration.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClubAdministration.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public Section[] Sections { get; set; }
        [BindProperty]
        public List<SelectListItem> SelectListSections { get; set; }
        [BindProperty]
        public MemberDto[] Members { get; set; }
        [BindProperty]
        public string SectionId { get; set; }
        public bool SectionIsSelected { get; set; }

        public async Task<IActionResult> OnGet()
        {
            SelectListSections = new List<SelectListItem>();
            SelectListSections.Add(new SelectListItem("Select a Section...", string.Empty));
            await PopulateSelectList();

            SectionId = string.Empty;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await PopulateSelectList();

            if (!string.IsNullOrEmpty(SectionId))
            {
                Members = await _unitOfWork.MemberRepository.GetMemberDtosBySectionId(int.Parse(SectionId));
                SectionIsSelected = true;
            }
            else
            {
                SectionId = string.Empty;
            }

            return Page();
        }

        private async Task PopulateSelectList()
        {
            Sections = await _unitOfWork.SectionRepository.GetAllAsync();

            foreach (var item in Sections)
            {
                SelectListSections.Add(new SelectListItem(item.Name, item.Id.ToString()));
            }
        }
    }
}

using ClubAdministration.Core.Contracts;
using ClubAdministration.Core.Entities;
using ClubAdministration.Persistence;
using ClubAdministration.Wpf.Common;
using ClubAdministration.Wpf.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClubAdministration.Wpf.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private Section _selectedSection;
        private Member _selectedMember;
        private ObservableCollection<Member> _members;
        private ObservableCollection<Section> _sections;

        public Member SelectedMember 
        {
            get => _selectedMember; 
            set
            {
                _selectedMember = value;
                OnPropertyChanged();
            }
        }

        public Section SelectedSection 
        {
            get => _selectedSection; 
            set
            {
                _selectedSection = value;
                if(SelectedSection != null)
                {
                    LoadMembersAsync();
                }
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Section> Sections 
        {
            get => _sections;
            set
            {
                _sections = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Member> Members 
        {
            get => _members;
            set
            {
                _members = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel(IWindowController windowController) : base(windowController)
        {
            LoadCommands();
        }

        private void LoadCommands()
        {
        }

        private ICommand _cmdEditMember;

        public ICommand CmdEditMember 
        { 
            get
            {
                if(_cmdEditMember == null)
                {
                    _cmdEditMember = new RelayCommand(
                        execute: _ =>
                        {
                            Controller.ShowWindow(new EditMemberViewModel(Controller, SelectedMember), true);
                            LoadMembersAsync();
                        },
                        canExecute: _ => SelectedMember != null);
                }
                return _cmdEditMember;
            }
            set
            {
                _cmdEditMember = value;
            }
        }

        private async Task LoadDataAsync()
        {
            using(IUnitOfWork uow = new UnitOfWork())
            {
                var sections = await uow.SectionRepository.GetAllWithMemberSectionsAndMembersAsync();

                _sections = new ObservableCollection<Section>(sections);
                _members = new ObservableCollection<Member>();
                SelectedSection = Sections.FirstOrDefault();
            }
        }

        private void LoadMembersAsync()
        {
            if(SelectedSection != null)
            {
                Members.Clear();

                foreach (var memberSection in SelectedSection.MemberSections)
                {
                    Members.Add(memberSection.Member);
                }
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        public static async Task<MainViewModel> CreateAsync(IWindowController windowController)
        {
            var viewModel = new MainViewModel(windowController);
            await viewModel.LoadDataAsync();
            return viewModel;
        }
    }
}

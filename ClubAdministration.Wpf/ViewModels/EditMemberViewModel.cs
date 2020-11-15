using ClubAdministration.Core.Contracts;
using ClubAdministration.Core.Entities;
using ClubAdministration.Persistence;
using ClubAdministration.Wpf.Common;
using ClubAdministration.Wpf.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ClubAdministration.Wpf.ViewModels
{
    public class EditMemberViewModel : BaseViewModel
    {
        private string _originalFirstName;
        private string _originalLastName;
        private string _lastName;
        private string _firstName;
        private Member _member;

        [Required(ErrorMessage = "Field must not be empty")]
        [MinLength(2, ErrorMessage ="Lastname must be atleast two characters long")]
        public string LastName 
        {
            get => _lastName; 
            set
            {
                _lastName = value;
                OnPropertyChanged();
                Validate();
            }
        }
        [Required(ErrorMessage ="Field must not be empty")]
        [MinLength(2, ErrorMessage = "Firstname must be atleast two characters long")]
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
                Validate();
            }
        }

        private ICommand _cmdSaveChanges;
        public ICommand CmdSaveChanges 
        { 
            get
            {
                if(_cmdSaveChanges == null)
                {
                    _cmdSaveChanges = new RelayCommand(
                        execute: async _ =>
                        {
                            using(IUnitOfWork uow = new UnitOfWork())
                            {
                                try
                                {
                                    _member.FirstName = FirstName;
                                    _member.LastName = LastName;
                                    uow.MemberRepository.UpdateMember(_member);
                                    await uow.SaveChangesAsync();
                                    Controller.CloseWindow(this);
                                }
                                catch(ValidationException ex)
                                {
                                    if(ex.Value is IEnumerable<string> properties)
                                    {
                                        foreach(var propertyName in properties)
                                        {
                                            AddErrorsToProperty(propertyName, new List<string> { ex.ValidationResult.ErrorMessage });
                                           
                                        }
                                        DbError = ex.ValidationResult.ErrorMessage;
                                    }
                                    else
                                    {
                                        DbError = ex.Message;
                                    }
                                    
                                    _member.FirstName = _originalFirstName;
                                    _member.LastName = _originalLastName;
                                }
                            }
                        },
                        canExecute: _ => IsValid);
                }
                return _cmdSaveChanges;
            }
            set
            {
                _cmdSaveChanges = value;
            }
        }

        public EditMemberViewModel(IWindowController controller, Member member) : base(controller)
        {
            _member = member;
            _originalFirstName = _member.FirstName;
            _originalLastName = _member.LastName;
            FirstName = _member.FirstName;
            LastName = _member.LastName;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }
    }
}

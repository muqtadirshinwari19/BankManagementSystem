using System.ComponentModel.DataAnnotations;
using BankSystem.Services.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.ViewModels
{
    public class RegistrationDTO
    {
        [Required(ErrorMessage ="Username is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage ="Email address should be Valid")]
        [Remote(action: "ValidEmail", controller: "Account", ErrorMessage ="We Already have an account on this email.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "ConfirmPassword is Required")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Password and Confrim Password are not Match")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Phone is Required")]
        [RegularExpression("^[0-9]*$",ErrorMessage ="Phone Number is not Valid")]
        public string Phone { get; set; }

        public UserTypeOption UserType { get; set; } = UserTypeOption.User;

    }
}

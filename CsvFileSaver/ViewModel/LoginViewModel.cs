﻿using System.ComponentModel.DataAnnotations;

namespace CsvFileSaver.ViewModel
{
    namespace UsersApp.ViewModels
    {
        public class LoginViewModel
        {
            [Required(ErrorMessage = "Email is required.")]
            [EmailAddress]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is required.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}

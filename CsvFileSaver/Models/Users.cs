﻿using Microsoft.AspNetCore.Identity;

namespace CsvFileSaver.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
    }
}

﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using platformAthletic.Attributes.Validation;
using platformAthletic.Model;
using platformAthletic.Helpers;

namespace platformAthletic.Models.ViewModels.User
{
    public abstract class BaseUserView
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Enter Email")]
        [ValidEmail(ErrorMessage = "Enter correct Email")]
        [UserEmailValidation(ErrorMessage = "Email already registered")]
        public string Email { get; set; }
     
    }
}
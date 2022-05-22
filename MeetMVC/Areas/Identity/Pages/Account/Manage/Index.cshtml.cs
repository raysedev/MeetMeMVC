﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using MeetMVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using MeetMVC.Infrastructure;

namespace MeetMVC.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }
        public string Image { get; set; }
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Sexuality")]
            public string Sexuality { get; set; }
            public string Gender { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public string Age { get; set; }
            public string About { get; set; }
            public string ImagePath { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            
            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Sexuality = user.Sexuality,
                Gender = user.Gender,
                City = user.City,
                Country = user.Country,
                Age = user.Age,
                About = user.About,
                ImagePath = user.ImagePath
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Image = user.ImagePath;

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if (Input.Sexuality != "")
            {
                user.Sexuality = Input.Sexuality;
                await _userManager.UpdateAsync(user);
            }

            if (Input.Gender != "")
            {
                user.Gender = Input.Gender;
                await _userManager.UpdateAsync(user);
            }

            if (Input.City != "")
            {
                user.City = Input.City;
                await _userManager.UpdateAsync(user);
            }

            if (Input.Country != "")
            {
                user.Country = Input.Country;
                await _userManager.UpdateAsync(user);
            }

            if (Input.Age != "")
            {
                user.Age = Input.Age;
                await _userManager.UpdateAsync(user);
            }

            if (Input.About != "")
            {
                user.About = Input.About;
                await _userManager.UpdateAsync(user);
            }

            if (Input.ImagePath != "")
            {
                user.ImagePath = Input.ImagePath;
                _unitOfWork.UploadImage(file);
                await _userManager.UpdateAsync(user);
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class EmployeeRegistrationInfo
    {
        public int AccountId { get; set; }
        public int EmployeeInfoId { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>First name is required.</li></ul>")]
        [StringLength(90, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter name upto 90 characters.</li></ul>")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "<ul class='parsley-errors-list filled'><li>Pleasese use alphabetic characters only.</li></ul>")]
        [DataType(DataType.Text, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter valid name.</li></ul>")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Last name is required.</li></ul>")]
        [StringLength(90, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter name upto 90 characters.</li></ul>")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "<ul class='parsley-errors-list filled'><li>Pleasese use alphabetic characters only.</li></ul>")]
        [DataType(DataType.Text, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter valid name.</li></ul>")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Father's name is required.</li></ul>")]
        [StringLength(190, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter father's name upto 190 characters.</li></ul>")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "<ul class='parsley-errors-list filled'><li>Pleasese use alphabetic characters only.</li></ul>")]
        [DataType(DataType.Text, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter valid father's name.</li></ul>")]
        public string FatherName { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Date of birth is required.</li></ul>")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM yyyy}")]
        [DataType(DataType.DateTime, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter valid date.</li></ul>")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Cell number is required.</li></ul>")]
        [StringLength(20, MinimumLength = 11, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter valid phone number.</li></ul>")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter digits only.</li></ul>")]
        public string CellNumber { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Personal email is required.</li></ul>")]
        [EmailAddress(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter valid email address.</li></ul>")]
        [StringLength(140, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter email upto 140 characters.</li></ul>")]
        public string PersonalEmail { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>CNIC is required.</li></ul>")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "<ul class='parsley-errors-list filled'><li>CNIC must be 13 digits.</li></ul>")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter digits only.</li></ul>")]
        public string CNIC { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Permanent address is required.</li></ul>")]
        [StringLength(400, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter address upto 400 characters.</li></ul>")]
        //[RegularExpression(@"^[\w\s.,!?€¥£¢$@#-]{0,290}$", ErrorMessage = "<ul class='parsley-errors-list filled'><li>Illegal characters are not allowed.</li></ul>")]
        [DataType(DataType.Text, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter valid address.</li></ul>")]
        public string PermanentAddress { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Present address is required.</li></ul>")]
        [StringLength(400, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter address upto 400 characters.</li></ul>")]
        //[RegularExpression(@"^[\w\s.,!?€¥£¢$@#-]{0,290}$", ErrorMessage = "<ul class='parsley-errors-list filled'><li>Illegal characters are not allowed.</li></ul>")]
        [DataType(DataType.Text, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter valid address.</li></ul>")]
        public string PresentAddress { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Required.</li></ul>")]
        [StringLength(2, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter Year(s) upto 2 digits.</li></ul>")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter digits only.</li></ul>")]
        public string ExperienceYears { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Required.</li></ul>")]
        [StringLength(2, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter Year(s) upto 2 digits.</li></ul>")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter digits only.</li></ul>")]
        public string ExperienceMonths { get; set; }


        [StringLength(25, MinimumLength = 9, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter valid phone number.</li></ul>")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter digits only.</li></ul>")]
        public string ResidencePhoneNumber { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Marital status is required.</li></ul>")]
        [StringLength(50, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter marital status upto 50 characters.</li></ul>")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "<ul class='parsley-errors-list filled'><li>Pleasese use alphabetic characters only.</li></ul>")]
        [DataType(DataType.Text, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter valid marital status.</li></ul>")]
        public string MaritalStatus { get; set; }

        public DateTime? DateOfJoin { get; set; }

    }
}

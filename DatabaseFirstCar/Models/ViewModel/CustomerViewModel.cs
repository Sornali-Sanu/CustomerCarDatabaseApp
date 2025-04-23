using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DatabaseFirstCar.Models.ViewModel
{
    public class CustomerViewModel
    {
        public CustomerViewModel()
        {
            this.ModelList = new List<int>();
        }
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Please enter customer First name")]
        [Display(Name = "First Name"), StringLength(30)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Customer Last name")]
        [Display(Name = "Last Name"), StringLength(30)]
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Date Of Birth")]
        [ValidateDate]
        public System.DateTime DateOfBirth { get; set; }
        [Range(1500000, 5000000)]
        public decimal Price { get; set; }
        [Display(Name = "IsRegular ?")]
        public bool IsRegular { get; set; }
        [Display(Name = "Profile Picure")]
        public string Picture { get; set; }
        [Display(Name = " Mobile Number ")]
        [StringLength(11, MinimumLength = 11)]
        public string MobileNo { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public HttpPostedFileBase PicturePath { get; set; }
        public List<int> ModelList { get; set; }
    }
}
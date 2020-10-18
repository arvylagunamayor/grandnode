using Grand.Core.ModelBinding;
using Grand.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Grand.Api.Models.Mobile.Customers
{
    public class MobileLoginModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
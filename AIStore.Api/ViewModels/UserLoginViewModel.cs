using System.ComponentModel.DataAnnotations;

namespace AIStore.Api.ViewModels
{
    public class UserLoginViewModel
    {
        [Required, DataType(DataType.EmailAddress)]
        [RegularExpression(@"(?i)^[-a-z0-9!#$%&'*+\/=?^_`{|}~]+(\.[-a-z0-9!#$%&'*+\/=?^_`{|}~]+)*@([a-z0-9]([-a-z0-9]{0,61}[a-z0-9])?\.)*[a-z]{1,20}$")]
        public string Login { get; set; }
    }
}

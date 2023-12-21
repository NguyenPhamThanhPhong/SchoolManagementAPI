using SchoolManagementAPI.Models.Enum;
using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618
namespace SchoolManagementAPI.RequestResponse.Request
{
    public class UpdateParameter
    {
        [Required]
        public string fieldName { get; set; }
        [Required]
        public Object value { get; set; }
        [Required]
        public UpdateOption option { get; set; }
    }

}

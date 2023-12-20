using MongoDB.Bson.Serialization.Attributes;
using SchoolManagementAPI.Models.Abstracts;
using SchoolManagementAPI.Models.Embeded.Account;
using SchoolManagementAPI.Models.Embeded.ReuseTypes;

#pragma warning disable CS8618

namespace SchoolManagementAPI.Models.Entities
{
    public class Lecturer : SchoolMember
    {

        public Lecturer() : base()
        {
            Role = "lecturer";
        }

    }
}

using MongoDB.Bson.Serialization.Attributes;

namespace SchoolManagementAPI.Models.Embeded.SchoolClass
{
    public class ExamMileStone
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartTime { get; set; }
        public string DateString { get; set; }
        public TimeSpan? Duration { get; set; }
        public string? Room { get; set; }
        public ExamMileStone()
        {
            Id = string.Empty; Name = string.Empty;
            DateString = string.Empty;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            StudentItem other = (StudentItem)obj;

            // Compare the Id property for equality
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            // Use the Id property hash code for hashing
            return Id.GetHashCode();
        }
    }
}

namespace SchoolManagementAPI.Models.Embeded.SchoolClass
{
    public class StudentItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Progress { get; set; }
        public int Midterm { get; set; }
        public int Practice { get; set; }
        public int Final { get; set; }

        public StudentItem() { 
            Id = string.Empty; 
            Name = string.Empty;
            Progress = -1;
            Midterm = -1;
            Practice = -1;
            Final = -1;
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

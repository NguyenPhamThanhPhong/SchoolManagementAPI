namespace SchoolManagementAPI.Models.Embeded.SchoolClass
{
    public class ExamMileStone
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public string? Room { get; set; }
    }
}

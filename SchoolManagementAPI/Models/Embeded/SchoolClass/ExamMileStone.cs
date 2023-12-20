namespace SchoolManagementAPI.Models.Embeded.SchoolClass
{
    public class ExamMileStone
    {
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public string? Room { get; set; }
    }
}

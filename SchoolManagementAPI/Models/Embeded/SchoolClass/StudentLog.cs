namespace SchoolManagementAPI.Models.Embeded.SchoolClass
{
#pragma warning disable CS8618
    public class StudentLog
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public List<Score> Scores { get; set; }
        public StudentLog()
        {
            Scores = new List<Score>();

        }
    }

    public class RollCall
    {
        public DateTime Time { get; set; }
        public bool IsPresent { get; set; }
    }
    public class Score
    {
        public int ExamOrder { get; set; }
        public float Grade { get; set; }
    }
}

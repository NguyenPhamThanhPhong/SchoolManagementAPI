namespace SchoolManagementAPI.Models.Embeded.SchoolClass
{
    public class StudentLog
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public List<RollCall> RollCalls { get; set; }
        public List<Score> Scores { get; set; }
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

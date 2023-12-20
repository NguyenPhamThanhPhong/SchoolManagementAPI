﻿using SchoolManagementAPI.Models.Embeded.ReuseTypes;
using SchoolManagementAPI.Models.Enum;

#pragma warning disable CS8618

namespace SchoolManagementAPI.Models.Embeded.Account
{
    public class CreditLog
    {
        public Semester Semester { get; set; }
        public List<CreditLogSubject> Subjects { get; set; }
    }
    public class CreditLogSubject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<int> Scores { get; set; }
        public StudyStatus Status { get; set; }
    }
}

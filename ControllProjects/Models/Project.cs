﻿namespace ControllProjects.Models
{
    public class Project
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<TeamMember> TeamMembers { get; set; }
        public List<TaskItem> Tasks { get; set; }

    }
}
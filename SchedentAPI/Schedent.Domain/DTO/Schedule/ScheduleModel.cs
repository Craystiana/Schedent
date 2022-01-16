namespace Schedent.Domain.DTO.Schedule
{
    public class ScheduleModel
    {
        public string ScheduleType { get; set; }

        public string Subject { get; set; }

        public string Professor { get; set; }

        public string Subgroup { get; set; }

        public int Week { get; set; }

        public float Duration { get; set; }

        public string StartsAt { get; set; }
    }
}

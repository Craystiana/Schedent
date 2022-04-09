using Schedent.Domain.Entities;

namespace Schedent.Domain.DTO.Import
{
    public class ImportLine
    {
        public Group Group { get; set; }
        public Subgroup Subgroup { get; set; }
        public string Day { get; set; }
        public string Start { get; set; }
        public int Duration { get; set; }
        public Subject Subject { get; set; }
        public ScheduleType Type { get; set; }
        public Professor Professor { get; set; }
        public int Week { get; set; }

    }
}

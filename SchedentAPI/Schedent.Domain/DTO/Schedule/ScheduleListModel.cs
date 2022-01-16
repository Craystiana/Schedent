using System.Collections.Generic;

namespace Schedent.Domain.DTO.Schedule
{
    public class ScheduleListModel
    {
        public string Day { get; set; }

        public IEnumerable<ScheduleModel> Schedules { get; set; }
    }
}

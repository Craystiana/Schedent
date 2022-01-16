export class ScheduleModel {
    public scheduleType: string;
    public subject: string;
    public professor: string;
    public subgroup: string;
    public week: number;
    public day: string;
    public duration: number;
    public startsAt: string;

    public constructor(scheduleType: string, subject: string, professor: string, subgroup: string, week: number, day: string, duration: number, startsAt: string){
        this.scheduleType = scheduleType;
        this.subject = subject;
        this.professor = professor;
        this.subgroup = subgroup;
        this.week = week;
        this.day = day;
        this.duration = duration;
        this.startsAt = startsAt;
    }
}
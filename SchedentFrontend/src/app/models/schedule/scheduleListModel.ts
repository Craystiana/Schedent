import { ScheduleModel } from "./scheduleModel";

export class ScheduleListModel {
    public day: string;
    public schedules: ScheduleModel[];

    public constructor(day: string, schedules: ScheduleModel[]){
        this.day = day;
        this.schedules = schedules;
    }
}
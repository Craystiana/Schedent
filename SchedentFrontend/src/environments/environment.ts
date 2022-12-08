// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false
};

export const API_URL = 'https://localhost:44389/api';
//export const API_URL = 'https://schedent.azurewebsites.net/api';
export const LOGIN_URL = '/user/login'; 
export const REGISTER_URL = '/user/register';
export const FACULTY_LIST_URL = '/faculty/all';
export const SECTION_LIST_URL = '/section/all?facultyId=';
export const GROUP_LIST_URL = '/group/all?sectionId=';
export const SUBGROUP_LIST_URL = '/subgroup/all?groupId=';
export const DOCUMENT_ADD_URL = '/document/add';
export const USER_SCHEDULE_URL = '/schedule/get';
export const SUBGROUP_SCHEDULE_URL = '/schedule/subgroup?subgroupId=';
export const NOTIFICATION_LIST_URL = '/notification/all';
export const USER_DETAILS_URL = '/user/details';
export const EDIT_PROFILE_URL = '/user/profile';
export const EDIT_DEVICE_TOKEN = '/user/devicetoken/';

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.

namespace Backend.Constants {
    public class Routes {
        public class Projects {
            public const string CREATE_ENTRY = "create"                     ;
            public const string GET_LIST     = "get_list"                   ;
            public const string GET_CONCRETE = "get_concrete/{projId}"      ;
            public const string REDACT_ENTRY = "edit/{projId}"              ;
            public const string REMOVE_ENTRY = "remove/{projId}"            ;
            public const string ADD_USER     = "add_user/{projId}/{userId}" ;
            public const string REMOVE_USER  = "rem_user/{projId}/{userId}" ;
            public const string GET_USERS    = "get_users_list/{projId}"    ;
        }

        public class ProjectTasks {
            public const string CREATE_ENTRY       = "create/{projId}"              ;
            public const string GET_LIST           = "get_list/{projId}"            ;
            public const string GET_LIST_BY_STATUS = "get_list/{projId}/{statusId}" ;
            public const string REDACT_ENTRY       = "edit/{projId}/{taskId}"       ;
            public const string REMOVE_ENTRY       = "remove/{projId}/{taskId}"     ;
        }
        
        public class Commentaries {
            public const string CREATE_ENTRY = "create/{taskId}"                ;
            public const string REDACT_ENTRY = "edit/{taskId}/{commentaryId}"   ;
            public const string REMOVE_ENTRY = "remove/{taskId}/{commentaryId}" ;
            public const string GET_LIST     = "get_list/{taskId}"              ;
        }
        public class Users {
            public const string CREATE_ENTRY     = "create"                ;
            public const string REDACT_ENTRY     = "edit/{userId}"         ;
            public const string REMOVE_ENTRY     = "remove/{userId}"       ;
            public const string GET_LIST         = "get_list"              ;
            public const string GET_PRTCPNT_LIST = "get_list/{projId}"     ;
            public const string GET_CONCRETE     = "get_concrete/{userId}" ;
        }

        public class Statuses {
            public const string CREATE_ENTRY     = "create"                  ;
            public const string REDACT_ENTRY     = "edit/{statusId}"         ;
            public const string REMOVE_ENTRY     = "remove/{statusId}"       ;
            public const string GET_LIST         = "get_list"                ;
            public const string GET_CONCRETE     = "get_concrete/{statusId}" ;
        }
    }
}

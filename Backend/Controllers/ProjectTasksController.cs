using Backend.Authentication;
using Backend.Constants;
using Backend.Data;
using Backend.Models;
using Backend.Validation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers {
    #region Контроллер ProjectTasksController
    /** Контроллер ProjectTasksController
     * <summary>
     *  Данный контроллер отвечает за конечные точки, 
     *  связанные с взаимодействием с сущностью задач.
     * </summary>
     */
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsAllowAny")]
    public class ProjectTasksController : ControllerBase {
        #region Поля контроллера
        /** Поле db
         * <summary>
         *  Контекст приложения.
         * </summary>
         */
        private ApplicationContext db            ;
        /** Поле authenticator
         * <summary>
         *  Объект-аутентификатор.
         * </summary>
         */
        private Authenticator      authenticator ;
        /** Поле config
         * <summary>
         *  Интерфейс конфигурации приложения.
         * </summary>
         */
        private IConfiguration     config        ;
        #endregion
        #region Конструктор контроллера
        /** Конструктор ProjectTasksController(ApplicationContext, IConfiguration) 
         * <summary>
         *  Конструктор осуществляет инъекцию 
         *  контекста приложения 
         *  и интерфейса конфигурации
         * </summary>
         * <param name="context">Контекст приложения</param>
         * <param name="config">Интерфейс конфигурации</param>
         */
        public ProjectTasksController(
            ApplicationContext context,
            IConfiguration config
        ) {
            db = context; this.config = config;
            authenticator = new Authenticator(context, config);
        }
        #endregion
        #region Общие процессы запросов
        /** Функция TryAuth(HttpRequest, UserRights)
         * <summary>
         *  Выполняет попытку аутентификации.
         * </summary>
         * <param name="request">Объект HTTP-запроса</param>
         * <param name="requiredRights">Требуемый уровень прав пользователя</param>
         * <returns>
         *  Возвращает объект ответа с ошибкой, <br/>
         *  либо значение null при успешной аутентификации.
         * </returns>
         */
        private IActionResult? TryAuth(HttpRequest request, UserRights requiredRights) {
            var user = authenticator.Authenticate(request);
            if(user == null) {
                return BadRequest(RespMsgs.TOKEN_INVALID);
            }
            if(user.Rights < UserRights.USER) {
                return Forbid(RespMsgs.NOT_ENOUGH_RIGHTS);
            }
            return null;
        }
        /** Функция ExecEntryPipeline(HttpRequest,UserRights,ProjectTask)
         * <summary>
         *  Функция осуществляет общий процесс валидации объекта, <br/>
         *  предлагаемого к записи в БД, а также попытку авторизации <br/>
         *  См. <see cref="TryAuth(HttpRequest, UserRights)"/>
         * </summary>
         * <param name="request">Объект HTTP-запроса</param>
         * <param name="requiredRights">Требуемый уровень прав.</param>
         * <param name="entry">Запись, подвергаемая валидации.</param>
         * <returns>
         *  Возвращает объект ответа с ошибкой, <br/>
         *  либо значение null при успешной аутентификации.
         * </returns>
         */
        private IActionResult? ExecEntryPipeline(
            HttpRequest request        ,
            UserRights  requiredRights ,
            ProjectTask entry
        ) {
            var authErrorResp = TryAuth(Request, UserRights.USER);
            if(authErrorResp != null) {
                return authErrorResp;
            }
            var validationMsg = Validator.Validate(entry);
            if(validationMsg != String.Empty) {
                return BadRequest(validationMsg);
            }
            return null;
        }

        /** Функция SetNulls(Project, ProjectTask)
         * <summary>
         *  Функция задаёт пустые объекты выходным переменным.<br/>
         *  <br/>
         *  Требуется для того, чтобы выходные переменные имели значения, отличные от null.
         * </summary>
         * <param name="project">Объект проекта</param>
         * <param name="task">Объект задачи</param>
         */
        private void SetNulls(out Project project, out ProjectTask task) {
            project = new Project(); task = new ProjectTask();
        }
        /** Функция ExecTaskEntryPipeline(Project, ProjectTask, int, int)
         * <summary>
         *  Функция осуществляет общий процесс получение записи задачи, <br/>
         *  а также комментария, предлагаемого к записи в БД. 
         * </summary>
         * <param name="project">Объект проекта</param>
         * <param name="entry">Объект задачи</param>
         * <param name="projId">Идентификатор проекта</param>
         * <param name="taskId">Идентификатор задачи</param>
         */
        private IActionResult? ExecProjectEntryPipeline(
            out Project project, out ProjectTask entry,
            int projId, int taskId
        ) {
            var proj = db.Projects
                .Include(x => x.Tasks)
                .FirstOrDefault(x => x.Id == projId);
            if (proj == null) {
                SetNulls(out project, out entry);
                return NotFound(RespMsgs.Projects.ID_NOT_FOUND);
            }

            var entr = db.Tasks.FirstOrDefault(x => x.Id == taskId);
            if (entr == null) {
                SetNulls(out project, out entry);
                return NotFound(RespMsgs.ProjectsTasks.ID_NOT_FOUND);
            }
            if(entr.ProjectId != projId) {
                SetNulls(out project, out entry);
                return BadRequest(RespMsgs.ProjectsTasks.ANOTHER_PROJECT_TASK);
            }

            project = proj;
            entry   = entr;
            return null;
        }
        #endregion
        #region Функция CreateEntry
        /** Функция CreateEntry(ProjectTask, int)
         * <summary>
         *  Функция соответствует конечной точке создания записи.
         * </summary>
         * <param name="data">Данные записи</param>
         * <param name="projId">Идентификатор проекта</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpPost(Routes.ProjectTasks.CREATE_ENTRY)]
        public IActionResult CreateEntry(
            [FromBody]  ProjectTask data  , 
            [FromRoute] int         projId
        ) {
            var errorResponse = ExecEntryPipeline(Request, UserRights.USER, data);
            if(errorResponse != null) {
                return errorResponse;
            }

            var project = db.Projects
                .Include(x => x.Tasks)
                .FirstOrDefault(x => x.Id == projId);
            if (project == null) {
                return NotFound(RespMsgs.Projects.ID_NOT_FOUND);
            }

            var timestamp = DateTime.UtcNow;
            data.CreationDate         = timestamp ;
            data.LastModificationDate = timestamp ;
            data.ProjectId            = projId    ;
            db.Tasks.Add(data);
            db.SaveChanges();
            data.Project = null;
            return Ok(data);
        }
        #endregion
        #region Функция RedactEntry
        /** Функция RedactEntry(ProjectTask, int, int)
         * <summary>
         *  Функция соответствует конечной точке редактирования записи.
         * </summary>
         * <param name="data">Данные записи</param>
         * <param name="projId">Идентификатор проекта</param>
         * <param name="taskId">Идентификатор записи</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpPut(Routes.ProjectTasks.REDACT_ENTRY)]
        public IActionResult RedactEntry(
            [FromBody]  ProjectTask data   ,
            [FromRoute] int         projId ,
            [FromRoute] int         taskId 
        ) {
            var errorResponse = ExecEntryPipeline(Request, UserRights.USER, data);
            if(errorResponse != null) {
                return errorResponse;
            }

            Project     project;
            ProjectTask entry  ;
            var errorResp = ExecProjectEntryPipeline(
                out project, out entry, projId, taskId
            );
            if (errorResp != null) {
                return errorResp;
            }

            entry.Name        = data.Name        ;
            entry.Description = data.Description ;
            entry.StatusId    = data.StatusId    ;
            entry.LastModificationDate = DateTime.UtcNow;

            db.Tasks.Update(entry);
            db.SaveChanges();
            entry.Project = null;
            return Ok(entry);
        }
        #endregion
        #region Функция RemoveEntry
        /** Функция RemoveEntry(int, int)
         * <summary>
         *  Функция соответствует конечной точке удаления записи.
         * </summary
         * <param name="projId">Идентификатор проекта</param>
         * <param name="taskId">Идентификатор записи</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpDelete(Routes.ProjectTasks.REMOVE_ENTRY)]
        public IActionResult RemoveEntry(
            [FromRoute] int projId,
            [FromRoute] int taskId
        ) {
            var authErrorResp = TryAuth(Request, UserRights.USER);
            if (authErrorResp != null) {
                return authErrorResp;
            }

            Project     project;
            ProjectTask entry  ;
            var errorResp = ExecProjectEntryPipeline(
                out project, out entry, projId, taskId
            );
            if (errorResp != null) {
                return errorResp;
            }

            db.Tasks.Remove(entry);
            db.SaveChanges();
            entry.Project = null;
            return Ok(entry);
        }
        #endregion
        #region Функция GetList
        /** Функция GetList(int, int, int, string, string)
         * <summary>
         *  Функция соответствует конечной точке получения <br/>
         *  пагинированного списка записей, <br/>
         *  входящих в заданный проект.
         * </summary>
         * <param name="projId">Идентификатор проекта</param>
         * <param name="pageNumber">Номер страницы</param>
         * <param name="pageSize">Размер страницы</param>
         * <param name="taskNameFilter">Фильтр наименования задачи</param>
         * <param name="taskNameSortionDirection">Направление сортировки</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpGet(Routes.ProjectTasks.GET_LIST)]
        public IActionResult GetList(
            [FromRoute] int projId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string taskNameFilter = "",
            [FromQuery] string taskNameSortionDirection = Sortion.ASC_KEY
        ) {
            var authErrorResponse = TryAuth(Request, UserRights.USER);
            if (authErrorResponse != null) {
                return authErrorResponse;
            }

            var entries = db.Tasks.Where(
                x => x.ProjectId == projId
            ).Where(x => x.Name.Contains(taskNameFilter));
            
            IOrderedQueryable<ProjectTask> sortedEntries;
            if (taskNameSortionDirection == Sortion.ASC_KEY) {
                sortedEntries = entries.OrderBy(x => x.Name);
            } else {
                sortedEntries = entries.OrderByDescending(x => x.Name);
            }

            var paginated = Page<ProjectTask>.FormPage(
                sortedEntries.ToList(), pageNumber, pageSize
            );

            return Ok(paginated);
        }
        #endregion
        #region Функция GetByStatus
        /** Функция GetByStatus(int, int, int, int, string, string)
         * <summary>
         *  Функция соответствует конечной точке получения пагинированного списка записей,
         *  входящих в заданный проект.
         * </summary>
         * <param name="projId">Идентификатор проекта</param>
         * <param name="statusId">Идентификатор статуса</param>
         * <param name="pageNumber">Номер страницы</param>
         * <param name="pageSize">Размер страницы</param>
         * <param name="taskNameFilter">Фильтр наименования задачи</param>
         * <param name="taskNameSortionDirection">Направление сортировки</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpGet(Routes.ProjectTasks.GET_LIST_BY_STATUS)]
        public IActionResult GetByStatus(
            [FromRoute] int projId  ,
            [FromRoute] int statusId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string taskNameFilter = "",
            [FromQuery] string taskNameSortionDirection = Sortion.ASC_KEY
        ) {
            var authErrorResponse = TryAuth(Request, UserRights.USER);
            if (authErrorResponse != null) {
                return authErrorResponse;
            }

            var entries = db.Tasks.Where(
                x => x.ProjectId == projId 
                &&   x.StatusId  == statusId
            ).Where(x => x.Name.Contains(taskNameFilter));
            
            IOrderedQueryable<ProjectTask> sortedEntries;
            if (taskNameSortionDirection == Sortion.ASC_KEY) {
                sortedEntries = entries.OrderBy(x => x.Name);
            } else {
                sortedEntries = entries.OrderByDescending(x => x.Name);
            }

            var paginated = Page<ProjectTask>.FormPage(
                sortedEntries.ToList(), pageNumber, pageSize
            );

            return Ok(entries);
        }
        #endregion
    }
    #endregion
}

using Backend.Constants;
using Backend.Data;
using Backend.Authentication;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.Validation;

namespace Backend.Controllers {
    #region Контроллер ProjectsController
    /** Контроллер ProjectsController
     * <summary>
     *  Данный контроллер отвечает за конечные точки, 
     *  связанные с взаимодействием с сущностью проектов.
     * </summary>
     */
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsAllowAny")]
    public class ProjectsController : ControllerBase {
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
        /** Конструктор ProjectsController(ApplicationContext, IConfiguration) 
         * <summary>
         *  Конструктор осуществляет инъекцию 
         *  контекста приложения 
         *  и интерфейса конфигурации
         * </summary>
         * <param name="context">Контекст приложения</param>
         * <param name="config">Интерфейс конфигурации</param>
         */
        public ProjectsController(
            ApplicationContext context, 
            IConfiguration     config
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
        /** Функция ExecEntryPipeline(HttpRequest,UserRights,Project)
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
            Project     entry
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
        #endregion
        #region Функция CreateEntry
        /** Функция CreateEntry(Project)
         * <summary>
         *  Функция соответствует конечной точке создания записи.
         * </summary>
         * <param name="data">Данные записи</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpPost(Routes.Projects.CREATE_ENTRY)]
        public IActionResult CreateEntry([FromBody] Project data) {
            var errorResponse = ExecEntryPipeline(Request, UserRights.USER, data);
            if(errorResponse != null) {
                return errorResponse;
            }

            var timestamp = DateTime.UtcNow;
            data.CreationDate         = timestamp;
            data.LastModificationDate = timestamp;
            db.Projects.Add(data);
            db.SaveChanges();
            return Ok(data);
        }
        #endregion
        #region Функция GetList
        /** Функция GetList
         * <summary>
         *  Функция соответствует конечной точке получения списка записей.
         * </summary>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpGet(Routes.Projects.GET_LIST)]
        public IActionResult GetList() {
            var authErrorResponse = TryAuth(Request, UserRights.USER);
            if(authErrorResponse != null) {
                return authErrorResponse;
            }

            var entries = db.Projects;

            return Ok(entries);
        }
        #endregion
        #region Функция GetEntry
        /** Функция GetEntry
         * <summary>
         *  Функция соответствует конечной точке получения конкретной записи.
         * </summary>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpGet(Routes.Projects.GET_CONCRETE)]
        public IActionResult GetEntry([FromRoute] int projId) {
            var authErrorResponse = TryAuth(Request, UserRights.USER);
            if(authErrorResponse != null) {
                return authErrorResponse;
            }

            var entry = db.Projects.FirstOrDefault(x => x.Id == projId);
            if(entry == null) {
                return NotFound(RespMsgs.ID_NOT_FOUND);
            }
            return Ok(entry);
        }
        #endregion
        #region Функция RedactEntry
        /** Функция RedactEntry(Project, int)
         * <summary>
         *  Функция соответствует конечной точке редактирования записи.
         * </summary>
         * <param name="data">Данные записи</param>
         * <param name="projId">Идентификатор записи</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpPut(Routes.Projects.REDACT_ENTRY)]
        public IActionResult RedactEntry([FromBody] Project data, [FromRoute] int projId) {
            var errorResponse = ExecEntryPipeline(Request, UserRights.USER, data);
            if(errorResponse != null) {
                return errorResponse;
            }

            var entry = db.Projects.FirstOrDefault(x => x.Id == projId);
            if (entry == null) {
                return NotFound(RespMsgs.ID_NOT_FOUND);
            }

            entry.Name        = data.Name        ;
            entry.Description = data.Description ;
            entry.LastModificationDate = DateTime.UtcNow;

            db.Projects.Update(entry);
            db.SaveChanges();
            return Ok(entry);
        }
        #endregion
        #region Функция RemoveEntry
        /** Функция RemoveEntry(int)
         * <summary>
         *  Функция соответствует конечной точке удаления записи.
         * </summary
         * <param name="projId">Идентификатор записи</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpDelete(Routes.Projects.REMOVE_ENTRY)]
        public IActionResult RemoveEntry([FromRoute] int projId) {
            var authErrorResponse = TryAuth(Request, UserRights.USER);
            if(authErrorResponse != null) {
                return authErrorResponse;
            }

            var entry = db.Projects.FirstOrDefault(x => x.Id == projId);
            if (entry == null) {
                return NotFound(RespMsgs.ID_NOT_FOUND);
            }

            db.Projects.Remove(entry);
            db.SaveChanges();
            return Ok(entry);
        }
        #endregion
        #region Функция AddUser
        /** Функция AddUser(int, int)
         * <summary>
         *  Функция соответствует конечной точке добавления пользователя в проект.
         * </summary
         * <param name="projId">Идентификатор записи</param>
         * <param name="userId">Идентификатор добавляемого пользователя</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpPut(Routes.Projects.ADD_USER)]
        public IActionResult AddUser([FromRoute] int projId, [FromRoute] int userId) {
            var authErrorResponse = TryAuth(Request, UserRights.USER);
            if (authErrorResponse != null) {
                return authErrorResponse;
            }

            var project = db.Projects
                .Include(x => x.Users)
                .FirstOrDefault(x => x.Id == projId);
            if (project == null) {
                return NotFound(RespMsgs.Projects.ID_NOT_FOUND);
            }
            var user = db.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null) {
                return NotFound(RespMsgs.Users.ID_NOT_FOUND);
            }
            if(user.ParticipatingProjects.Any(x => x.Id == projId)) {
                return BadRequest(RespMsgs.Users.ALREADY_IN_PROJECT);
            }

            project.Users.Add(user);
            db.Projects.Update(project);
            db.SaveChanges();
            user.ParticipatingProjects = new List<Project>() { };
            return Ok(user);
        }
        #endregion
        #region Функция RemoveUser
        /** Функция RemoveUser(int, int)
         * <summary>
         *  Функция соответствует конечной точке удаления пользователя из проекта.
         * </summary
         * <param name="projId">Идентификатор записи</param>
         * <param name="userId">Идентификатор удаляемого пользователя</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpPut(Routes.Projects.REMOVE_USER)]
        public IActionResult RemoveUser([FromRoute] int projId, [FromRoute] int userId) {
            var authErrorResponse = TryAuth(Request, UserRights.USER);
            if (authErrorResponse != null) {
                return authErrorResponse;
            }

            var project = db.Projects
                .Include(x => x.Users)
                .FirstOrDefault(x => x.Id == projId);
            if (project == null) {
                return NotFound(RespMsgs.Projects.ID_NOT_FOUND);
            }
            var user = project.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null) {
                return NotFound(RespMsgs.Projects.USER_NOT_FOUND);
            }

            project.Users.Remove(user);
            db.Projects.Update(project);
            db.SaveChanges();
            project.Users = new List<User>();
            return Ok(project);
        }
        #endregion
        #region Функция GetUsers
        /** Функция GetUsers(int)
         * <summary>
         *  Функция соответствует конечной точке <br/>
         *  получения пользователей, участавующих в проекте.
         * </summary
         * <param name="projId">Идентификатор проекта.</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpGet(Routes.Projects.GET_USERS)]
        public IActionResult GetUsers([FromRoute] int projId) {
            var authErrorResponse = TryAuth(Request, UserRights.USER);
            if (authErrorResponse != null) {
                return authErrorResponse;
            }
            var entry = db.Projects
                .Include(x => x.Users)
                .FirstOrDefault(x => x.Id == projId);
            if(entry == null) {
                return NotFound(RespMsgs.Projects.ID_NOT_FOUND);
            }
            foreach(var user in entry.Users) {
                user.ParticipatingProjects = new List<Project>();
            }
            return Ok(entry.Users);
        }
        #endregion
    }
    #endregion
}

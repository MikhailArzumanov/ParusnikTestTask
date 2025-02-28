using Backend.Auth;
using Backend.Authentication;
using Backend.Constants;
using Backend.Cryptography;
using Backend.Data;
using Backend.Models;
using Backend.Validation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers {
    #region Контроллер UsersController
    /** Контроллер UsersController
     * <summary>
     *  Данный контроллер отвечает за конечные точки, 
     *  связанные с взаимодействием с сущностью пользователей.
     * </summary>
     */
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsAllowAny")]
    public class UsersController : ControllerBase {
        #region Поля контроллера
        /** Поле db
         * <summary>
         *  Контекст приложения.
         * </summary>
         */
        private ApplicationContext db;
        /** Поле authenticator
         * <summary>
         *  Объект-аутентификатор.
         * </summary>
         */
        private Authenticator authenticator;
        /** Поле config
         * <summary>
         *  Интерфейс конфигурации приложения.
         * </summary>
         */
        private IConfiguration config;
        #endregion
        #region Конструктор контроллера
        /** Конструктор UsersController(ApplicationContext, IConfiguration) 
         * <summary>
         *  Конструктор осуществляет инъекцию 
         *  контекста приложения 
         *  и интерфейса конфигурации
         * </summary>
         * <param name="context">Контекст приложения</param>
         * <param name="config">Интерфейс конфигурации</param>
         */
        public UsersController(
            ApplicationContext context ,
            IConfiguration     config
        ) {
            db = context; this.config = config;
            authenticator = new Authenticator(context, config);
        }
        #endregion
        #region Общие процессы валидации
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
        /** Функция ExecEntryPipeline(HttpRequest,UserRights,User)
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
            User        entry
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
        #region Функция Authorize
        /** Функция Authorize(AuthData)
         * <summary>
         *  Функция соответствует конечной точке авторизации.
         * </summary>
         * <param name="authData">Данные авторизации</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpPost("authorize")]
        public IActionResult Authorize([FromBody] AuthData authData) {
            var user = db.Users.FirstOrDefault(
                x => x.Login == authData.Login 
                && x.Password == authData.Password
            );
            if(user == null) {
                return NotFound(RespMsgs.Users.USER_NOT_FOUND);
            }
            var tokenStr = Authorization.GetToken(user, config);
            var tokenObj = new { Self = tokenStr };
            return Ok(tokenObj);
        }
        #endregion
        #region Функция CreateEntry
        /** Функция CreateEntry(User)
         * <summary>
         *  Функция соответствует конечной точке создания записи.
         * </summary>
         * <param name="data">Данные записи</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpPost(Routes.Users.CREATE_ENTRY)]
        public IActionResult CreateEntry([FromBody] User data) {
            var errorResp = ExecEntryPipeline(Request, UserRights.USER, data);
            if (errorResp != null) {
                return errorResp;
            }

            var timestamp = DateTime.UtcNow;
            data.Rights   = UserRights.USER           ;
            data.Password = Hasher.Hash(data.Password);
            data.CreationDate         = timestamp;
            data.LastModificationDate = timestamp;
            db.Users.Add(data);
            db.SaveChanges();
            return Ok(data);
        }
        #endregion
        #region Функция RedactEntry
        /** Функция RedactEntry(User, int)
         * <summary>
         *  Функция соответствует конечной точке редактирования записи.
         * </summary>
         * <param name="data">Данные записи</param>
         * <param name="userId">Идентификатор записи</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpPut(Routes.Users.REDACT_ENTRY)]
        public IActionResult RedactEntry([FromBody] User data, [FromRoute] int userId) {
            var errorResp = ExecEntryPipeline(Request, UserRights.USER, data);
            if (errorResp != null) {
                return errorResp;
            }

            var entry = db.Users.FirstOrDefault(x => x.Id == userId);
            if(entry == null) {
                return NotFound(RespMsgs.Users.ID_NOT_FOUND);
            }

            entry.LastModificationDate = DateTime.UtcNow;
            entry.Email   = data.Email   ;
            entry.Surname = data.Surname ;
            entry.Name    = data.Name    ;
            entry.Login    =             data.Login    ;
            entry.Password = Hasher.Hash(data.Password);
            entry.Rights = data.Rights ;

            db.Users.Update(entry);
            db.SaveChanges();
            return Ok(entry);
        }
        #endregion
        #region Функция RemoveEntry
        /** Функция RemoveEntry(int)
         * <summary>
         *  Функция соответствует конечной точке удаления записи.
         * </summary>
         * <param name="userId">Идентификатор записи</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpDelete(Routes.Users.REMOVE_ENTRY)]
        public IActionResult RemoveEntry([FromRoute] int userId) {
            var authErrorResp = TryAuth(Request, UserRights.USER);
            if (authErrorResp != null) {
                return authErrorResp;
            }

            var entry = db.Users.FirstOrDefault(x => x.Id == userId);
            if(entry == null) {
                return NotFound(RespMsgs.Users.ID_NOT_FOUND);
            }

            db.Users.Remove(entry);
            db.SaveChanges();
            return Ok(entry);
        }
        #endregion
        #region Функция GetConcrete
        /** Функция GetConcrete(int)
         * <summary>
         *  Функция соответствует конечной точке получения записи.
         * </summary>
         * <param name="userId">Идентификатор записи</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpGet(Routes.Users.GET_CONCRETE)]
        public IActionResult GetConcrete([FromRoute] int userId) {
            var authErrorResp = TryAuth(Request, UserRights.USER);
            if (authErrorResp != null) {
                return authErrorResp;
            }
            
            var entry = db.Users.FirstOrDefault(x => x.Id == userId);
            if(entry == null) {
                return NotFound(RespMsgs.Users.ID_NOT_FOUND);
            }

            return Ok(entry);
        }
        #endregion
        #region Функция GetList
        /** Функция GetList(int, int, string, string)
         * <summary>
         *  Функция соответствует конечной точке получения пагинированного списка записей.
         * </summary>
         * <param name="pageNumber">Номер страницы</param>
         * <param name="pageSize">Размер страницы</param>
         * <param name="userNameFilter">Фильтр имени пользователя</param>
         * <param name="userNameSortionDirection">Направление сортировки</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpGet(Routes.Users.GET_LIST)]
        public IActionResult GetList(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string userNameFilter = "",
            [FromQuery] string userNameSortionDirection = Sortion.ASC_KEY
        ) {
            var authErrorResp = TryAuth(Request, UserRights.USER);
            if (authErrorResp != null) {
                return authErrorResp;
            }

            var entries = db.Users.Where(
                x => x.Name.Contains(userNameFilter)
                || x.Surname.Contains(userNameFilter)
            );
            
            IOrderedQueryable<User> sortedEntries;
            if (userNameSortionDirection == Sortion.ASC_KEY) {
                sortedEntries = entries.OrderBy(x => x.Name);
            } else {
                sortedEntries = entries.OrderByDescending(x => x.Name);
            }

            var paginated = Page<User>.FormPage(
                sortedEntries.ToList(), pageNumber, pageSize
            );
            return Ok(paginated);
        }
        #endregion
        #region Функция GetParticipantsList
        /** Функция GetParticipantsList(inr, int, int, string, string)
         * <summary>
         *  Функция соответствует конечной точке <br/>
         *  получения пагинированного списка записей пользователей, <br/>
         *  участвующих в заданном проекте.
         * </summary>
         * <param name="projId">Идентификатор проекта</param>
         * <param name="pageNumber">Номер страницы</param>
         * <param name="pageSize">Размер страницы</param>
         * <param name="userNameFilter">Фильтр имени пользователя</param>
         * <param name="userNameSortionDirection">Направление сортировки</param>
         * <returns>Возращает объект http-ответа</returns>
         */
        [HttpGet(Routes.Users.GET_PRTCPNT_LIST)]
        public IActionResult GetParticipantsList(
            [FromRoute] int projId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string userNameFilter = "",
            [FromQuery] string userNameSortionDirection = Sortion.ASC_KEY
        ) {
            var authErrorResp = TryAuth(Request, UserRights.USER);
            if (authErrorResp != null) {
                return authErrorResp;
            }

            var project = db.Projects
                .Include(x => x.Users)
                .FirstOrDefault(x => x.Id == projId);
            if(project == null) {
                return NotFound(RespMsgs.Projects.ID_NOT_FOUND);
            }

            var entries = project.Users.Where(
                x => x.Name.Contains(userNameFilter)
                || x.Surname.Contains(userNameFilter)
            );
            
            IOrderedEnumerable<User> sortedEntries;
            if (userNameSortionDirection == Sortion.ASC_KEY) {
                sortedEntries = entries.OrderBy(x => x.Name);
            } else {
                sortedEntries = entries.OrderByDescending(x => x.Name);
            }

            var paginated = Page<User>.FormPage(
                sortedEntries.ToList(), pageNumber, pageSize
            );
            foreach(var entry in paginated.Items) {
                entry.ParticipatingProjects = new List<Project>();
            }
            return Ok(paginated);
        }
        #endregion
    }
    #endregion
    #region Класс AuthData 
    /** Класс AuthData 
     * <summary>
     *  Представляет модель данных авторизации.
     * </summary>
     */
    public class AuthData {
        #region Поля
        /** Поле Login
         * <summary>
         *  Логин авторизации.
         * </summary>
         */
        public string Login    { get; set; } = String.Empty;
        /** Поле Password
         * <summary>
         *  Пароль авторизации.
         * </summary>
         */
        public string Password { get; set; } = String.Empty;
        #endregion
    }
    #endregion
}

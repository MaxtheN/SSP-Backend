using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.Core.Security;
using SspUis.BizLogicLayer.UserServices;
using SspUis.DataLayer.Repositories;
using WEBASE.Integration.MSPD.GSP;
using SspUis.BizLogicLayer.Models;
using SspUis.BizLogicLayer;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : WebaseController
    {
        private IUserService _service;


        public UserController(IUserService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;

        }

        [HttpPost]
        [Authorize(ModuleCode.UserView, ModuleCode.BranchesUserView, ModuleCode.AllUserView)]
        public PagedResult<UserListDto> GetList([FromBody] UserSortFilterPageOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.UserView, ModuleCode.BranchesUserView, ModuleCode.AllUserView)]
        [ProducesResponseType(typeof(UserDto), 200)]
        public IActionResult Get()
        {
            UserDto dto = _service.Get();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.UserView, ModuleCode.BranchesUserView, ModuleCode.AllUserView)]
        [ProducesResponseType(typeof(UserDto), 200)]
        public IActionResult Get(int id)
        {
            UserDto dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetAsSelectList([FromBody] UserSortFilterPageOptions options)
        {
            return Ok(_service.AsSelectList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.UserCreate, ModuleCode.BranchesUserCreate, ModuleCode.AllUserCreate)]
        [ProducesResponseType(typeof(HaveId<int>), 200)]
        public IActionResult Create(CreateUserDto dto)
        {
            if (ModelState.IsValid)
            {
                HaveId<int> result = _service.Create(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.UserEdit, ModuleCode.BranchesUserEdit, ModuleCode.AllUserEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateUserDlDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Update(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
         
        //[HttpPost("{id}")]
        //[Authorize(ModuleCode.UserDelete, ModuleCode.BranchesUserDelete, ModuleCode.AllUserDelete)]
        //[ProducesResponseType(200)]
        //public IActionResult Delete(int id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _service.Delete(id);    Edoc bilan muammo bo'ladi.
        //        Hujjatlar userId ga ulanganligi uchun boshqa odamga ko'rinmay qoladi EDOC da.

        //        if (_service.IsValid)
        //            return Ok();

        //        _service.CopyErrorsToModelState(ModelState);
        //    }

        //    return ValidationProblem(ModelState);
        //}

        [HttpGet]
        [ProducesResponseType(typeof(CreateUserDto), 200)]
        public async Task<IActionResult> GetByPassportData([FromQuery] GSPPersonInfoRequestForUserDto dto)
        {
            if (ModelState.IsValid)
            {
                var user = await _service.GetByPassportData(dto);

                if (_service.IsValid)
                    return Ok(user);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CheckUserNameRespDto), 200)]
        public IActionResult CheckUserName(CheckUserNameDto dto)
        {
            if (ModelState.IsValid)
            {
                var isBusy = _service.IsUserNameBusy(dto);

                if (_service.IsValid)
                    return Ok(new CheckUserNameRespDto
                    {
                        UserName = dto.UserName,
                        IsBusy = isBusy
                    });

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        public IActionResult SaveAsExecel(UserSortFilterPageOptions dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExecel(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "user_list.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ImportJusticeUser(List<ImportUserDlDto> listDto)
        {
            if (ModelState.IsValid)
            {
                await _service.ImportJusticeUser(listDto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}

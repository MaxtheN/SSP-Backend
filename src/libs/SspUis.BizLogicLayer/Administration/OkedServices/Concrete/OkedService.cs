using GenericServices;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using SspUis.DataLayer.Repositories;
using System.IO;
using WEBASE.Storage;
using OfficeOpenXml;
using SspUis.BizLogicLayer.Models;
using WEBASE.i18n;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace SspUis.BizLogicLayer.OkedServices
{
    public class OkedService : StatusGenericHandler, IOkedService
    {
        private readonly IOkedRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageService _storageService;
        private readonly ICultureHelper _cultureHelper;
        private readonly IAuthService _authService;

        public OkedService(IUnitOfWork unitOfWork, IAuthService authService,
            IStorageService storageService,ICultureHelper cultureHelper)
        {
            _repository = unitOfWork.OkedRepository;
            _unitOfWork = unitOfWork;
            _storageService = storageService;
            _cultureHelper = cultureHelper;
            _authService = authService;
        }

        PagedResult<OkedListDto> IOkedService.GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<OkedListDto>()
                                    .SortFilter(dto)
                                    .AsPagedResult(dto);
            return result;
        }

        OkedDto IOkedService.Get()
        {
            return new OkedDto();
        }

        OkedDto IOkedService.Get(int id)
        {
            var dto = _repository.ById<OkedDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        SelectList<int> IOkedService.AsSelectList(int level)
        {
            return _repository.ReadAsNoTracked<OkedListDto>()
                              .Where(a => a.Level == level)
                              //.FilterByParentId(parentId)
                              .AsSelectList();
        }
		SelectList<int> IOkedService.SelectList(string? search) => _repository.ReadAsNoTracked<OkedListDto>()
							  .AsSelectList(search);


		HaveId<int> IOkedService.Create(CreateOkedDlDto dto)
        {
            var entity = _repository.Create(dto);
            CombineStatuses(_repository);
            if (IsValid)
            {
                _unitOfWork.Save();
                return HaveId.Create(entity.Id);
            }
            return null;
        }

        void IOkedService.Update(UpdateOkedDlDto dto)
        {
            _repository.Update(dto);
            CombineStatuses(_repository);
            if (IsValid)
                _unitOfWork.Save();
        }

        void IOkedService.Delete(int id)
        {
            try
            {
                _repository.Delete(id);
                CombineStatuses(_repository);
                if (IsValid)
                    _unitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                AddError("Запись не может быть удален");
            }
        }

        Stream IOkedService.SaveAsExecel(SortFilterPageOptions dto)
        {
            var data = _repository.ReadAsNoTracked<OkedListDto>()
                        .SortFilter(dto)
                        .ToList();

            MemoryStream result = new MemoryStream();
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.OKED_LIST));

            if (IsValid && data != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excelPackage = new ExcelPackage(template);

                var namerange = excelPackage.Workbook.Names["ImportRow"];
                var currentrow = namerange.Start.Row;
                var ws = namerange.Worksheet;
                int i = 1;
                foreach (var item in data)
                {
                    ws.InsertRow(currentrow, 1, namerange.Start.Row);
                    ws.Cells[currentrow, 1].Value = i++;
                    ws.Cells[currentrow, 2].Value = item.Code;
                    ws.Cells[currentrow, 3].Value = item.FullName;
                    ws.Cells[currentrow, 4].Value = item.Parent;
                    ws.Cells[currentrow, 5].Value = item.Level;
                    ws.Cells[currentrow, 6].Value = item.State;
                    currentrow++;
                }
                ws.DeleteRow(namerange.Start.Row, 1);
                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
            }
            result.Position = 0;
            return result;


        }

    }
}

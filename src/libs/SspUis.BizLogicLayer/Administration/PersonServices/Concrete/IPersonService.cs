using Microsoft.AspNetCore.Http;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.DigitizationCenter.Models.GSP;
using StatusGeneric;
using System;
using System.IO;
using System.Threading.Tasks;
using WEBASE.Integration.MSPD.GSP;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.PersonServices
{
	public interface IPersonService : IStatusGeneric
	{
		PersonDto Get();
		PersonDto Get(int id);
		SelectList<int> AsSelectList();
		HaveId<int> Create(CreatePersonDlDto dto);
		void Update(UpdatePersonDlDto dto);
		void Delete(int id);
		Task<PersonDto> GetByPassportData(GSPPersonInfoRequestDto dto);
		Task<PersonDto> GetChildFromGsp(PersonFilterDto dto);
		Task<Stream> CheckExcelData(IFormFile formFile);
		IStorageFileInfo UploadFile(StorageFile file);
		StorageFile DownloadFile(Guid fileId);
		void DeleteFile(Guid fileId);
		void AddOrUpdateFiles(int personId, Guid pictureId);
		Task<PersonDto> GetByPassportDataFromDigital(GSPNewApiRequestDto dto);
	}
}

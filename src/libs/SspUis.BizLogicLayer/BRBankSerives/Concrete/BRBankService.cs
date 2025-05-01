using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using SspUis.BizLogicLayer.Appeal;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.DataLayer;
using SspUis.DataLayer.Repositories.Appeal;
using SspUis.Integration.DigitizationCenter.Models.GSP;
using StatusGeneric;
using WEBASE.Integration.MSPD.Sud;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.BRBankSerives.Concrete
{
    public class BRBankService : StatusGenericHandler, IBRBankService
    {
        private readonly IPersonService _personService;
        private readonly IAppealApplicationService _aapealService;
        private readonly IAppealApplicationService _appealApplicationService;
        private readonly IUnitOfWork _unitOfWork;

        public BRBankService(IPersonService personService, IAppealApplicationService aapealService, IAppealApplicationService appealApplicationService, IUnitOfWork unitOfWork)
        {
            _personService = personService;
            _aapealService = aapealService;
            _appealApplicationService = appealApplicationService;
            _unitOfWork = unitOfWork;
        }

        public void DeleteFile(Guid fileId)
        {
            _appealApplicationService.DeleteFile(fileId);
        }

        public StorageFile DownloadFile(Guid fileId)
        {
            var data = _appealApplicationService.DownloadFile(fileId);
            return data;
        }
        public IEnumerable<AppealApplicationFileDto> UploadFiles(params StorageFile[] files)
        {
            var data = _appealApplicationService.UploadFiles(files);
            return data;
        }

        public Task<PersonDto> GetByPassportDataFromDigital(BrBankPersonDlDto dto)
        {
            GSPNewApiRequestDto gSPNewApiRequestDto = new GSPNewApiRequestDto()
            {
                document = dto.document,
                birth_date = dto.birth_date,
                is_consent = "Y",
                is_photo = "Y",
                langId = 1,
                transaction_id = 2
            };
            var personData = _personService.GetByPassportDataFromDigital(gSPNewApiRequestDto);
            return personData;
        }

        public async Task<BRBankResponseDto> GetResult(CreateAppealApplicationDlDto dto)
        {
            var data = _aapealService.Get();
            dto.DocNumber = data.DocNumber;
            dto.DocOn = data.DocOn;
            HaveId<long> result = await _aapealService.Create(dto);
            return new BRBankResponseDto
            {
                DocNumber = dto.DocNumber,
                Success = true
            };
        }

        public AppealApplicationDto Get()
        {
            var data = _aapealService.Get();
            return data;
        }
    }
}

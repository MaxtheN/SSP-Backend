using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DualEdu.EnumEduTypeServices;
using SspUis.BizLogicLayer.EnumServices;
using SspUis.BizLogicLayer.Memship;
using SspUis.BizLogicLayer.ModuleServices;
using SspUis.BizLogicLayer.OrganizationServices;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Appeal;
using SspUis.DataLayer.EfClasses.DualEdu;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.EfClasses.Proposal;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WEBASE;
using WEBASE.EF;
using WEBASE.i18n;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.ManualServices
{
	public class ManualService : StatusGenericHandler, IManualService
	{
		private static bool MODULE_CODE_INITIALIZED = false;

		private readonly ICrudServices _service;
		private readonly DbContext _context;
		private readonly IAuthService _authService;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICultureHelper _cultureHelper;

		public ManualService(
			ICrudServices crudService,
			DbContext context,
			IAuthService authService,
			IUnitOfWork unitOfWork,
			ICultureHelper cultureHelper)
		{
			_service = crudService;
			_context = context;
			_authService = authService;
			_unitOfWork = unitOfWork;
			_cultureHelper = cultureHelper;
		}

		public IEnumerable<ModuleGroupSelectListDto> GetModuleSelectList()
		{
			if (!MODULE_CODE_INITIALIZED)
			{
				_context.ResolveModules<ModuleCode, Module, ModuleTranslate, ModuleSubGroup, ModuleSubGroupTranslate>();
				MODULE_CODE_INITIALIZED = true;
			}

			return _context.Set<Module>().AsSelectList();
		}

		public SelectList<int> TableSelectList()
		{
			return _context.Set<Table>().AsSelectList();
		}
        public SelectList<long> BankInExcelSelectList()
        {
            return _context.Set<Contractor>().AsBankSelectList();
        }

        public SelectList<int> StateSelectList()
		{
			return _context.Set<State>().AsSelectList();
		}

		public SelectList<int, LanguageSelectListDto> LanguageSelectList()
		{
			return _context.Set<Language>().AsSelectList();
		}
		public SelectList<int> LanguageDegreeSelectList()
		{
			return _context.Set<LanguageDegree>().AsSelectList();
		}
		public SelectList<int> GenderSelectList()
		{
			return _context.Set<Gender>().AsSelectList();
		}
		public SelectList<int> NotificationTypeSelectList()
		{
			return _context.Set<NotificationType>().AsSelectList();
		}

		public SelectList<int> StatusSelectList()
		{
			return _context.Set<Status>().AsSelectList();
		}

		public SelectList<int> SignOrganizationTypeSelectList(SignOrganizationTypeSelectListDtoFilter filter)
		{
			return _context.Set<SignOrganizationType>().AsSelectList(filter);
		}

		public SelectList<int> ApplicantTypeSelectList()
		{
			return _context.Set<ApplicantType>().AsSelectList();
		}
		public SelectList<int> OrganizationGroupSelectList()
		{
			return _context.Set<OrganizationGroup>().AsSelectList();
		}
		public SelectList<int> OrganizationCorruptionGroupSelectList()
		{
			var data = _context.Set<Organization>().Where(a => a.Inn.Equals("201794794") || a.Inn.Equals("305004694") || a.Inn.Equals("207325644")).AsSelectList();
			return data;
		}
		public SelectList<int> BusinessSectorSelectList()
		{
			return _context.Set<BusinessSector>().AsSelectList();
		}

		public SelectList<int> CompanyTypeSelectList()
		{
			return _context.Set<CompanyType>().AsSelectList();
		}

		public SelectList<int> ProposalSubjectSelectList()
		{
			return _context.Set<ProposalSubject>().AsSelectList();
		}

		public SelectList<int> ProposalDisclosureSelectList()
		{
			return _context.Set<ProposalDisclosure>().AsSelectList();
		}

		public SelectList<int> EmploymentTypeSelectList()
		{
			return _context.Set<EmploymentType>().AsSelectList();
		}

		public SelectList<int> RegionSelectList()
		{
			return _context.Set<Region>().AsSelectList();
		}

		public SelectList<int> DistrictSelectList(int? regionId)
		{
			return _context.Set<District>().Where(a => !regionId.HasValue || a.RegionId == regionId)
										   .AsSelectList();
		}

		public SelectList<long> MfySelectList(int? districtId)
		{
			return _context.Set<Mfy>().Where(a => !districtId.HasValue || a.DistrictId == districtId)
									  .AsSelectList();
		}

		public void ClearContractorInfo(int contractorId, bool isOnlyDocument = false)
		{
			try
			{
				_unitOfWork.Context.ClearContractorInfo(contractorId, isOnlyDocument);
			}
			catch (Exception ex)
			{
				AddError($"{ex.Message}: {ex.InnerException}");
			}
		}

		public SelectList<int> GetMonthSelectList()
		{
			TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
			return new SelectList<int>(
				Enumerable.Range(1, 12).Select(month => new SelectListItem<int>
				{
					Value = month,
					Text = ti.ToTitleCase(CultureInfo.GetCultureInfo(_cultureHelper.CurrentCulture.Code).DateTimeFormat.GetMonthName(month))
				}).OrderBy(a => a.Value)
			);
		}

		public SelectList<int> CustomJobTypeSelectList()
		{
			return _context.Set<CustomJobType>().Where(a => a.StateId == StateIdConst.ACTIVE).AsSelectList();
		}

		public SelectList<int> MeetingTypeSelectList()
		{
			return _context.Set<MeetingType>().AsSelectList();
		}

		public SelectList<int> BankCodeSelectList()
		{
			return _context.Set<BankCode>().AsSelectList();
		}

		public SelectList<int> ApplicationModelCodeSelectList()
		{
			return _context.Set<ApplicationModelCode>().AsSelectList();
		}

		public SelectList<int> BirthRegionSelectList()
		{
			return _context.Set<Person>().AsSelectList1();
		}

		public SelectList<int> ApplicationTypeStepSelectList(int applicationTypeId)
		{
			return _context.Set<ApplicationTypeStep>()
				.Include(x => x.Translates)
				.Where(x => x.ApplicationTypeId == applicationTypeId)
				.AsSelectList();
		}

		public SelectList<int> ApplicationTypeSelectList()
		{
			return _context.Set<ApplicationType>()
				.Include(x => x.Translates)
				.AsSelectList();
		}
		public SelectList<int> AppealFormatTypeSelectList()
		{
			return _context.Set<AppealFormatType>().AsSelectList();
		}
		public SelectList<int> AppealTypeSelectList()
		{
			return _context.Set<AppealType>().AsSelectList();
		}
		public SelectList<int> OrganizationAsSelectListByGroup(int[]? groupId)
		{
			groupId ??= new int[0];

			return _context.Set<Organization>()
				.Where(x => groupId.Length == 0 || groupId.Contains(x.OrganizationGroupId))
				.AsSelectList();
		}

		public SelectList<int> ContractorTypeSelectList()
		{
			return _context.Set<ContractorType>().AsSelectList();
		}

		public SelectList<int> RatingSelectList()
		{
			return _context.Set<Rating>().AsSelectList();
		}
		public SelectList<int> OkedTypeSelectList()
		{
			return _context.Set<OkedType>().Include(a => a.Translates).AsSelectList();
		}

		public SelectList<int> AccountNumberSelectList()
		{

			List<string> codes = new List<string>
			{    "20212000003781497001",
				 "20212000803781497025",
				 "20212000503781497051",
				 "20212000803781497026",
				 "20212000903781497007",
				 "20212000803781497022"
			};

			return new SelectList<int>(codes.Select(a => new SelectListItem<int>
			{
				Text = a,
			}));
		}

		public SelectList<int> AsSelectList()
		{
			return _context.Set<EduType>().AsSelectList();
		}
	}
}


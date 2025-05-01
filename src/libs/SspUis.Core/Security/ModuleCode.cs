using WEBASE;

namespace SspUis.Core.Security
{
    public enum ModuleCode
    {
        #region Country
        [ModuleCodeDescription(ModuleSubGroupCode.Country, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        CountryView,

        [ModuleCodeDescription(ModuleSubGroupCode.Country, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        CountryCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Country, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        CountryEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Country, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        CountryDelete,
        #endregion

        #region ContractorSurvey
        [ModuleCodeDescription(ModuleSubGroupCode.ContractorSurvey, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ContractorSurveyView,

        [ModuleCodeDescription(ModuleSubGroupCode.ContractorSurvey, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ContractorSurveyCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ContractorSurvey, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ContractorSurveyEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ContractorSurvey, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ContractorSurveyDelete,
        #endregion

        #region Questionnaire
        [ModuleCodeDescription(ModuleSubGroupCode.Questionnaire, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        QuestionnaireView,

        [ModuleCodeDescription(ModuleSubGroupCode.Questionnaire, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        QuestionnaireCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Questionnaire, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        QuestionnaireEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Questionnaire, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        QuestionnaireDelete,
        #endregion

        #region Position
        [ModuleCodeDescription(ModuleSubGroupCode.Position, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        PositionView,

        [ModuleCodeDescription(ModuleSubGroupCode.Position, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        PositionCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Position, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        PositionEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Position, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        PositionDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.Position, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        AllPositionView,

        [ModuleCodeDescription(ModuleSubGroupCode.Position, "Создать все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini yaratish")]
        [Translate(LanguageIdConst.RU, "Создать все")]
        [Translate(LanguageIdConst.EN, "Create all")]
        AllPositionCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Position, "Редактировать все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать все")]
        [Translate(LanguageIdConst.EN, "Edit all")]
        AllPositionEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Position, "Удалить все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini o'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить все")]
        [Translate(LanguageIdConst.EN, "Delete all")]
        AllPositionDelete,
        #endregion

        #region LandingPageDatum
        [ModuleCodeDescription(ModuleSubGroupCode.LandingPageDatum, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        LandingPageDatumView,

        [ModuleCodeDescription(ModuleSubGroupCode.LandingPageDatum, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        LandingPageDatumCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.LandingPageDatum, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        LandingPageDatumEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.LandingPageDatum, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        LandingPageDatumDelete,
        #endregion

        #region ControlFunction
        [ModuleCodeDescription(ModuleSubGroupCode.ControlFunction, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ControlFunctionView,

        [ModuleCodeDescription(ModuleSubGroupCode.ControlFunction, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ControlFunctionCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ControlFunction, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ControlFunctionEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ControlFunction, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ControlFunctionDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.ControlFunction, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        AllControlFunctionView,

        [ModuleCodeDescription(ModuleSubGroupCode.ControlFunction, "Создать все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini yaratish")]
        [Translate(LanguageIdConst.RU, "Создать все")]
        [Translate(LanguageIdConst.EN, "Create all")]
        AllControlFunctionCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ControlFunction, "Редактировать все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать все")]
        [Translate(LanguageIdConst.EN, "Edit all")]
        AllControlFunctionEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ControlFunction, "Удалить все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini o'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить все")]
        [Translate(LanguageIdConst.EN, "Delete all")]
        AllControlFunctionDelete,
        #endregion

        #region OrganizationInspectionType
        [ModuleCodeDescription(ModuleSubGroupCode.OrganizationInspectionType, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        OrganizationInspectionTypeView,

        [ModuleCodeDescription(ModuleSubGroupCode.OrganizationInspectionType, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        OrganizationInspectionTypeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.OrganizationInspectionType, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        OrganizationInspectionTypeEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.OrganizationInspectionType, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        OrganizationInspectionTypeDelete,
        #endregion

        #region PositionClassifier
        [ModuleCodeDescription(ModuleSubGroupCode.PositionClassifier, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        PositionClassifierView,

        [ModuleCodeDescription(ModuleSubGroupCode.PositionClassifier, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        PositionClassifierCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.PositionClassifier, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        PositionClassifierEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.PositionClassifier, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        PositionClassifierDelete,
        #endregion

        #region Region
        [ModuleCodeDescription(ModuleSubGroupCode.Region, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        RegionView,

        [ModuleCodeDescription(ModuleSubGroupCode.Region, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        RegionCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Region, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        RegionEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Region, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        RegionDelete,
        #endregion

        #region Citizenship
        [ModuleCodeDescription(ModuleSubGroupCode.Citizenship, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        CitizenshipView,

        [ModuleCodeDescription(ModuleSubGroupCode.Citizenship, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        CitizenshipCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Citizenship, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        CitizenshipEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Citizenship, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        CitizenshipDelete,
        #endregion

        #region Nationality
        [ModuleCodeDescription(ModuleSubGroupCode.Nationality, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        NationalityView,

        [ModuleCodeDescription(ModuleSubGroupCode.Nationality, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        NationalityCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Nationality, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        NationalityEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Nationality, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        NationalityDelete,
        #endregion

        #region District
        [ModuleCodeDescription(ModuleSubGroupCode.District, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        DistrictView,

        [ModuleCodeDescription(ModuleSubGroupCode.District, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        DistrictCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.District, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        DistrictEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.District, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        DistrictDelete,
        #endregion

        #region Mfy

        [ModuleCodeDescription(ModuleSubGroupCode.Mfy, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        MfyView,
        #endregion

        #region Oked
        [ModuleCodeDescription(ModuleSubGroupCode.Oked, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        OkedView,

        [ModuleCodeDescription(ModuleSubGroupCode.Oked, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        OkedCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Oked, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        OkedEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Oked, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        OkedDelete,
        #endregion

        #region Bank
        [ModuleCodeDescription(ModuleSubGroupCode.Bank, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        BankView,

        [ModuleCodeDescription(ModuleSubGroupCode.Bank, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        BankCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Bank, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        BankEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Bank, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        BankDelete,
        #endregion

        #region Organization
        [ModuleCodeDescription(ModuleSubGroupCode.Organization, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        OrganizationView,

        [ModuleCodeDescription(ModuleSubGroupCode.Organization, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        OrganizationCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Organization, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        OrganizationEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Organization, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        OrganizationDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.Organization, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        AllOrganizationView,

        [ModuleCodeDescription(ModuleSubGroupCode.Organization, "Создать все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini yaratish")]
        [Translate(LanguageIdConst.RU, "Создать все")]
        [Translate(LanguageIdConst.EN, "Create all")]
        AllOrganizationCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Organization, "Редактировать все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать все")]
        [Translate(LanguageIdConst.EN, "Edit all")]
        AllOrganizationEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Organization, "Удалить все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini o'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить все")]
        [Translate(LanguageIdConst.EN, "Delete all")]
        AllOrganizationDelete,
        #endregion


        #region OrganizationalStructure
        [ModuleCodeDescription(ModuleSubGroupCode.OrganizationalStructure, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        OrganizationalStructureView,

        [ModuleCodeDescription(ModuleSubGroupCode.OrganizationalStructure, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        OrganizationalStructureCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.OrganizationalStructure, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        OrganizationalStructureEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.OrganizationalStructure, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        OrganizationalStructureDelete,
        #endregion

        #region Role
        [ModuleCodeDescription(ModuleSubGroupCode.Role, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        RoleView,

        [ModuleCodeDescription(ModuleSubGroupCode.Role, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        RoleCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Role, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        RoleEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Role, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        RoleDelete,
        #endregion

        #region Access
        [ModuleCodeDescription(ModuleSubGroupCode.Access, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        AccessView,

        [ModuleCodeDescription(ModuleSubGroupCode.Access, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        AccessCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Access, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        AccessEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Access, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        AccessDelete,
        #endregion

        #region Person
        [ModuleCodeDescription(ModuleSubGroupCode.Person, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        PersonView,

        [ModuleCodeDescription(ModuleSubGroupCode.Person, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        PersonCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Person, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        PersonEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Person, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        PersonDelete,
        #endregion

        #region Employee
        [ModuleCodeDescription(ModuleSubGroupCode.Employee, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        EmployeeView,

        [ModuleCodeDescription(ModuleSubGroupCode.Employee, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        EmployeeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Employee, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        EmployeeEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Employee, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        EmployeeDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.Employee, "Просмотр по подразделениям")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бўлимлар бўйича кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bo'limlar bo'yicha ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр по подразделениям")]
        [Translate(LanguageIdConst.EN, "View by branches")]
        BranchesEmployeeView,

        [ModuleCodeDescription(ModuleSubGroupCode.Employee, "Создать по подразделениям")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бўлимлар бўйича яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bo'limlar bo'yicha yaratish")]
        [Translate(LanguageIdConst.RU, "Создать по подразделениям")]
        [Translate(LanguageIdConst.EN, "Create by branches")]
        BranchesEmployeeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Employee, "Редактировать по подразделениям")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бўлимлар бўйича таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bo'limlar bo'yicha tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать по подразделениям")]
        [Translate(LanguageIdConst.EN, "Edit by branches")]
        BranchesEmployeeEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Employee, "Удалить по подразделениям")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бўлимлар бўйича ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bo'limlar bo'yicha o'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить по подразделениям")]
        [Translate(LanguageIdConst.EN, "Delete by branches")]
        BranchesEmployeeDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.Employee, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        AllEmployeeView,

        [ModuleCodeDescription(ModuleSubGroupCode.Employee, "Создать все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini yaratish")]
        [Translate(LanguageIdConst.RU, "Создать все")]
        [Translate(LanguageIdConst.EN, "Create all")]
        AllEmployeeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Employee, "Редактировать все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать все")]
        [Translate(LanguageIdConst.EN, "Edit all")]
        AllEmployeeEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Employee, "Удалить все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini o'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить все")]
        [Translate(LanguageIdConst.EN, "Delete all")]
        AllEmployeeDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.Employee, "Сделать менеджером по персоналу")]
        [Translate(LanguageIdConst.UZ_CYRL, "HR менежери қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "HR menejeri qilish")]
        [Translate(LanguageIdConst.RU, "Сделать менеджером по персоналу")]
        [Translate(LanguageIdConst.EN, "Make HR manager")]
        EmployeeMakeHr,

        [ModuleCodeDescription(ModuleSubGroupCode.Employee, "Сделать инспектором")]
        [Translate(LanguageIdConst.UZ_CYRL, "Инспекторга ўтказиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Inspektorga o'tkazish")]
        [Translate(LanguageIdConst.RU, "Сделать инспектором")]
        [Translate(LanguageIdConst.EN, "Make inspector")]
        EmployeeMakeInspector,

        [ModuleCodeDescription(ModuleSubGroupCode.Employee, "Посмотреть с аттестацией")]
        [Translate(LanguageIdConst.UZ_CYRL, "Aттестация билан кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Attestatsiya bilan ko'rish")]
        [Translate(LanguageIdConst.RU, "Посмотреть с аттестацией")]
        [Translate(LanguageIdConst.EN, "View with attestation")]
        EmployeeWithAttestationView,
        #endregion

        #region User
        [ModuleCodeDescription(ModuleSubGroupCode.User, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        UserView,

        [ModuleCodeDescription(ModuleSubGroupCode.User, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        UserCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.User, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        UserEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.User, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        UserDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.User, "Просмотр по подразделениям")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бўлимлар бўйича кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bo'limlar bo'yicha ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр по подразделениям")]
        [Translate(LanguageIdConst.EN, "View by branches")]
        BranchesUserView,

        [ModuleCodeDescription(ModuleSubGroupCode.User, "Создать по подразделениям")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бўлимлар бўйича яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bo'limlar bo'yicha yaratish")]
        [Translate(LanguageIdConst.RU, "Создать по подразделениям")]
        [Translate(LanguageIdConst.EN, "Create by branches")]
        BranchesUserCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.User, "Редактировать по подразделениям")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бўлимлар бўйича таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bo'limlar bo'yicha tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать по подразделениям")]
        [Translate(LanguageIdConst.EN, "Edit by branches")]
        BranchesUserEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.User, "Удалить по подразделениям")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бўлимлар бўйича ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bo'limlar bo'yicha o'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить по подразделениям")]
        [Translate(LanguageIdConst.EN, "Delete by branches")]
        BranchesUserDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.User, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        AllUserView,

        [ModuleCodeDescription(ModuleSubGroupCode.User, "Создать все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini yaratish")]
        [Translate(LanguageIdConst.RU, "Создать все")]
        [Translate(LanguageIdConst.EN, "Create all")]
        AllUserCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.User, "Редактировать все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать все")]
        [Translate(LanguageIdConst.EN, "Edit all")]
        AllUserEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.User, "Удалить все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini o'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить все")]
        [Translate(LanguageIdConst.EN, "Delete all")]
        AllUserDelete,
        #endregion

        #region CheckType
        [ModuleCodeDescription(ModuleSubGroupCode.CheckType, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        CheckTypeView,

        [ModuleCodeDescription(ModuleSubGroupCode.CheckType, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        CheckTypeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.CheckType, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        CheckTypeEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.CheckType, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        CheckTypeDelete,
        #endregion

        #region CheckBasis
        [ModuleCodeDescription(ModuleSubGroupCode.CheckBasis, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        CheckBasisView,

        [ModuleCodeDescription(ModuleSubGroupCode.CheckBasis, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        CheckBasisCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.CheckBasis, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        CheckBasisEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.CheckBasis, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        CheckBasisDelete,
        #endregion

        #region VideoLesson
        [ModuleCodeDescription(ModuleSubGroupCode.VideoLesson, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        VideoLessonView,

        [ModuleCodeDescription(ModuleSubGroupCode.VideoLesson, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        VideoLessonCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.VideoLesson, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        VideoLessonEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.VideoLesson, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        VideoLessonDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.VideoLesson, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        AllVideoLessonView,

        [ModuleCodeDescription(ModuleSubGroupCode.VideoLesson, "Создать все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini yaratish")]
        [Translate(LanguageIdConst.RU, "Создать все")]
        [Translate(LanguageIdConst.EN, "Create all")]
        AllVideoLessonCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.VideoLesson, "Редактировать все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать все")]
        [Translate(LanguageIdConst.EN, "Edit all")]
        AllVideoLessonEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.VideoLesson, "Удалить все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini o'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить все")]
        [Translate(LanguageIdConst.EN, "Delete all")]
        AllVideoLessonDelete,
        #endregion

        #region VideoCategory
        [ModuleCodeDescription(ModuleSubGroupCode.VideoCategory, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        VideoCategoryView,

        [ModuleCodeDescription(ModuleSubGroupCode.VideoCategory, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        VideoCategoryCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.VideoCategory, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        VideoCategoryEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.VideoCategory, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        VideoCategoryDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.VideoCategory, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        AllVideoCategoryView,

        [ModuleCodeDescription(ModuleSubGroupCode.VideoCategory, "Создать все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini yaratish")]
        [Translate(LanguageIdConst.RU, "Создать все")]
        [Translate(LanguageIdConst.EN, "Create all")]
        AllVideoCategoryCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.VideoCategory, "Редактировать все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать все")]
        [Translate(LanguageIdConst.EN, "Edit all")]
        AllVideoCategoryEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.VideoCategory, "Удалить все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini o'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить все")]
        [Translate(LanguageIdConst.EN, "Delete all")]
        AllVideoCategoryDelete,
        #endregion

        #region OrganizationLegalForm
        [ModuleCodeDescription(ModuleSubGroupCode.OrganizationLegalForm, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        OrganizationLegalFormView,

        [ModuleCodeDescription(ModuleSubGroupCode.OrganizationLegalForm, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        OrganizationLegalFormCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.OrganizationLegalForm, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        OrganizationLegalFormEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.OrganizationLegalForm, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        OrganizationLegalFormDelete,
        #endregion

        #region News
        [ModuleCodeDescription(ModuleSubGroupCode.News, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        NewsView,

        [ModuleCodeDescription(ModuleSubGroupCode.News, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        NewsCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.News, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        NewsEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.News, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        NewsDelete,
        #endregion

        #region NewsTag
        [ModuleCodeDescription(ModuleSubGroupCode.NewsTag, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        NewsTagView,

        [ModuleCodeDescription(ModuleSubGroupCode.NewsTag, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        NewsTagCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.NewsTag, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        NewsTagEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.NewsTag, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        NewsTagDelete,
        #endregion

        #region Dashboard
        [ModuleCodeDescription(ModuleSubGroupCode.Dashboard, "Просмотр 20 тысяч предпринимателей")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш 20 минг тадбиркор")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish 20 ming tadbirkor")]
        [Translate(LanguageIdConst.RU, "Просмотр 20 тысяч предпринимателей")]
        [Translate(LanguageIdConst.EN, "View")]
        DashboardView,

        [ModuleCodeDescription(ModuleSubGroupCode.Dashboard, "Просмотр льготы для 20 тысячи предпринимателей")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш 20 минг тадбиркор имтиёзлар")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish 20 ming tadbirkor imtiyozlar")]
        [Translate(LanguageIdConst.RU, "Просмотр льготы для 20 тысячи предпринимателей")]
        [Translate(LanguageIdConst.EN, "View")]
        PartnerDashboardView,

        [ModuleCodeDescription(ModuleSubGroupCode.Dashboard, "Просмотр Членство")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш Аъзолик")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish A’zolik")]
        [Translate(LanguageIdConst.RU, "Просмотр Членство")]
        [Translate(LanguageIdConst.EN, "View")]
        MemshipDashboardView,

        [ModuleCodeDescription(ModuleSubGroupCode.Dashboard, "Просмотр Заявка на претензию")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш Даъво ариза")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish Da’vo ariza")]
        [Translate(LanguageIdConst.RU, "Просмотр Заявка на претензию")]
        [Translate(LanguageIdConst.EN, "View")]
        ClaimDashboardView,

        [ModuleCodeDescription(ModuleSubGroupCode.Dashboard, "Просмотр Кадровый")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш Кадр")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish Kadr")]
        [Translate(LanguageIdConst.RU, "Просмотр Кадровый")]
        [Translate(LanguageIdConst.EN, "View")]
        HrmDashboardView,

        #endregion

        #region Contractor
        [ModuleCodeDescription(ModuleSubGroupCode.Contractor, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ContractorView,

        [ModuleCodeDescription(ModuleSubGroupCode.Contractor, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ContractorEdit,

        #endregion

        #region Report
        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет по заявкам и контрактам(Членство)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ариза ва шартномалар бўйича хисобот")]
        [Translate(LanguageIdConst.UZ_LATN, "Ariza va shartnomalar bo'yicha hisobot(A'zolik)")]
        [Translate(LanguageIdConst.RU, "Отчет по заявкам и контрактам(Членство)")]
        [Translate(LanguageIdConst.EN, "ReportMemshipApplicationAndContractInfoView")]
        ReportMemshipApplicationAndContractInfoView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "A'zolik pullik hisobot2 (Org)")]
        [Translate(LanguageIdConst.UZ_CYRL, "A'zolik pullik hisobot2 (Org)")]
        [Translate(LanguageIdConst.UZ_LATN, "A'zolik pullik hisobot2 (Org)")]
        [Translate(LanguageIdConst.RU, "A'zolik pullik hisobot2 (Org)")]
        [Translate(LanguageIdConst.EN, "ReportMemshipContractView")]
        ReportMemshipContractView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет по заявкам по типу организации")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ташкилот турлари бўйича аризалар ҳисоботи")]
        [Translate(LanguageIdConst.UZ_LATN, "Tashkilot turlari bo'yicha arizalar hisoboti")]
        [Translate(LanguageIdConst.RU, "Отчет по заявкам по типу организации")]
        [Translate(LanguageIdConst.EN, "ReportMemshipApplicationByOrganizationView")]
        ReportMemshipApplicationByOrganizationView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет об обслуживании")]
        [Translate(LanguageIdConst.UZ_CYRL, "Хизматлар тўғрисидаги ҳисобот")]
        [Translate(LanguageIdConst.UZ_LATN, "Xizmatlar to'g'risidagi hisobot")]
        [Translate(LanguageIdConst.RU, "Отчет об обслуживании")]
        [Translate(LanguageIdConst.EN, "ReportServiceInfo")]
        ReportSrvServiceGetInfo,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Murojatlar hisoboti")]
        [Translate(LanguageIdConst.UZ_CYRL, "Murojatlar hisoboti")]
        [Translate(LanguageIdConst.UZ_LATN, "Murojatlar hisoboti")]
        [Translate(LanguageIdConst.RU, "Murojatlar hisoboti")]
        [Translate(LanguageIdConst.EN, "AppealApplicationReport")]
        AppealApplicationReport,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет о рабочем времени (колл-центр)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ish vaqti bo'yicha hisobot(call centr)")]
        [Translate(LanguageIdConst.UZ_LATN, "Иш вақти бўйича ҳисобот(салл сентр)")]
        [Translate(LanguageIdConst.RU, "Отчет о рабочем времени (колл-центр)")]
        [Translate(LanguageIdConst.EN, "Call Center Report By Week Report")]
        CallCenterReportByWeekReport,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет о направлении заявки и активности (колл-центр)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Мурожаат йўналиши ва фаолият бўйича ҳисобот (салл сентр)")]
        [Translate(LanguageIdConst.UZ_LATN, "Murojaat yo'nalishi va faoliyat bo'yicha hisobot (call centr)")]
        [Translate(LanguageIdConst.RU, "Отчет о направлении заявки и активности (колл-центр)")]
        [Translate(LanguageIdConst.EN, "Call Center Appeal Report By Oked Type")]
        CallCenterAppealReportByOkedType,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет о заявлениях по типам лиц")]
        [Translate(LanguageIdConst.UZ_CYRL, "Шахс турлари бўйича аризалар ҳисоботи")]
        [Translate(LanguageIdConst.UZ_LATN, "Shaxs turlari bo'yicha arizalar hisoboti")]
        [Translate(LanguageIdConst.RU, "Отчет о заявлениях по типам лиц")]
        [Translate(LanguageIdConst.EN, "ReportMemshipApplicationByPersonView")]
        ReportMemshipApplicationByPersonTypeView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет по заявкам и контрактам(Сотрудничество)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ариза ва шартномалар бўйича хисобот")]
        [Translate(LanguageIdConst.UZ_LATN, "Ariza va shartnomalar bo'yicha hisobot(Hamkorlik)")]
        [Translate(LanguageIdConst.RU, "Отчет по заявкам и контрактам(Сотрудничество)")]
        [Translate(LanguageIdConst.EN, "ReportPrtnApplicationAndContractInfoView")]
        ReportPrtnApplicationAndContractInfoView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет о динамике создания рабочих мест")]
        [Translate(LanguageIdConst.UZ_CYRL, "Иш ўрни яратиш динамикаси бўйича ҳисобот")]
        [Translate(LanguageIdConst.UZ_LATN, "Ish o'rni yaratish dinamikasi bo'yicha hisobot")]
        [Translate(LanguageIdConst.RU, "Отчет о динамике создания рабочих мест")]
        [Translate(LanguageIdConst.EN, "ReportOnTheDynamicsOfJobCreation")]
        ReportPrtnEmployeeJobView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет по заявкам и контрактам")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ариза ва шартномалар бўйича хисобот")]
        [Translate(LanguageIdConst.UZ_LATN, "Ariza va shartnomalar bo'yicha hisobot")]
        [Translate(LanguageIdConst.RU, "Отчет по заявкам и контрактам")]
        [Translate(LanguageIdConst.EN, "ReportPrtnCreditDemandInfo")]
        ReportPrtnCreditDemandInfoView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет по заявкам и контрактам (В разделе банки)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ариза ва шартномалар бўйича хисобот (Банклар кесимида)")]
        [Translate(LanguageIdConst.UZ_LATN, "Ariza va shartnomalar bo'yicha hisobot (Banklar kesimida)")]
        [Translate(LanguageIdConst.RU, "Отчет по заявкам и контрактам (В разделе банки)")]
        [Translate(LanguageIdConst.EN, "ReportPrtnCreditDemandInfo (ByBank)")]
        ReportPrtnCreditDemandInfoByBankView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет о предоставлении гарантий по кредитам")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кредитлар учун кафилликларни тақдим этиш бўйича ҳисобот")]
        [Translate(LanguageIdConst.UZ_LATN, "Kreditlar uchun kafilliklarni taqdim etish bo'yicha hisobot")]
        [Translate(LanguageIdConst.RU, "Отчет о предоставлении гарантий по кредитам")]
        [Translate(LanguageIdConst.EN, "TadbirkorFundReport")]
        ReportTadbirkorFundReportView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Региональный отчет (колл-центр)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кредитлар учун кафилликларни тақдим этиш бўйича ҳисобот")]
        [Translate(LanguageIdConst.UZ_LATN, "Ҳудуд кесимида ҳисобот(салл сентр)")]
        [Translate(LanguageIdConst.RU, "Региональный отчет (колл-центр)")]
        [Translate(LanguageIdConst.EN, "Call Center By Region Report View")]
        CallCenterByRegionReportView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Налоговый отчет")]
        [Translate(LanguageIdConst.UZ_CYRL, "Солиқ бўйича ҳисобот")]
        [Translate(LanguageIdConst.UZ_LATN, "Soliq bo'yicha hisobot")]
        [Translate(LanguageIdConst.RU, "Налоговый отчет")]
        [Translate(LanguageIdConst.EN, "TadbirkorFundReport")]
        GetTaxReport,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет по заявкам на банковские кредиты по областям и районам")]
        [Translate(LanguageIdConst.UZ_CYRL, "Вилоят ва туманлар бўйича банк кредитига мурожаатлар ҳисоботи")]
        [Translate(LanguageIdConst.UZ_LATN, "Viloyat va tumanlar bo‘yicha bank kreditiga murojaatlar hisoboti")]
        [Translate(LanguageIdConst.RU, "Отчет по заявкам на банковские кредиты по областям и районам")]
        [Translate(LanguageIdConst.EN, "BankCreditApplicationReportByRegionAndDistrictView")]
        GetBankCreditApplicationReportByRegionAndDistrictView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Штатное расписание (единое окно)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Штатлар жадвали (ягона ойнаси)")]
        [Translate(LanguageIdConst.UZ_LATN, "Shtatlar jadvali (yagona oynasi)")]
        [Translate(LanguageIdConst.RU, "Штатное расписание (единое окно)")]
        [Translate(LanguageIdConst.EN, "Staffing Single Report View")]
        StaffingSingleReportView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Штатное расписание для ССП (единое окно)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Штатлар жадвали ССП (ягона ойнаси)")]
        [Translate(LanguageIdConst.UZ_LATN, "Shtatlar jadvali SSP (yagona oynasi)")]
        [Translate(LanguageIdConst.RU, "Штатное расписание для ССП (единое окно)")]
        [Translate(LanguageIdConst.EN, "Staffing Single Report For Parent")]
        GetStaffingSingleReportForParentView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Buyruqlar Hisoboti")]
        [Translate(LanguageIdConst.UZ_CYRL, "Buyruqlar Hisoboti")]
        [Translate(LanguageIdConst.UZ_LATN, "Buyruqlar Hisoboti")]
        [Translate(LanguageIdConst.RU, "Buyruqlar Hisoboti")]
        [Translate(LanguageIdConst.EN, "HrmDocumentReport")]
        HrmDocumentReport,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет заявок и контрактов по регионам")]
        [Translate(LanguageIdConst.UZ_CYRL, "Вилоятлар бўйича ариза ва шартномалар хисоботи")]
        [Translate(LanguageIdConst.UZ_LATN, "Viloyatlar bo'yicha ariza va shartnomalar hisoboti")]
        [Translate(LanguageIdConst.RU, "Отчет заявок и контрактов по регионам")]
        [Translate(LanguageIdConst.EN, "ReportPrtnApplicationAndContractInfoByRegion")]
        ReportPrtnApplicationAndContractInfoByRegionView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет применение по типу контракта")]
        [Translate(LanguageIdConst.UZ_CYRL, "Шартнома тури бўйича ариза хисобот")]
        [Translate(LanguageIdConst.UZ_LATN, "Shartnoma turi bo'yicha ariza hisoboti")]
        [Translate(LanguageIdConst.RU, "Отчет применение по типу контракта")]
        [Translate(LanguageIdConst.EN, "ReportPrtnApplicationByContractTypeInfoView")]
        ReportPrtnApplicationByContractTypeInfoView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет документы, прошедшие суроги")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ҳисобот сурогидан ўтган документлар")]
        [Translate(LanguageIdConst.UZ_LATN, "Hisobot surogidan o'tgan dokumentlar")]
        [Translate(LanguageIdConst.RU, "Отчет документы, прошедшие суроги")]
        [Translate(LanguageIdConst.EN, "Report Partner Expired Documents View")]
        ReportPrntExpiredDocumentsView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет фонда предпринимателей")]
        [Translate(LanguageIdConst.UZ_CYRL, "Тадбиркорлар жамғарма хисоботи")]
        [Translate(LanguageIdConst.UZ_LATN, "Tadbirkorlar jamg'arma hisoboti")]
        [Translate(LanguageIdConst.RU, "Отчет фонда предпринимателей")]
        [Translate(LanguageIdConst.EN, "ReportPrtnCertificateByContractInfoView")]
        ReportPrtnCertificateByContractInfoView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет по банковскому кредиту")]
        [Translate(LanguageIdConst.UZ_CYRL, "Банк кредитлар ҳисоботи")]
        [Translate(LanguageIdConst.UZ_LATN, "Bank kreditlar hisoboti")]
        [Translate(LanguageIdConst.RU, "Отчет по банковскому кредиту")]
        [Translate(LanguageIdConst.EN, "ReportBankCreditView")]
        ReportBankCreditView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Налоговый отчет предпринимателей")]
        [Translate(LanguageIdConst.UZ_CYRL, "Тадбиркорлар солиқ ҳисоботи")]
        [Translate(LanguageIdConst.UZ_LATN, "Tadbirkorlar soliq hisoboti")]
        [Translate(LanguageIdConst.RU, "Налоговый отчет предпринимателей")]
        [Translate(LanguageIdConst.EN, "ReportSoliqByContractorView")]
        ReportSoliqByContractorView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Налоговый отчет предпринимателей")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қўшимча қиймат солиқ ҳисоботи")]
        [Translate(LanguageIdConst.UZ_LATN, "Qo'shimcha qiymat soliq hisoboti")]
        [Translate(LanguageIdConst.RU, "Налоговая отчетность по добавленной стоимости")]
        [Translate(LanguageIdConst.EN, "ReportTaxQqsAylanmaView")]
        ReportTaxQqsAylanmaView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Получить  заявление о государственных активах")]
        [Translate(LanguageIdConst.UZ_CYRL, "Давлат активлари учун ариза")]
        [Translate(LanguageIdConst.UZ_LATN, "Davlat aktivlari uchun ariza")]
        [Translate(LanguageIdConst.RU, "Получить  заявление о государственных активах")]
        [Translate(LanguageIdConst.EN, "GetStateAssetApplication Report")]
        ReportStateAssetApplicationView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Korrupsiya hisoboti")]
        [Translate(LanguageIdConst.UZ_CYRL, "Korrupsiya hisoboti")]
        [Translate(LanguageIdConst.UZ_LATN, "Korrupsiya hisoboti")]
        [Translate(LanguageIdConst.RU, "Korrupsiya hisoboti")]
        [Translate(LanguageIdConst.EN, "CorruptionApplication View")]
        CorruptionApplicationView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет о таможенных льготах для предпринимателей")]
        [Translate(LanguageIdConst.UZ_CYRL, "Тадбиркорлар божхона имтиёз ҳисоботи")]
        [Translate(LanguageIdConst.UZ_LATN, "Tadbirkorlar bojxona imtiyoz hisoboti")]
        [Translate(LanguageIdConst.RU, "Отчет о таможенных льготах для предпринимателей")]
        [Translate(LanguageIdConst.EN, "BojxonaImtiyozReportByContractorView")]
        BojxonaImtiyozReportByContractorView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет 1")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ҳисобот 1")]
        [Translate(LanguageIdConst.UZ_LATN, "Hisobot 1")]
        [Translate(LanguageIdConst.RU, "Отчет 1")]
        [Translate(LanguageIdConst.EN, "Report 1")]
        Report1View,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет 2")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ҳисобот 2")]
        [Translate(LanguageIdConst.UZ_LATN, "Hisobot 2")]
        [Translate(LanguageIdConst.RU, "Отчет 2")]
        [Translate(LanguageIdConst.EN, "Report 2")]
        Report2View,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет 3")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ҳисобот 3")]
        [Translate(LanguageIdConst.UZ_LATN, "Hisobot 3")]
        [Translate(LanguageIdConst.RU, "Отчет 3")]
        [Translate(LanguageIdConst.EN, "Report 3")]
        Report3View,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет 4")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ҳисобот 4")]
        [Translate(LanguageIdConst.UZ_LATN, "Hisobot 4")]
        [Translate(LanguageIdConst.RU, "Отчет 4")]
        [Translate(LanguageIdConst.EN, "Report 4")]
        Report4View,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет 5")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ҳисобот 5")]
        [Translate(LanguageIdConst.UZ_LATN, "Hisobot 5")]
        [Translate(LanguageIdConst.RU, "Отчет 5")]
        [Translate(LanguageIdConst.EN, "Report 5")]
        Report5View,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет 6")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ҳисобот 6")]
        [Translate(LanguageIdConst.UZ_LATN, "Hisobot 6")]
        [Translate(LanguageIdConst.RU, "Отчет 6")]
        [Translate(LanguageIdConst.EN, "Report 6")]
        Report6View,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет 7")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ҳисобот 7")]
        [Translate(LanguageIdConst.UZ_LATN, "Hisobot 7")]
        [Translate(LanguageIdConst.RU, "Отчет 7")]
        [Translate(LanguageIdConst.EN, "Report 7")]
        Report7View,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет 8")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ҳисобот 8")]
        [Translate(LanguageIdConst.UZ_LATN, "Hisobot 8")]
        [Translate(LanguageIdConst.RU, "Отчет 8")]
        [Translate(LanguageIdConst.EN, "Report 8")]
        Report8View,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет о деятельности сотрудников")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ходимлар ҳаракати бўйича ҳисобот")]
        [Translate(LanguageIdConst.UZ_LATN, "Xodimlar harakati bo'yicha hisobot")]
        [Translate(LanguageIdConst.RU, "Отчет о деятельности сотрудников")]
        [Translate(LanguageIdConst.EN, "ReportHrmEmployeeActivityInfo")]
        ReportHrmEmployeeActivityInfoView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет о деятельности сотрудников по регионам")]
        [Translate(LanguageIdConst.UZ_CYRL, "Вилоятлар бўйича xодимлар харакати ҳисоботи")]
        [Translate(LanguageIdConst.UZ_LATN, "Viloyatlar bo'yicha xodimlar harakati hisoboti")]
        [Translate(LanguageIdConst.RU, "Отчет о деятельности сотрудников по регионам")]
        [Translate(LanguageIdConst.EN, "ReportHrmEmployeeActivityInfoByRegion")]
        ReportHrmEmployeeActivityInfoByRegionView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет о субъектах хозяйствования, являющихся свободными членами Палаты")]
        [Translate(LanguageIdConst.UZ_CYRL, "Палатага бепул аъзо бўлган тадбиркорлик субъектлари тўғрисида ҳисобот")]
        [Translate(LanguageIdConst.UZ_LATN, "Palataga bepul a'zo bo'lgan tadbirkorlik subyektlari to'g'risida hisobot")]
        [Translate(LanguageIdConst.RU, "Отчет о субъектах хозяйствования, являющихся свободными членами Палаты")]
        [Translate(LanguageIdConst.EN, "Report on business entities that are free members of the Chamber")]
        ReportMemshipFreeOfChargeView,

        [ModuleCodeDescription(ModuleSubGroupCode.ApplicationStatusReport, "Статус заявок и сроки опоздания на подписание")]
        [Translate(LanguageIdConst.UZ_CYRL, "Аризаларнинг ҳолати ва имзолаш учун кечикган кунлар")]
        [Translate(LanguageIdConst.UZ_LATN, "Arizalarning holati va imzolash uchun kechikgan kunlar")]
        [Translate(LanguageIdConst.RU, "Статус заявок и сроки опоздания на подписание")]
        [Translate(LanguageIdConst.EN, "Status of applications and late days for signature")]
        ApplicationStatusView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет о том, какому предпринимателю пришло смс")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қайси тадбиркорга смс борганлиги тўғрисида ҳисобот")]
        [Translate(LanguageIdConst.UZ_LATN, "Qaysi tadbirkorga sms borganligi to'g'risida hisobot")]
        [Translate(LanguageIdConst.RU, "Отчет о том, какому предпринимателю пришло смс")]
        [Translate(LanguageIdConst.EN, "Report on which entrepreneur went sms")]
        ReportSmsLogView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет о Реестре членов чартера")]
        [Translate(LanguageIdConst.UZ_CYRL, "Хартия аъзолари Реестри hisoboti")]
        [Translate(LanguageIdConst.UZ_LATN, "Hisoboti reestriga azolari Xartiyasi")]
        [Translate(LanguageIdConst.RU, "Отчет о Реестре членов чартера")]
        [Translate(LanguageIdConst.EN, "The Charter of the Azolari hisoboti Registry")]
        ReportCharterMembersRegisterView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет о субъектах хозяйствования, являющихся полноправными членами Палаты")]
        [Translate(LanguageIdConst.UZ_CYRL, "Палатага пуллик аъзо бўлган тадбиркорлик субъектлари тўғрисида ҳисобот")]
        [Translate(LanguageIdConst.UZ_LATN, "Palataga pillik a'zo bo'lgan tadbirkorlik subyektlari to'g'risida hisobot")]
        [Translate(LanguageIdConst.RU, "Отчет о субъектах хозяйствования, являющихся полноправными членами Палаты")]
        [Translate(LanguageIdConst.EN, "Report on business entities that are paid members of the Chamber")]
        ReportMemshipPaidOfChargeView,

        [ModuleCodeDescription(ModuleSubGroupCode.Report, "Отчет о реализации проекта и предоставленных льготах")]
        [Translate(LanguageIdConst.UZ_CYRL, "Лойиха ижроси буйича хисобот")]
        [Translate(LanguageIdConst.UZ_LATN, "Loyixa ijrosi va berilgan imtiyozlar bo'yicha hisobot")]
        [Translate(LanguageIdConst.RU, "Отчет о реализации проекта и предоставленных льготах")]
        [Translate(LanguageIdConst.EN, "ReportOnProjectImplementationAndBenefitsGranted View")]
        ReportOnProjectImplementationAndBenefitsGrantedView,
        #endregion

        #region PrtnContractType
        [ModuleCodeDescription(ModuleSubGroupCode.PrtnContractType, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        PrtnContractTypeView,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnContractType, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        PrtnContractTypeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnContractType, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        PrtnContractTypeEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnContractType, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        PrtnContractTypeDelete,
        #endregion

        #region StateAssetApplication
        [ModuleCodeDescription(ModuleSubGroupCode.StateAssetApplication, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        StateAssetApplicationView,

        [ModuleCodeDescription(ModuleSubGroupCode.StateAssetApplication, "Повторная отправка приложения в DavActive")]
        [Translate(LanguageIdConst.UZ_CYRL, "Arizani DavAktivga qayta yuborish")]
        [Translate(LanguageIdConst.UZ_LATN, "Arizani DavAktivga qayta yuborish")]
        [Translate(LanguageIdConst.RU, "Повторная отправка приложения в DavActiveПовторная отправка приложения в DavActive")]
        [Translate(LanguageIdConst.EN, "Resending the application to DavActive")]
        StateAssetApplicationResendToDavAktiv,

        [ModuleCodeDescription(ModuleSubGroupCode.StateAssetApplication, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        StateAssetApplicationViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.StateAssetApplication, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        StateAssetApplicationCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.StateAssetApplication, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        StateAssetApplicationEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.StateAssetApplication, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        StateAssetApplicationDelete,
        #endregion

        #region ClaimApplication
        [ModuleCodeDescription(ModuleSubGroupCode.ClaimApplication, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ClaimApplicationView,

        [ModuleCodeDescription(ModuleSubGroupCode.ClaimApplication, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ClaimApplicationCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ClaimApplication, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ClaimApplicationEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ClaimApplication, "Провести")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўтказиш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'tkazish")]
        [Translate(LanguageIdConst.RU, "Провести")]
        [Translate(LanguageIdConst.EN, "Accept")]
        ClaimApplicationAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.ClaimApplication, "Отклонить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Рад этиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Rad etish")]
        [Translate(LanguageIdConst.RU, "Отклонить")]
        [Translate(LanguageIdConst.EN, "Reject")]
        ClaimApplicationReject,

        [ModuleCodeDescription(ModuleSubGroupCode.ClaimApplication, "Отменить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отменить")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        ClaimApplicationCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.ClaimApplication, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ClaimApplicationDelete,
        #endregion

        #region ExecutionApplication
        [ModuleCodeDescription(ModuleSubGroupCode.ExecutionApplication, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ExecutionApplicationView,

        [ModuleCodeDescription(ModuleSubGroupCode.ExecutionApplication, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ExecutionApplicationCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ExecutionApplication, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ExecutionApplicationEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ExecutionApplication, "Провести")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўтказиш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'tkazish")]
        [Translate(LanguageIdConst.RU, "Провести")]
        [Translate(LanguageIdConst.EN, "Accept")]
        ExecutionApplicationAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.ExecutionApplication, "Подписать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзолаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolash")]
        [Translate(LanguageIdConst.RU, "Подписать")]
        [Translate(LanguageIdConst.EN, "Sign")]
        ExecutionApplicationSign,

        [ModuleCodeDescription(ModuleSubGroupCode.ExecutionApplication, "Отменить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отменить")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        ExecutionApplicationCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.ExecutionApplication, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ExecutionApplicationDelete,
        #endregion

        #region MemshipApplication
        [ModuleCodeDescription(ModuleSubGroupCode.MemshipApplication, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        MemshipApplicationView,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipApplication, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        MemshipApplicationCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipApplication, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        MemshipApplicationEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipApplication, "Провести")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўтказиш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'tkazish")]
        [Translate(LanguageIdConst.RU, "Провести")]
        [Translate(LanguageIdConst.EN, "Accept")]
        MemshipApplicationAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipApplication, "Отклонить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Рад этиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Rad etish")]
        [Translate(LanguageIdConst.RU, "Отклонить")]
        [Translate(LanguageIdConst.EN, "Reject")]
        MemshipApplicationReject,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipApplication, "Отменить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отменить")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        MemshipApplicationCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipApplication, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        MemshipApplicationDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipApplication, "Отправлять")]
        [Translate(LanguageIdConst.UZ_CYRL, "Юбориш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yuborish")]
        [Translate(LanguageIdConst.RU, "Отправлять")]
        [Translate(LanguageIdConst.EN, "Send")]
        MemshipApplicationSend,
        #endregion
        
        #region ArbitrationCourtApplication
        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationCourtApplication, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ArbitrationCourtApplicationView,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationCourtApplication, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ArbitrationCourtApplicationCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationCourtApplication, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ArbitrationCourtApplicationEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationCourtApplication, "Провести")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўтказиш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'tkazish")]
        [Translate(LanguageIdConst.RU, "Провести")]
        [Translate(LanguageIdConst.EN, "Accept")]
        ArbitrationCourtApplicationAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationCourtApplication, "Отклонить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Рад этиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Rad etish")]
        [Translate(LanguageIdConst.RU, "Отклонить")]
        [Translate(LanguageIdConst.EN, "Reject")]
        ArbitrationCourtApplicationReject,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationCourtApplication, "Отменить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отменить")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        ArbitrationCourtApplicationCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationCourtApplication, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ArbitrationCourtApplicationDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationCourtApplication, "Отправлять")]
        [Translate(LanguageIdConst.UZ_CYRL, "Юбориш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yuborish")]
        [Translate(LanguageIdConst.RU, "Отправлять")]
        [Translate(LanguageIdConst.EN, "Send")]
        ArbitrationCourtApplicationSend,

        #endregion

        #region ArbitrationDashboard
        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationDashboard, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ArbitrationDashboardView,
        #endregion

        #region ServiceContractDashboard
        [ModuleCodeDescription(ModuleSubGroupCode.ServiceContractDashboard, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ServiceContractDashboardView,
        #endregion

        #region ArbitrationResult
        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationResult, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
		ArbitrationResultView,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationResult, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
		ArbitrationResultCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationResult, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ArbitrationResultEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationResult, "Провести")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўтказиш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'tkazish")]
        [Translate(LanguageIdConst.RU, "Провести")]
        [Translate(LanguageIdConst.EN, "Accept")]
        ArbitrationResultAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationResult, "Отклонить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Рад этиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Rad etish")]
        [Translate(LanguageIdConst.RU, "Отклонить")]
        [Translate(LanguageIdConst.EN, "Reject")]
        ArbitrationResultReject,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationResult, "Отменить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отменить")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        ArbitrationResultCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationResult, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ArbitrationResultDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationResult, "Отправлять")]
        [Translate(LanguageIdConst.UZ_CYRL, "Юбориш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yuborish")]
        [Translate(LanguageIdConst.RU, "Отправлять")]
        [Translate(LanguageIdConst.EN, "Send")]
        ArbitrationResultSend,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationResult, "Подписать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзолаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolash")]
        [Translate(LanguageIdConst.RU, "Подписать")]
        [Translate(LanguageIdConst.EN, "Sign")]
        ArbitrationResultSign,

        #endregion

        #region ArbitrationDiscussion
        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationDiscussion, "Просмотр")]
		[Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
		[Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
		[Translate(LanguageIdConst.RU, "Просмотр")]
		[Translate(LanguageIdConst.EN, "View")]
		ArbitrationDiscussionView,

		[ModuleCodeDescription(ModuleSubGroupCode.ArbitrationDiscussion, "Создать")]
		[Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
		[Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
		[Translate(LanguageIdConst.RU, "Создать")]
		[Translate(LanguageIdConst.EN, "Create")]
		ArbitrationDiscussionCreate,

		[ModuleCodeDescription(ModuleSubGroupCode.ArbitrationDiscussion, "Редактировать")]
		[Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
		[Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
		[Translate(LanguageIdConst.RU, "Редактировать")]
		[Translate(LanguageIdConst.EN, "Edit")]
		ArbitrationDiscussionEdit,

		[ModuleCodeDescription(ModuleSubGroupCode.ArbitrationDiscussion, "Провести")]
		[Translate(LanguageIdConst.UZ_CYRL, "Ўтказиш")]
		[Translate(LanguageIdConst.UZ_LATN, "O'tkazish")]
		[Translate(LanguageIdConst.RU, "Провести")]
		[Translate(LanguageIdConst.EN, "Accept")]
		ArbitrationDiscussionAccept,

		[ModuleCodeDescription(ModuleSubGroupCode.ArbitrationDiscussion, "Отклонить")]
		[Translate(LanguageIdConst.UZ_CYRL, "Рад этиш")]
		[Translate(LanguageIdConst.UZ_LATN, "Rad etish")]
		[Translate(LanguageIdConst.RU, "Отклонить")]
		[Translate(LanguageIdConst.EN, "Reject")]
		ArbitrationDiscussionReject,

		[ModuleCodeDescription(ModuleSubGroupCode.ArbitrationDiscussion, "Отменить")]
		[Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
		[Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
		[Translate(LanguageIdConst.RU, "Отменить")]
		[Translate(LanguageIdConst.EN, "Cancel")]
		ArbitrationDiscussionCancel,

		[ModuleCodeDescription(ModuleSubGroupCode.ArbitrationDiscussion, "Удалить")]
		[Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
		[Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
		[Translate(LanguageIdConst.RU, "Удалить")]
		[Translate(LanguageIdConst.EN, "Delete")]
		ArbitrationDiscussionDelete,

		[ModuleCodeDescription(ModuleSubGroupCode.ArbitrationDiscussion, "Отправлять")]
		[Translate(LanguageIdConst.UZ_CYRL, "Юбориш")]
		[Translate(LanguageIdConst.UZ_LATN, "Yuborish")]
		[Translate(LanguageIdConst.RU, "Отправлять")]
		[Translate(LanguageIdConst.EN, "Send")]
		ArbitrationDiscussionSend,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationDiscussion, "Подписать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзолаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolash")]
        [Translate(LanguageIdConst.RU, "Подписать")]
        [Translate(LanguageIdConst.EN, "Sign")]
        ArbitrationDiscussionSign,

        #endregion
        
        #region ArbitrationDelay
        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationDelay, "Просмотр")]
		[Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
		[Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
		[Translate(LanguageIdConst.RU, "Просмотр")]
		[Translate(LanguageIdConst.EN, "View")]
		ArbitrationDelayView,

		[ModuleCodeDescription(ModuleSubGroupCode.ArbitrationDelay, "Создать")]
		[Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
		[Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
		[Translate(LanguageIdConst.RU, "Создать")]
		[Translate(LanguageIdConst.EN, "Create")]
		ArbitrationDelayCreate,

		[ModuleCodeDescription(ModuleSubGroupCode.ArbitrationDelay, "Редактировать")]
		[Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
		[Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
		[Translate(LanguageIdConst.RU, "Редактировать")]
		[Translate(LanguageIdConst.EN, "Edit")]
		ArbitrationDelayEdit,

		[ModuleCodeDescription(ModuleSubGroupCode.ArbitrationDelay, "Провести")]
		[Translate(LanguageIdConst.UZ_CYRL, "Ўтказиш")]
		[Translate(LanguageIdConst.UZ_LATN, "O'tkazish")]
		[Translate(LanguageIdConst.RU, "Провести")]
		[Translate(LanguageIdConst.EN, "Accept")]
		ArbitrationDelayAccept,

		[ModuleCodeDescription(ModuleSubGroupCode.ArbitrationDelay, "Отклонить")]
		[Translate(LanguageIdConst.UZ_CYRL, "Рад этиш")]
		[Translate(LanguageIdConst.UZ_LATN, "Rad etish")]
		[Translate(LanguageIdConst.RU, "Отклонить")]
		[Translate(LanguageIdConst.EN, "Reject")]
		ArbitrationDelayReject,

		[ModuleCodeDescription(ModuleSubGroupCode.ArbitrationDelay, "Отменить")]
		[Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
		[Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
		[Translate(LanguageIdConst.RU, "Отменить")]
		[Translate(LanguageIdConst.EN, "Cancel")]
		ArbitrationDelayCancel,

		[ModuleCodeDescription(ModuleSubGroupCode.ArbitrationDelay, "Удалить")]
		[Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
		[Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
		[Translate(LanguageIdConst.RU, "Удалить")]
		[Translate(LanguageIdConst.EN, "Delete")]
		ArbitrationDelayDelete,

		[ModuleCodeDescription(ModuleSubGroupCode.ArbitrationDelay, "Отправлять")]
		[Translate(LanguageIdConst.UZ_CYRL, "Юбориш")]
		[Translate(LanguageIdConst.UZ_LATN, "Yuborish")]
		[Translate(LanguageIdConst.RU, "Отправлять")]
		[Translate(LanguageIdConst.EN, "Send")]
		ArbitrationDelaySend,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationDelay, "Подписать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзолаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolash")]
        [Translate(LanguageIdConst.RU, "Подписать")]
        [Translate(LanguageIdConst.EN, "Sign")]
        ArbitrationDelaySign,

        #endregion


        #region ArbitrationJudge
        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationJudge, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ArbitrationJudgeView,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationJudge, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ArbitrationJudgeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationJudge, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ArbitrationJudgeEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationJudge, "Провести")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўтказиш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'tkazish")]
        [Translate(LanguageIdConst.RU, "Провести")]
        [Translate(LanguageIdConst.EN, "Accept")]
        ArbitrationJudgeAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationJudge, "Отклонить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Рад этиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Rad etish")]
        [Translate(LanguageIdConst.RU, "Отклонить")]
        [Translate(LanguageIdConst.EN, "Reject")]
        ArbitrationJudgeReject,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationJudge, "Отменить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отменить")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        ArbitrationJudgeCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationJudge, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ArbitrationJudgeDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.ArbitrationJudge, "Отправлять")]
        [Translate(LanguageIdConst.UZ_CYRL, "Юбориш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yuborish")]
        [Translate(LanguageIdConst.RU, "Отправлять")]
        [Translate(LanguageIdConst.EN, "Send")]
        ArbitrationJudgeSend,
        #endregion

        #region SubsidyRequest
        [ModuleCodeDescription(ModuleSubGroupCode.SubsidyRequest, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        SubsidyRequestView,

        [ModuleCodeDescription(ModuleSubGroupCode.SubsidyRequest, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        SubsidyRequestCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.SubsidyRequest, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        SubsidyRequestEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.SubsidyRequest, "Провести")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўтказиш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'tkazish")]
        [Translate(LanguageIdConst.RU, "Провести")]
        [Translate(LanguageIdConst.EN, "Accept")]
        SubsidyRequestAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.SubsidyRequest, "Отклонить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Рад этиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Rad etish")]
        [Translate(LanguageIdConst.RU, "Отклонить")]
        [Translate(LanguageIdConst.EN, "Reject")]
        SubsidyRequestReject,

        [ModuleCodeDescription(ModuleSubGroupCode.SubsidyRequest, "Отменить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отменить")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        SubsidyRequestCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.SubsidyRequest, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        SubsidyRequestDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.SubsidyRequest, "Отправлять")]
        [Translate(LanguageIdConst.UZ_CYRL, "Юбориш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yuborish")]
        [Translate(LanguageIdConst.RU, "Отправлять")]
        [Translate(LanguageIdConst.EN, "Send")]
        SubsidyRequestSend,
        #endregion


        #region DocDualApplication
        [ModuleCodeDescription(ModuleSubGroupCode.DualApplication, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        DualApplicationView,

        [ModuleCodeDescription(ModuleSubGroupCode.DualApplication, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        DualApplicationCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.DualApplication, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        DualApplicationEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.DualApplication, "Провести")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўтказиш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'tkazish")]
        [Translate(LanguageIdConst.RU, "Провести")]
        [Translate(LanguageIdConst.EN, "Accept")]
        DualApplicationAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.DualApplication, "Отклонить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Рад этиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Rad etish")]
        [Translate(LanguageIdConst.RU, "Отклонить")]
        [Translate(LanguageIdConst.EN, "Reject")]
        DualApplicationReject,

        [ModuleCodeDescription(ModuleSubGroupCode.DualApplication, "Отменить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отменить")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        DualApplicationCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.DualApplication, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        DualApplicationDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.DualApplication, "Отправлять")]
        [Translate(LanguageIdConst.UZ_CYRL, "Юбориш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yuborish")]
        [Translate(LanguageIdConst.RU, "Отправлять")]
        [Translate(LanguageIdConst.EN, "Send")]
        DualApplicationSend,
        #endregion

        #region JoinAntiCorruptionApplication
        [ModuleCodeDescription(ModuleSubGroupCode.JoinAntiCorruptionApplication, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        JoinAntiCorruptionApplicationView,

        [ModuleCodeDescription(ModuleSubGroupCode.JoinAntiCorruptionApplication, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        JoinAntiCorruptionApplicationCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.JoinAntiCorruptionApplication, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        JoinAntiCorruptionApplicationEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.JoinAntiCorruptionApplication, "Провести")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўтказиш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'tkazish")]
        [Translate(LanguageIdConst.RU, "Провести")]
        [Translate(LanguageIdConst.EN, "Accept")]
        JoinAntiCorruptionApplicationAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.JoinAntiCorruptionApplication, "Отклонить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Рад этиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Rad etish")]
        [Translate(LanguageIdConst.RU, "Отклонить")]
        [Translate(LanguageIdConst.EN, "Reject")]
        JoinAntiCorruptionApplicationReject,

        [ModuleCodeDescription(ModuleSubGroupCode.JoinAntiCorruptionApplication, "Отменить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отменить")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        JoinAntiCorruptionApplicationCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.JoinAntiCorruptionApplication, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        JoinAntiCorruptionApplicationDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.JoinAntiCorruptionApplication, "Отправлять")]
        [Translate(LanguageIdConst.UZ_CYRL, "Юбориш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yuborish")]
        [Translate(LanguageIdConst.RU, "Отправлять")]
        [Translate(LanguageIdConst.EN, "Send")]
        JoinAntiCorruptionApplicationSend,
        #endregion

        #region PrtnRejectReason
        [ModuleCodeDescription(ModuleSubGroupCode.PrtnRejectReason, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        PrtnRejectReasonView,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnRejectReason, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        PrtnRejectReasonCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnRejectReason, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        PrtnRejectReasonEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnRejectReason, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        PrtnRejectReasonDelete,
        #endregion

        #region PrtnContract
        [ModuleCodeDescription(ModuleSubGroupCode.PrtnContract, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        PrtnContractView,


        [ModuleCodeDescription(ModuleSubGroupCode.PrtnContract, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        PrtnContractViewAll,


        [ModuleCodeDescription(ModuleSubGroupCode.PrtnContract, "Просмотр по регионам")]
        [Translate(LanguageIdConst.UZ_CYRL, "Вилоятлар бўйича кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Viloyatlar bo'yicha ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр по регионам")]
        [Translate(LanguageIdConst.EN, "View by region")]
        PrtnContractViewByRegion,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnContract, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        PrtnContractCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnContract, "Создать вручную")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қўлда яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qo'lda yaratish")]
        [Translate(LanguageIdConst.RU, "Создать вручную")]
        [Translate(LanguageIdConst.EN, "Create manually")]
        PrtnContractCreateManually, // Admin tomondan toxtab qogan arizalani CREATE qvorish uchun

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnContract, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        PrtnContractEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnContract, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        PrtnContractDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnContract, "Отозвать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қайтариб олиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qaytarib olish")]
        [Translate(LanguageIdConst.RU, "Отозвать")]
        [Translate(LanguageIdConst.EN, "Revoke")]
        PrtnContractRevoke,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnContract, "Отменить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отменить")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        PrtnContractCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnContract, "Подписать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзолаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolash")]
        [Translate(LanguageIdConst.RU, "Подписать")]
        [Translate(LanguageIdConst.EN, "Sign")]
        PrtnContractSign,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnContract, "Отклонить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Рад этиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Rad etish")]
        [Translate(LanguageIdConst.RU, "Отклонить")]
        [Translate(LanguageIdConst.EN, "Reject")]
        PrtnContractReject,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnContract, "Прошел экспертизу")]
        [Translate(LanguageIdConst.UZ_CYRL, "Экспертизадан ўтди")]
        [Translate(LanguageIdConst.UZ_LATN, "Ekspertizadan o'tdi")]
        [Translate(LanguageIdConst.RU, "Прошел экспертизу")]
        [Translate(LanguageIdConst.EN, "Passport expertise")]
        PrtnContractPassExpertise,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnContract, "Не прошел экспертизу")]
        [Translate(LanguageIdConst.UZ_CYRL, "Экспертизадан ўтмади")]
        [Translate(LanguageIdConst.UZ_LATN, "Ekspertizadan o'tdi")]
        [Translate(LanguageIdConst.RU, "Не прошел экспертизу")]
        [Translate(LanguageIdConst.EN, "Not passport expertise")]
        PrtnContractNotPassExpertise,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnContract, "PrtnContractReSendForExpertise")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ekspertizaga qayta yuborish")]
        [Translate(LanguageIdConst.UZ_LATN, "Ekspertizaga qayta yuborish")]
        [Translate(LanguageIdConst.RU, "Ekspertizaga qayta yuborish")]
        [Translate(LanguageIdConst.EN, "Re send for expertise")]
        PrtnContractReSendForExpertise,
        #endregion

        #region PrtnCertificate
        [ModuleCodeDescription(ModuleSubGroupCode.PrtnCertificate, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        PrtnCertificateView,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnCertificate, "Просмотр по регионам")]
        [Translate(LanguageIdConst.UZ_CYRL, "Вилоятлар бўйича кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Viloyatlar bo'yicha ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр по регионам")]
        [Translate(LanguageIdConst.EN, "View by region")]
        PrtnCertificateViewByRegion,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnCertificate, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        PrtnCertificateViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnCertificate, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        PrtnCertificateCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnCertificate, "Отменить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отменить")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        PrtnCertificateCancel,

        //[ModuleCodeDescription(ModuleSubGroupCode.PrtnCertificate, "Редактировать")]
        //[Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        //[Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        //[Translate(LanguageIdConst.RU, "Редактировать")]
        //[Translate(LanguageIdConst.EN, "Edit")]
        //PrtnCertificateEdit,

        //[ModuleCodeDescription(ModuleSubGroupCode.PrtnCertificate, "Удалить")]
        //[Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        //[Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        //[Translate(LanguageIdConst.RU, "Удалить")]
        //[Translate(LanguageIdConst.EN, "Delete")]
        //PrtnCertificateDelete,
        #endregion

        #region PrtnCreditDemand
        [ModuleCodeDescription(ModuleSubGroupCode.PrtnCreditDemand, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        PrtnCreditDemandView,

        //[ModuleCodeDescription(ModuleSubGroupCode.PrtnCreditDemand, "Просмотр по регионам")]
        //[Translate(LanguageIdConst.UZ_CYRL, "Вилоятлар бўйича кўриш")]
        //[Translate(LanguageIdConst.UZ_LATN, "Viloyatlar bo'yicha ko'rish")]
        //[Translate(LanguageIdConst.RU, "Просмотр по регионам")]
        //[Translate(LanguageIdConst.EN, "View by region")]
        //PrtnCreditDemandViewByRegion,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnCreditDemand, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        PrtnCreditDemandViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnCreditDemand, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        PrtnCreditDemandCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnCreditDemand, "Отменить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отменить")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        PrtnCreditDemandCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnCreditDemand, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        PrtnCreditDemandEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.PrtnCreditDemand, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        PrtnCreditDemandDelete,
        #endregion

        #region Application
        [ModuleCodeDescription(ModuleSubGroupCode.Application, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ApplicationView,

        [ModuleCodeDescription(ModuleSubGroupCode.Application, "Просмотр по регионам")]
        [Translate(LanguageIdConst.UZ_CYRL, "Вилоятлар бўйича кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Viloyatlar bo'yicha ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр по регионам")]
        [Translate(LanguageIdConst.EN, "View by region")]
        ApplicationViewByRegion,

        [ModuleCodeDescription(ModuleSubGroupCode.Application, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        ApplicationViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.Application, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ApplicationCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Application, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ApplicationEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Application, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ApplicationDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.Application, "Отклонить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Рад этиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Rad etish")]
        [Translate(LanguageIdConst.RU, "Отклонить")]
        [Translate(LanguageIdConst.EN, "Reject")]
        ApplicationReject,

        [ModuleCodeDescription(ModuleSubGroupCode.Application, "Провести")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўтказиш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'tkazish")]
        [Translate(LanguageIdConst.RU, "Провести")]
        [Translate(LanguageIdConst.EN, "Accept")]
        ApplicationAccept,
        #endregion

        #region Department
        [ModuleCodeDescription(ModuleSubGroupCode.Department, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        DepartmentView,

        [ModuleCodeDescription(ModuleSubGroupCode.Department, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        DepartmentViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.Department, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        DepartmentCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Department, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        DepartmentEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Department, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        DepartmentDelete,

        #endregion

        #region BusinessmanCard
        [ModuleCodeDescription(ModuleSubGroupCode.BusinessmanCard, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        BusinessmanCardView,

        [ModuleCodeDescription(ModuleSubGroupCode.BusinessmanCard, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        BusinessmanCardViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.BusinessmanCard, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        BusinessmanCardCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.BusinessmanCard, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        BusinessmanCardEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.BusinessmanCard, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        BusinessmanCardDelete,

        #endregion

        #region BusinessActivityType
        [ModuleCodeDescription(ModuleSubGroupCode.BusinessActivityType, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        BusinessActivityTypeView,

        #endregion

        #region Proposal
        [ModuleCodeDescription(ModuleSubGroupCode.Proposal, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ProposalView,

        [ModuleCodeDescription(ModuleSubGroupCode.Proposal, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        ProposalViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.Proposal, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ProposalCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Proposal, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ProposalEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Proposal, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ProposalDelete,

        #endregion

        #region Mediation
        [ModuleCodeDescription(ModuleSubGroupCode.Mediation, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        MediationView,

        [ModuleCodeDescription(ModuleSubGroupCode.Mediation, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        MediationViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.Mediation, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        MediationCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Mediation, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        MediationEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Mediation, "Провести")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўтказиш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'tkazish")]
        [Translate(LanguageIdConst.RU, "Провести")]
        [Translate(LanguageIdConst.EN, "Accept")]
        MediationAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.Mediation, "Отменить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отменить")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        MediationCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.Mediation, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        MediationDelete,
        #endregion

        #region ApplicationForCourt
        [ModuleCodeDescription(ModuleSubGroupCode.ApplicationForCourt, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ApplicationForCourtView,

        [ModuleCodeDescription(ModuleSubGroupCode.ApplicationForCourt, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        ApplicationForCourtViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.ApplicationForCourt, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ApplicationForCourtCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ApplicationForCourt, "Модератор искового документа")]
        [Translate(LanguageIdConst.UZ_CYRL, "Даъво аризаси(судга) ҳужжатининг модератори")]
        [Translate(LanguageIdConst.UZ_LATN, "Da'vo arizasi(sudga) hujjatining moderatori")]
        [Translate(LanguageIdConst.RU, "Модератор искового документа")]
        [Translate(LanguageIdConst.EN, "Moderator of the lawsuit document")]
        AcademicDegreeLawsuit,

        [ModuleCodeDescription(ModuleSubGroupCode.ApplicationForCourt, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ApplicationForCourtEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ApplicationForCourt, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ApplicationForCourtDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.ApplicationForCourt, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        ApplicationForCourtAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.ApplicationForCourt, "Отменить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отменить")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        ApplicationForCourtCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.ApplicationForCourt, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        ApplicationForCourtAcceptEmployee,
        #endregion



        #region IdentityDocument
        [ModuleCodeDescription(ModuleSubGroupCode.IdentityDocument, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        IdentityDocumentView,

        [ModuleCodeDescription(ModuleSubGroupCode.IdentityDocument, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        IdentityDocumentViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.IdentityDocument, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        IdentityDocumentCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.IdentityDocument, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        IdentityDocumentEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.IdentityDocument, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        IdentityDocumentDelete,

        #endregion

        #region RelativeDegree
        [ModuleCodeDescription(ModuleSubGroupCode.RelativeDegree, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        RelativeDegreeView,

        [ModuleCodeDescription(ModuleSubGroupCode.RelativeDegree, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        RelativeDegreeViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.RelativeDegree, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        RelativeDegreeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.RelativeDegree, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        RelativeDegreeEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.RelativeDegree, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        RelativeDegreeDelete,

        #endregion

        #region WorkSchedule
        [ModuleCodeDescription(ModuleSubGroupCode.WorkSchedule, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        WorkScheduleView,

        [ModuleCodeDescription(ModuleSubGroupCode.WorkSchedule, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        WorkScheduleViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.WorkSchedule, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        WorkScheduleCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.WorkSchedule, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        WorkScheduleEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.WorkSchedule, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        WorkScheduleDelete,

        #endregion

        #region ItemOfExpense
        [ModuleCodeDescription(ModuleSubGroupCode.ItemOfExpense, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        ItemOfExpenseViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.ItemOfExpense, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ItemOfExpenseView,

        [ModuleCodeDescription(ModuleSubGroupCode.ItemOfExpense, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ItemOfExpenseCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ItemOfExpense, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ItemOfExpenseEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ItemOfExpense, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ItemOfExpenseDelete,

        #endregion

        #region EmployeeManage
        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeManage, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        EmployeeManageView,
        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeManage, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        AllEmployeeManageView,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeManage, "Просмотр для документов")]
        [Translate(LanguageIdConst.UZ_CYRL, "Хужатлар учун кориш")]
        [Translate(LanguageIdConst.UZ_LATN, "Hujjatlar uchun ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр для документов")]
        [Translate(LanguageIdConst.EN, "For Other Employee Manage View")]
        ForOtherEmployeeManageView,
        #endregion

        #region CalculationKind
        [ModuleCodeDescription(ModuleSubGroupCode.CalculationKind, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        CalculationKindView,

        [ModuleCodeDescription(ModuleSubGroupCode.CalculationKind, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        CalculationKindViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.CalculationKind, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        CalculationKindCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.CalculationKind, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        CalculationKindEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.CalculationKind, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        CalculationKindDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.Signer, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        SignerView,

        #endregion

        #region AppointEmployee
        [ModuleCodeDescription(ModuleSubGroupCode.AppointEmployee, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        AppointEmployeeView,

        [ModuleCodeDescription(ModuleSubGroupCode.AppointEmployee, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        AppointEmployeeViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.AppointEmployee, "Просмотр (вышестоящий)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш (юқори турувчи)")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish (yuqori turuvchi)")]
        [Translate(LanguageIdConst.RU, "Просмотр (вышестоящий)")]
        [Translate(LanguageIdConst.EN, "Appoint Employee Header View")]
        AppointEmployeeHeaderView,

        [ModuleCodeDescription(ModuleSubGroupCode.AppointEmployee, "Просмотр (Подписавший)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш (Имзоловчи)")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish (Imzolovchi)")]
        [Translate(LanguageIdConst.RU, "Просмотр (Подписавший)")]
        [Translate(LanguageIdConst.EN, "Appoint Employee Signer View")]
        AppointEmployeeSignerView,

        [ModuleCodeDescription(ModuleSubGroupCode.AppointEmployee, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        AppointEmployeeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.AppointEmployee, "Создать (админ)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш (админ)")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish (admin)")]
        [Translate(LanguageIdConst.RU, "Создать (админ)")]
        [Translate(LanguageIdConst.EN, "Create (admin)")]
        AllAppointEmployeeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.AppointEmployee, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        AppointEmployeeEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.AppointEmployee, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        AppointEmployeeCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.AppointEmployee, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        AppointEmployeeAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.AppointEmployee, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        AppointEmployeeDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.AppointEmployee, "Подписать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзолаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolash")]
        [Translate(LanguageIdConst.RU, "Подписать")]
        [Translate(LanguageIdConst.EN, "Sign")]
        AppointEmployeeSign,

        [ModuleCodeDescription(ModuleSubGroupCode.AppointEmployee, "Без подписывающего")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзоловчисиз")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolovchisiz")]
        [Translate(LanguageIdConst.RU, "Без подписывающего")]
        [Translate(LanguageIdConst.EN, "Appoint Employee Without Signer")]
        AppointEmployeeWithoutSigner,

        [ModuleCodeDescription(ModuleSubGroupCode.AppointEmployee, "Подписать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзолаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolash")]
        [Translate(LanguageIdConst.RU, "Подпишите в любом случае")]
        [Translate(LanguageIdConst.EN, "SignAnyway")]
        AppointEmployeeSignAnyway,
        #endregion

        #region Timesheet
        [ModuleCodeDescription(ModuleSubGroupCode.Timesheet, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        TimesheetView,

        [ModuleCodeDescription(ModuleSubGroupCode.Timesheet, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        TimesheetViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.Timesheet, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        TimesheetCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Timesheet, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        TimesheetCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.Timesheet, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        TimesheetAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.Timesheet, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        TimesheetEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Timesheet, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        TimesheetDelete,
        #endregion

        #region Timesheet
        [ModuleCodeDescription(ModuleSubGroupCode.Integration, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        IntegrationView,
        #endregion

        #region EmployeeSendTrain
        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSendTrain, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        EmployeeSendTrainView,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSendTrain, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        EmployeeSendTrainViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSendTrain, "Просмотр (Подписавший)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш (Имзоловчи)")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish (Imzolovchi)")]
        [Translate(LanguageIdConst.RU, "Просмотр (Подписавший)")]
        [Translate(LanguageIdConst.EN, "Employee Send Train View")]
        EmployeeSendTrainSignerView,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSendTrain, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        EmployeeSendTrainCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSendTrain, "Создать (админ)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш (админ)")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish (admin)")]
        [Translate(LanguageIdConst.RU, "Создать (админ)")]
        [Translate(LanguageIdConst.EN, "Create (admin)")]
        AllEmployeeSendTrainCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSendTrain, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        EmployeeSendTrainEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSendTrain, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        EmployeeSendTrainDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSendTrain, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        EmployeeSendTrainCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSendTrain, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        EmployeeSendTrainAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSendTrain, "Подписать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзолаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolash")]
        [Translate(LanguageIdConst.RU, "Подписать")]
        [Translate(LanguageIdConst.EN, "Sign")]
        EmployeeSendTrainSign,

        #endregion

        #region EmployeeSendStudy
        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSendStudy, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        EmployeeSendStudyView,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSendStudy, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        EmployeeSendStudyViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSendStudy, "Просмотр (Подписавший)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш (Имзоловчи)")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish (Imzolovchi)")]
        [Translate(LanguageIdConst.RU, "Просмотр (Подписавший)")]
        [Translate(LanguageIdConst.EN, "Employee Send Train View")]
        EmployeeSendStudySignerView,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSendStudy, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        EmployeeSendStudyCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSendStudy, "Создать (админ)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш (админ)")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish (admin)")]
        [Translate(LanguageIdConst.RU, "Создать (админ)")]
        [Translate(LanguageIdConst.EN, "Create (admin)")]
        AllEmployeeSendStudyCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSendStudy, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        EmployeeSendStudyEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSendStudy, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        EmployeeSendStudyDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSendStudy, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        EmployeeSendStudyCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSendStudy, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        EmployeeSendStudyAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSendStudy, "Подписать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзолаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolash")]
        [Translate(LanguageIdConst.RU, "Подписать")]
        [Translate(LanguageIdConst.EN, "Sign")]
        EmployeeSendStudySign,

        #endregion

        #region EmployeeLeaveOrder
        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeLeaveOrder, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        EmployeeLeaveOrderView,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeLeaveOrder, "Просмотр (Подписавший)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш (Имзоловчи)")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish (Imzolovchi)")]
        [Translate(LanguageIdConst.RU, "Просмотр (Подписавший)")]
        [Translate(LanguageIdConst.EN, "Employee Leave Order Signer View")]
        EmployeeLeaveOrderSignerView,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeLeaveOrder, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        EmployeeLeaveOrderViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeLeaveOrder, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        EmployeeLeaveOrderCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeLeaveOrder, "Создать (админ)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш (админ)")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish (admin)")]
        [Translate(LanguageIdConst.RU, "Создать (админ)")]
        [Translate(LanguageIdConst.EN, "Create (admin)")]
        AllEmployeeLeaveOrderCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeLeaveOrder, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        EmployeeLeaveOrderEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeLeaveOrder, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        EmployeeLeaveOrderDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeLeaveOrder, "Подписать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзолаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolash")]
        [Translate(LanguageIdConst.RU, "Подписать")]
        [Translate(LanguageIdConst.EN, "Sign")]
        EmployeeLeaveOrderSign,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeLeaveOrder, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        EmployeeLeaveOrderCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeLeaveOrder, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        EmployeeLeaveOrderAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeLeaveOrder, "Без подписывающего")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзоловчисиз")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolovchisiz")]
        [Translate(LanguageIdConst.RU, "Без подписывающего")]
        [Translate(LanguageIdConst.EN, "Appoint Employee Without Signer")]
        EmployeeLeaveOrderWithoutSigner,
        #endregion

        #region Chastisement
        [ModuleCodeDescription(ModuleSubGroupCode.Chastisement, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ChastisementView,

        [ModuleCodeDescription(ModuleSubGroupCode.Chastisement, "Просмотр (Подписавший)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш (Имзоловчи)")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish (Imzolovchi)")]
        [Translate(LanguageIdConst.RU, "Просмотр (Подписавший)")]
        [Translate(LanguageIdConst.EN, "Employee Leave Order Signer View")]
        ChastisementSignerView,

        [ModuleCodeDescription(ModuleSubGroupCode.Chastisement, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        ChastisementViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.Chastisement, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ChastisementCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Chastisement, "Создать (админ)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш (админ)")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish (admin)")]
        [Translate(LanguageIdConst.RU, "Создать (админ)")]
        [Translate(LanguageIdConst.EN, "Create (admin)")]
        AllChastisementCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Chastisement, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ChastisementEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Chastisement, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ChastisementDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.Chastisement, "Подписать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзолаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolash")]
        [Translate(LanguageIdConst.RU, "Подписать")]
        [Translate(LanguageIdConst.EN, "Sign")]
        ChastisementSign,

        [ModuleCodeDescription(ModuleSubGroupCode.Chastisement, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        ChastisementCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.Chastisement, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        ChastisementAccept,
        #endregion

        #region CandidatesConfirmation
        [ModuleCodeDescription(ModuleSubGroupCode.CandidatesConfirmation, "View Heade")]
        [Translate(LanguageIdConst.UZ_CYRL, "View Heade")]
        [Translate(LanguageIdConst.UZ_LATN, "View Heade")]
        [Translate(LanguageIdConst.RU, "View Heade")]
        [Translate(LanguageIdConst.EN, "View Heade")]
        CandidatesConfirmationViewHeader,

        [ModuleCodeDescription(ModuleSubGroupCode.CandidatesConfirmation, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        CandidatesConfirmationView,

        [ModuleCodeDescription(ModuleSubGroupCode.CandidatesConfirmation, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        CandidatesConfirmationCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.CandidatesConfirmation, "Создать (админ)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш (админ)")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish (admin)")]
        [Translate(LanguageIdConst.RU, "Создать (админ)")]
        [Translate(LanguageIdConst.EN, "Create (admin)")]
        AllCandidatesConfirmationCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.CandidatesConfirmation, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        CandidatesConfirmationEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.CandidatesConfirmation, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        CandidatesConfirmationDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.CandidatesConfirmation, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        CandidatesConfirmationCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.CandidatesConfirmation, "Отправлять")]
        [Translate(LanguageIdConst.UZ_CYRL, "Юбориш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yuborish")]
        [Translate(LanguageIdConst.RU, "Отправлять")]
        [Translate(LanguageIdConst.EN, "Send")]
        CandidatesConfirmationSend,

        [ModuleCodeDescription(ModuleSubGroupCode.CandidatesConfirmation, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        CandidatesConfirmationAccept,
        #endregion

        #region TaxBenefit
        [ModuleCodeDescription(ModuleSubGroupCode.TaxBenefit, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        TaxBenefitView,

        [ModuleCodeDescription(ModuleSubGroupCode.TaxBenefit, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        TaxBenefitViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.TaxBenefit, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        TaxBenefitCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.TaxBenefit, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        TaxBenefitEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.TaxBenefit, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        TaxBenefitCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.TaxBenefit, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        TaxBenefitAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.TaxBenefit, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        TaxBenefitDelete,
        #endregion

        #region MassPlannedCalculation
        [ModuleCodeDescription(ModuleSubGroupCode.MassPlannedCalculation, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        MassPlannedCalculationView,

        [ModuleCodeDescription(ModuleSubGroupCode.MassPlannedCalculation, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        MassPlannedCalculationViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.MassPlannedCalculation, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        MassPlannedCalculationCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.MassPlannedCalculation, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        MassPlannedCalculationEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.MassPlannedCalculation, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        MassPlannedCalculationDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.MassPlannedCalculation, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        MassPlannedCalculationCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.MassPlannedCalculation, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        MassPlannedCalculationAccept,
        #endregion

        #region MemshipCertificate
        [ModuleCodeDescription(ModuleSubGroupCode.MemshipCertificate, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        MemshipCertificateView,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipCertificate, "Проверка категории предприятия")]
        [Translate(LanguageIdConst.UZ_CYRL, "Корхона тоифасини текшириш")]
        [Translate(LanguageIdConst.UZ_LATN, "Korxona toifasini tekshirish")]
        [Translate(LanguageIdConst.RU, "Проверка категории предприятия")]
        [Translate(LanguageIdConst.EN, "View")]
        MemshipCertificateCheckFromSoliq,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipCertificate, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        MemshipCertificateViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipCertificate, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        MemshipCertificateCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipCertificate, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        MemshipCertificateEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipCertificate, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        MemshipCertificateDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipCertificate, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        MemshipCertificateCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipCertificate, "Продление срока действия членского свидетельство")]
        [Translate(LanguageIdConst.UZ_CYRL, "Аъзолик гувоҳномаси маддатини узайтириш")]
        [Translate(LanguageIdConst.UZ_LATN, "A'zolik guvohnomasi maddatini uzaytirish")]
        [Translate(LanguageIdConst.RU, "Продление срока действия членского свидетельство")]
        [Translate(LanguageIdConst.EN, "Memship Certificate Prolong")]
        MemshipCertificateProlong,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipCertificate, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        MemshipCertificateAccept,
        #endregion
        

        #region ContractorCategoryCriterion
        [ModuleCodeDescription(ModuleSubGroupCode.ContractorCategoryCriterion, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ContractorCategoryCriterionView,

        [ModuleCodeDescription(ModuleSubGroupCode.ContractorCategoryCriterion, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        ContractorCategoryCriterionViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.ContractorCategoryCriterion, "Просмотр по регионам")]
        [Translate(LanguageIdConst.UZ_CYRL, "Вилоятлар бўйича кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Viloyatlar bo'yicha ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр по регионам")]
        [Translate(LanguageIdConst.EN, "View by region")]
        ContractorCategoryCriterionByRegion,

        [ModuleCodeDescription(ModuleSubGroupCode.ContractorCategoryCriterion, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ContractorCategoryCriterionCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ContractorCategoryCriterion, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ContractorCategoryCriterionEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ContractorCategoryCriterion, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ContractorCategoryCriterionDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.ContractorCategoryCriterion, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        ContractorCategoryCriterionCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.ContractorCategoryCriterion, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        ContractorCategoryCriterionAccept,
		#endregion

		#region MemshipContract
		[ModuleCodeDescription(ModuleSubGroupCode.MemshipContract, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        MemshipContractView,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipContract, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        MemshipContractViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipContract, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        MemshipContractCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipContract, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        MemshipContractEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipContract, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        MemshipContractDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipContract, "Перевод членского договора на платный")]
        [Translate(LanguageIdConst.UZ_CYRL, "Аъзолик шартномасини пулликга ўтказиш")]
        [Translate(LanguageIdConst.UZ_LATN, "A'zolik shartnomasini pullikga o'tkazish")]
        [Translate(LanguageIdConst.RU, "Перевод членского договора на платный")]
        [Translate(LanguageIdConst.EN, "Change Contractor To Pad")]
        ChangeContractorToPad,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipContract, "Редактировать соглашение о членстве")]
        [Translate(LanguageIdConst.UZ_CYRL, "Аъзолик шартномасини таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "A'zolik shartnomasini tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать соглашение о членстве")]
        [Translate(LanguageIdConst.EN, "Change Contractor Parametirs")]
        ChangeContractorParametirs,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipContract, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        MemshipContractCancel,


        [ModuleCodeDescription(ModuleSubGroupCode.MemshipContract, "Подписать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзолаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolash")]
        [Translate(LanguageIdConst.RU, "Подписать")]
        [Translate(LanguageIdConst.EN, "Sign")]
        MemshipContractSign,


        [ModuleCodeDescription(ModuleSubGroupCode.MemshipContract, "Отклонить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Рад этиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Rad etish")]
        [Translate(LanguageIdConst.RU, "Отклонить")]
        [Translate(LanguageIdConst.EN, "Reject")]
        MemshipContractReject,
        #endregion

        #region TempCalcKind
        [ModuleCodeDescription(ModuleSubGroupCode.TempCalcKind, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        TempCalcKindView,

        [ModuleCodeDescription(ModuleSubGroupCode.TempCalcKind, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        TempCalcKindViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.TempCalcKind, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        TempCalcKindCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.TempCalcKind, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        TempCalcKindEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.TempCalcKind, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        TempCalcKindDelete,


        [ModuleCodeDescription(ModuleSubGroupCode.TempCalcKind, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        TempCalcKindCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.TempCalcKind, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        TempCalcKindAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.TempCalcKind, "Подписать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзолаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolash")]
        [Translate(LanguageIdConst.RU, "Подписать")]
        [Translate(LanguageIdConst.EN, "Sign")]
        TempCalcKindSign,
        #endregion

        #region WorkDayOff
        [ModuleCodeDescription(ModuleSubGroupCode.WorkDayOff, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        WorkDayOffView,

        [ModuleCodeDescription(ModuleSubGroupCode.WorkDayOff, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        WorkDayOffViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.WorkDayOff, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        WorkDayOffCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.WorkDayOff, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        WorkDayOffEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.WorkDayOff, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        WorkDayOffCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.WorkDayOff, "Подписать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзолаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolash")]
        [Translate(LanguageIdConst.RU, "Подписать")]
        [Translate(LanguageIdConst.EN, "Sign")]
        WorkDayOffSign,

        [ModuleCodeDescription(ModuleSubGroupCode.WorkDayOff, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        WorkDayOffDelete,
        #endregion

        #region RecallLeave
        [ModuleCodeDescription(ModuleSubGroupCode.RecallLeave, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        RecallLeaveView,

        [ModuleCodeDescription(ModuleSubGroupCode.RecallLeave, "Просмотр (Подписавший)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш (Имзоловчи)")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish (Imzolovchi)")]
        [Translate(LanguageIdConst.RU, "Просмотр (Подписавший)")]
        [Translate(LanguageIdConst.EN, "Recall Leave View")]
        RecallLeaveSignerView,

        [ModuleCodeDescription(ModuleSubGroupCode.RecallLeave, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        RecallLeaveViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.RecallLeave, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        RecallLeaveCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.RecallLeave, "Создать (админ)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш (админ)")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish (admin)")]
        [Translate(LanguageIdConst.RU, "Создать (админ)")]
        [Translate(LanguageIdConst.EN, "Create (admin)")]
        AllRecallLeaveCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.RecallLeave, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        RecallLeaveEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.RecallLeave, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        RecallLeaveCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.RecallLeave, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        RecallLeaveAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.RecallLeave, "Подписать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзолаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolash")]
        [Translate(LanguageIdConst.RU, "Подписать")]
        [Translate(LanguageIdConst.EN, "Sign")]
        RecallLeaveSign,

        [ModuleCodeDescription(ModuleSubGroupCode.RecallLeave, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        RecallLeaveDelete,
        #endregion

        #region EmployeeSickLeave
        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSickLeave, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        EmployeeSickLeaveView,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSickLeave, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        EmployeeSickLeaveViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSickLeave, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        EmployeeSickLeaveCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSickLeave, "Создать (админ)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш (админ)")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish (admin)")]
        [Translate(LanguageIdConst.RU, "Создать (админ)")]
        [Translate(LanguageIdConst.EN, "Create (admin)")]
        AllEmployeeSickLeaveCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSickLeave, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        EmployeeSickLeaveEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSickLeave, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        EmployeeSickLeaveDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSickLeave, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        EmployeeSickLeaveCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeSickLeave, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        EmployeeSickLeaveAccept,
        #endregion

        #region PlannedCalculation
        [ModuleCodeDescription(ModuleSubGroupCode.PlannedCalculation, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        PlannedCalculationView,

        [ModuleCodeDescription(ModuleSubGroupCode.PlannedCalculation, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        PlannedCalculationViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.PlannedCalculation, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        PlannedCalculationCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.PlannedCalculation, "Создать (админ)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш (админ)")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish (admin)")]
        [Translate(LanguageIdConst.RU, "Создать (админ)")]
        [Translate(LanguageIdConst.EN, "Create (admin)")]
        AllPlannedCalculationCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.PlannedCalculation, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        PlannedCalculationEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.PlannedCalculation, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        PlannedCalculationDelete,
        #endregion

        #region TaxBenefitType
        [ModuleCodeDescription(ModuleSubGroupCode.TaxBenefitType, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        TaxBenefitTypeView,

        [ModuleCodeDescription(ModuleSubGroupCode.TaxBenefitType, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        TaxBenefitTypeViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.TaxBenefitType, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        TaxBenefitTypeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.TaxBenefitType, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        TaxBenefitTypeEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.TaxBenefitType, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        TaxBenefitTypeDelete,
        #endregion

        #region FixedMinimumValue
        [ModuleCodeDescription(ModuleSubGroupCode.FixedMinimumValue, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        FixedMinimumValueView,

        [ModuleCodeDescription(ModuleSubGroupCode.FixedMinimumValue, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        FixedMinimumValueViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.FixedMinimumValue, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        FixedMinimumValueCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.FixedMinimumValue, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        FixedMinimumValueEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.FixedMinimumValue, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        FixedMinimumValueDelete,
        #endregion 

        #region TariffScale
        [ModuleCodeDescription(ModuleSubGroupCode.TariffScale, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        TariffScaleView,

        [ModuleCodeDescription(ModuleSubGroupCode.TariffScale, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        TariffScaleViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.TariffScale, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        TariffScaleCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.TariffScale, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        TariffScaleEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.TariffScale, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        TariffScaleDelete,
        #endregion

        #region SettlementAccountSource
        [ModuleCodeDescription(ModuleSubGroupCode.SettlementAccountSource, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        SettlementAccountSourceView,

        [ModuleCodeDescription(ModuleSubGroupCode.SettlementAccountSource, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        SettlementAccountSourceViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.SettlementAccountSource, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        SettlementAccountSourceCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.SettlementAccountSource, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        SettlementAccountSourceEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.SettlementAccountSource, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        SettlementAccountSourceDelete,
        #endregion

        #region TariffScaleCoef
        [ModuleCodeDescription(ModuleSubGroupCode.TariffScaleCoef, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        TariffScaleCoefView,

        [ModuleCodeDescription(ModuleSubGroupCode.TariffScaleCoef, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        TariffScaleCoefViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.TariffScaleCoef, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        TariffScaleCoefCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.TariffScaleCoef, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        TariffScaleCoefEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.TariffScaleCoef, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        TariffScaleCoefDelete,
        #endregion

        #region StaffTypeBasicTariff
        [ModuleCodeDescription(ModuleSubGroupCode.StaffTypeBasicTariff, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        StaffTypeBasicTariffView,

        [ModuleCodeDescription(ModuleSubGroupCode.StaffTypeBasicTariff, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        StaffTypeBasicTariffViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.StaffTypeBasicTariff, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        StaffTypeBasicTariffCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.StaffTypeBasicTariff, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        StaffTypeBasicTariffEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.StaffTypeBasicTariff, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        StaffTypeBasicTariffDelete,
        #endregion

        #region SourceCode
        [ModuleCodeDescription(ModuleSubGroupCode.SourceCode, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        SourceCodeView,

        [ModuleCodeDescription(ModuleSubGroupCode.SourceCode, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        SourceCodeViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.SourceCode, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        SourceCodeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.SourceCode, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        SourceCodeEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.SourceCode, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        SourceCodeDelete,
        #endregion

        #region AppealTypeArrive
        [ModuleCodeDescription(ModuleSubGroupCode.AppealTypeArrive, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        AppealTypeArriveView,

        [ModuleCodeDescription(ModuleSubGroupCode.AppealTypeArrive, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        AppealTypeArriveCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.AppealTypeArrive, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        AppealTypeArriveEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.AppealTypeArrive, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        AppealTypeArriveDelete,
        #endregion

        #region AppealDescription
        [ModuleCodeDescription(ModuleSubGroupCode.AppealDescription, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        AppealDescriptionView,

        [ModuleCodeDescription(ModuleSubGroupCode.AppealDescription, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        AppealDescriptionCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.AppealDescription, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        AppealDescriptionEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.AppealDescription, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        AppealDescriptionDelete,
        #endregion

        #region LevelCode
        [ModuleCodeDescription(ModuleSubGroupCode.LevelCode, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        LevelCodeView,

        [ModuleCodeDescription(ModuleSubGroupCode.LevelCode, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        LevelCodeViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.LevelCode, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        LevelCodeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.LevelCode, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        LevelCodeEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.LevelCode, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        LevelCodeDelete,
        #endregion

        #region StaffingIndicator
        [ModuleCodeDescription(ModuleSubGroupCode.StaffingIndicator, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        StaffingIndicatorView,

        [ModuleCodeDescription(ModuleSubGroupCode.StaffingIndicator, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        StaffingIndicatorCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.StaffingIndicator, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        StaffingIndicatorEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.StaffingIndicator, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        StaffingIndicatorDelete,
        #endregion

        #region PositionCategory
        [ModuleCodeDescription(ModuleSubGroupCode.PositionCategory, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        PositionCategoryView,

        [ModuleCodeDescription(ModuleSubGroupCode.PositionCategory, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        PositionCategoryViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.PositionCategory, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        PositionCategoryCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.PositionCategory, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        PositionCategoryEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.PositionCategory, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        PositionCategoryDelete,
        #endregion

        #region PositionClassification
        [ModuleCodeDescription(ModuleSubGroupCode.PositionClassification, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        PositionClassificationView,

        [ModuleCodeDescription(ModuleSubGroupCode.PositionClassification, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        PositionClassificationViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.PositionClassification, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        PositionClassificationCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.PositionClassification, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        PositionClassificationEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.PositionClassification, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        PositionClassificationDelete,
        #endregion

        #region PositionType
        [ModuleCodeDescription(ModuleSubGroupCode.PositionType, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        PositionTypeView,

        [ModuleCodeDescription(ModuleSubGroupCode.PositionType, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        PositionTypeViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.PositionType, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        PositionTypeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.PositionType, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        PositionTypeEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.PositionType, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        PositionTypeDelete,
        #endregion


        #region AcademicDegree
        [ModuleCodeDescription(ModuleSubGroupCode.AcademicDegree, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        AcademicDegreeView,

        [ModuleCodeDescription(ModuleSubGroupCode.AcademicDegree, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        AcademicDegreeViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.AcademicDegree, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        AcademicDegreeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.AcademicDegree, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        AcademicDegreeEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.AcademicDegree, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        AcademicDegreeDelete,
        #endregion

        #region DegreeTitle
        [ModuleCodeDescription(ModuleSubGroupCode.DegreeTitle, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        DegreeTitleView,

        [ModuleCodeDescription(ModuleSubGroupCode.DegreeTitle, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        DegreeTitleViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.DegreeTitle, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        DegreeTitleCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.DegreeTitle, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        DegreeTitleEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.DegreeTitle, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        DegreeTitleDelete,
        #endregion

        #region ElectionMember
        [ModuleCodeDescription(ModuleSubGroupCode.ElectionMember, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ElectionMemberView,

        [ModuleCodeDescription(ModuleSubGroupCode.ElectionMember, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        ElectionMemberViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.ElectionMember, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ElectionMemberCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ElectionMember, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ElectionMemberEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ElectionMember, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ElectionMemberDelete,
        #endregion

        #region EducationItem
        [ModuleCodeDescription(ModuleSubGroupCode.EducationItem, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        EducationItemView,

        [ModuleCodeDescription(ModuleSubGroupCode.EducationItem, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        EducationItemAll,

        [ModuleCodeDescription(ModuleSubGroupCode.EducationItem, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        EducationItemCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.EducationItem, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        EducationItemEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.EducationItem, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        EducationItemDelete,
        #endregion

        #region LanguageProficiency
        [ModuleCodeDescription(ModuleSubGroupCode.LanguageProficiency, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        LanguageProficiencyView,

        [ModuleCodeDescription(ModuleSubGroupCode.LanguageProficiency, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        LanguageProficiencyViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.LanguageProficiency, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        LanguageProficiencyCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.LanguageProficiency, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        LanguageProficiencyEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.LanguageProficiency, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        LanguageProficiencyDelete,
        #endregion


        #region ScientificDegree
        [ModuleCodeDescription(ModuleSubGroupCode.ScientificDegree, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ScientificDegreeView,

        [ModuleCodeDescription(ModuleSubGroupCode.ScientificDegree, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        ScientificDegreeViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.ScientificDegree, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ScientificDegreeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ScientificDegree, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ScientificDegreeEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ScientificDegree, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ScientificDegreeDelete,
        #endregion

        #region StateAward
        [ModuleCodeDescription(ModuleSubGroupCode.StateAward, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        StateAwardView,

        [ModuleCodeDescription(ModuleSubGroupCode.StateAward, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        StateAwardViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.StateAward, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        StateAwardCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.StateAward, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        StateAwardEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.StateAward, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        StateAwardDelete,
        #endregion

        #region MilitaryRank
        [ModuleCodeDescription(ModuleSubGroupCode.MilitaryRank, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        MilitaryRankView,

        [ModuleCodeDescription(ModuleSubGroupCode.MilitaryRank, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        MilitaryRankViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.MilitaryRank, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        MilitaryRankCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.MilitaryRank, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        MilitaryRankEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.MilitaryRank, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        MilitaryRankDelete,
        #endregion

        #region Currency
        [ModuleCodeDescription(ModuleSubGroupCode.Currency, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        CurrencyView,

        [ModuleCodeDescription(ModuleSubGroupCode.Currency, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        CurrencyCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Currency, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        CurrencyEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Currency, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        CurrencyDelete,
        #endregion

        #region StaffingTemplate
        [ModuleCodeDescription(ModuleSubGroupCode.StaffingTemplate, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        StaffingTemplateView,

        [ModuleCodeDescription(ModuleSubGroupCode.StaffingTemplate, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        StaffingTemplateCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.StaffingTemplate, "Создать (админ)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш (админ)")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish (admin)")]
        [Translate(LanguageIdConst.RU, "Создать (админ)")]
        [Translate(LanguageIdConst.EN, "Create (admin)")]
        AllStaffingTemplateCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.StaffingTemplate, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        StaffingTemplateEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.StaffingTemplate, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        StaffingTemplateCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.StaffingTemplate, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        StaffingTemplateAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.StaffingTemplate, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        StaffingTemplateDelete,
        #endregion

        #region Staffing
        [ModuleCodeDescription(ModuleSubGroupCode.Staffing, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        StaffingView,

        [ModuleCodeDescription(ModuleSubGroupCode.Staffing, "Просмотр (вышестоящий)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш (юқори турувчи)")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish (yuqori turuvchi)")]
        [Translate(LanguageIdConst.RU, "Просмотр (вышестоящий)")]
        [Translate(LanguageIdConst.EN, "Staffing Header View")]
        StaffingHeaderView,

        [ModuleCodeDescription(ModuleSubGroupCode.Staffing, "Просмотр (Подведомственное учреждение)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш (Бўйсунувчи муассаса)")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish (Bo'ysunuvchi muassasa )")]
        [Translate(LanguageIdConst.RU, "Просмотр (Подведомственное учреждение)")]
        [Translate(LanguageIdConst.EN, "Staffing For Own OrgView")]
        StaffingForOwnOrgView,

        [ModuleCodeDescription(ModuleSubGroupCode.Staffing, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        StaffingCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Staffing, "Создать (админ)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш (админ)")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish (admin)")]
        [Translate(LanguageIdConst.RU, "Создать (админ)")]
        [Translate(LanguageIdConst.EN, "Create (admin)")]
        AllStaffingCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Staffing, "Клонировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Клонлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Klonlash")]
        [Translate(LanguageIdConst.RU, "Клонировать")]
        [Translate(LanguageIdConst.EN, "Clone")]
        StaffingClone,

        [ModuleCodeDescription(ModuleSubGroupCode.Staffing, "Архивирование")]
        [Translate(LanguageIdConst.UZ_CYRL, "Архивлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Arxivlash")]
        [Translate(LanguageIdConst.RU, "Архивирование")]
        [Translate(LanguageIdConst.EN, "Archive")]
        StaffingArchive,

        [ModuleCodeDescription(ModuleSubGroupCode.Staffing, "Разархивировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Архивдан чиқариш")]
        [Translate(LanguageIdConst.UZ_LATN, "Arxivdan chiqarish")]
        [Translate(LanguageIdConst.RU, "Разархивировать")]
        [Translate(LanguageIdConst.EN, "ReArchive")]
        StaffingReArchive,

        [ModuleCodeDescription(ModuleSubGroupCode.Staffing, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        StaffingEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Staffing, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        StaffingDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.Staffing, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        StaffingCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.Staffing, "Отправлять")]
        [Translate(LanguageIdConst.UZ_CYRL, "Юбориш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yuborish")]
        [Translate(LanguageIdConst.RU, "Отправлять")]
        [Translate(LanguageIdConst.EN, "Send")]
        StaffingSend,

        [ModuleCodeDescription(ModuleSubGroupCode.Staffing, "Отозвать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қайтариб олиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qaytarib olish")]
        [Translate(LanguageIdConst.RU, "Отозвать")]
        [Translate(LanguageIdConst.EN, "Revoke")]
        StaffingRevoke,

        [ModuleCodeDescription(ModuleSubGroupCode.Staffing, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Receieved")]
        StaffingReceieved,

        [ModuleCodeDescription(ModuleSubGroupCode.Staffing, "Отклонить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Рад этиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Rad etish")]
        [Translate(LanguageIdConst.RU, "Отклонить")]
        [Translate(LanguageIdConst.EN, "Reject")]
        StaffingReject,

        [ModuleCodeDescription(ModuleSubGroupCode.Staffing, "Принят")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилинган")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilingan")]
        [Translate(LanguageIdConst.RU, "Принят")]
        [Translate(LanguageIdConst.EN, "Accept")]
        StaffingAccept,
        #endregion

        #region OrderToSendBusinessTrip
        [ModuleCodeDescription(ModuleSubGroupCode.OrderToSendBusinessTrip, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        OrderToSendBusinessTripView,

        [ModuleCodeDescription(ModuleSubGroupCode.OrderToSendBusinessTrip, "Просмотр (Подписавший)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш (Имзоловчи)")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish (Imzolovchi)")]
        [Translate(LanguageIdConst.RU, "Просмотр (Подписавший)")]
        [Translate(LanguageIdConst.EN, "Order To Send Business Trip View")]
        OrderToSendBusinessTripSignerView,

        [ModuleCodeDescription(ModuleSubGroupCode.OrderToSendBusinessTrip, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        OrderToSendBusinessTripViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.OrderToSendBusinessTrip, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        OrderToSendBusinessTripCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.OrderToSendBusinessTrip, "Создать (админ)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш (админ)")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish (admin)")]
        [Translate(LanguageIdConst.RU, "Создать (админ)")]
        [Translate(LanguageIdConst.EN, "Create (admin)")]
        AllOrderToSendBusinessTripCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.OrderToSendBusinessTrip, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        OrderToSendBusinessTripEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.OrderToSendBusinessTrip, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        OrderToSendBusinessTripDelete,


        [ModuleCodeDescription(ModuleSubGroupCode.OrderToSendBusinessTrip, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        OrderToSendBusinessTripCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.OrderToSendBusinessTrip, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        OrderToSendBusinessTripAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.OrderToSendBusinessTrip, "Подписать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзолаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolash")]
        [Translate(LanguageIdConst.RU, "Подписать")]
        [Translate(LanguageIdConst.EN, "Sign")]
        OrderToSendBusinessTripSign,
        #endregion

        #region AppError
        [ModuleCodeDescription(ModuleSubGroupCode.AppError, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        AppErrorView,
        #endregion

        #region QualificationCategory
        [ModuleCodeDescription(ModuleSubGroupCode.QualificationCategory, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        QualificationCategoryView,

        [ModuleCodeDescription(ModuleSubGroupCode.QualificationCategory, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        QualificationCategoryCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.QualificationCategory, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        QualificationCategoryEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.QualificationCategory, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        QualificationCategoryDelete,
        #endregion

        #region ContractorActivityGroup
        [ModuleCodeDescription(ModuleSubGroupCode.ContractorActivityGroup, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ContractorActivityGroupView,

        [ModuleCodeDescription(ModuleSubGroupCode.ContractorActivityGroup, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ContractorActivityGroupCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ContractorActivityGroup, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ContractorActivityGroupEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ContractorActivityGroup, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ContractorActivityGroupDelete,
        #endregion

        #region ContractorActivityType
        [ModuleCodeDescription(ModuleSubGroupCode.ContractorActivityType, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ContractorActivityTypeView,

        [ModuleCodeDescription(ModuleSubGroupCode.ContractorActivityType, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ContractorActivityTypeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ContractorActivityType, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ContractorActivityTypeEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ContractorActivityType, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ContractorActivityTypeDelete,
        #endregion

        #region ContractorRating
        [ModuleCodeDescription(ModuleSubGroupCode.ContractorRating, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ContractorRatingView,

        [ModuleCodeDescription(ModuleSubGroupCode.ContractorRating, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ContractorRatingCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ContractorRating, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ContractorRatingEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ContractorRating, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ContractorRatingDelete,
        #endregion
        #region ContractorUnionActivityType
        [ModuleCodeDescription(ModuleSubGroupCode.ContractorUnionActivityType, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ContractorUnionActivityTypeView,

        [ModuleCodeDescription(ModuleSubGroupCode.ContractorUnionActivityType, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ContractorUnionActivityTypeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ContractorUnionActivityType, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ContractorUnionActivityTypeEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ContractorUnionActivityType, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ContractorUnionActivityTypeDelete,
        #endregion

        #region NeedChamberService

        [ModuleCodeDescription(ModuleSubGroupCode.NeedChamberService, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        NeedChamberServiceView,

        [ModuleCodeDescription(ModuleSubGroupCode.NeedChamberService, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        NeedChamberServiceCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.NeedChamberService, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        NeedChamberServiceEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.NeedChamberService, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        NeedChamberServiceDelete,

        ///////// G R O U P /////////
        [ModuleCodeDescription(ModuleSubGroupCode.NeedChamberService, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        NeedChamberServiceGroupView,

        [ModuleCodeDescription(ModuleSubGroupCode.NeedChamberService, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        NeedChamberServiceGroupCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.NeedChamberService, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        NeedChamberServiceGroupEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.NeedChamberService, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        NeedChamberServiceGroupDelete,
        #endregion

        #region ClaimTheme
        [ModuleCodeDescription(ModuleSubGroupCode.ClaimTheme, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ClaimThemeView,

        [ModuleCodeDescription(ModuleSubGroupCode.ClaimTheme, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ClaimThemeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ClaimTheme, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ClaimThemeEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ClaimTheme, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ClaimThemeDelete,
        #endregion

        #region ClaimOrganizationType
        [ModuleCodeDescription(ModuleSubGroupCode.ClaimOrganizationType, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ClaimOrganizationTypeView,

        [ModuleCodeDescription(ModuleSubGroupCode.ClaimOrganizationType, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ClaimOrganizationTypeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ClaimOrganizationType, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ClaimOrganizationTypeEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ClaimOrganizationType, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ClaimOrganizationTypeDelete,
        #endregion

        #region ClaimOrganization
        [ModuleCodeDescription(ModuleSubGroupCode.ClaimOrganization, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ClaimOrganizationView,

        [ModuleCodeDescription(ModuleSubGroupCode.ClaimOrganization, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ClaimOrganizationCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ClaimOrganization, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ClaimOrganizationEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ClaimOrganization, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ClaimOrganizationDelete,
        #endregion

        #region MediationPlan
        [ModuleCodeDescription(ModuleSubGroupCode.MediationPlan, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        MediationPlanView,

        [ModuleCodeDescription(ModuleSubGroupCode.MediationPlan, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        MediationPlanViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.MediationPlan, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        MediationPlanCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.MediationPlan, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        MediationPlanEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.MediationPlan, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        MediationPlanCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.MediationPlan, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        MediationPlanAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.MediationPlan, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        MediationPlanDelete,
        #endregion

        #region JoinAntiCorruptionResult
        [ModuleCodeDescription(ModuleSubGroupCode.JoinAntiCorruptionResult, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        JoinAntiCorruptionResultView,

        [ModuleCodeDescription(ModuleSubGroupCode.JoinAntiCorruptionResult, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        JoinAntiCorruptionResultViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.JoinAntiCorruptionResult, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        JoinAntiCorruptionResultCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.JoinAntiCorruptionResult, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        JoinAntiCorruptionResultEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.JoinAntiCorruptionResult, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        JoinAntiCorruptionResultCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.JoinAntiCorruptionResult, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        JoinAntiCorruptionResultAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.JoinAntiCorruptionResult, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        JoinAntiCorruptionResultDelete,
        #endregion

        #region JoinAntiCorruptionCertificate
        [ModuleCodeDescription(ModuleSubGroupCode.JoinAntiCorruptionCertificate, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        JoinAntiCorruptionCertificateView,

        [ModuleCodeDescription(ModuleSubGroupCode.JoinAntiCorruptionCertificate, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        JoinAntiCorruptionCertificateViewAll,
        #endregion

        #region CustomJob
        [ModuleCodeDescription(ModuleSubGroupCode.CustomJob, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        CustomJobView,

        [ModuleCodeDescription(ModuleSubGroupCode.CustomJob, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        CustomJobViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.CustomJob, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        CustomJobCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.CustomJob, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        CustomJobEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.CustomJob, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        CustomJobCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.CustomJob, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        CustomJobAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.CustomJob, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        CustomJobDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.CustomJob, "Утвердить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Тасдиқлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tasdiqlash")]
        [Translate(LanguageIdConst.RU, "Утвердить")]
        [Translate(LanguageIdConst.EN, "Approve")]
        CustomJobApprove,

        #endregion

        #region Specialty
        [ModuleCodeDescription(ModuleSubGroupCode.Specialty, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        SpecialtyView,

        [ModuleCodeDescription(ModuleSubGroupCode.Specialty, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        SpecialtyViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.Specialty, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        SpecialtyCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Specialty, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        SpecialtyEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Specialty, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        SpecialtyDelete,
        #endregion

        #region MemshipPaymentOrder
        [ModuleCodeDescription(ModuleSubGroupCode.MemshipPaymentOrder, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        MemshipPaymentOrderView,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipPaymentOrder, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        MemshipPaymentOrderViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipPaymentOrder, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        MemshipPaymentOrderCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipPaymentOrder, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        MemshipPaymentOrderEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipPaymentOrder, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        MemshipPaymentOrderCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipPaymentOrder, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        MemshipPaymentOrderAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipPaymentOrder, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        MemshipPaymentOrderDelete,
        #endregion

        #region ServicePaymentOrder
        [ModuleCodeDescription(ModuleSubGroupCode.ServicePaymentOrder, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ServicePaymentOrderView,

        [ModuleCodeDescription(ModuleSubGroupCode.ServicePaymentOrder, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        ServicePaymentOrderViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.ServicePaymentOrder, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ServicePaymentOrderCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ServicePaymentOrder, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ServicePaymentOrderEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ServicePaymentOrder, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        ServicePaymentOrderCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.ServicePaymentOrder, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        ServicePaymentOrderAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.ServicePaymentOrder, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ServicePaymentOrderDelete,
        #endregion

        #region Debt
        [ModuleCodeDescription(ModuleSubGroupCode.Debt, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        DebtView,

        [ModuleCodeDescription(ModuleSubGroupCode.Debt, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        DebtViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.Debt, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        DebtCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Debt, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        DebtEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Debt, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        DebtDelete,
        #endregion

        #region MemshipYearlyPlan
        [ModuleCodeDescription(ModuleSubGroupCode.MemshipYearlyPlan, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        MemshipYearlyPlanView,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipYearlyPlan, "Регион")]
        [Translate(LanguageIdConst.UZ_CYRL, "Вилоят")]
        [Translate(LanguageIdConst.UZ_LATN, "Viloyat")]
        [Translate(LanguageIdConst.RU, "Регион")]
        [Translate(LanguageIdConst.EN, "Region")]
        MemshipYearlyPlanForRegion,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipYearlyPlan, "Район (город)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Туман (шаҳар)")]
        [Translate(LanguageIdConst.UZ_LATN, "Tuman (shahar)")]
        [Translate(LanguageIdConst.RU, "Район (город)")]
        [Translate(LanguageIdConst.EN, "District")]
        MemshipYearlyPlanForDistrict,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipYearlyPlan, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        MemshipYearlyPlanViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipYearlyPlan, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        MemshipYearlyPlanCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipYearlyPlan, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        MemshipYearlyPlanEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipYearlyPlan, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        MemshipYearlyPlanCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipYearlyPlan, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        MemshipYearlyPlanAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipYearlyPlan, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        MemshipYearlyPlanDelete,
        #endregion

        #region EmployeeMissedDay
        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeMissedDay, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        EmployeeMissedDayView,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeMissedDay, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        EmployeeMissedDayCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeMissedDay, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        EmployeeMissedDayEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeMissedDay, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        EmployeeMissedDayDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeMissedDay, "Утвердить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Тасдиқлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tasdiqlash")]
        [Translate(LanguageIdConst.RU, "Утвердить")]
        [Translate(LanguageIdConst.EN, "Approve")]
        EmployeeMissedDayApprove,

        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeMissedDay, "Отменено")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилинган")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilingan")]
        [Translate(LanguageIdConst.RU, "Отменено")]
        [Translate(LanguageIdConst.EN, "Cancel approve")]
        EmployeeMissedDayCancelApprove,
        #endregion


        #region KpiGrating
        [ModuleCodeDescription(ModuleSubGroupCode.KpiGrating, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        KpiGratingView,

        //[ModuleCodeDescription(ModuleSubGroupCode.KpiGrating, "Регион")]
        //[Translate(LanguageIdConst.UZ_CYRL, "Вилоят")]
        //[Translate(LanguageIdConst.UZ_LATN, "Viloyat")]
        //[Translate(LanguageIdConst.RU, "Регион")]
        //[Translate(LanguageIdConst.EN, "Region")]
        //SrvYearlyPlanForRegion,

        //[ModuleCodeDescription(ModuleSubGroupCode.KpiGrating, "Район (город)")]
        //[Translate(LanguageIdConst.UZ_CYRL, "Туман (шаҳар)")]
        //[Translate(LanguageIdConst.UZ_LATN, "Tuman (shahar)")]
        //[Translate(LanguageIdConst.RU, "Район (город)")]
        //[Translate(LanguageIdConst.EN, "District")]
        //SrvYearlyPlanForDistrict,

        [ModuleCodeDescription(ModuleSubGroupCode.KpiGrating, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        KpiGratingViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.KpiGrating, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        KpiGratingCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.KpiGrating, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        KpiGratingEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.KpiGrating, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        KpiGratingCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.KpiGrating, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        KpiGratingAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.KpiGrating, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        KpiGratingDelete,
        #endregion

        #region KpiPlanForEmployee
        [ModuleCodeDescription(ModuleSubGroupCode.KpiPlanForEmployee, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        KpiPlanForEmployeeView,

        //[ModuleCodeDescription(ModuleSubGroupCode.KpiGrating, "Регион")]
        //[Translate(LanguageIdConst.UZ_CYRL, "Вилоят")]
        //[Translate(LanguageIdConst.UZ_LATN, "Viloyat")]
        //[Translate(LanguageIdConst.RU, "Регион")]
        //[Translate(LanguageIdConst.EN, "Region")]
        //SrvYearlyPlanForRegion,

        //[ModuleCodeDescription(ModuleSubGroupCode.KpiGrating, "Район (город)")]
        //[Translate(LanguageIdConst.UZ_CYRL, "Туман (шаҳар)")]
        //[Translate(LanguageIdConst.UZ_LATN, "Tuman (shahar)")]
        //[Translate(LanguageIdConst.RU, "Район (город)")]
        //[Translate(LanguageIdConst.EN, "District")]
        //SrvYearlyPlanForDistrict,

        [ModuleCodeDescription(ModuleSubGroupCode.KpiPlanForEmployee, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        KpiPlanForEmployeeViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.KpiPlanForEmployee, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        KpiPlanForEmployeeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.KpiPlanForEmployee, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        KpiPlanForEmployeeEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.KpiPlanForEmployee, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        KpiPlanForEmployeeCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.KpiPlanForEmployee, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        KpiPlanForEmployeeAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.KpiPlanForEmployee, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        KpiPlanForEmployeeDelete,
        #endregion

        #region KpiRatingEmployee
        [ModuleCodeDescription(ModuleSubGroupCode.KpiRatingEmployee, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        KpiRatingEmployeeView,

        [ModuleCodeDescription(ModuleSubGroupCode.KpiRatingEmployee, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        KpiRatingEmployeeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.KpiRatingEmployee, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        KpiRatingEmployeeEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.KpiRatingEmployee, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        KpiRatingEmployeeCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.KpiRatingEmployee, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        KpiRatingEmployeeAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.KpiRatingEmployee, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        KpiRatingEmployeeDelete,
        #endregion


        #region MemshipNewContractor
        [ModuleCodeDescription(ModuleSubGroupCode.MemshipNewContractor, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        MemshipNewContractorView,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipNewContractor, "Регион")]
        [Translate(LanguageIdConst.UZ_CYRL, "Вилоят")]
        [Translate(LanguageIdConst.UZ_LATN, "Viloyat")]
        [Translate(LanguageIdConst.RU, "Регион")]
        [Translate(LanguageIdConst.EN, "Region")]
        MemshipNewContractorForRegion,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipNewContractor, "Район (город)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Туман (шаҳар)")]
        [Translate(LanguageIdConst.UZ_LATN, "Tuman (shahar)")]
        [Translate(LanguageIdConst.RU, "Район (город)")]
        [Translate(LanguageIdConst.EN, "District")]
        MemshipNewContractorForDistrict,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipNewContractor, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        MemshipNewContractorViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipNewContractor, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        MemshipNewContractorCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipNewContractor, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        MemshipNewContractorEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipNewContractor, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        MemshipNewContractorCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipNewContractor, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        MemshipNewContractorAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.MemshipNewContractor, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        MemshipNewContractorDelete,
        #endregion

        #region SrvYearlyPlan
        [ModuleCodeDescription(ModuleSubGroupCode.SrvYearlyPlan, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        SrvYearlyPlanView,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvYearlyPlan, "Регион")]
        [Translate(LanguageIdConst.UZ_CYRL, "Вилоят")]
        [Translate(LanguageIdConst.UZ_LATN, "Viloyat")]
        [Translate(LanguageIdConst.RU, "Регион")]
        [Translate(LanguageIdConst.EN, "Region")]
        SrvYearlyPlanForRegion,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvYearlyPlan, "Район (город)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Туман (шаҳар)")]
        [Translate(LanguageIdConst.UZ_LATN, "Tuman (shahar)")]
        [Translate(LanguageIdConst.RU, "Район (город)")]
        [Translate(LanguageIdConst.EN, "District")]
        SrvYearlyPlanForDistrict,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvYearlyPlan, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        SrvYearlyPlanViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvYearlyPlan, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        SrvYearlyPlanCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvYearlyPlan, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        SrvYearlyPlanEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvYearlyPlan, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        SrvYearlyPlanCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvYearlyPlan, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        SrvYearlyPlanAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvYearlyPlan, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        SrvYearlyPlanDelete,
        #endregion

        



        #region SrvApplicationYearlyPlan
        [ModuleCodeDescription(ModuleSubGroupCode.SrvYearlyPlan, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        SrvApplicationYearlyPlanView,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvYearlyPlan, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        SrvApplicationYearlyPlanViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvYearlyPlan, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        SrvApplicationYearlyPlanCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvYearlyPlan, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        SrvApplicationYearlyPlanEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvYearlyPlan, "Отмена")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Bekor qilish")]
        [Translate(LanguageIdConst.RU, "Отмена")]
        [Translate(LanguageIdConst.EN, "Cancel")]
        SrvApplicationYearlyPlanCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvYearlyPlan, "Принимать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қабул қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Qabul qilish")]
        [Translate(LanguageIdConst.RU, "Принимать")]
        [Translate(LanguageIdConst.EN, "Accept")]
        SrvApplicationYearlyPlanAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvYearlyPlan, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        SrvApplicationYearlyPlanDelete,
        #endregion

        #region Institute
        [ModuleCodeDescription(ModuleSubGroupCode.Institute, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        InstituteView,

        [ModuleCodeDescription(ModuleSubGroupCode.Institute, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        InstituteViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.Institute, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        InstituteCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Institute, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        InstituteEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Institute, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        InstituteDelete,
        #endregion

        #region DualEducationType
        [ModuleCodeDescription(ModuleSubGroupCode.DualEducationType, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        DualEducationTypeView,

        [ModuleCodeDescription(ModuleSubGroupCode.DualEducationType, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        DualEducationTypeViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.DualEducationType, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        DualEducationTypeCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.DualEducationType, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        DualEducationTypeEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.DualEducationType, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        DualEducationTypeDelete,
        #endregion

        #region NOTIFY(SENDSMSCONFIG)
        [ModuleCodeDescription(ModuleSubGroupCode.SendSmsConfig, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        SendSmsConfigView,

        [ModuleCodeDescription(ModuleSubGroupCode.SendSmsConfig, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        SendSmsConfigViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.SendSmsConfig, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        SendSmsConfigCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.SendSmsConfig, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        SendSmsConfigEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.SendSmsConfig, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        SendSmsConfigDelete,
        #endregion

        #region RESTRICTION_SENDING_APPLICATION
        [ModuleCodeDescription(ModuleSubGroupCode.RestrictionSending, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        RestrictionSendingAppView,

        [ModuleCodeDescription(ModuleSubGroupCode.RestrictionSending, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        RestrictionSendingAppViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.RestrictionSending, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        RestrictionSendingAppCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.RestrictionSending, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        RestrictionSendingAppEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.RestrictionSending, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        RestrictionSendingAppDelete,
        #endregion

        #region SignCriterion
        [ModuleCodeDescription(ModuleSubGroupCode.SignCriterion, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        SignCriterionView,

        [ModuleCodeDescription(ModuleSubGroupCode.SignCriterion, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        SignCriterionViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.SignCriterion, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        SignCriterionCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.SignCriterion, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        SignCriterionEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.SignCriterion, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        SignCriterionDelete,
        #endregion

        #region AdditionalAgreement
        [ModuleCodeDescription(ModuleSubGroupCode.AdditionalAgreement, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        AdditionalAgreementView,

        [ModuleCodeDescription(ModuleSubGroupCode.AdditionalAgreement, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        AdditionalAgreementViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.AdditionalAgreement, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        AdditionalAgreementCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.AdditionalAgreement, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        AdditionalAgreementEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.AdditionalAgreement, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        AdditionalAgreementDelete,
        #endregion

        #region SRV

        #region SERVICE PRICE
        [ModuleCodeDescription(ModuleSubGroupCode.SrvServicePrice, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        SrvServicePriceView,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServicePrice, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        SrvServicePriceViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServicePrice, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        SrvServicePriceCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServicePrice, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        SrvServicePriceEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServicePrice, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        SrvServicePriceDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServicePrice, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        SrvServicePriceAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServicePrice, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        SrvServicePriceCancel,
        #endregion

        #region SERVICE APPLICATION
        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceApplication, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        SrvServiceApplicationView,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceApplication, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        SrvServiceApplicationViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceApplication, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        SrvServiceApplicationCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceApplication, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        SrvServiceApplicationEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceApplication, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        SrvServiceApplicationDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceApplication, "Oтменять")]
        [Translate(LanguageIdConst.UZ_CYRL, "бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "bekor qilish")]
        [Translate(LanguageIdConst.RU, "отменять")]
        [Translate(LanguageIdConst.EN, "cancel")]
        SrvServiceApplicationCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceApplication, "определение")]
        [Translate(LanguageIdConst.UZ_CYRL, "тасдиқлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "tasdiqlash")]
        [Translate(LanguageIdConst.RU, "определение")]
        [Translate(LanguageIdConst.EN, "accept")]
        SrvServiceApplicationAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceApplication, "определение")]
        [Translate(LanguageIdConst.UZ_CYRL, "тасдиқлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "tasdiqlash")]
        [Translate(LanguageIdConst.RU, "определение")]
        [Translate(LanguageIdConst.EN, "accept")]
        SrvServiceApplicationReject,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceApplication, "получено")]
        [Translate(LanguageIdConst.UZ_CYRL, "олинган")]
        [Translate(LanguageIdConst.UZ_LATN, "olingan")]
        [Translate(LanguageIdConst.RU, "получено")]
        [Translate(LanguageIdConst.EN, "Recieved")]
        SrvServiceApplicationReceived,
        #endregion

        #region COMPLETE SERVICE
        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceComplete, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        CompleteServiceView,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceComplete, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        CompleteServiceViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceComplete, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        CompleteServiceCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceComplete, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        CompleteServiceEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceComplete, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        CompleteServiceDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceComplete, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        CompleteServiceAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceComplete, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        CompleteServiceCancel,
        #endregion

        #region SERVICE CONTRACT
        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceContract, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ServiceContractView,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceContract, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        ServiceContractViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceContract, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ServiceContractCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceContract, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ServiceContractEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceContract, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ServiceContractDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceContract, "Oтменять")]
        [Translate(LanguageIdConst.UZ_CYRL, "бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "bekor qilish")]
        [Translate(LanguageIdConst.RU, "отменять")]
        [Translate(LanguageIdConst.EN, "cancel")]
        SrvServiceContractCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceContract, "определение")]
        [Translate(LanguageIdConst.UZ_CYRL, "тасдиқлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "tasdiqlash")]
        [Translate(LanguageIdConst.RU, "определение")]
        [Translate(LanguageIdConst.EN, "accept")]
        SrvServiceContractAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceContract, "Подписание")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзоланмоқда")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolanmoqda")]
        [Translate(LanguageIdConst.RU, "Подписание")]
        [Translate(LanguageIdConst.EN, "Signing")]
        SrvServiceContractSigning,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceContract, "Подписание")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзоланмоқда")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolanmoqda")]
        [Translate(LanguageIdConst.RU, "Подписание")]
        [Translate(LanguageIdConst.EN, "Signing")]
        SrvServiceContractSigned,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceContract, "определение")]
        [Translate(LanguageIdConst.UZ_CYRL, "тасдиқлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "tasdiqlash")]
        [Translate(LanguageIdConst.RU, "определение")]
        [Translate(LanguageIdConst.EN, "accept")]
        SrvServiceContractReject,
        #endregion

        #region SERVICE DEED
        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceDeed, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ServiceDeedView,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceDeed, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        ServiceDeedViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceDeed, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ServiceDeedCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceDeed, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ServiceDeedEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceDeed, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ServiceDeedDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceDeed, "Oтменять")]
        [Translate(LanguageIdConst.UZ_CYRL, "бекор қилиш")]
        [Translate(LanguageIdConst.UZ_LATN, "bekor qilish")]
        [Translate(LanguageIdConst.RU, "отменять")]
        [Translate(LanguageIdConst.EN, "cancel")]
        SrvServiceDeedCancel,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceDeed, "определение")]
        [Translate(LanguageIdConst.UZ_CYRL, "тасдиқлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "tasdiqlash")]
        [Translate(LanguageIdConst.RU, "определение")]
        [Translate(LanguageIdConst.EN, "accept")]
        SrvServiceDeedAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceDeed, "Подписание")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзоланмоқда")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolanmoqda")]
        [Translate(LanguageIdConst.RU, "Подписание")]
        [Translate(LanguageIdConst.EN, "Signing")]
        SrvServiceDeedSigning,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceDeed, "Подписание")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзоланмоқда")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolanmoqda")]
        [Translate(LanguageIdConst.RU, "Подписание")]
        [Translate(LanguageIdConst.EN, "Signing")]
        SrvServiceDeedSigned,

        [ModuleCodeDescription(ModuleSubGroupCode.SrvServiceDeed, "определение")]
        [Translate(LanguageIdConst.UZ_CYRL, "тасдиқлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "tasdiqlash")]
        [Translate(LanguageIdConst.RU, "определение")]
        [Translate(LanguageIdConst.EN, "accept")]
        SrvServiceDeedReject,
        #endregion

        #region SERVICE DEED SWOT
        [ModuleCodeDescription(ModuleSubGroupCode.DeedSwotReport, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        DeedSwotReportView,

		#endregion

		#region Mono REPORT
		[ModuleCodeDescription(ModuleSubGroupCode.MonoApplicationReport, "Просмотр")]
		[Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
		[Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
		[Translate(LanguageIdConst.RU, "Просмотр")]
		[Translate(LanguageIdConst.EN, "View")]
		MonoReportView,

		#endregion
		#endregion

		#region Partisanship
		[ModuleCodeDescription(ModuleSubGroupCode.Partisanship, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        PartisanshipView,

        [ModuleCodeDescription(ModuleSubGroupCode.Partisanship, "Просмотр все")]
        [Translate(LanguageIdConst.UZ_CYRL, "Барчасини кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Barchasini ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр все")]
        [Translate(LanguageIdConst.EN, "View all")]
        PartisanshipViewAll,

        [ModuleCodeDescription(ModuleSubGroupCode.Partisanship, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        PartisanshipCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.Partisanship, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        PartisanshipEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.Partisanship, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        PartisanshipDelete,
        #endregion

        #region AppealApplication
        [ModuleCodeDescription(ModuleSubGroupCode.AppealApplication, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        AppealApplicationView,

        [ModuleCodeDescription(ModuleSubGroupCode.AppealApplication, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        AppealApplicationCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.AppealApplication, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        AppealApplicationEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.AppealApplication, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        AppealApplicationDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.AppealApplication, "Подписать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзолаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolash")]
        [Translate(LanguageIdConst.RU, "Подписать")]
        [Translate(LanguageIdConst.EN, "Sign")]
        AppealApplicationSign,

        [ModuleCodeDescription(ModuleSubGroupCode.AppealApplication, "Отклонить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Рад этиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Rad etish")]
        [Translate(LanguageIdConst.RU, "Отклонить")]
        [Translate(LanguageIdConst.EN, "Reject")]
        AppealApplicationReject,

        [ModuleCodeDescription(ModuleSubGroupCode.AppealApplication, "Провести")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўтказиш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'tkazish")]
        [Translate(LanguageIdConst.RU, "Провести")]
        [Translate(LanguageIdConst.EN, "Accept")]
        AppealApplicationAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.AppealApplication, "SendToEdoc")]
        [Translate(LanguageIdConst.UZ_CYRL, "SendToEdoc")]
        [Translate(LanguageIdConst.UZ_LATN, "SendToEdoc")]
        [Translate(LanguageIdConst.RU, "SendToEdoc")]
        [Translate(LanguageIdConst.EN, "SendToEdoc")]
        AppealApplicationSendToEdoc,
        #endregion

        #region CallCenterAppeal
        [ModuleCodeDescription(ModuleSubGroupCode.CallCenterAppeal, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        CallCenterAppealView,

        [ModuleCodeDescription(ModuleSubGroupCode.CallCenterAppeal, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        CallCenterAppealCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.CallCenterAppeal, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        CallCenterAppealEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.CallCenterAppeal, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        CallCenterAppealDelete,

        [ModuleCodeDescription(ModuleSubGroupCode.CallCenterAppeal, "Подписать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Имзолаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Imzolash")]
        [Translate(LanguageIdConst.RU, "Подписать")]
        [Translate(LanguageIdConst.EN, "Sign")]
        CallCenterAppealSign,

        [ModuleCodeDescription(ModuleSubGroupCode.CallCenterAppeal, "Отклонить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Рад этиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Rad etish")]
        [Translate(LanguageIdConst.RU, "Отклонить")]
        [Translate(LanguageIdConst.EN, "Reject")]
        CallCenterAppealReject,

        [ModuleCodeDescription(ModuleSubGroupCode.CallCenterAppeal, "Провести")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўтказиш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'tkazish")]
        [Translate(LanguageIdConst.RU, "Провести")]
        [Translate(LanguageIdConst.EN, "Accept")]
        CallCenterAppealAccept,

        [ModuleCodeDescription(ModuleSubGroupCode.CallCenterAppeal, "SendToEdoc")]
        [Translate(LanguageIdConst.UZ_CYRL, "SendToEdoc")]
        [Translate(LanguageIdConst.UZ_LATN, "SendToEdoc")]
        [Translate(LanguageIdConst.RU, "SendToEdoc")]
        [Translate(LanguageIdConst.EN, "SendToEdoc")]
        CallCenterAppealSendToEdoc,
        #endregion

        #region EmployeeTurnstileReport
        [ModuleCodeDescription(ModuleSubGroupCode.EmployeeTurnstileReport, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        EmployeeTurnstileReport,
		#endregion

		#region Indicator
		[ModuleCodeDescription(ModuleSubGroupCode.Indicator, "Просмотр")]
		[Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
		[Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
		[Translate(LanguageIdConst.RU, "Просмотр")]
		[Translate(LanguageIdConst.EN, "View")]
		IndicatorView,

		[ModuleCodeDescription(ModuleSubGroupCode.Indicator, "Создать")]
		[Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
		[Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
		[Translate(LanguageIdConst.RU, "Создать")]
		[Translate(LanguageIdConst.EN, "Create")]
		IndicatorCreate,

		[ModuleCodeDescription(ModuleSubGroupCode.Indicator, "Редактировать")]
		[Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
		[Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
		[Translate(LanguageIdConst.RU, "Редактировать")]
		[Translate(LanguageIdConst.EN, "Edit")]
		IndicatorEdit,

		[ModuleCodeDescription(ModuleSubGroupCode.Indicator, "Удалить")]
		[Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
		[Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
		[Translate(LanguageIdConst.RU, "Удалить")]
		[Translate(LanguageIdConst.EN, "Delete")]
		IndicatorDelete,
        #endregion

        #region IndicatorDepartment
        [ModuleCodeDescription(ModuleSubGroupCode.IndicatorDepartment, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        IndicatorDepartmentView,

        [ModuleCodeDescription(ModuleSubGroupCode.IndicatorDepartment, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        IndicatorDepartmentCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.IndicatorDepartment, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        IndicatorDepartmentEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.IndicatorDepartment, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        IndicatorDepartmentDelete,
        #endregion

        #region UniteOfMeasure
        [ModuleCodeDescription(ModuleSubGroupCode.UniteOfMeasure, "Просмотр")]
		[Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
		[Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
		[Translate(LanguageIdConst.RU, "Просмотр")]
		[Translate(LanguageIdConst.EN, "View")]
		UniteOfMeasureView,

		[ModuleCodeDescription(ModuleSubGroupCode.UniteOfMeasure, "Создать")]
		[Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
		[Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
		[Translate(LanguageIdConst.RU, "Создать")]
		[Translate(LanguageIdConst.EN, "Create")]
		UniteOfMeasureCreate,

		[ModuleCodeDescription(ModuleSubGroupCode.UniteOfMeasure, "Редактировать")]
		[Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
		[Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
		[Translate(LanguageIdConst.RU, "Редактировать")]
		[Translate(LanguageIdConst.EN, "Edit")]
		UniteOfMeasureEdit,

		[ModuleCodeDescription(ModuleSubGroupCode.UniteOfMeasure, "Удалить")]
		[Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
		[Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
		[Translate(LanguageIdConst.RU, "Удалить")]
		[Translate(LanguageIdConst.EN, "Delete")]
		UniteOfMeasureDelete,
        #endregion

        #region newReport
        [ModuleCodeDescription(ModuleSubGroupCode.ExpiredReport, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ExpiredReportView,

        [ModuleCodeDescription(ModuleSubGroupCode.ExpiredReport, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ExpiredReportCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ExpiredReport, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ExpiredReportEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ExpiredReport, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ExpiredReportDelete,
        #endregion

        #region ClaimApplication
        [ModuleCodeDescription(ModuleSubGroupCode.ClaimReport, "Просмотр")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кўриш")]
        [Translate(LanguageIdConst.UZ_LATN, "Ko'rish")]
        [Translate(LanguageIdConst.RU, "Просмотр")]
        [Translate(LanguageIdConst.EN, "View")]
        ClaimApplicationReportView,

        [ModuleCodeDescription(ModuleSubGroupCode.ClaimReport, "Создать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Яратиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Yaratish")]
        [Translate(LanguageIdConst.RU, "Создать")]
        [Translate(LanguageIdConst.EN, "Create")]
        ClaimApplicationReportCreate,

        [ModuleCodeDescription(ModuleSubGroupCode.ClaimReport, "Редактировать")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таҳрирлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Tahrirlash")]
        [Translate(LanguageIdConst.RU, "Редактировать")]
        [Translate(LanguageIdConst.EN, "Edit")]
        ClaimApplicationReportEdit,

        [ModuleCodeDescription(ModuleSubGroupCode.ClaimReport, "Удалить")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ўчириш")]
        [Translate(LanguageIdConst.UZ_LATN, "O'chirish")]
        [Translate(LanguageIdConst.RU, "Удалить")]
        [Translate(LanguageIdConst.EN, "Delete")]
        ClaimApplicationReportDelete,
        #endregion
    }
}

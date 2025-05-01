using WEBASE;

namespace SspUis.Core.Security
{
    public enum ModuleSubGroupCode
    {

        #region Manuals

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Право использования")]
        [Translate(LanguageIdConst.UZ_CYRL, "Фойдаланиш ҳуқуқи")]
        [Translate(LanguageIdConst.UZ_LATN, "Foydalanish huquqi")]
        [Translate(LanguageIdConst.RU, "Право использования")]
        [Translate(LanguageIdConst.EN, "Access")]
        Access,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Валюта")]
        [Translate(LanguageIdConst.UZ_CYRL, "Валюта")]
        [Translate(LanguageIdConst.UZ_LATN, "Valyuta")]
        [Translate(LanguageIdConst.RU, "Валюта")]
        [Translate(LanguageIdConst.EN, "Currency")]
        Currency,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Страна")]
        [Translate(LanguageIdConst.UZ_CYRL, "Давлат")]
        [Translate(LanguageIdConst.UZ_LATN, "Davlat")]
        [Translate(LanguageIdConst.RU, "Страна")]
        [Translate(LanguageIdConst.EN, "Country")]
        Country,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Анкета предпринимателя")]
        [Translate(LanguageIdConst.UZ_CYRL, "Тадбиркор сўровномаси")]
        [Translate(LanguageIdConst.UZ_LATN, "Tadbirkor so'rovnomasi")]
        [Translate(LanguageIdConst.RU, "Анкета предпринимателя")]
        [Translate(LanguageIdConst.EN, "ContractorSurvey")]
        ContractorSurvey,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Опросник")]
        [Translate(LanguageIdConst.UZ_CYRL, "Анкета")]
        [Translate(LanguageIdConst.UZ_LATN, "Anketa")]
        [Translate(LanguageIdConst.RU, "Опросник")]
        [Translate(LanguageIdConst.EN, "Questionnaire")]
        Questionnaire,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Должность")]
        [Translate(LanguageIdConst.UZ_CYRL, "Лавозим")]
        [Translate(LanguageIdConst.UZ_LATN, "Lavozim")]
        [Translate(LanguageIdConst.RU, "Должность")]
        [Translate(LanguageIdConst.EN, "Position")]
        Position,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Тип партнерского контракта")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ҳамкорлик шартномаси тури")]
        [Translate(LanguageIdConst.UZ_LATN, "Hamkorlik shartnomasi turi")]
        [Translate(LanguageIdConst.RU, "Тип партнерского контракта")]
        [Translate(LanguageIdConst.EN, "PrtnContractType")]
        PrtnContractType,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Причина отказа партнерского контракта")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ҳамкорлик шартномасини қайтариш сабаби")]
        [Translate(LanguageIdConst.UZ_LATN, "Hamkorlik shartnomasini qaytarish sababi")]
        [Translate(LanguageIdConst.RU, "Причина отказа партнерского контракта")]
        [Translate(LanguageIdConst.EN, "PrtnRejectReason")]
        PrtnRejectReason,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Окно для введение данных")]
        [Translate(LanguageIdConst.UZ_CYRL, "Маълумот киритиш ойнаси")]
        [Translate(LanguageIdConst.UZ_LATN, "Ma'lumot kiritish oynasi")]
        [Translate(LanguageIdConst.RU, "Окно для введение данных")]
        [Translate(LanguageIdConst.EN, "Landing Page Data")]
        LandingPageDatum,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Функции контроля")]
        [Translate(LanguageIdConst.UZ_CYRL, "Назорат функсия")]
        [Translate(LanguageIdConst.UZ_LATN, "Nazorat funksiya")]
        [Translate(LanguageIdConst.RU, "Функции контроля")]
        [Translate(LanguageIdConst.EN, "Control Function")]
        ControlFunction,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Тип проверки организации")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ташкилот текшириш тури")]
        [Translate(LanguageIdConst.UZ_LATN, "Tashkilot tekshiruvi turi")]
        [Translate(LanguageIdConst.RU, "Тип проверки организации")]
        [Translate(LanguageIdConst.EN, "Organization Inspection Type")]
        OrganizationInspectionType,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Классификатор Должностей")]
        [Translate(LanguageIdConst.UZ_CYRL, "Лавозимлар Классификатори")]
        [Translate(LanguageIdConst.UZ_LATN, "Lavozimlar Klasifikatщкш")]
        [Translate(LanguageIdConst.RU, "Классификатор Должностей")]
        [Translate(LanguageIdConst.EN, "Position Classifier")]
        PositionClassifier,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Тип проверки")]
        [Translate(LanguageIdConst.UZ_CYRL, "Текшириш тури")]
        [Translate(LanguageIdConst.UZ_LATN, "Tekshirish turi")]
        [Translate(LanguageIdConst.RU, "Тип проверки")]
        [Translate(LanguageIdConst.EN, "CheckType")]
        CheckType,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Проверить основу")]
        [Translate(LanguageIdConst.UZ_CYRL, "Текшириш асоси")]
        [Translate(LanguageIdConst.UZ_LATN, "Tekshirish asosi")]
        [Translate(LanguageIdConst.RU, "Проверить основу")]
        [Translate(LanguageIdConst.EN, "CheckBasis")]
        CheckBasis,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Область")]
        [Translate(LanguageIdConst.UZ_CYRL, "Вилоят")]
        [Translate(LanguageIdConst.UZ_LATN, "Viloyat")]
        [Translate(LanguageIdConst.RU, "Область")]
        [Translate(LanguageIdConst.EN, "Region")]
        Region,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Гражданство")]
        [Translate(LanguageIdConst.UZ_CYRL, "Фукоролик")]
        [Translate(LanguageIdConst.UZ_LATN, "Fuqorolik")]
        [Translate(LanguageIdConst.RU, "Гражданство")]
        [Translate(LanguageIdConst.EN, "Citizenship")]
        Citizenship,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Национальность")]
        [Translate(LanguageIdConst.UZ_CYRL, "Миллати")]
        [Translate(LanguageIdConst.UZ_LATN, "Millati")]
        [Translate(LanguageIdConst.RU, "Национальность")]
        [Translate(LanguageIdConst.EN, "Nationality")]
        Nationality,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Район")]
        [Translate(LanguageIdConst.UZ_CYRL, "Туман")]
        [Translate(LanguageIdConst.UZ_LATN, "Tuman")]
        [Translate(LanguageIdConst.RU, "Район")]
        [Translate(LanguageIdConst.EN, "District")]
        District,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Мфй")]
        [Translate(LanguageIdConst.UZ_CYRL, "Мфй")]
        [Translate(LanguageIdConst.UZ_LATN, "Mfy")]
        [Translate(LanguageIdConst.RU, "Мфй")]
        [Translate(LanguageIdConst.EN, "Mfy")]
        Mfy,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Окэд")]
        [Translate(LanguageIdConst.UZ_CYRL, "Окэд")]
        [Translate(LanguageIdConst.UZ_LATN, "Oked")]
        [Translate(LanguageIdConst.RU, "Окэд")]
        [Translate(LanguageIdConst.EN, "Oked")]
        Oked,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Банк")]
        [Translate(LanguageIdConst.UZ_CYRL, "Банк")]
        [Translate(LanguageIdConst.UZ_LATN, "Bank")]
        [Translate(LanguageIdConst.RU, "Банк")]
        [Translate(LanguageIdConst.EN, "Bank")]
        Bank,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Организация")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ташкилот")]
        [Translate(LanguageIdConst.UZ_LATN, "Tashkilot")]
        [Translate(LanguageIdConst.RU, "Организация")]
        [Translate(LanguageIdConst.EN, "Organization")]
        Organization,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Организационная структура")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ташкилий тузилма")]
        [Translate(LanguageIdConst.UZ_LATN, "Tashkiliy tuzilma")]
        [Translate(LanguageIdConst.RU, "Организационная структура")]
        [Translate(LanguageIdConst.EN, "OrganizationalStructure")]
        OrganizationalStructure,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Персона")]
        [Translate(LanguageIdConst.UZ_CYRL, "Шахс")]
        [Translate(LanguageIdConst.UZ_LATN, "Shaxs")]
        [Translate(LanguageIdConst.RU, "Персона")]
        [Translate(LanguageIdConst.EN, "Person")]
        Person,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Сотрудник")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ходим")]
        [Translate(LanguageIdConst.UZ_LATN, "Xodim")]
        [Translate(LanguageIdConst.RU, "Сотрудник")]
        [Translate(LanguageIdConst.EN, "Employee")]
        Employee,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Категории видео")]
        [Translate(LanguageIdConst.UZ_CYRL, "Видео тоифалари")]
        [Translate(LanguageIdConst.UZ_LATN, "Video toifalari")]
        [Translate(LanguageIdConst.RU, "Категории видео")]
        [Translate(LanguageIdConst.EN, "Video Category")]
        VideoCategory,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Новости")]
        [Translate(LanguageIdConst.UZ_CYRL, "Янгиликлар")]
        [Translate(LanguageIdConst.UZ_LATN, "Yangiliklar")]
        [Translate(LanguageIdConst.RU, "Новости")]
        [Translate(LanguageIdConst.EN, "News")]
        News,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Новостной тег")]
        [Translate(LanguageIdConst.UZ_CYRL, "Янгиликлар Теги")]
        [Translate(LanguageIdConst.UZ_LATN, "Yangiliklar Tegi")]
        [Translate(LanguageIdConst.RU, "Новостной тег")]
        [Translate(LanguageIdConst.EN, "News Tag")]
        NewsTag,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Отделение")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бўлим")]
        [Translate(LanguageIdConst.UZ_LATN, "Bo'lim")]
        [Translate(LanguageIdConst.RU, "Отделение")]
        [Translate(LanguageIdConst.EN, "Department")]
        Department,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Предприниматель карточка")]
        [Translate(LanguageIdConst.UZ_CYRL, "Тадбиркор картаси")]
        [Translate(LanguageIdConst.UZ_LATN, "Tadbirkor kartasi")]
        [Translate(LanguageIdConst.RU, "Предприниматель карточка")]
        [Translate(LanguageIdConst.EN, "BusinessmanCard")]
        BusinessmanCard,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Фонд синхронизация")]
        [Translate(LanguageIdConst.UZ_CYRL, "Жамғарма синхронизация")]
        [Translate(LanguageIdConst.UZ_LATN, "Jamg'arma sinxronzatsiya")]
        [Translate(LanguageIdConst.RU, "Фонд синхронизация")]
        [Translate(LanguageIdConst.EN, "BusinessActivityType")]
        BusinessActivityType,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Субъект предпринимательства")]
        [Translate(LanguageIdConst.UZ_CYRL, "Тадбиркорлик субйекти")]
        [Translate(LanguageIdConst.UZ_LATN, "Tadbirkorlik subyekti")]
        [Translate(LanguageIdConst.RU, "Субъект предпринимательства")]
        [Translate(LanguageIdConst.EN, "Contractor")]
        Contractor,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Форма юридической собственности организации")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ташкилотнинг ҳуқуқий мулкчилик шакли")]
        [Translate(LanguageIdConst.UZ_LATN, "Tashkilotning huquqiy mulkchilik shakli")]
        [Translate(LanguageIdConst.RU, "Форма юридической собственности организации")]
        [Translate(LanguageIdConst.EN, "Organization legal form")]
        OrganizationLegalForm,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Степень родства")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қариндошлик даражаси")]
        [Translate(LanguageIdConst.UZ_LATN, "Qarindoshlik darajasi")]
        [Translate(LanguageIdConst.RU, "Степень родства")]
        [Translate(LanguageIdConst.EN, "Relative degree")]
        RelativeDegree,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Тип документа")]
        [Translate(LanguageIdConst.UZ_CYRL, "Хужжат тури")]
        [Translate(LanguageIdConst.UZ_LATN, "Hujjat turi")]
        [Translate(LanguageIdConst.RU, "Тип документа")]
        [Translate(LanguageIdConst.EN, "Identity document")]
        IdentityDocument,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "График рабочего времени")]
        [Translate(LanguageIdConst.UZ_CYRL, "Иш вақти жадвали")]
        [Translate(LanguageIdConst.UZ_LATN, "Ish vaqti jadvali")]
        [Translate(LanguageIdConst.RU, "График рабочего времени")]
        [Translate(LanguageIdConst.EN, "Work schedule")]
        WorkSchedule,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Статья расходов")]
        [Translate(LanguageIdConst.UZ_CYRL, "Статья расходов")]
        [Translate(LanguageIdConst.UZ_LATN, "Статья расходов")]
        [Translate(LanguageIdConst.RU, "Статья расходов")]
        [Translate(LanguageIdConst.EN, "Item of expense")]
        ItemOfExpense,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Управления сотрудниками")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ходимларни бошқариш")]
        [Translate(LanguageIdConst.UZ_LATN, "Xodimlarni boshqarish")]
        [Translate(LanguageIdConst.RU, "Управления сотрудниками")]
        [Translate(LanguageIdConst.EN, "EmployeeManage")]
        EmployeeManage,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Вид расчета")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ҳисоблаш тури")]
        [Translate(LanguageIdConst.UZ_LATN, "Hisoblash turi")]
        [Translate(LanguageIdConst.RU, "Вид расчета")]
        [Translate(LanguageIdConst.EN, "Calculation kind")]
        CalculationKind,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Тип налоговой льготы")]
        [Translate(LanguageIdConst.UZ_CYRL, "Солиқ имтиёзлари тури")]
        [Translate(LanguageIdConst.UZ_LATN, "Soliq imtiyozlari turi")]
        [Translate(LanguageIdConst.RU, "Тип налоговой льготы")]
        [Translate(LanguageIdConst.EN, "Tax Benefit Type")]
        TaxBenefitType,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "БРВ, БВИП, МРОТ, пенсии по возрасту и пособий")]
        [Translate(LanguageIdConst.UZ_CYRL, "БҲМ, ПҲБМ, МҲЭКМ, ёшга доир пенсия ва нафақалар")]
        [Translate(LanguageIdConst.UZ_LATN, "BHM, PHBM, MHEKM, yoshga doir pensiya va nafaqalar")]
        [Translate(LanguageIdConst.RU, "БРВ, БВИП, МРОТ, пенсии по возрасту и пособий")]
        [Translate(LanguageIdConst.EN, "Fixed Minimum Value")]
        FixedMinimumValue,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Тарифная сетка")]
        [Translate(LanguageIdConst.UZ_CYRL, "Тариф шкаласи")]
        [Translate(LanguageIdConst.UZ_LATN, "Tarif shkalasi")]
        [Translate(LanguageIdConst.RU, "Тарифная сетка")]
        [Translate(LanguageIdConst.EN, "TariffScale")]
        TariffScale,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "SettlementAccountSource")]
        [Translate(LanguageIdConst.UZ_CYRL, "SettlementAccountSource")]
        [Translate(LanguageIdConst.UZ_LATN, "SettlementAccountSource")]
        [Translate(LanguageIdConst.RU, "SettlementAccountSource")]
        [Translate(LanguageIdConst.EN, "SettlementAccountSource")]
        SettlementAccountSource,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Разряды тарифной сетка")]
        [Translate(LanguageIdConst.UZ_CYRL, "Тарифлар шкаласининг даражалари")]
        [Translate(LanguageIdConst.UZ_LATN, "Tariflar shkalasining darajalari")]
        [Translate(LanguageIdConst.RU, "Разряды тарифной сетка")]
        [Translate(LanguageIdConst.EN, "TariffScaleCoef")]
        TariffScaleCoef,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Тип штатного расписания Базовый тариф")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ходимлар тури Aсосий тариф")]
        [Translate(LanguageIdConst.UZ_LATN, "Xodimlar turi Asosiy tarif")]
        [Translate(LanguageIdConst.RU, "Тип штатного расписания Базовый тариф")]
        [Translate(LanguageIdConst.EN, "Staff Type Basic Tariff")]
        StaffTypeBasicTariff,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Источник")]
        [Translate(LanguageIdConst.UZ_CYRL, "Манбаси")]
        [Translate(LanguageIdConst.UZ_LATN, "Manbasi")]
        [Translate(LanguageIdConst.RU, "Источник")]
        [Translate(LanguageIdConst.EN, "Source Code")]
        SourceCode,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Appeal Type Arrive")]
        [Translate(LanguageIdConst.UZ_CYRL, "Appeal Type Arrive")]
        [Translate(LanguageIdConst.UZ_LATN, "Appeal Type Arrive")]
        [Translate(LanguageIdConst.RU, "Appeal Type Arrive")]
        [Translate(LanguageIdConst.EN, "Appeal Type Arrive")]
        AppealTypeArrive,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Appeal Description")]
        [Translate(LanguageIdConst.UZ_CYRL, "Appeal Description")]
        [Translate(LanguageIdConst.UZ_LATN, "Appeal Description")]
        [Translate(LanguageIdConst.RU, "Appeal Description")]
        [Translate(LanguageIdConst.EN, "Appeal Description")]
        AppealDescription,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Уровень бюджета")]
        [Translate(LanguageIdConst.UZ_CYRL, "Бюджет даражаси")]
        [Translate(LanguageIdConst.UZ_LATN, "Budjet darajasi")]
        [Translate(LanguageIdConst.RU, "Уровень бюджета")]
        [Translate(LanguageIdConst.EN, "Level Code")]
        LevelCode,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Показатели штатного расписания")]
        [Translate(LanguageIdConst.UZ_CYRL, "Штат жадвали кўрсатгичлари")]
        [Translate(LanguageIdConst.UZ_LATN, "Shtat jadvali ko‘rsatgichlari")]
        [Translate(LanguageIdConst.RU, "Показатели штатного расписания")]
        [Translate(LanguageIdConst.EN, "StaffingIndicator")]
        StaffingIndicator,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Категория должности")]
        [Translate(LanguageIdConst.UZ_CYRL, "Лавозим тоифаси")]
        [Translate(LanguageIdConst.UZ_LATN, "Lavozim toifasi")]
        [Translate(LanguageIdConst.RU, "Категория должности")]
        [Translate(LanguageIdConst.EN, "Position Category")]
        PositionCategory,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Классификатор должностей")]
        [Translate(LanguageIdConst.UZ_CYRL, "Лавозимлар классификатори")]
        [Translate(LanguageIdConst.UZ_LATN, "Lavozimlar klassifikatori")]
        [Translate(LanguageIdConst.RU, "Классификатор должностей")]
        [Translate(LanguageIdConst.EN, "Position Classification")]
        PositionClassification,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Тип должности")]
        [Translate(LanguageIdConst.UZ_CYRL, "Лавозим тури")]
        [Translate(LanguageIdConst.UZ_LATN, "Lavozim turi")]
        [Translate(LanguageIdConst.RU, "Тип должности")]
        [Translate(LanguageIdConst.EN, "Position Type")]
        PositionType,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Учёная степень")]
        [Translate(LanguageIdConst.UZ_CYRL, "Илмий даражаси")]
        [Translate(LanguageIdConst.UZ_LATN, "Ilmiy daraja")]
        [Translate(LanguageIdConst.RU, "Учёная степень")]
        [Translate(LanguageIdConst.EN, "Academic Degree")]
        AcademicDegree,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Единство меры")]
        [Translate(LanguageIdConst.UZ_CYRL, "Улчов бирлиги")]
        [Translate(LanguageIdConst.UZ_LATN, "O'lchov birligi")]
        [Translate(LanguageIdConst.RU, "Единство меры")]
        [Translate(LanguageIdConst.EN, "Degree Title")]
        DegreeTitle,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Степень наград")]
        [Translate(LanguageIdConst.UZ_CYRL, "Даражали унвони")]
        [Translate(LanguageIdConst.UZ_LATN, "Darajali unvoni")]
        [Translate(LanguageIdConst.RU, "Степень наград")]
        [Translate(LanguageIdConst.EN, "Unite Of Measure")]
        UniteOfMeasure,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Выборные органы")]
        [Translate(LanguageIdConst.UZ_CYRL, "Сайланадиган органлар")]
        [Translate(LanguageIdConst.UZ_LATN, "Saylanadigan organlar")]
        [Translate(LanguageIdConst.RU, "Выборные органы")]
        [Translate(LanguageIdConst.EN, "Election Member")]
        ElectionMember,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Образовательный предмет")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таълим жиҳозлари")]
        [Translate(LanguageIdConst.UZ_LATN, "Ta'lim jihozlari")]
        [Translate(LanguageIdConst.RU, "Образовательный предмет")]
        [Translate(LanguageIdConst.EN, "Education Item")]
        EducationItem,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Владение иностранными языками")]
        [Translate(LanguageIdConst.UZ_CYRL, "Тил билиши")]
        [Translate(LanguageIdConst.UZ_LATN, "Til bilishi")]
        [Translate(LanguageIdConst.RU, "Владение иностранными языками")]
        [Translate(LanguageIdConst.EN, "Language Proficiency")]
        LanguageProficiency,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Учёное звание")]
        [Translate(LanguageIdConst.UZ_CYRL, "Илмий унвони")]
        [Translate(LanguageIdConst.UZ_LATN, "Ilmiy unvoni")]
        [Translate(LanguageIdConst.RU, "Учёное звание")]
        [Translate(LanguageIdConst.EN, "Scientific Degree")]
        ScientificDegree,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Государственные награды")]
        [Translate(LanguageIdConst.UZ_CYRL, "Давлат мукофотлари")]
        [Translate(LanguageIdConst.UZ_LATN, "Davlat mukofotlari")]
        [Translate(LanguageIdConst.RU, "Государственные награды")]
        [Translate(LanguageIdConst.EN, "State Award")]
        StateAward,


        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Во́инское зва́ние ")]
        [Translate(LanguageIdConst.UZ_CYRL, "Харбий унвон")]
        [Translate(LanguageIdConst.UZ_LATN, "Harbiy unvon")]
        [Translate(LanguageIdConst.RU, "Во́инское зва́ние")]
        [Translate(LanguageIdConst.EN, "Military Rank")]
        MilitaryRank,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Квалификационная категория")]
        [Translate(LanguageIdConst.UZ_CYRL, "Малака тоифаси")]
        [Translate(LanguageIdConst.UZ_LATN, "Malaka toifasi")]
        [Translate(LanguageIdConst.RU, "Квалификационная категория")]
        [Translate(LanguageIdConst.EN, "QualificationCategory")]
        QualificationCategory,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Индикатор")]
        [Translate(LanguageIdConst.UZ_CYRL, "Курсаткич")]
        [Translate(LanguageIdConst.UZ_LATN, "Koʻrsatkich")]
        [Translate(LanguageIdConst.RU, "Индикатор")]
        [Translate(LanguageIdConst.EN, "Indicator")]
        Indicator,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Indicator Department")]
        [Translate(LanguageIdConst.UZ_CYRL, "Indicator Department")]
        [Translate(LanguageIdConst.UZ_LATN, "Indicator Department")]
        [Translate(LanguageIdConst.RU, "Indicator Department")]
        [Translate(LanguageIdConst.EN, "Indicator Department")]
        IndicatorDepartment,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Предмет заявки")]
        [Translate(LanguageIdConst.UZ_CYRL, "Мурожаат предмети")]
        [Translate(LanguageIdConst.UZ_LATN, "Murojaat predmeti")]
        [Translate(LanguageIdConst.RU, "Предмет заявки")]
        [Translate(LanguageIdConst.EN, "Claim theme")]
        ClaimTheme,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Тип органа, в который направляется претензия")]
        [Translate(LanguageIdConst.UZ_CYRL, "Да'во аризаси юбориладиган орган тури")]
        [Translate(LanguageIdConst.UZ_LATN, "Da'vo arizasi yuboriladigan organ turi")]
        [Translate(LanguageIdConst.RU, "Тип органа, в который направляется претензия")]
        [Translate(LanguageIdConst.EN, "Claim Organization Type")]
        ClaimOrganizationType,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Орган, в который направляется претензия")]
        [Translate(LanguageIdConst.UZ_CYRL, "Да'во аризаси юбориладиган орган")]
        [Translate(LanguageIdConst.UZ_LATN, "Da'vo arizasi yuboriladigan organ")]
        [Translate(LanguageIdConst.RU, "Орган, в который направляется претензия")]
        [Translate(LanguageIdConst.EN, "Claim Organization")]
        ClaimOrganization,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Группа видов деятельности")]
        [Translate(LanguageIdConst.UZ_CYRL, "Фаолият тури гурухи")]
        [Translate(LanguageIdConst.UZ_LATN, "Faoliyat turi guruhi")]
        [Translate(LanguageIdConst.RU, "Группа видов деятельности")]
        [Translate(LanguageIdConst.EN, "Contractor Activity Group")]
        ContractorActivityGroup,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Рейтинг")]
        [Translate(LanguageIdConst.UZ_CYRL, "Рейтинг")]
        [Translate(LanguageIdConst.UZ_LATN, "Reyting")]
        [Translate(LanguageIdConst.RU, "Reyting")]
        [Translate(LanguageIdConst.EN, "Contractor Rating")]
        ContractorRating,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Тип активности")]
        [Translate(LanguageIdConst.UZ_CYRL, "Фаолият тури")]
        [Translate(LanguageIdConst.UZ_LATN, "Faoliyat turi")]
        [Translate(LanguageIdConst.RU, "Тип активности")]
        [Translate(LanguageIdConst.EN, "Contractor Activity Type")]
        ContractorActivityType,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Тип союза активности")]
        [Translate(LanguageIdConst.UZ_CYRL, "Иттифок Фаолият тури")]
        [Translate(LanguageIdConst.UZ_LATN, "Ittifoq Faoliyat turi")]
        [Translate(LanguageIdConst.RU, "Тип союза активности")]
        [Translate(LanguageIdConst.EN, "Contractor Union Activity Type")]
        ContractorUnionActivityType,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Потребность в палатных услугах")]
        [Translate(LanguageIdConst.UZ_CYRL, "Палата хизматларига эҳтиёж")]
        [Translate(LanguageIdConst.UZ_LATN, "Palata xizmatlariga ehtiyoj")]
        [Translate(LanguageIdConst.RU, "Потребность в палатных услугах")]
        [Translate(LanguageIdConst.EN, "Need Chamber Service")]
        NeedChamberService,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Специальность")]
        [Translate(LanguageIdConst.UZ_CYRL, "Мутахассислик")]
        [Translate(LanguageIdConst.UZ_LATN, "Mutaxassislik")]
        [Translate(LanguageIdConst.RU, "Специальность")]
        [Translate(LanguageIdConst.EN, "Specialty")]
        Specialty,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Институт")]
        [Translate(LanguageIdConst.UZ_CYRL, "Институти")]
        [Translate(LanguageIdConst.UZ_LATN, "Instituti")]
        [Translate(LanguageIdConst.RU, "Институт")]
        [Translate(LanguageIdConst.EN, "Institute")]
        Institute,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Дуальный тип образования")]
        [Translate(LanguageIdConst.UZ_CYRL, "Дуал таълим тури")]
        [Translate(LanguageIdConst.UZ_LATN, "Dual ta'lim turi")]
        [Translate(LanguageIdConst.RU, "Дуальный тип образования")]
        [Translate(LanguageIdConst.EN, "Dual Education Type")]
        DualEducationType,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "настройка отправки смс")]
        [Translate(LanguageIdConst.UZ_CYRL, "смс мосламаларини юбориш")]
        [Translate(LanguageIdConst.UZ_LATN, "sms moslamalarini yuborish")]
        [Translate(LanguageIdConst.RU, "настройка отправки смс")]
        [Translate(LanguageIdConst.EN, "Send SMS Config")]
        SendSmsConfig,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "критерий подписи")]
        [Translate(LanguageIdConst.UZ_CYRL, "имзо мезони")]
        [Translate(LanguageIdConst.UZ_LATN, "imzo mezoni")]
        [Translate(LanguageIdConst.RU, "критерий подписи")]
        [Translate(LanguageIdConst.EN, "sign criterion")]
        SignCriterion,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Partisanship ")]
        [Translate(LanguageIdConst.UZ_CYRL, "Partisanship ")]
        [Translate(LanguageIdConst.UZ_LATN, "Partisanship")]
        [Translate(LanguageIdConst.RU, "Partisanship")]
        [Translate(LanguageIdConst.EN, "Partisanship")]
        Partisanship,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Обращение ")]
        [Translate(LanguageIdConst.UZ_CYRL, "Мурожаат ")]
        [Translate(LanguageIdConst.UZ_LATN, "Murojaat")]
        [Translate(LanguageIdConst.RU, "Обращение")]
        [Translate(LanguageIdConst.EN, "Appeal Application")]
        AppealApplication,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "CallCenterAppeal ")]
        [Translate(LanguageIdConst.UZ_CYRL, "CallCenterAppeal ")]
        [Translate(LanguageIdConst.UZ_LATN, "CallCenterAppeal")]
        [Translate(LanguageIdConst.RU, "CallCenterAppeal")]
        [Translate(LanguageIdConst.EN, "Call Center Appeal")]
        CallCenterAppeal,

        #endregion

        #region Documents

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Договор о сотрудничестве")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ҳамкорлик шартномаси")]
        [Translate(LanguageIdConst.UZ_LATN, "Hamkorlik shartnomasi")]
        [Translate(LanguageIdConst.RU, "Договор о сотрудничестве")]
        [Translate(LanguageIdConst.EN, "Partner contract")]
        PrtnContract,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Назначить сотрудника")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ходимни тайинлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Xodimni tayinlash")]
        [Translate(LanguageIdConst.RU, "Назначить сотрудника")]
        [Translate(LanguageIdConst.EN, "Appoint Employee")]
        AppointEmployee,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Заявление о государственном активе")]
        [Translate(LanguageIdConst.UZ_CYRL, "Давлат активлари учун ариза")]
        [Translate(LanguageIdConst.UZ_LATN, "Davlat aktivlari uchun ariza")]
        [Translate(LanguageIdConst.RU, "Заявление о государственном активе")]
        [Translate(LanguageIdConst.EN, "StateAssetApplication")]
        StateAssetApplication,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Членский взнос за соглашение")]
        [Translate(LanguageIdConst.UZ_CYRL, "Азолик шартномаси толови")]
        [Translate(LanguageIdConst.UZ_LATN, "Azolik shartnomasi to'lovi")]
        [Translate(LanguageIdConst.RU, "Членский взнос за соглашение")]
        [Translate(LanguageIdConst.EN, "Memship Payment Order")]
        MemshipPaymentOrder,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Сервисное взнос за соглашение")]
        [Translate(LanguageIdConst.UZ_CYRL, "Хизмат  шартномаси толови")]
        [Translate(LanguageIdConst.UZ_LATN, "Xizmat shartnomasi to'lovi")]
        [Translate(LanguageIdConst.RU, "Сервисное взнос за соглашение")]
        [Translate(LanguageIdConst.EN, "Service Payment Order")]
        ServicePaymentOrder,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Tadbirkorlik subyektlari bo'yicha qoldiqlarni kiritish")]
        [Translate(LanguageIdConst.UZ_CYRL, "Tadbirkorlik subyektlari bo'yicha qoldiqlarni kiritish")]
        [Translate(LanguageIdConst.UZ_LATN, "Tadbirkorlik subyektlari bo'yicha qoldiqlarni kiritish")]
        [Translate(LanguageIdConst.RU, "Tadbirkorlik subyektlari bo'yicha qoldiqlarni kiritish")]
        [Translate(LanguageIdConst.EN, "Tadbirkorlik subyektlari bo'yicha qoldiqlarni kiritish")]
        Debt,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Заявление о предъявлении претензии")]
        [Translate(LanguageIdConst.UZ_CYRL, "Даъволик аризаси")]
        [Translate(LanguageIdConst.UZ_LATN, "Da'volik arizasi")]
        [Translate(LanguageIdConst.RU, "Заявление о предъявлении претензии")]
        [Translate(LanguageIdConst.EN, "ClaimApplication")]
        ClaimApplication,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Loyha ijrosi boyicha ma'lumot kiritish hujjati")]
        [Translate(LanguageIdConst.UZ_CYRL, "Loyha ijrosi boyicha ma'lumot kiritish hujjati")]
        [Translate(LanguageIdConst.UZ_LATN, "Loyha ijrosi boyicha ma'lumot kiritish hujjati")]
        [Translate(LanguageIdConst.RU, "Loyha ijrosi boyicha ma'lumot kiritish hujjati")]
        [Translate(LanguageIdConst.EN, "Execution Application")]
        ExecutionApplication,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Заявление о членстве")]
        [Translate(LanguageIdConst.UZ_CYRL, "Аъзолик аризаси")]
        [Translate(LanguageIdConst.UZ_LATN, "A`zolik arizasi")]
        [Translate(LanguageIdConst.RU, "Заявление о членстве")]
        [Translate(LanguageIdConst.EN, "MemshipApplication")]
        MemshipApplication,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Заявление в Третейский суд")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ҳакамлик судига ариза")]
        [Translate(LanguageIdConst.UZ_LATN, "Hakamlik sudiga ariza")]
        [Translate(LanguageIdConst.RU, "Заявление в Третейский суд")]
        [Translate(LanguageIdConst.EN, "ArbitrationCourtApplication")]
        ArbitrationCourtApplication,


        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "ArbitrationResult")]
        [Translate(LanguageIdConst.UZ_CYRL, "ArbitrationResult")]
        [Translate(LanguageIdConst.UZ_LATN, "ArbitrationResult")]
        [Translate(LanguageIdConst.RU, "ArbitrationResult")]
        [Translate(LanguageIdConst.EN, "ArbitrationResult")]
        ArbitrationResult,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Hakamlik sudyalari")]
        [Translate(LanguageIdConst.UZ_CYRL, "Hakamlik sudyalari")]
        [Translate(LanguageIdConst.UZ_LATN, "Hakamlik sudyalari")]
        [Translate(LanguageIdConst.RU, "Hakamlik sudyalari")]
        [Translate(LanguageIdConst.EN, "ArbitrationJudge")]
        ArbitrationJudge,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Заявление о антикоррупции")]
        [Translate(LanguageIdConst.UZ_CYRL, "Коррупцияга қарши курашиш аризаси")]
        [Translate(LanguageIdConst.UZ_LATN, "Korruptsiyaga qarshi kurashish arizasi")]
        [Translate(LanguageIdConst.RU, "Заявление о антикоррупции")]
        [Translate(LanguageIdConst.EN, "JoinAntiCorruptionApplication")]
        JoinAntiCorruptionApplication,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Сертификат")]
        [Translate(LanguageIdConst.UZ_CYRL, "Сертификат")]
        [Translate(LanguageIdConst.UZ_LATN, "Sertifikat")]
        [Translate(LanguageIdConst.RU, "Сертификат")]
        [Translate(LanguageIdConst.EN, "Certificate")]
        PrtnCertificate,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "CreditDemand")]
        [Translate(LanguageIdConst.UZ_CYRL, "CreditDemand")]
        [Translate(LanguageIdConst.UZ_LATN, "CreditDemand")]
        [Translate(LanguageIdConst.RU, "CreditDemand")]
        [Translate(LanguageIdConst.EN, "CreditDemand")]
        PrtnCreditDemand,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Видео уроков")]
        [Translate(LanguageIdConst.UZ_CYRL, "Видео дарслар")]
        [Translate(LanguageIdConst.UZ_LATN, "Video darslar")]
        [Translate(LanguageIdConst.RU, "Видео уроков")]
        [Translate(LanguageIdConst.EN, "Video Lesson")]
        VideoLesson,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Заявление")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ариза")]
        [Translate(LanguageIdConst.UZ_LATN, "Ariza")]
        [Translate(LanguageIdConst.RU, "Заявление")]
        [Translate(LanguageIdConst.EN, "Application")]
        Application,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "DualApplication")]
        [Translate(LanguageIdConst.UZ_CYRL, "DualApplication")]
        [Translate(LanguageIdConst.UZ_LATN, "DualApplication")]
        [Translate(LanguageIdConst.RU, "DualApplication")]
        [Translate(LanguageIdConst.EN, "Dual Application")]
        DualApplication,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "SubsidyRequest")]
        [Translate(LanguageIdConst.UZ_CYRL, "SubsidyRequest")]
        [Translate(LanguageIdConst.UZ_LATN, "SubsidyRequest")]
        [Translate(LanguageIdConst.RU, "SubsidyRequest")]
        [Translate(LanguageIdConst.EN, "SubsidyRequest")]
        SubsidyRequest,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Табель")]
        [Translate(LanguageIdConst.UZ_CYRL, "Табель")]
        [Translate(LanguageIdConst.UZ_LATN, "Tabel")]
        [Translate(LanguageIdConst.RU, "Табель")]
        [Translate(LanguageIdConst.EN, "Timesheet")]
        Timesheet,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "integratsiya")]
        [Translate(LanguageIdConst.UZ_CYRL, "Integratsiya")]
        [Translate(LanguageIdConst.UZ_LATN, "Integratsiya")]
        [Translate(LanguageIdConst.RU, "Integratsiya")]
        [Translate(LanguageIdConst.EN, "Integration")]
        Integration,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Отправка на повышение квалификации")]
        [Translate(LanguageIdConst.UZ_CYRL, "Малака оширишга юбориш")]
        [Translate(LanguageIdConst.UZ_LATN, "Malaka oshirishga yuborish")]
        [Translate(LanguageIdConst.RU, "Отправка на повышение квалификации")]
        [Translate(LanguageIdConst.EN, "Employee Send Train")]
        EmployeeSendTrain,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "O'quv ta'tili buyrug'i")]
        [Translate(LanguageIdConst.UZ_CYRL, "O'quv ta'tili buyrug'i")]
        [Translate(LanguageIdConst.UZ_LATN, "O'quv ta'tili buyrug'i")]
        [Translate(LanguageIdConst.RU, "O'quv ta'tili buyrug'i")]
        [Translate(LanguageIdConst.EN, "Employee Send Study")]
        EmployeeSendStudy,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Приказы на трудовой отпуск и отпуск без содержания")]
        [Translate(LanguageIdConst.UZ_CYRL, "Меҳнат таътили ёки иш ҳақи сақланмаган ҳолда таътил бериш буйруғи")]
        [Translate(LanguageIdConst.UZ_LATN, "Mehnat ta'tili yoki ish haqi saqlanmagan holda ta'til berish buyrug‘i")]
        [Translate(LanguageIdConst.RU, "Приказы на трудовой отпуск и отпуск без содержания")]
        [Translate(LanguageIdConst.EN, "Employee Leave Order")]
        EmployeeLeaveOrder,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Intizomiy jazo qo'llash")]
        [Translate(LanguageIdConst.UZ_CYRL, "Intizomiy jazo qo'llash")]
        [Translate(LanguageIdConst.UZ_LATN, "Intizomiy jazo qo'llash")]
        [Translate(LanguageIdConst.RU, "Intizomiy jazo qo'llash")]
        [Translate(LanguageIdConst.EN, "Chastisement")]
        Chastisement,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Nomzodlarni Tasdiqlash")]
        [Translate(LanguageIdConst.UZ_CYRL, "Номзодларни Тасдиқлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Nomzodlarni Tasdiqlash")]
        [Translate(LanguageIdConst.RU, "Подтверждение кандидатов")]
        [Translate(LanguageIdConst.EN, "Candidates Confirmation")]
        CandidatesConfirmation,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Налоговая льгота")]
        [Translate(LanguageIdConst.UZ_CYRL, "Солиқ имтиёзлари")]
        [Translate(LanguageIdConst.UZ_LATN, "Soliq imtiyozlari")]
        [Translate(LanguageIdConst.RU, "Налоговая льгота")]
        [Translate(LanguageIdConst.EN, "Tax benefit")]
        TaxBenefit,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Плановые Виды расчётов (Общие)")]
        [Translate(LanguageIdConst.UZ_CYRL, "Доимий тўлов турлари умумий")]
        [Translate(LanguageIdConst.UZ_LATN, "Doimiy to'lov turlari umumiy")]
        [Translate(LanguageIdConst.RU, "Плановые Виды расчётов (Общие)")]
        [Translate(LanguageIdConst.EN, "Mass planned calculation")]
        MassPlannedCalculation,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Сертификат о вступлении в членство")]
        [Translate(LanguageIdConst.UZ_CYRL, "Аъзолик сертификати")]
        [Translate(LanguageIdConst.UZ_LATN, "Aʼzolik sertifikati")]
        [Translate(LanguageIdConst.RU, "Сертификат о вступлении в членство")]
        [Translate(LanguageIdConst.EN, "Membership certificate")]
        MemshipCertificate,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "ContractorCategoryCriterion")]
        [Translate(LanguageIdConst.UZ_CYRL, "ContractorCategoryCriterion")]
        [Translate(LanguageIdConst.UZ_LATN, "Tadbirkor kategoriyalari mezoni")]
        [Translate(LanguageIdConst.RU, "ContractorCategoryCriterion")]
        [Translate(LanguageIdConst.EN, "ContractorCategoryCriterion")]
        ContractorCategoryCriterion,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Соглашение о членстве")]
        [Translate(LanguageIdConst.UZ_CYRL, "Аъзолик шартномаси")]
        [Translate(LanguageIdConst.UZ_LATN, "A`zolik shartnomasi")]
        [Translate(LanguageIdConst.RU, "Соглашение о членстве")]
        [Translate(LanguageIdConst.EN, "Memship Contract")]
        MemshipContract,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Временный вид расчета")]
        [Translate(LanguageIdConst.UZ_CYRL, "Вақтинчалик ҳисоблаш тури")]
        [Translate(LanguageIdConst.UZ_LATN, "Vaqtinchalik hisoblash turi")]
        [Translate(LanguageIdConst.RU, "Временный вид расчета")]
        [Translate(LanguageIdConst.EN, "Temp Calc Kind")]
        TempCalcKind,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Выходной рабочий день")]
        [Translate(LanguageIdConst.UZ_CYRL, "Дам олиш куни")]
        [Translate(LanguageIdConst.UZ_LATN, "Dam olish kuni")]
        [Translate(LanguageIdConst.RU, "Выходной рабочий день")]
        [Translate(LanguageIdConst.EN, "Work Day Off")]
        WorkDayOff,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Отзыв работника из отпуска или отпуска без содержания")]
        [Translate(LanguageIdConst.UZ_CYRL, "Меҳнат таътилидан ёки ўз ҳисобидан таътилдан чақириб олиш")]
        [Translate(LanguageIdConst.UZ_LATN, "Mehnat ta'tilidan yoki o‘z hisobidan ta'tildan chaqirib olish")]
        [Translate(LanguageIdConst.RU, "Отзыв работника из отпуска или отпуска без содержания")]
        [Translate(LanguageIdConst.EN, "Recall Leave")]
        RecallLeave,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Штатное расписание")]
        [Translate(LanguageIdConst.UZ_CYRL, "Кадрлар билан таъминлаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Kadrlar bilan ta'minlash")]
        [Translate(LanguageIdConst.RU, "Штатное расписание")]
        [Translate(LanguageIdConst.EN, "Staffing")]
        Staffing,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Типовое штатное расписание")]
        [Translate(LanguageIdConst.UZ_CYRL, "Намунавий штатлар жадвали")]
        [Translate(LanguageIdConst.UZ_LATN, "Namunaviy shtatlar jadvali")]
        [Translate(LanguageIdConst.RU, "Типовое штатное расписание")]
        [Translate(LanguageIdConst.EN, "StaffingTemplate")]
        StaffingTemplate,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "EmployeeMissedDay")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ишланмаган кун")]
        [Translate(LanguageIdConst.UZ_LATN, "Ishlanmagan kun")]
        [Translate(LanguageIdConst.RU, "Пропущенный день")]
        [Translate(LanguageIdConst.EN, "EmployeeMissedDay")]
        EmployeeMissedDay,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Больничный лист работника")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ходимнинг касаллик варақаси")]
        [Translate(LanguageIdConst.UZ_LATN, "Xodimning kasallik varaqasi")]
        [Translate(LanguageIdConst.RU, "Больничный лист работника")]
        [Translate(LanguageIdConst.EN, "Employee sick leave")]
        EmployeeSickLeave,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Командировка")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ҳизмат сафари")]
        [Translate(LanguageIdConst.UZ_LATN, "Hizmat safari")]
        [Translate(LanguageIdConst.RU, "Командировка")]
        [Translate(LanguageIdConst.EN, "Order To Send Business Trip")]
        OrderToSendBusinessTrip,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Запланированные платежи")]
        [Translate(LanguageIdConst.UZ_CYRL, "Режалаштирилган тўловлар")]
        [Translate(LanguageIdConst.UZ_LATN, "Rejalashtirilgan to'lovlar")]
        [Translate(LanguageIdConst.RU, "Запланированные платежи")]
        [Translate(LanguageIdConst.EN, "Planned Calculation")]
        PlannedCalculation,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Предложение")]
        [Translate(LanguageIdConst.UZ_CYRL, "Таклиф")]
        [Translate(LanguageIdConst.UZ_LATN, "Taklif")]
        [Translate(LanguageIdConst.RU, "Предложение")]
        [Translate(LanguageIdConst.EN, "Proposal")]
        Proposal,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Посредничество")]
        [Translate(LanguageIdConst.UZ_CYRL, "Воситачилик")]
        [Translate(LanguageIdConst.UZ_LATN, "Vositachilik")]
        [Translate(LanguageIdConst.RU, "Посредничество")]
        [Translate(LanguageIdConst.EN, "Mediation")]
        Mediation,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Заявка на претензию")]
        [Translate(LanguageIdConst.UZ_CYRL, "Даво аризаси")]
        [Translate(LanguageIdConst.UZ_LATN, "Da`vo arizasi")]
        [Translate(LanguageIdConst.RU, "Заявка на претензию")]
        [Translate(LanguageIdConst.EN, "Application For Court")]
        ApplicationForCourt,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Арбитражный суд")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ҳакамлик суди")]
        [Translate(LanguageIdConst.UZ_LATN, "Hakamlik sudi")]
        [Translate(LanguageIdConst.RU, "Арбитражный суд")]
        [Translate(LanguageIdConst.EN, "Court of Arbitration")]
        ArbitrationForCourt,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Планировать собрание медиация")]
        [Translate(LanguageIdConst.UZ_CYRL, "Медиация йигинини режалаштириш")]
        [Translate(LanguageIdConst.UZ_LATN, "Mediatsiya yiginini rejalashtirish")]
        [Translate(LanguageIdConst.RU, "Планировать собрание медиация")]
        [Translate(LanguageIdConst.EN, "Mediation plan")]
        MediationPlan,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Решение о присоединении к антикоррупционной Хартии")]
        [Translate(LanguageIdConst.UZ_CYRL, "Коррупцияга қарши курашиш Хартиясига қўшилиш бўйича қарор")]
        [Translate(LanguageIdConst.UZ_LATN, "Korrupsiyaga qarshi kurashish Xartiyasiga qo'shilish bo'yicha qaror")]
        [Translate(LanguageIdConst.RU, "Решение о присоединении к антикоррупционной Хартии")]
        [Translate(LanguageIdConst.EN, "Join Anti Corruption Result")]
        JoinAntiCorruptionResult,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Присоединяйтесь к антикоррупционному сертификату")]
        [Translate(LanguageIdConst.UZ_CYRL, "Коррупцияга kарши cертификатга kўшилинг")]
        [Translate(LanguageIdConst.UZ_LATN, "Korruptsiyaga qarshi sertifikatga qo'shiling")]
        [Translate(LanguageIdConst.RU, "Присоединяйтесь к антикоррупционному сертификату")]
        [Translate(LanguageIdConst.EN, "Join Anti Corruption Certificate")]
        JoinAntiCorruptionCertificate,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Memship Yearly Plan")]
        [Translate(LanguageIdConst.UZ_CYRL, "Memship Yearly Plan")]
        [Translate(LanguageIdConst.UZ_LATN, "Memship Yearly Plan")]
        [Translate(LanguageIdConst.RU, "Memship Yearly Plan")]
        [Translate(LanguageIdConst.EN, "Memship Yearly Plan")]
        MemshipYearlyPlan,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Memship New Contractor")]
        [Translate(LanguageIdConst.UZ_CYRL, "Memship New Contractor")]
        [Translate(LanguageIdConst.UZ_LATN, "Memship New Contractor")]
        [Translate(LanguageIdConst.RU, "Memship New Contractor")]
        [Translate(LanguageIdConst.EN, "Memship New Contractor")]
        MemshipNewContractor,
        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Srv Yearly Plan")]
        [Translate(LanguageIdConst.UZ_CYRL, "Srv Yearly Plan")]
        [Translate(LanguageIdConst.UZ_LATN, "Srv Yearly Plan")]
        [Translate(LanguageIdConst.RU, "Srv Yearly Plan")]
        [Translate(LanguageIdConst.EN, "Srv Yearly Plan")]
        SrvYearlyPlan,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "KpiRatingEmployee")]
        [Translate(LanguageIdConst.UZ_CYRL, "KpiRatingEmployee")]
        [Translate(LanguageIdConst.UZ_LATN, "KpiRatingEmployee")]
        [Translate(LanguageIdConst.RU, "KpiRatingEmployee")]
        [Translate(LanguageIdConst.EN, "KpiRatingEmployee")]
        KpiRatingEmployee,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "SrvApplicationYearlyPlan")]
        [Translate(LanguageIdConst.UZ_CYRL, "SrvApplicationYearlyPlan")]
        [Translate(LanguageIdConst.UZ_LATN, "SrvApplicationYearlyPlan")]
        [Translate(LanguageIdConst.RU, "SrvApplicationYearlyPlan")]
        [Translate(LanguageIdConst.EN, "SrvApplicationYearlyPlan")]
        SrvApplicationYearlyPlan,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "SrvServicePrice")]
        [Translate(LanguageIdConst.UZ_CYRL, "SrvServicePrice")]
        [Translate(LanguageIdConst.UZ_LATN, "SrvServicePrice")]
        [Translate(LanguageIdConst.RU, "SrvServicePrice")]
        [Translate(LanguageIdConst.EN, "SrvServicePrice")]
        SrvServicePrice,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "SrvServiceApplication")]
        [Translate(LanguageIdConst.UZ_CYRL, "SrvServiceApplication")]
        [Translate(LanguageIdConst.UZ_LATN, "SrvServiceApplication")]
        [Translate(LanguageIdConst.RU, "SrvServiceApplication")]
        [Translate(LanguageIdConst.EN, "SrvServiceApplication")]
        SrvServiceApplication,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "SrvServiceContract")]
        [Translate(LanguageIdConst.UZ_CYRL, "SrvServiceContract")]
        [Translate(LanguageIdConst.UZ_LATN, "SrvServiceContract")]
        [Translate(LanguageIdConst.RU, "SrvServiceContract")]
        [Translate(LanguageIdConst.EN, "SrvServiceContract")]
        SrvServiceContract,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Акт")]
        [Translate(LanguageIdConst.UZ_CYRL, "Далолатнома")]
        [Translate(LanguageIdConst.UZ_LATN, "Dalolatnoma")]
        [Translate(LanguageIdConst.RU, "Акт")]
        [Translate(LanguageIdConst.EN, "Srv Service Deed")]
        SrvServiceDeed,

        [ModuleSubGroupDescription(ModuleGroupIdConst.REPORTS, "Ҳудудий бошқармалар томонидан ко’рсатилган хизматлар бо’йича ҳисобот")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ҳудудий бошқармалар томонидан ко’рсатилган хизматлар бо’йича ҳисобот")]
        [Translate(LanguageIdConst.UZ_LATN, "Hududiy boshqarmalar tomonidan ko’rsatilgan xizmatlar bo’yicha hisobot")]
        [Translate(LanguageIdConst.RU, "Отчет об услугах, оказанных администрациями регионов")]
        [Translate(LanguageIdConst.EN, "Report on services provided by regional administrations")]
        DeedSwotReport,

		[ModuleSubGroupDescription(ModuleGroupIdConst.REPORTS, "Моно марказ хисоботи")]
		[Translate(LanguageIdConst.UZ_CYRL, "Моно марказ хисоботи")]
		[Translate(LanguageIdConst.UZ_LATN, "Mono markaz hisoboti")]
		[Translate(LanguageIdConst.RU, "Отчёт моно центр")]
		[Translate(LanguageIdConst.EN, "Mono application Report")]
		MonoApplicationReport,

		[ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "SrvServiceComplete")]
        [Translate(LanguageIdConst.UZ_CYRL, "SrvServiceComplete")]
        [Translate(LanguageIdConst.UZ_LATN, "SrvServiceComplete")]
        [Translate(LanguageIdConst.RU, "SrvServiceComplete")]
        [Translate(LanguageIdConst.EN, "SrvServiceComplete")]
        SrvServiceComplete,

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Дополнительный договор")]
        [Translate(LanguageIdConst.UZ_CYRL, "Қўшимча шартнома")]
        [Translate(LanguageIdConst.UZ_LATN, "Qo'shimcha shartnoma")]
        [Translate(LanguageIdConst.RU, "Дополнительный договор")]
        [Translate(LanguageIdConst.EN, "Additional Agreement")]
        AdditionalAgreement,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Подписать приказы")]
        [Translate(LanguageIdConst.UZ_CYRL, "Буйруқларни имзолаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Buyruqlarni imzolash")]
        [Translate(LanguageIdConst.RU, "Подписать приказы")]
        [Translate(LanguageIdConst.EN, "Signer")]
        Signer,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Подписать приказы")]
        [Translate(LanguageIdConst.UZ_CYRL, "Буйруқларни имзолаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Buyruqlarni imzolash")]
        [Translate(LanguageIdConst.RU, "Подписать приказы")]
        [Translate(LanguageIdConst.EN, "Signer")]
        KpiGrating,
        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "Подписать приказы")]
        [Translate(LanguageIdConst.UZ_CYRL, "Буйруқларни имзолаш")]
        [Translate(LanguageIdConst.UZ_LATN, "Buyruqlarni imzolash")]
        [Translate(LanguageIdConst.RU, "Подписать приказы")]
        [Translate(LanguageIdConst.EN, "Signer")]
        KpiPlanForEmployee,

        #endregion

        #region Reports

        [ModuleSubGroupDescription(ModuleGroupIdConst.REPORTS, "Дашборд")]
        [Translate(LanguageIdConst.UZ_CYRL, "Дашборд")]
        [Translate(LanguageIdConst.UZ_LATN, "Dashbord")]
        [Translate(LanguageIdConst.RU, "Дашборд")]
        [Translate(LanguageIdConst.EN, "Dashboard")]
        Dashboard,

        [ModuleSubGroupDescription(ModuleGroupIdConst.REPORTS, "Дашборд")]
        [Translate(LanguageIdConst.UZ_CYRL, "Дашборд")]
        [Translate(LanguageIdConst.UZ_LATN, "Dashbord")]
        [Translate(LanguageIdConst.RU, "Дашборд")]
        [Translate(LanguageIdConst.EN, "Dashboard")]
        ServiceContractDashboard,

        [ModuleSubGroupDescription(ModuleGroupIdConst.REPORTS, "Дашборд")]
        [Translate(LanguageIdConst.UZ_CYRL, "Дашборд")]
        [Translate(LanguageIdConst.UZ_LATN, "Dashbord")]
        [Translate(LanguageIdConst.RU, "Дашборд")]
        [Translate(LanguageIdConst.EN, "Dashboard")]
        ArbitrationDashboard,

        [ModuleSubGroupDescription(ModuleGroupIdConst.REPORTS, "Отчет")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ҳисобот")]
        [Translate(LanguageIdConst.UZ_LATN, "Hisobot")]
        [Translate(LanguageIdConst.RU, "Отчет")]
        [Translate(LanguageIdConst.EN, "Report")]
        Report,

        [ModuleSubGroupDescription(ModuleGroupIdConst.REPORTS, "Отчет о турникете для сотрудников")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ходимлар турникет ҳисоботлари")]
        [Translate(LanguageIdConst.UZ_LATN, "Xodimlar turniket hisobotlari")]
        [Translate(LanguageIdConst.RU, "Отчет о турникете для сотрудников")]
        [Translate(LanguageIdConst.EN, "Employee turnstile reports")]
        EmployeeTurnstileReport,


        [ModuleSubGroupDescription(ModuleGroupIdConst.REPORTS, "Отчет о просрочке исполнения")]
        [Translate(LanguageIdConst.UZ_CYRL, "Ижро муддати кечикиши бўйича ҳисобот")]
        [Translate(LanguageIdConst.UZ_LATN, "Ijro muddati kechikishi bo'yicha hisobot")]
        [Translate(LanguageIdConst.RU, "Отчет о просрочке исполнения")]
        [Translate(LanguageIdConst.EN, "Late execution Report")]
        ExpiredReport,

        [ModuleSubGroupDescription(ModuleGroupIdConst.REPORTS, "Отчёт о Исковое заявление")]
        [Translate(LanguageIdConst.UZ_CYRL, "Даво аризаси хисоботи")]
        [Translate(LanguageIdConst.UZ_LATN, "Da'vo arizasi hisoboti")]
        [Translate(LanguageIdConst.RU, "Отчёт о Исковое заявление")]
        [Translate(LanguageIdConst.EN, "Claim Application Report")]
        ClaimReport,

        [ModuleSubGroupDescription(ModuleGroupIdConst.REPORTS, "Статус заявок и сроки опоздания на подписание")]
        [Translate(LanguageIdConst.UZ_CYRL, "Аризаларнинг ҳолати ва имзолаш учун кечикган кунлар")]
        [Translate(LanguageIdConst.UZ_LATN, "Arizalarning holati va imzolash uchun kechikgan kunlar")]
        [Translate(LanguageIdConst.RU, "Статус заявок и сроки опоздания на подписание")]
        [Translate(LanguageIdConst.EN, "Status of applications and late days for signature")]
        ApplicationStatusReport,

        #endregion

        #region System

        [ModuleSubGroupDescription(ModuleGroupIdConst.MANUALS, "Ошибки приложения")]
        [Translate(LanguageIdConst.UZ_CYRL, "Иловадаги хатолар")]
        [Translate(LanguageIdConst.UZ_LATN, "Ilovadagi xatolar")]
        [Translate(LanguageIdConst.RU, "Ошибки приложения")]
        [Translate(LanguageIdConst.EN, "Application errors")]
        AppError,

        [ModuleSubGroupDescription(ModuleGroupIdConst.SYSTEM, "Пользователь")]
        [Translate(LanguageIdConst.UZ_CYRL, "Фойдаланувчи")]
        [Translate(LanguageIdConst.UZ_LATN, "Foydalanuvchi")]
        [Translate(LanguageIdConst.RU, "Пользователь")]
        [Translate(LanguageIdConst.EN, "User")]
        User,

        [ModuleSubGroupDescription(ModuleGroupIdConst.SYSTEM, "Роль")]
        [Translate(LanguageIdConst.UZ_CYRL, "Роль")]
        [Translate(LanguageIdConst.UZ_LATN, "Rol")]
        [Translate(LanguageIdConst.RU, "Роль")]
        [Translate(LanguageIdConst.EN, "Role")]
        Role,

        [ModuleSubGroupDescription(ModuleGroupIdConst.SYSTEM, "CustomJob")]
        [Translate(LanguageIdConst.UZ_CYRL, "CustomJob")]
        [Translate(LanguageIdConst.UZ_LATN, "CustomJob")]
        [Translate(LanguageIdConst.RU, "CustomJob")]
        [Translate(LanguageIdConst.EN, "CustomJob")]
        CustomJob,

        [ModuleSubGroupDescription(ModuleGroupIdConst.SYSTEM, "SendingRestriction")]
        [Translate(LanguageIdConst.UZ_CYRL, "Чекловни Юбориш")]
        [Translate(LanguageIdConst.UZ_LATN, "Cheklovni Yuborish")]
        [Translate(LanguageIdConst.RU, "Отправка Чеклона")]
        [Translate(LanguageIdConst.EN, "SendingRestriction")]
        RestrictionSending,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "ArbitrationDiscussion")]
        [Translate(LanguageIdConst.UZ_CYRL, "Хакамлик Мухокамаси")]
        [Translate(LanguageIdConst.UZ_LATN, "Hakamlik  Muhokamasi")]
        [Translate(LanguageIdConst.RU, "Судейское обсуждение")]
        [Translate(LanguageIdConst.EN, "Arbitration Discussion")]
        ArbitrationDiscussion,

        [ModuleSubGroupDescription(ModuleGroupIdConst.DOCUMENTS, "ArbitrationDelay")]
        [Translate(LanguageIdConst.UZ_CYRL, "ArbitrationDelay")]
        [Translate(LanguageIdConst.UZ_LATN, "ArbitrationDelay")]
        [Translate(LanguageIdConst.RU, "ArbitrationDelay")]
        [Translate(LanguageIdConst.EN, "ArbitrationDelay")]
        ArbitrationDelay,
        #endregion
    }
}

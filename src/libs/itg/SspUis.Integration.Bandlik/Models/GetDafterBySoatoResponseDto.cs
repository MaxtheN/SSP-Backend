using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Bandlik.Models
{
    public class GetDaftarBySoatoResponseDto:BandlikResponseDto
    {
        public GetDaftarBySoatoResultDto Result { get; set; }
    }
    public class GetDaftarBySoatoResultDto
    {
        public string Msg { get; set; }
        public GetDaftarBySoatoDataDto Result { get; set; }
        public bool Success { get; set; }
    }
    public class GetDaftarBySoatoDataDto
    {
        public int CurrentPage { get; set; }
        public List<DaftarBySoatoDatum> Data { get; set; }
        public int From { get; set; }
        public int LastPage { get; set; }
        public int PerPage { get; set; }
        public int To { get; set; }
        public int Total { get; set; }
    }
    public class DaftarBySoatoDatum
    {
        public DaftarBySoatoChildField ChildField { get; set; }
        public int? ChildFieldId { get; set; }
        public DaftarBySoatoCitizen Citizen { get; set; }
        public DaftarBySoatoCitizenHistory CitizenHistory { get; set; }
        public DaftarBySoatoCity City { get; set; }
        public int CityId { get; set; }
        public DaftarBySoatoFamilyStatus FamilyStatus { get; set; }
        public int FamilyStatusId { get; set; }
        public DaftarBySoatoField Field { get; set; }
        public int? FieldId { get; set; }
        public int Id { get; set; }
        public DaftarBySoatoMakhalla Makhalla { get; set; }
        public int MakhallaId { get; set; }
        public int? NeedWork { get; set; }
        public DaftarBySoatoOtherSocial OtherSocial { get; set; }
        public int OtherSocialId { get; set; }
        public object Pin { get; set; }
        public int PlaceId { get; set; }
        public DaftarBySoatoPlaceStatus PlaceStatus { get; set; }
        public DaftarBySoatoRegion Region { get; set; }
        public int RegionId { get; set; }
        public DaftarBySoatoSipChildProblem SipChildProblem { get; set; }
        public DaftarBySoatoSipProblem SipProblem { get; set; }
        public int? SipProblemChildId { get; set; }
        public int? SipProblemId { get; set; }
        public string SipProblemOther { get; set; }
    }

    public class DaftarBySoatoCitizen
    {
        public string BirthDate { get; set; }
        public int Degree { get; set; }
        public string Firstname { get; set; }
        public string LivingPlace { get; set; }
        public string Passport { get; set; }
        public string Patronymic { get; set; }
        public string Phone { get; set; }
        public object Pin { get; set; }
        public string Surname { get; set; }
    }

    public class DaftarBySoatoCitizenHistory
    {
        public string Amount { get; set; }
        public string EnterDate { get; set; }
        public int Id { get; set; }
        public string OutDate { get; set; }
        public DaftarBySoatoOutReason OutReason { get; set; }
        public int ReasonId { get; set; }
        public int SurveyId { get; set; }
    }

    public class DaftarBySoatoCity
    {
        public int Id { get; set; }
        public string NameCyrl { get; set; }
        public int Soato { get; set; }
    }
    public class DaftarBySoatoRegion
    {
        public int Id { get; set; }
        public string NameCyrl { get; set; }
        public int Soato { get; set; }
    }
    public class DaftarBySoatoChildField
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DaftarBySoatoFamilyStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DaftarBySoatoField
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DaftarBySoatoMakhalla
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DaftarBySoatoOtherSocial
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DaftarBySoatoOutReason
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DaftarBySoatoPlaceStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    public class DaftarBySoatoSipChildProblem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DaftarBySoatoSipProblem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }



}

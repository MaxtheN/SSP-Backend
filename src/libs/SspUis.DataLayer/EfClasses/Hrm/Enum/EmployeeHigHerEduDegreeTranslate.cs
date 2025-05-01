using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses
{
    [Table("enum_employee_higher_edu_degree_translate", Schema = "hrm")]
    public partial class EmployeeHigherEduDegreeTranslate : TranslateEntity<EmployeeHigherEduDegreeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(EmployeeHigherEduDegree.Translates))]
        public virtual EmployeeHigherEduDegree Owner { get; set; }
    }
}
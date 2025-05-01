using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_businessman_user_in_contractor", Schema = "my")]
    public partial class BusinessmanUserInContractor : IHaveStateId
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("businessman_user_id")]
        public int BusinessmanUserId { get; set; }
        [Column("contractor_id")]
        public long ContractorId { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(BusinessmanUserId))]
        [InverseProperty(nameof(EfClasses.BusinessmanUser.BusinessmanUserInContractors))]
        public virtual BusinessmanUser BusinessmanUser { get; set; }
        [ForeignKey(nameof(ContractorId))]
        [InverseProperty(nameof(EfClasses.Contractor.BusinessmanUserInContractors))]
        public virtual Contractor Contractor { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }

        public void Passive(ref BusinessmanUserInContractor entity)
        {
            entity.StateId = StateIdConst.PASSIVE;
        }
    }
}

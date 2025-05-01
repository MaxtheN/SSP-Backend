using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Integration.Bojxona;
using StatusGeneric;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.IntegrationServices.Bojxona
{
    public class BojxonaIntegrationService :StatusGenericHandler, IBojxonaIntegrationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BojxonaIntegrationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public HaveId<int> CreateBojxonaImtiyoz(BojxonaImtiyozDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = _unitOfWork.Context.BojxonaImtiyozs.FirstOrDefault(a => a.Inn == dto.Inn);
                    if (entity == null)
                    {
                        entity = new BojxonaImtiyoz
                        {
                            Inn = dto.Inn,
                            AppCount = dto.AppCount,
                            RejCount = dto.RejCount,
                            DevCount = dto.DevCount,
                            Sum = dto.Sum,
                            GrChanCount = dto.GrChanCount,
                        };
                        _unitOfWork.Context.BojxonaImtiyozs.Add(entity);
                    }
                    else
                    {
                        entity.Inn = dto.Inn;
                        entity.AppCount = dto.AppCount;
                        entity.RejCount = dto.RejCount;
                        entity.DevCount = dto.DevCount;
                        entity.Sum = dto.Sum;
                        entity.GrChanCount = dto.GrChanCount;
                    }

                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return null;
                    }

                    _unitOfWork.Context.SaveChanges();
                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return null;
                    }

                    transaction.Commit();
                    return HaveId.Create(entity.Id);
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}

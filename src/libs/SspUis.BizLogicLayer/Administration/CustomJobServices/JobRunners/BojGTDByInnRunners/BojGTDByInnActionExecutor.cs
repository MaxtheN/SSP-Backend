using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Exapidata.Boj;
using SspUis.DataLayer.EfClasses.Exapidata.Tax;
using SspUis.Integration.Bojxona.Models;
using SspUis.Integration.Bojxona.Services;
using SspUis.Integration.Soliq.Models;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners.BojGTDByInnRunners
{
    public interface IBojGTDByInnActionExecutor : ICustomJobActionExecuter<BojGTDByInnActionInputData>
    {
        
    }

    public class BojGTDByInnActionExecutor : StatusGenericHandler, IBojGTDByInnActionExecutor
    {
        private readonly IBojxonaService _bojxonaService;
        private readonly IUnitOfWork _unitOfWork;

        public BojGTDByInnActionExecutor(IBojxonaService bojxonaService, IUnitOfWork unitOfWork)
        {
            _bojxonaService = bojxonaService;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomJobActionExecuteResult> Execute(CustomJob job, BojGTDByInnActionInputData actionInputData)
        {
            int year = string.IsNullOrEmpty(job.ExtendData) ? DateTime.Today.Year : int.Parse(job.ExtendData);
            bool isHaveAnyData = false;
            var result = new CustomJobActionExecuteResult
            {
                FromCache = !job.IsForceUpdate
            };
            var entity = GetEntity(actionInputData.Tin, year);
            bool isGetFromApi = false;

            if (entity == null || entity.Count==0)
            {
                isGetFromApi = true;
                result.FromCache = false;
               // entity = new List<GTDByInn>();
                
               // _unitOfWork.Context.Add(entity);
            }
            else if (job.IsForceUpdate)
                isGetFromApi = true;

            if (isGetFromApi)
            {
                var dataFromIntegration = await _bojxonaService.GetGTDByInn(new GetGTDByInnRequestDto { Stir = actionInputData.Tin, Year = year.ToString() });
                CombineStatuses(_bojxonaService);
                if (dataFromIntegration == null || dataFromIntegration.Count==0 || HasErrors )
                {
                    foreach (var item in entity)
                    {
                        _unitOfWork.Context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Detached; 
                    }
                }
                else
                {
                    isHaveAnyData = true;
                    foreach (var item in entity)
                    {
                        _unitOfWork.Context.GTDByInns.Remove(item);
                    }
                    var entityItem = new GTDByInn();
                    var entityItemGood = new GTDByInnGood();
                    foreach (var item in dataFromIntegration)
                    {
                        entityItem.Year = year;
                        entityItem.Tin = actionInputData.Tin;
                        entityItem.Organization1Adress = item.Organization1adress;
                        entityItem.Organization1Name = item.Organization1name;
                        entityItem.Organization2Adress = item.Organization2adress;
                        entityItem.Organization2Name = item.Organization2name;
                        entityItem.EkimCountryCode = item.EkimcountryCode;
                        entityItem.ExternalId = item.Id;
                        entityItem.Mode = item.Mode;
                        entityItem.TypeIncoterms = item.Typeincoterms;
                        entityItem.TypeTransport = item.Typetransport;
                        foreach(var itemGood in item.Goods)
                        {
                            entityItemGood.UnitGoods = itemGood.UnitGoods;
                            entityItemGood.NetMassGoods = itemGood.NetMassGoods;
                            entityItemGood.NumberGoods = itemGood.NumberGoods;
                            entityItemGood.CodeTiftnGoods = itemGood.CodeTiftnGoods;
                            entityItemGood.AdditionalUnitgoods = itemGood.AdditionalUnitgoods;
                            entityItemGood.NumContract = itemGood.NumContract;
                            entityItemGood.UnitGoods = itemGood.UnitGoods;
                            entityItemGood.ValueGoods = itemGood.ValueGoods;
                            entityItemGood.ProductName = itemGood.ProductName;
                            entityItem.Goods.Add(entityItemGood);
                        }
                        _unitOfWork.Context.GTDByInns.Add(entityItem);
                    }
                    _unitOfWork.Save();
                }
            }
            if (HasErrors)
                return null;

            if (!result.FromCache && !isHaveAnyData)
            {
                AddError("Bojxonadan ma'lumot topilmadi!");
                return null;
            }
            return result;
        }

        private List<GTDByInn> GetEntity(string tin, int year)
        {
            return _unitOfWork.Context.GTDByInns.Include(a => a.Goods)
                .Where(a => a.Tin == tin && a.Year == year).ToList();
        }

    }
}

using System;
using System.Linq;
using Newtonsoft.Json;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.ReportServices;

public class MemshipDocsInfoDtoFilter
{
    public int? RegionId { get; set; }
    public int? DistrictId { get; set; }
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public long? ContractorId { get; set; }
    public bool? IsOld { get; set; }
    public int? ContractorCategoryId { get; set; }
    public int? MemshipContractTypeId { get; set; }
    public DateOnly? FromDocDate { get; set; }
    public DateOnly? ToDocDate { get; set; }
    public bool? IsPinfl { get; set; }
}
public class MemshipDocsInfoReestrDtoFilter : MemshipDocsInfoDtoFilter, ISortFilterOptions, IPageOptions
{
    public MemshipDocsInfoReestrDtoFilter()
    {
        Init();
    }

    private string _orderType;
    public string Search { get; set; }

    public string SortBy { get; set; }

    public virtual string OrderType
    {
        get
        {
            return _orderType;
        }
        set
        {
            _orderType = (new string[2] { "ASC", "DESC" }.Contains(value.AsString().ToUpper()) ? value.ToUpper() : "ASC");
        }
    }

    private int _page;

    private int _pageSize = 20;


    public virtual int Page
    {
        get
        {
            return _page;
        }
        set
        {
            _page = ((value <= 0) ? 1 : value);
        }
    }

    public virtual int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = ((value > 0 && value <= 1000) ? value : 20);
        }
    }

    private void Init()
    {
        _orderType = "ASC";
    }

    public virtual bool HasSort()
    {
        return !SortBy.NullOrEmpty();
    }

    public virtual bool HasSearch()
    {
        return !Search.NullOrEmpty();
    }
}

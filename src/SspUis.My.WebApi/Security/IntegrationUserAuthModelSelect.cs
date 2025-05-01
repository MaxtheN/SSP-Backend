using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using SspUis.Core.Configurations;

namespace SspUis.My.WebApi.Security;

public static class IntegrationUserAuthModelSelect
{
    public static IEnumerable<IntegrationUserAuthModel> MapToAuthModel(this IEnumerable<IntegrationUserConfig> users)
    {
        return users.Select(a => new IntegrationUserAuthModel
        {
            UserName = a.UserName,
            Password = a.Password,
            Inn = a.Inn,
        });
    }
}

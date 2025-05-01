using System;
using System.Linq.Expressions;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer;

public class MemshipContractTypeIdConst
{
    /// <summary>
    /// Pullik
    /// </summary>
    public const int PAID = 1;

    /// <summary>
    /// Tekin
    /// </summary>
    public const int FREE = 2;
}

public class SignerPositionId
{
    /// <summary>
    /// Rais
    /// </summary>
    public const int Chairman = 6;

    /// <summary>
    /// Boshqarma boshliqi
    /// </summary>

    public const int DepartmentHead = 9;
    /// <summary>
    /// Boshqarma boshliqi birinchi o'rinbosari
    /// </summary>
    public const int DebutyHeadOfDepartment1 = 29;
    /// <summary>
    /// Boshqarma boshliqi o'rinbosari
    /// </summary>
    public const int DebutyHeadOfDepartment = 13;

}

public class ContractorCategoryIdConst
{
    public const int URTA_KORHONA = 1;
    public const int MIKROFIRMA = 2;
    public const int KICHIK_KORXONA = 3;
    public const int YIRIK_KORXONA = 4;

    public static bool IsPayed(int contractorCategoryId, int opfId)
    {
        //opfId = opfId ?? 0;
        return (contractorCategoryId == YIRIK_KORXONA
                    || opfId == OpfIdConst.UYUSHMA);
    }

    //public static bool IsPayed(int contractorCategoryId, int? opfId)
    //{
    //    opfId = opfId ?? 0;
    //    return (contractorCategoryId == YIRIK_KORXONA
    //                || opfId == OpfIdConst.UYUSHMA);
    //}

    public static Expression<Func<ContractorCategory, bool>> IsPayedExpression()
    {
        // Create a parameter expression for the Contractor entity
        var contractor = Expression.Parameter(typeof(ContractorCategory), "c");

        // Create a constant expression for the YIRIK_KORXONA value
        var yirikKorxona = Expression.Constant(YIRIK_KORXONA);

        // Create a member expression for the ContractorCategoryId property
        var contractorCategoryId = Expression.Property(contractor, "Id");

        // Create a binary expression for the equality comparison
        var equalYirikKorxona = Expression.Equal(contractorCategoryId, yirikKorxona);

        // Create a constant expression for the OpfIdConst.UYUSHMA value
        var uyushma = Expression.Constant(OpfIdConst.UYUSHMA);

        // Create a member expression for the OpfId property
        var opfId = Expression.Property(contractor, "OpfId");

        // Create a coalesce expression for the OpfId value or zero
        var opfIdOrZero = Expression.Coalesce(opfId, Expression.Constant(0));

        // Create a binary expression for the equality comparison
        var equalUyushma = Expression.Equal(opfIdOrZero, uyushma);

        // Create a binary expression for the logical or operation
        var orExpression = Expression.OrElse(equalYirikKorxona, equalUyushma);

        // Create a lambda expression from the binary expression and the parameter expression
        var lambda = Expression.Lambda<Func<ContractorCategory, bool>>(orExpression, contractor);

        // Return the lambda expression
        return lambda;
    }


}

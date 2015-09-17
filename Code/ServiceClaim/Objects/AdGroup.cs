using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceClaim.Objects
{
    public enum AdGroup
    {
        None,
        ZipClaimClient,
        ZipClaimClientCounterView,
        ZipClaimClientZipView,
        SuperAdmin,
        ServiceAdmin,
        ServiceManager,
        ServiceEngeneer,
        ServiceOperator,
        ServiceControler,
        ServiceTech,
        ServiceClaimContractorAccess,
        //---доступы
        ServiceClaimClassifier,
        ServiceClaimClientAccess
    }
}
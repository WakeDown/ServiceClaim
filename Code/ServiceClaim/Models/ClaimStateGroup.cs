using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class ClaimStateGroup:DbModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SysName { get; set; }
        public int OrderNum { get; set; }
        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }
        public string BorderColor { get; set; }
        public int ClaimCount { get; set; }

        public static IEnumerable<ClaimStateGroup> GetFilterList(string servAdminSid = null, string servEngeneerSid = null, DateTime? dateStart = null, DateTime? dateEnd = null, int? topRows = null, string managerSid = null, string techSid = null, string serialNum = null, int? idDevice = null, bool activeClaimsOnly = false, int? idClaimState = null, string client = null, string clientSdNum = null, int? claimId = null, string deviceName = null, int? pageNum = null, int[] groupStateList = null, string address = null, string servManagerSid = null, int? idState = null, string dateCreate = null, string curSpec = null)
        {
            string groupStates = null;
            if (groupStateList != null && groupStateList.Any()) groupStates = String.Join(",", groupStateList);
            
            Uri uri = new Uri($"{OdataServiceUri}/ClaimState/GetGroupFilterList?servAdminSid={servAdminSid}&servManagerSid={servManagerSid}&servEngeneerSid={servEngeneerSid}&dateStart={dateStart}&dateEnd={dateEnd}&topRows={topRows}&managerSid={managerSid}&techSid={techSid}&serialNum={serialNum}&idDevice={idDevice}&activeClaimsOnly={activeClaimsOnly}&idClaimState={idClaimState}&client={client}&clientSdNum={clientSdNum}&claimId={claimId}&deviceName={deviceName}&pageNum={pageNum}&groupStates={groupStates}&address={address}&idState={idState}&dateCreate={dateCreate}&curSpec={curSpec}");

            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ClaimStateGroup>>(jsonString);
            return model;
        }
    }
}
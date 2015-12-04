using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class Claim:DbModel
    {
        public int Id { get; set; }
        public string Sid { get; set; }
        public int IdContractor { get; set; }
        public Contractor Contractor { get; set; }
        public Contract Contract { get; set; }
        public int IdContract { get; set; }
        public Device Device { get; set; }
        public string ContractorName { get; set; }
        public string ContractName { get; set; }
        public string DeviceName { get; set; }
       
        public ClaimState State { get; set; }
        public DateTime DateStateChange { get; set; }
        public int? IdWorkType { get; set; }
        public WorkType WorkType { get; set; }
        public string SpecialistSid { get; set; }
        public EmployeeSm Specialist { get; set; }
        public ServiceSheet ServiceSheet4Save { get; set; }
        public DateTime DateCreate { get; set; }
        public ServiceIssue ServiceIssue4Save { get; set; }
        public string ClientSdNum { get; set; }
        public string CurEngeneerSid { get; set; }
        public string CurAdminSid { get; set; }
        public string CurTechSid { get; set; }
        public string CurManagerSid { get; set; }
        public EmployeeSm Admin { get; set; }
        public EmployeeSm Engeneer { get; set; }
        public EmployeeSm Manager { get; set; }
        public EmployeeSm Tech { get; set; }
        public EmployeeSm Changer { get; set; }
        public bool ContractUnknown { get; set; }
        public bool DeviceUnknown { get; set; }
        public string AddressStrId { get; set; }
        public string DateStateChangeStr => string.Format("{0:dd.MM.yy HH:mm}", DateStateChange);
        public string DateCreateStr => string.Format("{0:dd.MM.yy HH:mm}", DateCreate);
        public string ClaimTypeSysName { get; set; }
        public int? IdCity { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public bool DeviceCollective { get; set; }
        public string CurClientManagerSid { get; set; }
        public EmployeeSm ClientManager { get; set; }

        public string StateChangeDateDiffStr
        {
            get
            {
                string result = "";
                double mins  = (DateTime.Now - DateStateChange).TotalMinutes;
                int hrs = (int) mins/60;
                int mns = (int) mins%60;
                if (hrs < 0) hrs = 0;
                if (mns < 0) mns = 0;
                //if (hrs == 0)
                //{
                //    result = String.Format("{0}м", mns);
                //}
                //else
                //{
                    result = String.Format("{0}ч. {1}м", hrs, mns);
                //}
                return result;
            }
        }

        public string CreateDateDiffStr
        {
            get
            {
                string result = "";
                double mins = (DateTime.Now - DateCreate).TotalMinutes;
                int hrs = (int)mins / 60;
                int mns = (int)mins % 60;
                //if (hrs == 0)
                //{
                //    result = String.Format("{0}м", mns);
                //}
                //else
                //{
                result = String.Format("{0}ч. {1}м", hrs, mns);
                //}
                return result;
            }
        }

        public string Descr { get; set; }

        public Claim()
        {
            ServiceSheet4Save=new ServiceSheet();
            ServiceIssue4Save=new ServiceIssue();
            //Contractor=new Contractor();
            //Contract = new Contract();
            //Device=new Device();
            //Admin=new EmployeeSm();
            //Engeneer=new EmployeeSm();
        }

        public static Claim Get(int id)
        {
            Uri uri = new Uri(String.Format("{0}/Claim/Get?id={1}", OdataServiceUri, id));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<Claim>(jsonString);
            return model;
            //FillSelf(model);
        }

        //private void FillSelf(Claim model)
        //{
        //    Id = model.Id;
        //    Sid = model.Sid;
        //    Contractor = model.Contractor;
        //    Contract = model.Contract;
        //    Device = model.Device;
        //    ContractorName = model.ContractorName;
        //    ContractName = model.ContractName;
        //    DeviceName = model.DeviceName;
        //    Admin = model.Admin;
        //    Engeneer = model.Engeneer;
        //    State = model.State;
        //    IdWorkType = model.IdWorkType;
        //    WorkType = model.WorkType;
        //    SpecialistSid = model.SpecialistSid;
        //    Specialist = model.Specialist;
        //    DateCreate = model.DateCreate;
        //    DateStateChange = model.DateStateChange;
        //    ClientSdNum = model.ClientSdNum;
        //    CurEngeneerSid = model.CurEngeneerSid;
        //    CurAdminSid = model.CurAdminSid;
        //    CurTechSid = model.CurTechSid;
        //    CurManagerSid = model.CurManagerSid;
        //}

        public async Task<ListResult<Claim>> GetListAsync(string servAdminSid = null, string servEngeneerSid = null, DateTime? dateStart = null, DateTime? dateEnd = null, int? topRows = null, string managerSid = null, string techSid = null, string serialNum = null, int? idDevice = null, bool activeClaimsOnly = false, int? idClaimState = null, int? clientId = null, string clientSdNum = null, int? claimId = null, string deviceName = null, int? pageNum = null, int[] groupStateList = null, string address = null, string servManagerSid = null)
        {
            string groupStates = null;
            if (groupStateList != null && groupStateList.Any()) groupStates=String.Join(",", groupStateList);


            Uri uri = new Uri($"{OdataServiceUri}/Claim/GetListAsync?servAdminSid={servAdminSid}&servManagerSid={servManagerSid}&servEngeneerSid={servEngeneerSid}&dateStart={dateStart}&dateEnd={dateEnd}&topRows={topRows}&managerSid={managerSid}&techSid={techSid}&serialNum={serialNum}&idDevice={idDevice}&activeClaimsOnly={activeClaimsOnly}&idClaimState={idClaimState}&clientId={clientId}&clientSdNum={clientSdNum}&claimId={claimId}&deviceName={deviceName}&pageNum={pageNum}&groupStates={groupStates}&address={address}");
            string jsonString = await GetApiClientAsync().GetStringAsync(uri);
            var model = JsonConvert.DeserializeObject<ListResult<Claim>>(jsonString);
            return model;
        }

        public static ListResult<Claim> GetList(string servAdminSid = null, string servEngeneerSid = null, DateTime? dateStart = null, DateTime? dateEnd = null, int? topRows = null, string managerSid = null, string techSid = null, string serialNum = null, int? idDevice = null, bool activeClaimsOnly = false, int? idClaimState = null, int? clientId = null)
        {
            Uri uri = new Uri($"{OdataServiceUri}/Claim/GetList?servAdminSid={servAdminSid}&servEngeneerSid={servEngeneerSid}&dateStart={dateStart}&dateEnd={dateEnd}&topRows={topRows}&managerSid={managerSid}&techSid={techSid}&serialNum={serialNum}&idDevice={idDevice}&activeClaimsOnly={activeClaimsOnly}&idClaimState={idClaimState}&clientId={clientId}");
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<ListResult<Claim>>(jsonString);
            return model;
        }

        public async Task<ResponseMessage> SaveAsync()
        {
            Uri uri = new Uri(String.Format("{0}/Claim/Save", OdataServiceUri));
            string json = JsonConvert.SerializeObject(this);
            var content = new StringContent(json,Encoding.UTF8,"application/json");
            var response = await GetApiClientAsync().PostAsync(uri, content);
            //bool result = PostJson(uri, json, out responseMessage);
            //var response =  DeserializeResponse();
            var res = DeserializeResponse(await response.Content.ReadAsStreamAsync());
            if (response.StatusCode == HttpStatusCode.OK)
            {
                throw new Exception(res.ErrorMessage);
            }
            return  res;
        }

        public bool Save(out ResponseMessage responseMessage)
        {
            Uri uri = new Uri(String.Format("{0}/Claim/Save", OdataServiceUri));
            string json = JsonConvert.SerializeObject(this);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            //var response = GetApiClientAsync().PostAsync(uri, content).Result;
            bool result = PostJson(uri, json, out responseMessage);
            //var response =  DeserializeResponse();
            return result;
        }

        //public bool Save(out ResponseMessage responseMessage)
        //{
        //    Uri uri = new Uri(String.Format("{0}/Claim/Save", OdataServiceUri));
        //    string json = JsonConvert.SerializeObject(this);
        //    bool result = PostJson(uri, json, out responseMessage);
        //    return result;
        //}

        //public bool SaveAndGoNextState(out ResponseMessage responseMessage)
        //{
        //    Uri uri = new Uri(String.Format("{0}/Claim/SaveAndGoNextState", OdataServiceUri));
        //    string json = JsonConvert.SerializeObject(this);
        //    bool result = PostJson(uri, json, out responseMessage);
        //    return result;
        //}

        public bool Go(out ResponseMessage responseMessage)
        {
            Uri uri = new Uri(String.Format("{0}/Claim/Go", OdataServiceUri));
            string json = JsonConvert.SerializeObject(this);
            bool result = PostJson(uri, json, out responseMessage);
            return result;
        }

        public bool GoBack(out ResponseMessage responseMessage)
        {
            Uri uri = new Uri(String.Format("{0}/Claim/GoBack", OdataServiceUri));
            string json = JsonConvert.SerializeObject(this);
            bool result = PostJson(uri, json, out responseMessage);
            return result;
        }

        //public bool SaveAndGoEndState(out ResponseMessage responseMessage)
        //{
        //    Uri uri = new Uri(String.Format("{0}/Claim/SaveAndGoEndState", OdataServiceUri));
        //    string json = JsonConvert.SerializeObject(this);
        //    bool result = PostJson(uri, json, out responseMessage);
        //    return result;
        //}

        public IEnumerable<Claim2ClaimState> GetStateHistory()
        {
            Uri uri = new Uri(String.Format("{0}/Claim/GetStateHistory?id={1}", OdataServiceUri, Id));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<Claim2ClaimState>>(jsonString);
            return model;
        }

        public static IEnumerable<Claim2ClaimState> GetClaimStateHistory(int idClaim, int? topRows)
        {
            Uri uri = new Uri($"{OdataServiceUri}/Claim/GetStateHistory?id={idClaim}&topRows={topRows}");
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<Claim2ClaimState>>(jsonString);
            return model;
        }

        //public static IEnumerable<Claim2ClaimState> GetStateHistory()
        //{
        //    Uri uri = new Uri(String.Format("{0}/Claim/GetStateHistory?id={1}", OdataServiceUri, Id));
        //    string jsonString = GetJson(uri);
        //    var model = JsonConvert.DeserializeObject<IEnumerable<Claim2ClaimState>>(jsonString);
        //    return model;
        //}

        public static IEnumerable<KeyValuePair<string, string>> GetWorkTypeSpecialistSelectionList(int idWorkType)
        {
            Uri uri = new Uri(String.Format("{0}/Claim/GetWorkTypeSpecialistSelectionList?idWorkType={1}", OdataServiceUri, idWorkType));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<string, string>>>(jsonString);

            return model;
        }

        public SelectList GetCurrentClaimSpecialistSelectionList()
        {
            Uri uri = new Uri(String.Format("{0}/Claim/GetCurrentClaimSpecialistList?id={1}", OdataServiceUri, Id));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<string, string>>>(jsonString);

            return new SelectList(model, "Key", "Value");
        }

        public ServiceSheet GetLastServiceSheet()
        {
            Uri uri = new Uri(String.Format("{0}/Claim/GetLastServiceSheet?idClaim={1}", OdataServiceUri, Id));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<ServiceSheet>(jsonString);
            return model;
        }

        public static IEnumerable<ServiceSheet> GetClaimServiceSheetList(int idClaim, bool? payedWrap = null)
        {
            Uri uri = new Uri($"{OdataServiceUri}/Claim/GetClaimServiceSheetList?idClaim={idClaim}&payedWrap={payedWrap}");
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ServiceSheet>>(jsonString);
            return model;
        }

        public IEnumerable<ServiceSheet> GetClaimServiceSheetList(bool? payedWrap = null)
        {
            Uri uri = new Uri($"{OdataServiceUri}/Claim/GetClaimServiceSheetList?idClaim={Id}&payedWrap={payedWrap}");
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ServiceSheet>>(jsonString);
            return model;
        }

        public static IEnumerable<ZipClaim> GetClaimZipClaimList(int idClaim)
        {
            Uri uri = new Uri(String.Format("{0}/Claim/GetClaimZipClaimList?idClaim={1}", OdataServiceUri, idClaim));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ZipClaim>>(jsonString);
            return model;
        }

        public IEnumerable<ZipClaim> GetClaimZipClaimList()
        {
            Uri uri = new Uri(String.Format("{0}/Claim/GetClaimZipClaimList?idClaim={1}", OdataServiceUri, Id));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ZipClaim>>(jsonString);
            return model;
        }

        public IEnumerable<ServiceSheetZipItem> GetOrderedZipItemList(int? notInserviceSheetId)
        {
            Uri uri = new Uri($"{OdataServiceUri}/Claim/GetOrderedZipItemList?claimId={Id}&notInserviceSheetId={notInserviceSheetId}");
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ServiceSheetZipItem>>(jsonString);
            return model;
        }

        public IEnumerable<ServiceSheetZipItem> GetCartridgeList()
        {
            return new List<ServiceSheetZipItem>();
        }
    }
}
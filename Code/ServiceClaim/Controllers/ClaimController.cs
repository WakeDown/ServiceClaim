using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Newtonsoft.Json;
using ServiceClaim.Helpers;
using ServiceClaim.Models;
using ServiceClaim.Objects;

namespace ServiceClaim.Controllers
{

    public class ClaimController : BaseController
    {
        //[HttpGet]
        //public ActionResult Index2(int? id)
        //{
        //    if (!id.HasValue) return RedirectToAction("New");
        //    Claim model = new Claim(id.Value);
        //    return View(model);
        //}
        [HttpPost]
        public JsonResult Add2Session(string name, string value)
        {
            Session[name] = value;
            return Json(new {});
        }
        [HttpPost]
        public JsonResult GetFromSession(string name)
        {
            var value = Session[name];
            return Json(new { val= value });
        }

        [HttpPost]
        public JsonResult RemoveFromSession(string name)
        {
            Session.Remove(name);
            return Json(new {});
        }

        [HttpGet]
        public ActionResult Index(int? id)
        {
            if (
                !CurUser.HasAccess(AdGroup.ServiceTech, AdGroup.ServiceAdmin, AdGroup.ServiceControler,
                    AdGroup.ServiceEngeneer, AdGroup.ServiceManager, AdGroup.ServiceOperator, AdGroup.AddNewClaim, AdGroup.ServiceClaimView))
                return HttpNotFound();

            if (!id.HasValue || id <= 0) return RedirectToAction("New");
            Claim model = new Claim();
            try
            {
                model = new Claim(id.Value);
            }
            catch (Exception ex)
            {
                return RedirectToAction("HandledError", "Error", new { message = ex.Message });
            }

            return View(model);
        }

        //[HttpPost]
        //public ActionResult Index(Claim model)
        //{
        //    //var model = new Claim(id);
        //    return View();
        //}

        public JsonResult ClaimSave(int? id, string descr)
        {
            //try
            //{
            if (!id.HasValue) throw new ArgumentException("Не указана заявка!");
            ResponseMessage responseMessage;
            var model = new Claim();
            model.Id = id.Value;
            model.Descr = descr;
            model.Save(CurUser.Sid);
            //bool complete = model.Save(out responseMessage);
            //if (!complete) throw new Exception(responseMessage.ErrorMessage);
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { errorMessage = ex.Message });
            //}
            return Json(new { });
        }

        //public JsonResult ClaimSave(int? id, string descr)
        //{
        //    try
        //    {
        //        if (!id.HasValue) throw new ArgumentException("Не указана заявка!");
        //        ResponseMessage responseMessage;
        //        var model = new Claim();
        //        model.Id = id.Value;
        //        model.Descr = descr;
        //        bool complete = model.Save(out responseMessage);
        //        if (!complete) throw new Exception(responseMessage.ErrorMessage);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { errorMessage = ex.Message });
        //    }
        //    return Json(new { });
        //}

        public JsonResult ClaimContinue(int? id, string descr)
        {
            //try
            //{
            if (!id.HasValue) throw new ArgumentException("Не указана заявка!");
            ResponseMessage responseMessage;
            var model = new Claim();
            model.Id = id.Value;
            model.Descr = descr;
            model.Save(CurUser.Sid);
            //if (!complete) throw new Exception(responseMessage.ErrorMessage);
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { errorMessage = ex.Message });
            //}
            return Json(new { });
        }

        //public JsonResult ClaimContinue(int? id, string descr)
        //{
        //    try
        //    {
        //        if (!id.HasValue) throw new ArgumentException("Не указана заявка!");
        //        ResponseMessage responseMessage;
        //        var model = new Claim();
        //        model.Id = id.Value;
        //        model.Descr = descr;
        //        bool complete = model.Save(out responseMessage);
        //        if (!complete) throw new Exception(responseMessage.ErrorMessage);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { errorMessage = ex.Message });
        //    }
        //    return Json(new { });
        //}
        //public JsonResult ClaimEnd(int? id, string descr)
        //{
        //    try
        //    {
        //        if (!id.HasValue) throw new ArgumentException("Не указана заявка!");
        //        ResponseMessage responseMessage;
        //        var model = new Claim();
        //        model.Id = id.Value;
        //        model.Descr = descr;
        //        bool complete = model.SaveAndGoEndState(out responseMessage);
        //        if (!complete) throw new Exception(responseMessage.ErrorMessage);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { errorMessage = ex.Message });
        //    }
        //    return Json(new { });
        //}
        public async Task<ActionResult> List(int? state, int? client, int? topRows)
        {
            //if (!CurUser.HasAccess(AdGroup.ServiceTech, AdGroup.ServiceAdmin, AdGroup.ServiceControler,
            //        AdGroup.ServiceEngeneer, AdGroup.ServiceManager, AdGroup.ServiceOperator))
            //    return HttpNotFound();

            //ViewBag.userIsEngeneer = ViewBag.CurUser.HasAccess(AdGroup.ServiceEngeneer);

            //ListResult<Claim> result = await new Claim().GetListAsync(topRows: topRows, idClaimState: state, clientId: client);
            //return View(result);
            return View();
        }

        [HttpPost]
        public JsonResult GetClaimList(int? idDevice = null, string client = null, int? claimId = null, string clientSdNum = null, string deviceName = null, string serialNum = null, int? topRows = null, int? pageNum = null, int[] groupStateList = null, string address = null, int? idState = null, string dateCreate = null, string curSpec = null)
        {
            //if (!CurUser.HasAccess(AdGroup.ServiceTech, AdGroup.ServiceAdmin, AdGroup.ServiceControler,
            //        AdGroup.ServiceEngeneer, AdGroup.ServiceManager, AdGroup.ServiceOperator))
            //    return null;

            //ViewBag.userIsEngeneer = ViewBag.CurUser.HasAccess(AdGroup.ServiceEngeneer);

            string servAdminSid = null;
            string servEngeneerSid = null;
            string managerSid = null;
            string techSid = null;
            string servManagerSid = null;
            if (CurUser.Is(AdGroup.ServiceAdmin)) servAdminSid = CurUser.Sid;
            if (CurUser.Is(AdGroup.ServiceEngeneer)) servEngeneerSid = CurUser.Sid;
            if (CurUser.Is(AdGroup.ServiceCenterManager)) servManagerSid = CurUser.Sid;
            if (CurUser.Is(AdGroup.ServiceManager)) managerSid = CurUser.Sid;
            if (CurUser.Is(AdGroup.ServiceTech)) techSid = CurUser.Sid;

            //var result = Claim.GetList();
            string groupStates = null;
            if (groupStateList != null && groupStateList.Any()) groupStates=String.Join(",", groupStateList);
            ListResult<Claim> result = Claim.GetList(GetCurUser(), adminSid: servAdminSid, engeneerSid: servEngeneerSid, managerSid: managerSid, techSid: techSid, servManagerSid: servManagerSid,  client: client, claimId: claimId, clientSdNum: clientSdNum, deviceName: deviceName, serialNum: serialNum, topRows: topRows, pageNum: pageNum, groupStates: groupStates, address: address, idDevice: idDevice, idState: idState, dateCreate: dateCreate, curSpec: curSpec);
            return Json(result);
        }

        [HttpPost]
        public PartialViewResult GetListItem(int? claimId)
        {
            if (!claimId.HasValue) return null;
            var claim = new Claim(claimId.Value);
            ViewBag.userIsEngeneer = ViewBag.CurUser.HasAccess(AdGroup.ServiceEngeneer);
            return PartialView("ListItem", claim);
        }

        //public ActionResult List(int? state, int? client, int? topRows)
        //{
        //    if (
        //        !CurUser.HasAccess(AdGroup.ServiceTech, AdGroup.ServiceAdmin, AdGroup.ServiceControler,
        //            AdGroup.ServiceEngeneer, AdGroup.ServiceManager, AdGroup.ServiceOperator))
        //        return HttpNotFound();

        //    ListResult<Claim> result = Claim.GetList(topRows: topRows, idClaimState: state, clientId: client);
        //    return View(result);
        //}

        [HttpGet]
        public ActionResult New()
        {
            return View(new Claim());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(Claim model)
        {
            //if (!CurUser.UserCanCreateClaim()) return RedirectToAction("AccessDenied", "Error");
            //Создаем заявку с основными полями и одельно первый статус с комментарием
            try
            {
                ResponseMessage responseMessage;
                model.DeviceUnknown = Request.Form["Device"] == "DeviceUnknown";
                model.ContractUnknown = Request.Form.AllKeys.Contains("ContractUnknown");
                model.ClaimTypeSysName = Request.Form["ClaimTypeSysName"];
                model.Contractor = new Contractor() { Id = MainHelper.GetValueInt(Request.Form["ctrList"]) };
                if (!model.ContractUnknown) model.Contract = new Contract() { Id = MainHelper.GetValueInt(Request.Form["contList"]) };
                if (!model.DeviceUnknown) model.Device = new Device() { Id = MainHelper.GetValueInt(Request.Form["devList"]) };
                model.Descr = Request.Form["descr"];
                model.ClientSdNum = Request.Form["client_sd_num"];
                model.AddressStrId = Request.Form["addrList"];
                model.ContactName = Request.Form["contactName"];
                model.ContactPhone = Request.Form["contactPhone"];
                model.DeviceCollective = Request.Form["Device"]== "DeviceCollective";
                model.Save(CurUser.Sid);
                //bool result = model.Save(out responseMessage);
                //var response = DbModel.DeserializeResponse(result);
                //if (!result) throw new Exception(responseMessage.ErrorMessage);
                return RedirectToAction("Index", new { id = model.Id });
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View("New", model);
            }

            //return RedirectToAction("New", model);
        }

        //[HttpPost]
        //public ActionResult New(Claim model)
        //{
        //    //if (!CurUser.UserCanCreateClaim()) return RedirectToAction("AccessDenied", "Error");

        //    //Создаем заявку с основными полями и одельно первый статус с комментарием
        //    try
        //    {
        //        ResponseMessage responseMessage;
        //        model.Contractor = new Contractor() { Id = MainHelper.GetValueInt(Request.Form["ctrList"]) };
        //        model.Contract = new Contract() { Id = MainHelper.GetValueInt(Request.Form["contList"]) };
        //        model.Device = new Device() { Id = MainHelper.GetValueInt(Request.Form["devList"]) };
        //        model.Descr = Request.Form["descr"];
        //        bool complete = model.Save(out responseMessage);
        //        if (!complete) throw new Exception(responseMessage.ErrorMessage);

        //        return RedirectToAction("Index", new { id = responseMessage.Id });
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["error"] = ex.Message;
        //        return View("New", model);
        //    }

        //    //return RedirectToAction("New", model);
        //}

        [HttpPost]
        public JsonResult GetCtors(int? idContractor = null, string contractorName = null, int? idContract = null, string contractNumber = null, int? idDevice = null, string deviceName = null)
        {
            var list = Contractor.GetServiceList(idContractor, contractorName, idContract, contractNumber, idDevice, deviceName);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetConts(int? idContractor = null, string contractorName = null, int? idContract = null, string contractNumber = null, int? idDevice = null, string deviceName = null, string addrStrId = null)
        {
            var list = Contract.GetList(idContractor, contractorName, idContract, contractNumber, idDevice, deviceName, addrStrId);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetAddrs(int? idContractor = null, int? idContract = null, int? idDevice = null, string addrName = null)
        {
            var list = Address.GetList(idContractor, idContract, idDevice, addrName);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetDevices(int? idContractor = null, string contractorName = null, int? idContract = null, string contractNumber = null, int? idDevice = null, string deviceName = null, string serialNum = null, string addrStrId = null)
        {
            var result = Device.GetSearchList(idContractor, contractorName, idContract, contractNumber, idDevice, deviceName, serialNum);
            return Json(result);
        }


        [HttpPost]
        public async Task<ActionResult> SetWorkType(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
               await model.Go(CurUser);
                //bool complete = model.Go(out responseMessage);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);

                //return View("WindowClose");
                if (!String.IsNullOrEmpty(Request.Form["AddNew"]))
                {
                    return RedirectToAction("New");
                }
                else
                {
                    return RedirectToAction("List");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }
        }

        [HttpPost]
        public async Task<ActionResult> EngeneerSelect(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                //model.ServiceIssue4Save.Descr = model.Descr;
                //model.ServiceIssue4Save.IdClaim = model.Id;
                //model.SpecialistSid = model.SpecialistSid;
                //bool complete = model.Go(out responseMessage);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
               await model.Go(GetCurUser());
                //return View("WindowClose");

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }
        }

        [HttpPost]
        public async Task<ActionResult> SpecialistSelect(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                //model.ServiceIssue4Save.Descr = model.Descr;
                //model.ServiceIssue4Save.IdClaim = model.Id;
                //model.SpecialistSid = model.SpecialistSid;
                //bool complete = model.Go(out responseMessage);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
               await model.Go(GetCurUser());
                //return View("WindowClose");


                return RedirectToAction("List");
                
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }
        }

        [HttpPost]
        public async Task<ActionResult> TechConfirmWork(Claim model)
        {
            try
            {
                if (!String.IsNullOrEmpty(Request.Form["ClaimWorkConfirm"]))
                {
                    ResponseMessage responseMessage;
                    //bool complete = model.Go(out responseMessage);
                    //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                    await model.Go(GetCurUser());
                    return RedirectToAction("Index", new { id = model.Id });
                }
                else if (!String.IsNullOrEmpty(Request.Form["ClaimWorkCancel"]))
                {
                    ResponseMessage responseMessage;
                    //model.Descr = Request.Form["ClaimWorkCancelDescr"];
                    //bool complete = model.GoBack(out responseMessage);
                    //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                    await model.Go(GetCurUser(), false);
                    return RedirectToAction("Index", new { id = model.Id });
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            //return View("WindowClose");
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<ActionResult> ConfirmWork(Claim model)
        {
            try
            {
                if (!String.IsNullOrEmpty(Request.Form["ClaimWorkConfirm"]))
                {
                    ResponseMessage responseMessage;
                    //bool complete = model.Go(out responseMessage);
                    //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                    await model.Go(GetCurUser());
                    return RedirectToAction("Index", new { id = model.Id });
                }
                else if (!String.IsNullOrEmpty(Request.Form["ClaimWorkCancel"]))
                {
                    ResponseMessage responseMessage;
                    //model.Descr = Request.Form["ClaimWorkCancelDescr"];
                    //bool complete = model.GoBack(out responseMessage);
                    //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                    await model.Go(GetCurUser(), false);
                    //return RedirectToAction("Index", new { id = responseMessage.Id });
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            //return View("WindowClose");
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<ActionResult> ServiceSheetTechForm(Claim model)
        {
            try
            {
                ResponseMessage responseMessage = null;
                bool complete = false;
                if (!String.IsNullOrEmpty(Request.Form["ServiceSheetSave"]))
                {
                    //complete = model.Go(out responseMessage);
                    await model.Go(GetCurUser());
                }
                else if (!String.IsNullOrEmpty(Request.Form["ServiceSheetCancel"]))
                {
                    //complete = model.GoBack(out responseMessage);
                    await model.Go(GetCurUser(),false);
                    return RedirectToAction("List");
                }

                if (responseMessage == null) responseMessage = new ResponseMessage();
                if (!complete) throw new Exception(responseMessage.ErrorMessage);

                return RedirectToAction("Index", new { id = model.Id });
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }
        }

        [HttpPost]
        public async Task<ActionResult> ServiceSheetTechCollectiveForm(Claim model)
        {
            try
            {
                ResponseMessage responseMessage = null;
                bool complete = false;
                if (!String.IsNullOrEmpty(Request.Form["ServiceSheetSave"]))
                {
                    //complete = model.Go(out responseMessage);
                    await model.Go(GetCurUser());
                }
                else if (!String.IsNullOrEmpty(Request.Form["ServiceSheetCancel"]))
                {
                    await model.Go(GetCurUser(),false);
                    //complete = model.GoBack(out responseMessage);
                    return RedirectToAction("List");
                }

                if (responseMessage == null) responseMessage = new ResponseMessage();
                if (!complete) throw new Exception(responseMessage.ErrorMessage);

                return RedirectToAction("Index", new { id = model.Id });
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }
        }

        [HttpPost]
        public async Task<ActionResult> StateServadmSetWait(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                //bool complete = model.Go(out responseMessage);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                await model.Go(GetCurUser());
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            //return View("WindowClose");
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<ActionResult> StateEngOutWait(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                //bool complete = model.Go(out responseMessage);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                await model.Go(GetCurUser());
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            //return View("WindowClose");
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<JsonResult> StateEngOutWaitAsync(int claimId)
        {
            ResponseMessage responseMessage;
            //bool complete = (new Claim() { Id = claimId }).Go(out responseMessage);
            //if (!complete) throw new Exception(responseMessage.ErrorMessage);
            await (new Claim() { Id = claimId }).Go(GetCurUser());
            return Json(new { });
        }

        [HttpPost]

        public async Task<ActionResult> ServiceSheetForm(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                //bool complete = model.Go(out responseMessage);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                await model.Go(GetCurUser());
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }
            if (model.ServiceSheet4Save.ZipClaim.HasValue && model.ServiceSheet4Save.ZipClaim.Value)
            {
                return RedirectToAction("Index", new {id = model.Id});
            }
            else
            {
                //return View("WindowClose");
                return RedirectToAction("List");
            }
            //return View("WindowClose");
            //return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<ActionResult> ServiceSheetFormCollective(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                //bool complete = model.Go(out responseMessage);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                await model.Go(GetCurUser());
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }
            if (model.ServiceSheet4Save.ZipClaim.HasValue && model.ServiceSheet4Save.ZipClaim.Value)
            {
                return RedirectToAction("Index", new { id = model.Id });
            }
            else
            {
                //return View("WindowClose");
                return RedirectToAction("List");
            }
            //return View("WindowClose");
            //return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<ActionResult> StateDone(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                //bool complete = model.Go(out responseMessage);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                await model.Go(GetCurUser());
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            //return View("WindowClose");
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<ActionResult> SetServEngOnWork(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                //bool complete = model.Go(out responseMessage);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                await model.Go(GetCurUser());
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            //return View("WindowClose");
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<JsonResult> SetServEngOnWorkAsync(int claimId)
        {
            //try
            //{
            ResponseMessage responseMessage;
            //bool complete = (new Claim() { Id = claimId }).Go(out responseMessage);
            //if (!complete) throw new Exception(responseMessage.ErrorMessage);
            await (new Claim() { Id = claimId }).Go(GetCurUser());
            //}
            //catch (Exception ex)
            //{
            //    return Json(new {error= ex.Message });
            //}

            return Json(new { });
        }

        [HttpPost]
        public async Task<ActionResult> ZipGetOnCheck(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                //bool complete = model.Go(out responseMessage);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                await model.Go(GetCurUser());
                return RedirectToAction("Index", new { id = model.Id });
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            //return View("WindowClose");
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<ActionResult> ZipOrder(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                //bool complete = model.Go(out responseMessage);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                await model.Go(GetCurUser());
                //var claim = Claim.Get(model.Id);
                //ServiceSheet lastServSheet = claim.GetLastServiceSheet();
                //var zipList = lastServSheet.GetOrderedZipItemList();
                //string zipListStr = JsonConvert.SerializeObject(zipList);

                //string zipListStr = "{\"zipList\":[";
                //foreach (var item in zipList)
                //{
                //    zipListStr += $"{{\"PartNum\":\"{item.PartNum}\",\"Name\":\"{item.Name}\",\"Count\":\"{item.Count}\"}}";
                //}
                //zipListStr += "]}";

                //string zipOrderUrl =
                //   $"{ConfigurationManager.AppSettings["zipClaimHost"]}/Claims/Editor?snum={claim.Device.SerialNum}&ssid={lastServSheet.Id}&servid={claim.Id}&esid={(String.IsNullOrEmpty(claim.CurEngeneerSid) ? claim.CurTechSid : claim.CurEngeneerSid)}&asid={(String.IsNullOrEmpty(claim.CurAdminSid) ? claim.CurTechSid : claim.CurAdminSid)}&csdnum={claim.ClientSdNum}&cmnt={Url.Encode(lastServSheet.Descr)}&cntr={lastServSheet.CounterMono}&cntrc={lastServSheet.CounterColor}&dvst={lastServSheet.DeviceEnabled}&zip={zipListStr}";
                //return Redirect(zipOrderUrl);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            //return View("WindowClose");
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<ActionResult> ZipConfirm(Claim model)
        {
            try
            {
                if (!String.IsNullOrEmpty(Request.Form["Confirm"]))
                {
                    ResponseMessage responseMessage;
                    //bool complete = model.Go(out responseMessage);
                    //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                    await model.Go(GetCurUser());
                    return RedirectToAction("Index", new { id = model.Id });
                }
                else if (!String.IsNullOrEmpty(Request.Form["Cancel"]))
                {
                    ResponseMessage responseMessage;
                    //model.Descr = Request.Form["ClaimWorkCancelDescr"];
                    //bool complete = model.GoBack(out responseMessage);
                    //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                    await model.Go(GetCurUser(),false);
                    //return RedirectToAction("Index", new { id = responseMessage.Id });
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            //return View("WindowClose");
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<ActionResult> ContractChoice(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                //bool complete = model.Go(out responseMessage);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                await model.Go(GetCurUser());
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            //return View("WindowClose");
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<ActionResult> ZipOrdered(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                //bool complete = model.Go(out responseMessage);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                await model.Go(GetCurUser());
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            //return View("WindowClose");
            return RedirectToAction("List");
        }


        [HttpPost]
        public async Task<ActionResult> ZipOrderConfirm(Claim model)
        {
            try
            {
                if (!String.IsNullOrEmpty(Request.Form["ZipOrderConfirm"]))
                {
                    ResponseMessage responseMessage;
                    //bool complete = model.Go(out responseMessage);
                    //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                    await model.Go(GetCurUser());
                    return RedirectToAction("Index", new { id = model.Id });
                }
                else if (!String.IsNullOrEmpty(Request.Form["ZipOrderCancel"]))
                {
                    ResponseMessage responseMessage;
                    //model.Descr = Request.Form["ClaimWorkCancelDescr"];
                    //bool complete = model.GoBack(out responseMessage);
                    //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                    await model.Go(GetCurUser(),false);
                    //return RedirectToAction("Index", new { id = responseMessage.Id });
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            //return View("WindowClose");
            return RedirectToAction("List");
        }

        [HttpPost]
        public JsonResult GetWorkTypeSpecialistSelectionList(int idWorkType)
        {
            var list = Claim.GetWorkTypeSpecialistSelectionList(idWorkType);
            return Json(list);
        }

        public ActionResult ServiceSheet(int? id)
        {
            if (!id.HasValue) return HttpNotFound();
            var model = new ServiceSheet(id.Value);
            return View(model);
        }

        //public async Task<JsonResult> GetClaimList(int? idDevice, string serialNum, string clientSdNum)
        //{
        //    var list = await new Claim().GetListAsync(idDevice: idDevice, serialNum: serialNum, clientSdNum: clientSdNum);
        //    return Json(list);
        //}

        //[HttpGet]
        //public ActionResult FullClaimHistory(int? idClaim)
        //{
        //    var stateHistory = Claim.GetStateHistory();
        //}

        [HttpGet]
        public ActionResult СlaimStateHistory(int? idClaim, bool? full = false)
        {
            if (!idClaim.HasValue) return HttpNotFound();
            int? topRows = null;
            if (full.HasValue && !full.Value)
            {
                topRows = 3;
            }

            var stateHistory = Claim.GetClaimStateHistory(idClaim.Value, topRows);

            ViewBag.ShowBtnGetAll = !topRows.HasValue || stateHistory.Count() < topRows.Value;
            return PartialView("StateHistory", stateHistory);
        }
        [HttpPost]
        public JsonResult AddServiceSheetIssuedZipItem(ServiceSheetZipItem model)
        {
            try
            {
                ResponseMessage responseMessage;
                model.IssuedSave(CurUser.Sid);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                return Json(new { id = model.Id });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }

            return Json(new { });
        }

        [HttpPost]
        public JsonResult AddServiceSheetClientGivenInstalledZipItem(ServiceSheetZipItem model)
        {
            try
            {
                ResponseMessage responseMessage;
                model.ClientGivenInstalledSave(CurUser.Sid);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                return Json(new { id = model.Id });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }

            return Json(new { });
        }

        [HttpPost]
        public JsonResult ServiceSheetIssuedZipItemDelete(int id)
        {
            try
            {
                ResponseMessage responseMessage;
                ServiceSheetZipItem.IssuedDelete(id, CurUser.Sid);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return null;
        }

        [HttpPost]
        public JsonResult AddServiceSheetOrderedZipItem(ServiceSheetZipItem model)
        {
            try
            {
                ResponseMessage responseMessage;
                model.OrderedSave(CurUser.Sid);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                return Json(new { id = model.Id });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }

            return Json(new { });
        }

        [HttpPost]
        public JsonResult ServiceSheetOrderedZipItemDelete(int id)
        {
            try
            {
                ResponseMessage responseMessage;
                 ServiceSheetZipItem.OrderedDelete(id, CurUser.Sid);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return null;
        }

        [HttpPost]
        public async Task<ActionResult> ZipIssue(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                //bool complete = model.Go(out responseMessage);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                await model.Go(GetCurUser());
                //return View("WindowClose");
                return RedirectToAction("List");
                //return RedirectToAction("Index", new { id = responseMessage.Id });
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            //return View("WindowClose");
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<ActionResult> CartridgeList(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                //bool complete = model.Go(out responseMessage);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                await model.Go(GetCurUser());
                //return View("WindowClose");
                return RedirectToAction("List");
                //return RedirectToAction("Index", new { id = responseMessage.Id });
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            //return View("WindowClose");
            //return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<ActionResult> CartridgeRefill(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                //bool complete = model.Go(out responseMessage);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                await model.Go(GetCurUser());
                //return View("WindowClose");
                return RedirectToAction("List");
                //return RedirectToAction("Index", new { id = responseMessage.Id });
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            return RedirectToAction("List");
            //return View("WindowClose");
            //return RedirectToAction("List");
        }

        [HttpPost]
        public JsonResult ServiceSheetZipItemSetInstalled(int id, int idServiceSheet)
        {
            try
            {
                ResponseMessage responseMessage;
                ServiceSheetZipItem.SetInstalled(id, idServiceSheet, CurUser.Sid);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return null;
        }

        [HttpPost]
        public JsonResult ServiceSheetZipItemSetInstalledCancel(int id, int idServiceSheet)
        {
            try
            {
                ResponseMessage responseMessage;
                ServiceSheetZipItem.SetInstalled(id, idServiceSheet, CurUser.Sid, false);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return null;
        }

        [HttpPost]
        public JsonResult ServiceSheetZipItemClientGivenInstalledDelete(int id)
        {
            try
            {
                ResponseMessage responseMessage;
               ServiceSheetZipItem.ClientGivenInstalledDelete(id, CurUser.Sid);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return null;
        }

        [HttpPost]
        public JsonResult ServiceSheetSaveNotInstalledComment(int id, string comment)
        {
            try
            {
                ResponseMessage responseMessage;
               (new ServiceSheet() {Id=id, NotInstalledComment = comment}).SaveNotInstalledComment();
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return null;
        }
        [HttpPost]
        public JsonResult SaveServiceSheetIsPayed(int serviceSheetId)
        {
            //try
            //{
                ResponseMessage responseMessage;
                 (new ServiceSheet() { Id = serviceSheetId, IsPayed = true }).SavePayed(CurUser);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
            //}
            //catch (Exception ex)
            //{
            //    return Json(ex.Message);
            //}
            return null;
        }
        [HttpPost]
        public JsonResult SaveServiceSheetIsNotPayed(int serviceSheetId, string comment)
        {
            //try
            //{
                ResponseMessage responseMessage;
                (new ServiceSheet() {Id=serviceSheetId, IsPayed = false,NotPayedComment = comment}).SavePayed(CurUser);
                //if (!complete) throw new Exception(responseMessage.ErrorMessage);
            //}
            //catch (Exception ex)
            //{
            //    return Json(ex.Message);
            //}
            return null;
        }

        [HttpPost]
        public PartialViewResult ClaimServiceSheetList(int idClaim)
        {
            var list = new Claim() {Id = idClaim}.GetClaimServiceSheetList();
            return PartialView("ClaimServiceSheetList", list);
        }

        [HttpPost]
        public JsonResult GetClientFilterList()
        {
           return Json(Contractor.GetServiceClaimFilterList());
        }

        [HttpPost]
        public JsonResult GetStateFilterList()
        {
            return Json(ClaimState.GetFilterList());
        }

        [HttpPost]
        public JsonResult GetStateGroupFilterList(int? idDevice = null, string client = null, int? claimId = null, string clientSdNum = null, string deviceName = null, string serialNum = null, int? topRows = null, int? pageNum = null, int[] groupStateList = null, string address = null, int? idState = null, string dateCreate = null, string curSpec = null)
        {
            string servAdminSid = null;
            string servEngeneerSid = null;
            string managerSid = null;
            string techSid = null;
            string servManagerSid = null;
            if (CurUser.Is(AdGroup.ServiceAdmin)) servAdminSid = CurUser.Sid;
            if (CurUser.Is(AdGroup.ServiceEngeneer)) servEngeneerSid = CurUser.Sid;
            if (CurUser.Is(AdGroup.ServiceCenterManager)) servManagerSid = CurUser.Sid;
            if (CurUser.Is(AdGroup.ServiceManager)) managerSid = CurUser.Sid;
            if (CurUser.Is(AdGroup.ServiceTech)) techSid = CurUser.Sid;

            return Json(ClaimStateGroup.GetFilterList(servAdminSid: servAdminSid, servEngeneerSid: servEngeneerSid, managerSid: managerSid, techSid: techSid, servManagerSid: servManagerSid, client: client, claimId: claimId, clientSdNum: clientSdNum, deviceName: deviceName, serialNum: serialNum, topRows: topRows, pageNum: pageNum, groupStateList: groupStateList, address: address, idDevice: idDevice, idState: idState, dateCreate: dateCreate, curSpec: curSpec));
        }

        [HttpPost]
        public PartialViewResult GetClaimServiceSheetList(int? idClaim)
        {
            if (!idClaim.HasValue) return null;
            var list = Claim.GetClaimServiceSheetList(idClaim.Value);
            return PartialView("ClaimServiceSheetList", list);
        }
        [HttpPost]
        public PartialViewResult GetClaimZipClaimList(int? idClaim)
        {
            if (!idClaim.HasValue) return null;
            var list = Claim.GetClaimZipClaimList(idClaim.Value);
            return PartialView("ClaimZipList", list);
        }
        [HttpPost]
        public PartialViewResult GetServiceSheetPaperGet(int? idClaim)
        {
            if (!ViewBag.CurUser.HasAccess(AdGroup.ServiceAdmin)) return null;

            if (!idClaim.HasValue) return null;
           var  list = Claim.GetClaimServiceSheetList(idClaim.Value, false);
            return PartialView("ServiceSheetPaperGet", list);
        }

        [HttpPost]
        public JsonResult CheckDeviceIsExists(string serialNum, int idClaim)
        {
            bool exists = Device.CheckSerialNumIsExists(serialNum, idClaim);
            return Json(new {exists = exists});
        }

        [HttpPost]
        public JsonResult GetModelSelectionList(string model)
        {
            var list = Device.GetModelSelectionList(model);
             return Json(list);
        }

        [HttpPost]
        public ActionResult ClaimCancel(Claim model)
        {
            if (CurUser.HasAccess(AdGroup.ServiceControler, AdGroup.ServiceClaimCancelClaim))
            {
                model.Cancel(CurUser.Sid);
            }
            else
            {
                return HttpNotFound();
            }

            return RedirectToAction("List");
        }
    }
}
﻿using System;
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


        [HttpGet]
        public ActionResult Index(int? id)
        {
            if (
                !CurUser.HasAccess(AdGroup.ServiceTech, AdGroup.ServiceAdmin, AdGroup.ServiceControler,
                    AdGroup.ServiceEngeneer, AdGroup.ServiceManager, AdGroup.ServiceOperator))
                return HttpNotFound();

            if (!id.HasValue || id <= 0) return RedirectToAction("New");
            Claim model=new Claim();
            try
            {
                model = new Claim(id.Value);
            }
            catch (Exception ex)
            {
                return RedirectToAction("HandledError", "Error", new { message= ex.Message});
            }
            
            return View(model);
        }

        //[HttpPost]
        //public ActionResult Index(Claim model)
        //{
        //    //var model = new Claim(id);
        //    return View();
        //}

        public async Task<JsonResult> ClaimSave(int? id, string descr)
        {
            //try
            //{
            if (!id.HasValue) throw new ArgumentException("Не указана заявка!");
                ResponseMessage responseMessage;
                var model = new Claim();
                model.Id = id.Value;
                model.Descr = descr;
                 await model.SaveAsync();
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

        public async Task<JsonResult> ClaimContinue(int? id, string descr)
        {
            //try
            //{
                if (!id.HasValue) throw new ArgumentException("Не указана заявка!");
                ResponseMessage responseMessage;
                var model = new Claim();
                model.Id = id.Value;
                model.Descr = descr;
                await model.SaveAsync();
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
            if (
                !CurUser.HasAccess(AdGroup.ServiceTech, AdGroup.ServiceAdmin, AdGroup.ServiceControler,
                    AdGroup.ServiceEngeneer, AdGroup.ServiceManager, AdGroup.ServiceOperator))
                return HttpNotFound();

            ListResult<Claim> result = await new Claim().GetListAsync(topRows: topRows, idClaimState: state, clientId: client);
            return View(result);
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
            return View();
        }

        [HttpPost]
        public ActionResult New(Claim model)
        {
            //if (!CurUser.UserCanCreateClaim()) return RedirectToAction("AccessDenied", "Error");

            //Создаем заявку с основными полями и одельно первый статус с комментарием
            try
            {
                ResponseMessage responseMessage;
                model.Contractor = new Contractor() { Id = MainHelper.GetValueInt(Request.Form["ctrList"]) };
                model.Contract = new Contract() { Id = MainHelper.GetValueInt(Request.Form["contList"]) };
                model.Device = new Device() { Id = MainHelper.GetValueInt(Request.Form["devList"]) };
                model.Descr = Request.Form["descr"];
                model.ClientSdNum = Request.Form["client_sd_num"];
                bool result = model.Save(out responseMessage);
                //var response = DbModel.DeserializeResponse(result);
                if (!result) throw new Exception(responseMessage.ErrorMessage);
                return RedirectToAction("Index", new { id = responseMessage.Id });
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
        public JsonResult GetConts(int? idContractor = null, string contractorName = null, int? idContract = null, string contractNumber = null, int? idDevice = null, string deviceName = null)
        {
            var list = Contract.GetList(idContractor, contractorName, idContract, contractNumber, idDevice, deviceName);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetDevices(int? idContractor = null, string contractorName = null, int? idContract = null, string contractNumber = null, int? idDevice = null, string deviceName = null, string serialNum = null)
        {
            var result = Device.GetSearchList(idContractor, contractorName, idContract, contractNumber, idDevice, deviceName, serialNum);
            return Json(result);
        }

       
        [HttpPost]
        public ActionResult SetWorkType(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                bool complete = model.Go(out responseMessage);
                if (!complete) throw new Exception(responseMessage.ErrorMessage);

                return View("WindowClose");
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new {id=model.Id});
            }
        }
        

        [HttpPost]
        public ActionResult SpecialistSelect(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                //model.ServiceIssue4Save.Descr = model.Descr;
                //model.ServiceIssue4Save.IdClaim = model.Id;
                //model.SpecialistSid = model.SpecialistSid;
                bool complete = model.Go(out responseMessage);
                if (!complete) throw new Exception(responseMessage.ErrorMessage);

                return View("WindowClose");

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }
        }

        [HttpPost]
        public ActionResult ConfirmWork(Claim model)
        {
            try
            {
                if (!String.IsNullOrEmpty(Request.Form["ClaimWorkConfirm"]))
                {
                    ResponseMessage responseMessage;
                    bool complete = model.Go(out responseMessage);
                    if (!complete) throw new Exception(responseMessage.ErrorMessage);

                    return RedirectToAction("Index", new { id = responseMessage.Id });
                }
                else if (!String.IsNullOrEmpty(Request.Form["ClaimWorkCancel"]))
                {
                    ResponseMessage responseMessage;
                    //model.Descr = Request.Form["ClaimWorkCancelDescr"];
                    bool complete = model.GoBack(out responseMessage);
                    if (!complete) throw new Exception(responseMessage.ErrorMessage);

                    //return RedirectToAction("Index", new { id = responseMessage.Id });
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            return View("WindowClose");
            //return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult ServiceSheetTechForm(Claim model)
        {
            try
            {
                ResponseMessage responseMessage = null;
                bool complete = false;
                if (!String.IsNullOrEmpty(Request.Form["ServiceSheetSave"]))
                {
                    complete = model.Go(out responseMessage);

                }
                else if (!String.IsNullOrEmpty(Request.Form["ServiceSheetCancel"]))
                {
                    complete = model.GoBack(out responseMessage);
                    return RedirectToAction("List");
                }
                
                if (responseMessage == null)responseMessage = new ResponseMessage();
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
        public ActionResult StateServadmSetWait(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                bool complete = model.Go(out responseMessage);
                if (!complete) throw new Exception(responseMessage.ErrorMessage);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            return View("WindowClose");
            //return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult StateEngOutWait(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                bool complete = model.Go(out responseMessage);
                if (!complete) throw new Exception(responseMessage.ErrorMessage);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            return View("WindowClose");
            //return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult ServiceSheetForm(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                bool complete = model.Go(out responseMessage);
                if (!complete) throw new Exception(responseMessage.ErrorMessage);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            return RedirectToAction("Index", new { id = model.Id });
            //return View("WindowClose");
            //return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult StateDone(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                bool complete = model.Go(out responseMessage);
                if (!complete) throw new Exception(responseMessage.ErrorMessage);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            return View("WindowClose");
            //return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult SetServEngOnWork(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                bool complete = model.Go(out responseMessage);
                if (!complete) throw new Exception(responseMessage.ErrorMessage);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            return View("WindowClose");
            //return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult ZipOrder(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                bool complete = model.Go(out responseMessage);
                if (!complete) throw new Exception(responseMessage.ErrorMessage);
                var claim = new Claim(model.Id);
                ServiceSheet lastServSheet = claim.GetLastServiceSheet();
                var zipList = lastServSheet.GetZipItemList();
                string zipListStr = JsonConvert.SerializeObject(zipList);

                //string zipListStr = "{\"zipList\":[";
                //foreach (var item in zipList)
                //{
                //    zipListStr += $"{{\"PartNum\":\"{item.PartNum}\",\"Name\":\"{item.Name}\",\"Count\":\"{item.Count}\"}}";
                //}
                //zipListStr += "]}";

                 string zipOrderUrl =
                    $"{ConfigurationManager.AppSettings["zipClaimHost"]}/Claims/Editor?snum={claim.Device.SerialNum}&ssid={lastServSheet.Id}&servid={claim.Id}&esid={(String.IsNullOrEmpty(claim.CurEngeneerSid) ? claim.CurTechSid : claim.CurEngeneerSid)}&asid={(String.IsNullOrEmpty(claim.CurAdminSid) ? claim.CurTechSid : claim.CurAdminSid)}&csdnum={claim.ClientSdNum}&cmnt={Url.Encode(lastServSheet.Descr)}&cntr={lastServSheet.CounterMono}&cntrc={lastServSheet.CounterColor}&dvst={lastServSheet.DeviceEnabled}&zip={zipListStr}";
                return Redirect(zipOrderUrl);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            return View("WindowClose");
            //return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult ZipConfirm(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                bool complete = model.Go(out responseMessage);
                if (!complete) throw new Exception(responseMessage.ErrorMessage);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            return View("WindowClose");
            //return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult ZipOrdered(Claim model)
        {
            try
            {
                ResponseMessage responseMessage;
                bool complete = model.Go(out responseMessage);
                if (!complete) throw new Exception(responseMessage.ErrorMessage);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            return View("WindowClose");
            //return RedirectToAction("List");
        }


        [HttpPost]
        public ActionResult ZipOrderConfirm(Claim model)
        {
            try
            {
                if (!String.IsNullOrEmpty(Request.Form["ZipOrderConfirm"]))
                {
                    ResponseMessage responseMessage;
                    bool complete = model.Go(out responseMessage);
                    if (!complete) throw new Exception(responseMessage.ErrorMessage);

                    return RedirectToAction("Index", new { id = responseMessage.Id });
                }
                else if (!String.IsNullOrEmpty(Request.Form["ZipOrderCancel"]))
                {
                    ResponseMessage responseMessage;
                    //model.Descr = Request.Form["ClaimWorkCancelDescr"];
                    bool complete = model.GoBack(out responseMessage);
                    if (!complete) throw new Exception(responseMessage.ErrorMessage);

                    //return RedirectToAction("Index", new { id = responseMessage.Id });
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", new { id = model.Id });
            }

            return View("WindowClose");
            //return RedirectToAction("List");
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
            var model = Models.ServiceSheet.Get(id.Value);
            return View(model);
        }

        public async Task<JsonResult> GetClaimList(int? idDevice, string serialNum, string clientSdNum)
        {
            var list = await new Claim().GetListAsync(idDevice: idDevice,serialNum: serialNum, clientSdNum: clientSdNum);
            return Json(list); 
        }

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
        public JsonResult AddServiceSheetZipItem(ServiceSheetZipItem model)
        {
            try
            {
                ResponseMessage responseMessage;
                bool complete = model.Save(out responseMessage);
                if (!complete) throw new Exception(responseMessage.ErrorMessage);
                return Json(new { id = responseMessage.Id });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }

            return Json(new { });
        }

        [HttpPost]
        public JsonResult ServiceSheetZipItemDelete(int id)
        {
            try
            {
                ResponseMessage responseMessage;
                bool complete = ServiceSheetZipItem.Delete(id, out responseMessage);
                if (!complete) throw new Exception(responseMessage.ErrorMessage);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return null;
        }


        [HttpPost]
        public ActionResult ZipIssue(Claim model)
        {
            try
            {
                    ResponseMessage responseMessage;
                    bool complete = model.Go(out responseMessage);
                    if (!complete) throw new Exception(responseMessage.ErrorMessage);

                return View("WindowClose");
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
    }
}
using MyEvernote.BusinessLayer;
using MyEvernote.BusinessLayer.Results;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObjects;
using MyEvernote.WebApp.Models;
using MyEverNote.Entities.ValueObjects;
using MyEverNote.WebApp.filter;
using MyEverNote.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEverNote.WebApp.Controllers
{
    [Exc]
    public class HomeController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();
        EvernoteUserManager everNoteUserManager = new EvernoteUserManager();

        // GET: Home
        public ActionResult Index()
        {
            //if (TempData["mm"] !=null)
            //{
            //    return View(TempData["mm"] as List<Note>);
            //}
            //object o = 0;
            //int a = 1;         Yazdığım hata sınıfının testi için yazmıştım. 
            //int c = a / (int)o;

            //throw new Exception("Herhangi bir hata oluştu.");

            return View(noteManager.ListQueryable().Where(x=> x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList()); //Tarihe göre descending ederek getir.
            //return View(nm.GetNoteQueryable().OrderByDescending(x => x.ModifiedOn));//Alternatif


        }
        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //if (cat == null)
            //{
            //    return HttpNotFound();
            //    //return RedirectToAction("Index", "Home");
            //}

            //List<Note> notes = cat.Notes.Where(
            //    x => x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList()

            List<Note> notes = noteManager.ListQueryable().Where(
                x => x.IsDraft == false && x.CategoryId == id).OrderByDescending(
                x => x.ModifiedOn).ToList();

            return View("Index", notes);
        }
        public ActionResult MostLiked()
        {



            return View("Index", noteManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }
        public ActionResult About()
        {
            return View();
        }

        [Auth]
        public ActionResult ShowProfile()
        {
            //   EvernoteUser currentUser = Session["login"] as EvernoteUser; Yoruma aldım çünkü currentSession classımı yazdım.
            //Sessiondaki kullanıcıyı atadım.

            BusinessLayerResult<EvernoteUser> res = everNoteUserManager.GetUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors


                };
                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }


        [Auth]
        public ActionResult EditProfile()
        {

           //EvernoteUser currentUser = Session["login"] as EvernoteUser;
         
            BusinessLayerResult<EvernoteUser> res = everNoteUserManager.GetUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }


        [Auth]
        [HttpPost]
        public ActionResult EditProfile(EvernoteUser model, HttpPostedFileBase ProfileImage)
        {
            //ModifiedUserName'i hata mesajlarında basmaması için yazdım.
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                    (ProfileImage.ContentType == "image/jpeg" ||
                    ProfileImage.ContentType == "image/jpg" ||
                    ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFileName = filename;
                }


                BusinessLayerResult<EvernoteUser> res = everNoteUserManager.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi.",
                        RedirectingUrl = "/Home/EditProfile"
                    };

                    return View("Error", errorNotifyObj);
                }

                // Profil güncellendiği için session güncellendi.
                //  Session["login"] = res.Result; Yoruma aldım başka türlü set edecceğim.
                CurrentSession.Set<EvernoteUser>("login", res.Result);

                return RedirectToAction("ShowProfile");
            }
            //validation a takılıyorsa tekrardan modeli dön
            return View(model);
        }

        [Auth]
        public ActionResult DeleteProfile(EvernoteUser user)
        {
            //  EvernoteUser currentUser = Session["login"] as EvernoteUser;

            BusinessLayerResult<EvernoteUser> res = everNoteUserManager.RemoveUserById(CurrentSession.User.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profile Silinemedi",
                    RedirectingUrl = "/Home/ShowProfile"
                };
                return View("Error", errorNotifyObj);
            }
            Session.Clear();

            return RedirectToAction("Index");

        }
        public ActionResult TestNotify()
        {
            ErrorViewModel model = new ErrorViewModel()
            {
                Header = "Yönlendirme.",
                Title = " Ok Test",
                RedirectingTimeOut = 3000,
                Items = new List<ErrorMessageObj>() { new ErrorMessageObj() { Message = "deneme" } }
            };


            return View("Error", model);
        }


        [HttpGet]
        public ActionResult Login()
        {




            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                BusinessLayerResult<EvernoteUser> res = everNoteUserManager.LoginUser(model);
                if (res.Errors.Count > 0)
                {
                    if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "http://Home/Activate/1234-4567-7890";
                    }
                    //tüm error listesinde foreach ile dön herbiri için ilgili stringi ilgili model erora dön.
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message)); //eğer hata varsa modelstate'te basıp bunu geri döndür.


                    return View(model);
                }

                // Session["login"] = res.Result; //sessionda kullanıcı bilgilerini saklama
                CurrentSession.Set<EvernoteUser>("login", res.Result);
                return RedirectToAction("Index"); //yönlendirme
            }

            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {

                BusinessLayerResult<EvernoteUser> res = everNoteUserManager.RegisterUser(model);

                if (res.Errors.Count > 0)
                {
                    //tüm error listesinde foreach ile dön herbiri için ilgili stringi ilgili model erora dön.
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                //EvernoteUser user = null;
                //try
                //{
                //  user = eum.RegisterUser(model);
                //}
                //catch (Exception ex)
                //{

                //    ModelState.AddModelError("", ex.Message);
                //}
                //if (user==null)
                //{
                //    return View(model);
                //}
                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login",
                    //itemslere mesajı ekliyorum.
                    //Ancak zahmetli olduğu için miras aldığı sınıfın ctoruna yazdım ve ağağıdaki şekilde kullandım.
                    //Ancak zahmetli olduğu için miras aldığı sınıfın ctoruna yazdım ve ağağıdaki şekilde kullandım.


                };
                notifyObj.Items.Add(" Lütfen e-posta adresinize gönderdiğimiz aktivasyon linkine tıklayarak hesabınızı aktive ediniz. Hesabınızı aktive etmeden not ekleyemez ve beğenme yapamazsınız");
                return View("Ok", notifyObj);

            }
            return View(model);



        }

        public ActionResult UserActivate(Guid id)
        {

            BusinessLayerResult<EvernoteUser> res = everNoteUserManager.ActivateUser(id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors


                };

                //TempData["errors"]  = res.Errors;

                return RedirectToAction("Error", errorNotifyObj);
            }

            OkViewModel okNotifyObj = new OkViewModel()
            {
                Title = "Hesap aktifleştirildi",
                RedirectingUrl = "/Home/Login",


            };
            okNotifyObj.Items.Add("Hesabınız aktifleştirildi. Artık not paylaşabilir ve beğenme yapabilirsiniz");

            return View("Ok", okNotifyObj);
        }

        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult HasError()
        {
            return View();
        }
    }
}
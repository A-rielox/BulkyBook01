using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        // EDIT-CREATE GET
        public IActionResult Upsert(int? id)
        {
            Company company = new();
            
            if (id == null || id == 0)
            {
                // con id = null o 0 --> crear producto, sino (else) editar
                return View(company);
            }
            else
            {
                // editar producto
                company = _unitOfWork.Company.GetFirstOrDefault( u => u.Id == id );

                return View(company);
            }            
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {
                if(obj.Id == 0)
                {
                    _unitOfWork.Company.Add(obj);
                    TempData["success"] = "Company created successfully!";
                }
                else
                {
                    _unitOfWork.Company.Update(obj);
                    TempData["success"] = "Company updated successfully!";
                }

                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            
            return View(obj);
            
        }

    //////////////////////////////

    #region API CALLS
    // se llama en product.js y carga la info en la tabla
    [HttpGet]
    public IActionResult GetAll()
        {
            //cuidado xq ponia con espacio despues de la coma y no funcionaba
            // ... .GetAll(includeProperties:"Category, CoverType");
            var companyList = _unitOfWork.Company.GetAll();
            return Json(new {data = companyList });
        }

    // DELETE POST
    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var obj = _unitOfWork.Company.GetFirstOrDefault(c => c.Id == id);
        if (obj == null)
        {
            return Json(new { success = false, message = "Error while deleting" });
        }

        _unitOfWork.Company.Remove(obj);
        _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
    }
    #endregion
}

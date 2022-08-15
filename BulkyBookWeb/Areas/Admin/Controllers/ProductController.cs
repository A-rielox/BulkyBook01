﻿using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        //para tener acceso a la base de datos ( esta usando dependency injection )
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll();
            return View(objProductList);
        }

        // CREATE GET
        // va a implementar upsert

        //////////////////////////////

        // EDIT GET
        public IActionResult Upsert(int? id)
        {
            // voy a poner un dropdown para covertype y category, xlo q voy a 
            // necesitar los objects de estos, a traves de unitOfWork puedo 
            // acceder a cualquier repositorio
            // SelectListItem es para dropdown, va a ser una collection de SelectListItems
            // .Select --> Projects each element of a sequence into a new form.
            // Parameters
            //      source IEnumerable<TSource>
            //      A sequence of values to invoke a transform function on.
            //      Returns IEnumerable<TResult>
            //      An IEnumerable<T> whose elements are the result of invoking the
            //      transform function on each element of source.

            // VIEWBAG --> transfiere data desde controller hacia view, la data dura solo
            // una http request
            // con TempData dura 2 reqs, por eso se usa para mensajes

            // con estos Views... la view no esta binded a un solo modelo, asi que 
            // el prefiere q una view este binded a un solo modelo, asi que prefiere
            // crear un view model que sea responsable product upsert view.
            // de esta forma si hay q agregar mas categorias se agregan en 
            // ProductVM en lugar de aca en el controller, de esta manera el controller
            // esta tightly binded to ProductVM y no loosley binded con los IEnumerable
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(
            //    u => new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value = u.Id.ToString(),
            //    });
            //IEnumerable<SelectListItem> CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
            //    u => new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value = u.Id.ToString()
            //    });

            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(
                    i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString(),
                    }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
                    i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString(),
                    }),
            };

            if (id == null || id == 0)
            {
                // con id = null o 0 --> crear producto, sino (else) editar
                // le creo la prop CategoryList y le paso el valor de CategoryList
                //ViewBag.CategoryList = CategoryList;
                // con ViewData hay q hacer casting antes de usarla
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                // editar producto

            }
            
            return View(productVM);
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                //_unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully!";

                return RedirectToAction("Index");
            }
            else
            {
                return View(obj);
            }
        }

        //////////////////////////////

        // DELETE GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var productFromDbFirst = _unitOfWork.Product.GetFirstOrDefault(c => c.Id == id);
            if (productFromDbFirst == null)
            {
                return NotFound();
            }

            return View(productFromDbFirst);
        }

        // DELETE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(c => c.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.ViewModels
{
    public class ProductVM
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
        public Product Product { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CoverTypeList {get; set; }
    }
}
//= _unitOfWork.Category.GetAll().Select(
//            u => new SelectListItem
//            {
//                Text = u.Name,
//                Value = u.Id.ToString(),
//            });
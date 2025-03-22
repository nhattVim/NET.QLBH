using QLBH.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QLBH.Pages
{
    [BindProperties]
    public class ContactPageModel : PageModel
    {
        [BindProperty]
        public Contact contact { get; set; }

        public ContactPageModel()
        {
            contact = new Contact();
        }

        public string Message { get; set; }

        public void OnPost()
        {
            if (ModelState.IsValid)
            {
                Message = "Du lieu gui den hop le";
            }
            else
            {
                Message = "Du lieu gui den khong hop le";
            }
        }

        public void OnGet() { }
    }
}

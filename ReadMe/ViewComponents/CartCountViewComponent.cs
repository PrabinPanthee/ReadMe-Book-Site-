using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReadMe.DataAccess.Repository.IRepository;
using ReadMe.Models.Models;

namespace ReadMe.ViewComponents
{
    public class CartCountViewComponent : ViewComponent
    {
        private readonly IUnitOfWorkcs _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public CartCountViewComponent(IUnitOfWorkcs unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userEmail =  HttpContext.User.Identity.Name;

            if (string.IsNullOrEmpty(userEmail))
            {
                // Handle the case where the user is not logged in
                return View(0); // Or any other default value or message
            }

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                // Handle the case where the user is not found
                return View(0); // Or any other default value or message
            }

            var cart =  _unitOfWork.cartRepository.Get(c => c.ApplicationUserId == user.Id, "CartItems");
            var count = cart?.CartItems.Sum(ci => ci.Quantity) ?? 0;
            return View(count);
        }
    }


}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReadMe.DataAccess.Repository.IRepository;
using ReadMe.Models.Models;
using ReadMe.Models.ViewModels;
using System.Security.Claims;
using System.Text;

namespace ReadMe.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWorkcs _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(IUnitOfWorkcs unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userEmail = User.Identity.Name;

            if (string.IsNullOrEmpty(userEmail))
            {
                // Handle the case where the user is not logged in
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var user = _userManager.FindByEmailAsync(userEmail).Result;

            if (user == null)
            {
                // Handle the case where the user is not found
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var cart = _unitOfWork.cartRepository.Get(c => c.ApplicationUserId == user.Id, "CartItems.Product");

            if (cart == null)
            {
                cart = new Cart
                {
                    CartItems = new List<CartItem>() // Ensure CartItems is not null
                };
            }
            else if (cart.CartItems == null)
            {
                cart.CartItems = new List<CartItem>();
            }

            return View(cart);
        }


        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Ensure this matches with your database Id
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(); // Handle unauthenticated user scenario
            }

            var product = _unitOfWork.productRepository.Get(p => p.ProductId == productId);
            if (product == null)
            {
                return NotFound();
            }

            // Fetch the current user's cart
            var cart = _unitOfWork.cartRepository.Get(c => c.ApplicationUserId == userId, "CartItems.Product");
            if (cart == null)
            {
                cart = new Cart { ApplicationUserId = userId };
                _unitOfWork.cartRepository.Add(cart);
                _unitOfWork.Save();
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem == null)
            {
                cartItem = new CartItem { Product = product, Quantity = quantity };
                cart.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
                _unitOfWork.cartItemRepository.Update(cartItem); // Ensure to update existing item
            }

            _unitOfWork.cartRepository.Update(cart); // Ensure to update the cart
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        public IActionResult Update(int cartItemId, int quantity)
        {
            var cartItem = _unitOfWork.cartItemRepository.Get(ci => ci.Id == cartItemId);
            if (cartItem == null)
            {
                return NotFound();
            }

            cartItem.Quantity = quantity;
            _unitOfWork.cartItemRepository.Update(cartItem);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult Remove(int cartItemId)
        {
            var cartItem = _unitOfWork.cartItemRepository.Get(ci => ci.Id == cartItemId);
            if (cartItem == null)
            {
                return NotFound();
            }

            _unitOfWork.cartItemRepository.Remove(cartItem);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Checkout(decimal totalPrice)
        {
            var userEmail = User.Identity.Name;

            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            // Fetch the current user's information from ApplicationUser
            var user = _userManager.FindByEmailAsync(userEmail).Result as ApplicationUser;

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Create a ViewModel and pass user details along with the total price to the view
            var checkoutViewModel = new CheckoutViewModel
            {
                Email = user.Email,
                Name = user.Name,
                StreetAddress = user.StreetAddress,
                City = user.City,
                State = user.State,
                PostalCode = user.PostalCode,
                TotalPrice = totalPrice // Pass the total price to the ViewModel
            };

            return View(checkoutViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> InitiateKhaltiPayment(decimal totalPrice)
        {
            // Ensure that the user is authenticated and totalPrice is valid
            var userEmail = User.Identity.Name;

            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var user = await _userManager.FindByEmailAsync(userEmail) as ApplicationUser;
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Khalti payment initialization
            var url = "https://a.khalti.com/api/v2/epayment/initiate/";
            var payload = new
            {
                return_url = Url.Action("PaymentConfirmation", "Cart", null, Request.Scheme),
                website_url = "https://localhost:7056/",
                amount = totalPrice*100, // Khalti requires amount in paisa
                purchase_order_id = Guid.NewGuid().ToString(),
                purchase_order_name = "test",
                customer_info = new
                {
                    name = user.Name,
                    email = user.Email,
                    
                }
            };

            var jsonPayload = JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Key 5f3811ecdf774c769b1b2e693294a982");  // Use your test or live secret key

            var response = await client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<KhaltiResponse>(responseContent);
            if (responseObj != null && !string.IsNullOrEmpty(responseObj.payment_url))
            {
                // Redirect the user to the Khalti payment interface
                return Redirect(responseObj.payment_url);
            }

            // Optionally, you can deserialize and inspect the response for success or failure
            // var responseObj = JsonConvert.DeserializeObject<KhaltiResponse>(responseContent);
            
            return BadRequest("Failed to initiate khalti payment"); // You can return a view or JSON depending on the response
        }

        [HttpGet]
        public IActionResult PaymentConfirmation(string pidx, string transaction_id, string tidx, decimal amount, decimal total_amount, string mobile, string status, string purchase_order_id, string purchase_order_name)
        {
            // You can add logic here to verify or store the payment details if needed

            // Create a ViewModel to pass the payment details to the view
            var paymentConfirmationViewModel = new PaymentConfirmationViewModel
            {
                Pidx = pidx,
                TransactionId = transaction_id,
                Tidx = tidx,
                Amount = amount,
                TotalAmount = total_amount,
                Mobile = mobile,
                Status = status,
                PurchaseOrderId = purchase_order_id,
                PurchaseOrderName = purchase_order_name
            };

            // Return the view and pass the ViewModel
            return View(paymentConfirmationViewModel);
        }

    }
}

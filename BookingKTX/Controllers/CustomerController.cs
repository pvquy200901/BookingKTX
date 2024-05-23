using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using static BookingKTX.Controllers.CustomerController;

namespace BookingKTX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }

        public class ItemLoginCustomer
        {
            public string username { get; set; } = "";
            public string password { get; set; } = "";
        }


        [HttpPost]
        [Route("loginCustomer")]
        public IActionResult Login(ItemLoginCustomer item)
        {
            return Ok(Program.api_customer.login(item.username, item.password));
        }


        public class HttpItemCustomer
        {
            public string? code { get; set; }
            public string? username { get; set; }
            public string? password { get; set; }
            public string? address { get; set; }
            public string? email { get; set; }
            public string? displayName { get; set; }
            public string? numberPhone { get; set; }
            public IFormFile? avatar { get; set; } 
        }

        [HttpPost]
        [Route("createCustomer")]
        public async Task<IActionResult> CreateUserAsync( [FromForm] HttpItemCustomer user)
        {
            byte[] image = new byte[0];
           if (user.avatar != null)
            {
               image = new byte[user.avatar.Length];
                using (MemoryStream ms = new MemoryStream())
                {
                    user.avatar?.CopyTo(ms);
                    image = ms.ToArray();

                }
            }
            bool flag = await Program.api_customer.createUserAsync( user.username, user.password, user.displayName, user.numberPhone, user.address, image);
            if (flag)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("editCustomer")]
        public async Task<IActionResult> editUserAsync([FromForm] HttpItemCustomer user)
        {
            byte[] image = new byte[0];
            if (user.avatar != null)
            {
                image = new byte[user.avatar.Length];
                using (MemoryStream ms = new MemoryStream())
                {
                    user.avatar?.CopyTo(ms);
                    image = ms.ToArray();

                }
            }
            bool flag = await Program.api_customer.editUserAsync(user.code, user.password, user.displayName, user.numberPhone, user.address, image);
            if (flag)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("getDetailCustomer")]
        public IActionResult GetCustomer([FromHeader] string code)
        {
            return Ok(Program.api_customer.detailCustomer(code));
        }

        [HttpGet]
        [Route("getListCartProduct")]
        public IActionResult getListCartProduct([FromHeader] string token)
        {
            return Ok(Program.api_customer.getListCartProduct(token));
        }


        [HttpGet]
        [Route("getCountCartProduct")]
        public IActionResult countCart([FromHeader] string token)
        {
            return Ok(Program.api_customer.countCarProduct(token));
        }

        [HttpGet]
        [Route("getListProduct")]
        public IActionResult GetListProduct(string code)
        {
            return Ok(Program.api_product.getListProduct(code));
        }

        [HttpGet]
        [Route("getListProductForUser")]
        public IActionResult GetListProductForUser([FromHeader] string token)
        {
            return Ok(Program.api_product.getListProductForUser(token));
        }

        [HttpGet]
        [Route("searchProduct")]
        public IActionResult searchProduct()
        {
            return Ok(Program.api_product.SearchProduct());
        }

        [HttpGet]
        [Route("getListProductBestSeller")]
        public IActionResult getListProductBestSeller()
        {
            return Ok(Program.api_product.getListProductBestSeller());
        }


        [HttpGet]
        [Route("getDetailProduct")]
        public IActionResult GetDetailProduct(string code)
        {
            return Ok(Program.api_product.getDetailProduct(code));
        }

        public class ItemCartProduct
        {
            public string product { get; set; } = "";
            public int quantity { get; set; } = 0;
        }
        public class ItemCartProductUpdate
        {
            public long ID { get; set; }
            public int quantity { get; set; } = 0;
        }

        [HttpPost]
        [Route("addToCart")]
        public async Task<IActionResult> AddImagesProduct([FromHeader] string token, ItemCartProduct itemCart)
        {
            bool tmp = await Program.api_product.addToCart(token, itemCart.product, itemCart.quantity);
            if (!tmp)
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
        }

        [HttpPost]
        [Route("update-cart-product")]
        public async Task<IActionResult> updateCartProduct(ItemCartProductUpdate itemCart)
        {
            bool tmp = await Program.api_customer.updateCartProduct( itemCart.quantity, itemCart.ID);
            if (!tmp)
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
        }
        public class ItemCartProductDelete
        {
            public string ID { get; set; } = "";
        }
        [HttpPost]
        [Route("delete-cart-product")]
        public async Task<IActionResult> deleteCartProduct(ItemCartProductDelete item)
        {
            bool tmp = await Program.api_customer.deleteCartProduct(item.ID);
            if (!tmp)
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
        }
    }
}

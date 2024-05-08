using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

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
            public string code { get; set; } = "";
            public string username { get; set; } = "";
            public string password { get; set; } = "";
            public string address { get; set; } = "";
            public string displayName { get; set; } = "";
            public string numberPhone { get; set; } = "";
        }

        [HttpPost]
        [Route("createCustomer")]
        public async Task<IActionResult> CreateUserAsync(HttpItemCustomer user)
        {
            bool flag = await Program.api_customer.createUserAsync( user.username, user.password, user.displayName, user.numberPhone, user.address);
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
        public async Task<IActionResult> editUserAsync(HttpItemCustomer user)
        {
            bool flag = await Program.api_customer.editUserAsync(user.code, user.password, user.displayName, user.numberPhone, user.address);
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
        [Route("getDetailProduct")]
        public IActionResult GetDetailProduct(string code)
        {
            return Ok(Program.api_product.getDetailProduct(code));
        }

        [HttpPost]
        [Route("addToCart")]
        public async Task<IActionResult> AddImagesProduct([FromHeader] string token, [FromBody] string product)
        {
            bool tmp = await Program.api_product.addToCart(token, product);
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

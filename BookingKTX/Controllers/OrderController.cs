using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BookingKTX.Controllers.UserController;

namespace BookingKTX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        public class HttpItemOrder
        {
            public List<string> idCartProduct { get; set; } = new List<string>();
            public string note { get; set; } = "";
            public double total { get; set; } = 0;
            public string phone { get; set; } = "";
            public string address { get; set; } = "";
        }

        [HttpPost]
        [Route("createOrder")]
        public async Task<IActionResult> CreateOrderAsync([FromHeader] string token, HttpItemOrder order)
        {
            bool flag = await Program.api_order.createOrderAsync(order.idCartProduct, token, order.total, order.note, order.phone, order.address);
            if (flag)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("confirmOrder")]
        public async Task<IActionResult> ConfirmOrderAsync([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkRoles(token, new string[] { "admin", "manager" });
            if(id > 0)
            {
                bool flag = await Program.api_order.confirmOrderAsync(code);
                if (flag)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return Unauthorized();
            }
           
        }

        [HttpDelete]
        [Route("deleteOrder")]
        public async Task<IActionResult> DeleteOrderAsync([FromHeader] string token, string code)
        {
            bool flag = await Program.api_order.cancelOrderAsync(token, code);
            if (flag)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("finishOrder")]
        public async Task<IActionResult> FinishOrderAsync([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkRoles(token, new string[] { "admin", "manager" });
            if (id > 0)
            {
                bool flag = await Program.api_order.finishOrderAsync(code);
                if (flag)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return Unauthorized();
            }

        }
        
        public class ItemState
        {
            public string? user { get; set; }
            public string code { get; set; } = "";
            public string state { get; set; } = "";
        }
        [HttpPost]
        [Route("setStateOrder")]
        public async Task<IActionResult> SetStateOrderAsync([FromHeader] string token, ItemState state)
        {
            long id = Program.api_user.checkRoles(token, new string[] { "admin", "manager", "shipper" });
            if (id > 0)
            {
                bool flag = await Program.api_order.setStateOrderAsync(state.user, state.code, state.state);
                if (flag)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return Unauthorized();
            }

        }

        [HttpGet]
        [Route("getListOrder")]
        public IActionResult getListOrder([FromHeader] string token)
        {
            return Ok(Program.api_order.getListOrder(token));
        }


        [HttpGet]
        [Route("getListFinishOrder")]
        public IActionResult getListFinishOrder([FromHeader] string token)
        {
            return Ok(Program.api_order.getListFinishOrder(token));
        }
    }
}

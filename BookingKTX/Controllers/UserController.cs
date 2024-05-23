using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingKTX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        public class ItemLoginUser
        {
            public string username { get; set; } = "";
            public string password { get; set; } = "";
        }


        [HttpPost]
        [Route("login")]
        public IActionResult Login(ItemLoginUser item)
        {
            return Ok(Program.api_user.login(item.username, item.password));
        }


        public class HttpItemUser
        {
            public string user { get; set; } = "";
            public string username { get; set; } = "";
            public string password { get; set; } = "";
            public string des { get; set; } = "";
            public string role { get; set; } = "";
            public string displayName { get; set; } = "";
            public string numberPhone { get; set; } = "";
        }

        [HttpPost]
        [Route("createUser")]
        public async Task<IActionResult> CreateUserAsync([FromHeader] string token, HttpItemUser user)
        {
            long id = Program.api_user.checkRoles(token, new string[] { "admin", "manager" });
            if (id >= 0)
            {
               
                bool flag = await Program.api_user.createUserAsync(token, user.user, user.username, user.password, user.displayName, user.numberPhone, user.role);
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

        [HttpPut]
        [Route("editUser")]
        public async Task<IActionResult> editUserAsync([FromHeader] string token, HttpItemUser user)
        {
            long id = Program.api_user.checkSystem(token);
            if (id >= 0)
            {
                bool flag = await Program.api_user.editUserAsync(token, user.user, user.password, user.displayName, user.numberPhone);
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


        public class ItemLocation
        {
            public string latitude { get; set; } = "";
            public string longitude { get; set; } = "";
        }
        [HttpPut]
        [Route("updateLatLongUser")]
        public async Task<IActionResult> updateLatLongUser([FromHeader] string token, ItemLocation user)
        {
            bool flag = await Program.api_user.updateLatLongUserAsync(token, user.latitude, user.longitude);
            if (flag)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete]
        [Route("{code}/delete")]
        public async Task<IActionResult> deleteUserAsync([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkSystem(token);
            if (id >= 0)
            {
                bool flag = await Program.api_user.deleteUserAsync(token, code);
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
        [Route("getListUser")]
        public IActionResult GetListUser([FromHeader] string token)
        {
            //Khong chan user nua
            long id = Program.api_user.checkUser(token);
            if (id >= 0)
            {
                return Ok(Program.api_user.listUser(token));
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getListUserShipper")]
        public IActionResult GetListUserShipper([FromHeader] string token)
        {
            //Khong chan user nua
            long id = Program.api_user.checkUser(token);
            if (id >= 0)
            {
                return Ok(Program.api_user.listUserShipper(token));
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getListUserShop")]
        public IActionResult getListUserShop([FromHeader] string token)
        {
            //Khong chan user nua
            long id = Program.api_user.checkUser(token);
            if (id >= 0)
            {
                return Ok(Program.api_user.listUserForShop(token));
            }
            else
            {
                return Unauthorized();
            }
        }

        public class HttpItemShop
        {
            public string? code { get; set; }
            public string name { get; set; } = "";
            public string? type { get; set; } = "";
            public IFormFile? image { get; set; }
        }

        [HttpPost]
        [Route("createShop")]
        public async Task<IActionResult> CreateShopAsync([FromHeader] string token, [FromForm] HttpItemShop shop)
        {
            long id = Program.api_user.checkRoles(token, new string[] { "admin", "manager" });
            if (id >= 0)
            {
                if (shop.image != null)
                {
                    byte[] image = new byte[shop.image.Length];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        shop.image?.CopyTo(ms);
                        image = ms.ToArray();

                    }

                    bool flag = await Program.api_shop.createShopAsync(token, shop.name, shop.type, image);
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
                    return BadRequest();
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPut]
        [Route("editShop")]
        public async Task<IActionResult> editShopAsync([FromHeader] string token, [FromForm] HttpItemShop shop)
        {
            long id = Program.api_user.checkSystem(token);
            if (id >= 0)
            {
                byte[] image = null;
                if(shop.image != null)
                {
                    image = new byte[shop.image.Length];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        shop.image?.CopyTo(ms);
                        image = ms.ToArray();

                    }
                }
                bool flag = await Program.api_shop.editShopAsync(token, shop.code, shop.name,shop.type, image);
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
        [Route("{code}/deleteShop")]
        public async Task<IActionResult> deleteShopAsync([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkSystem(token);
            if (id >= 0)
            {
                bool flag = await Program.api_shop.deleteShopAsync(token, code);
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
        [Route("getListShop")]
        public IActionResult GetListShop()
        {
            return Ok(Program.api_shop.getListShop());
        }

        [HttpGet]
        [Route("getInfoShop")]
        public IActionResult GetListShop([FromHeader] string token)
        {
            return Ok(Program.api_shop.getInfoShop(token));
        }

        [HttpPost]
        [Route("createProduct")]
        public async Task<IActionResult> CreateProduct([FromHeader] string token, string code, string name, decimal price, int quantity, List<IFormFile> file)
        {
            long id = Program.api_user.checkRoles(token, new string[] { "admin", "manager" });
            if (id >= 0)
            {
                if (file != null)
                {
                    List<byte[]> byteArrayList = new List<byte[]>();

                    foreach (var item in file)
                    {
                        byte[] image = new byte[item.Length];
                        using (MemoryStream ms = new MemoryStream())
                        {
                            item?.CopyTo(ms);
                            image = ms.ToArray();

                        }

                        byteArrayList.Add(image);
                    }
                    bool tmp = await Program.api_product.createProductAsync(token,code,name, price, quantity,byteArrayList);
                    if (!tmp)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok();
                    }
                }
                else { return BadRequest(); }
            }
            else
            {
                return Unauthorized();
            }
        }

        public class HttpItemProduct
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public decimal price { get; set; } 
            public int quantity { get; set; }
        } 

        [HttpPut]
        [Route("editProduct")]
        public async Task<IActionResult> editProductAsync([FromHeader] string token, HttpItemProduct item)
        {
            long id = Program.api_user.checkSystem(token);
            if (id >= 0)
            {
                bool flag = await Program.api_product.editProductAsync(token,item.code, item.name, item.price, item.quantity);
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
        [Route("deleteProduct")]
        public async Task<IActionResult> deleteProductAsync([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkSystem(token);
            if (id >= 0)
            {
                bool flag = await Program.api_product.deleteProductAsync(token, code);
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

        [HttpPut]
        [Route("addImagesProduct")]
        public async Task<IActionResult> AddImagesProduct([FromHeader] string token, string code, List<IFormFile> file)
        {
            long id = Program.api_user.checkRoles(token, new string[] { "admin", "manager" });
            if (id >= 0)
            {
                if (file != null)
                {
                    List<byte[]> byteArrayList = new List<byte[]>();

                    foreach (var item in file)
                    {
                        byte[] image = new byte[item.Length];
                        using (MemoryStream ms = new MemoryStream())
                        {
                            item?.CopyTo(ms);
                            image = ms.ToArray();

                        }

                        byteArrayList.Add(image);
                    }
                    bool tmp = await Program.api_product.addImageProductAsync(token, code, byteArrayList);
                    if (!tmp)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok();
                    }
                }
                else { return BadRequest(); }
            }
            else
            {
                return Unauthorized();
            }
        }


        [HttpDelete]
        [Route("deleteImageProduct")]
        public async Task<IActionResult> deleteImageProductAsync([FromHeader] string token, string code, string image)
        {
            long id = Program.api_user.checkSystem(token);
            if (id >= 0)
            {
                bool flag = await Program.api_product.deleteImageProductAsync(token, code, image);
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


        [HttpPut]
        [Route("addImageShop")]
        public async Task<IActionResult> AddImageShop([FromHeader] string token, string code, IFormFile file)
        {
            long id = Program.api_user.checkRoles(token, new string[] { "admin", "manager" });
            if (id >= 0)
            {
                if (file != null)
                {
                    byte[] image = new byte[file.Length];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file?.CopyTo(ms);
                        image = ms.ToArray();

                    }
                    bool tmp = await Program.api_shop.addImageShopAsync(token, code, image);
                    if (!tmp)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok();
                    }
                }
                else { return BadRequest(); }
            }
            else
            {
                return Unauthorized();
            }
        }


        [HttpDelete]
        [Route("deleteImageShop")]
        public async Task<IActionResult> deleteImageShopAsync([FromHeader] string token, string code, string image)
        {
            long id = Program.api_user.checkSystem(token);
            if (id >= 0)
            {
                bool flag = await Program.api_shop.deleteImageShopAsync(token, code, image);
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
        [Route("getListOrderForUser")]
        public IActionResult getListOrder([FromHeader] string token)
        {
            return Ok(Program.api_order.getListOrderForUser(token));
        }


    }
}

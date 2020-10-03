using Marcel.Access;
using Marcel.DbModels.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Marcel.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DishController : ControllerBase
    {
        private readonly ILogger<DishController> logger;
        private readonly IDishAccess dishAccess;

        public DishController(ILogger<DishController> logger, IDishAccess dishAccess)
        {
            this.logger = logger;
            this.dishAccess = dishAccess ?? throw new ArgumentNullException(nameof(dishAccess));
        }

        [HttpGet]
        public IEnumerable<DishResponse> Get()
        {
            return dishAccess.GetAll().Select(x => new DishResponse
            { 
                MenuTitle = x.MenuTitle, 
                MenuDescription = x.MenuDescription, 
                MenuSectionTitle = x.MenuSectionTitle, 
                DishName = x.DishName, 
                DishDescription = x.DishDescription });
        }
    }
}
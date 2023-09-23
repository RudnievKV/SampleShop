using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;

using System;
using System.Collections.Generic;
using SampleShop.Interfaces;
using SampleShop.Model;

namespace SampleShop.Controllers
{

    public class ItemsController : ControllerBase
    {
        private readonly IItemsService _service;

        public ItemsController(IItemsService service)
        {
            _service = service;
        }

        // GET api/items
        [FunctionName(nameof(GetAll))]
        public IActionResult GetAll([HttpTrigger("get", Route = "items")]HttpRequest request)
        {
            var items = _service.All();
            return Ok(items);
        }
    }
}
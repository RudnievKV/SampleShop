using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;

using System;
using System.Collections.Generic;
using SampleShop.Interfaces;
using SampleShop.Model;
using System.Collections;

namespace SampleShop.Controllers
{
	public class OrdersController : ControllerBase
	{
		private readonly IOrdersService service;

		public OrdersController(IOrdersService service)
		{
			this.service = service;
		}

		// GET api/orders
		[FunctionName(nameof(GetAllOrders))]
		public IActionResult GetAllOrders([HttpTrigger("get", Route = "orders")]HttpRequest request)
		{
			var orders = service.All();
			return Ok(orders);
		}

		// GET api/orders/5
		[FunctionName(nameof(GetById))]
		public IActionResult GetById([HttpTrigger("get", Route = "orders/{id:int}")]HttpRequest request, int id)
		{
			var orders = service.GetById(id);
			return Ok(orders);
		}

		// GET api/orders/dates/?start=2018-01-03&end=2018-02-03
		[FunctionName(nameof(GetByDates))]
		public IActionResult GetByDates([HttpTrigger("get", Route = "orders/dates")]HttpRequest request)
		{

			DateTime start, end;

			try
			{
				start = DateTime.Parse(request.Query["start"]);
				end = DateTime.Parse(request.Query["end"]);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

			var orders = service.GetByDates(start, end);
			return Ok(orders);
		}

		// GET api/orders/items/?day=2018-01-15
		[FunctionName(nameof(GetItemsSoldByDay))]
		public IActionResult GetItemsSoldByDay([HttpTrigger("get", Route = "orders/items")]HttpRequest request)
		{

			DateTime day;
			try
			{
				day = DateTime.Parse(request.Query["day"]);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

			var items = service.GetItemsSoldByDay(day);
			return Ok(items);
		}

		// POST api/orders
		[FunctionName(nameof(Post))]
		public IActionResult Post([HttpTrigger("post", Route = "orders")]HttpRequest request, [FromBody] Order value)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var order = service.Add(value);
			if (order != null)
			{
				return Ok(order);;
			}

			return BadRequest("Failed Validation");
		}

		// DELETE api/items/5
		[FunctionName(nameof(Remove))]
		public IActionResult Remove([HttpTrigger("delete", Route = "orders/{id}")]HttpRequest request, int id)
		{
			service.Delete(id);

			return Ok();
		}
	}
}

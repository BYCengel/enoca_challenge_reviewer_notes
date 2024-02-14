using AutoMapper;
using enoca_challenge.Dto;
using enoca_challenge.Interface;
using enoca_challenge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;

namespace enoca_challenge.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrdersController : Controller
	{
		private readonly IOrdersRepository _ordersRepository;
		private readonly ICarriersRepository _carrierRepository;
		private readonly ICConfigRepository _configRepository;
		private readonly IMapper _mapper;
		public OrdersController(IOrdersRepository ordersRepository, ICarriersRepository carriersRepository,ICConfigRepository cConfigRepository, IMapper mapper)
		{
			_ordersRepository = ordersRepository;
			_carrierRepository = carriersRepository;
			_configRepository = cConfigRepository;
			_mapper = mapper;
		}

		[HttpGet]
		public IActionResult GetOrders()
		{
			var orders = _mapper.Map<List<OrderDto>>(_ordersRepository.GetOrders());
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			return Ok(orders);
		}

		[HttpGet("{orderId}")]
		public IActionResult GetOrders(int orderId)
		{
			if (!_ordersRepository.OrderExists(orderId))
				return NotFound();

			var orders = _mapper.Map<OrderDto>(_ordersRepository.GetOrders(orderId));
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(orders);
		}

		[HttpPost]
		public IActionResult AddOrder([FromQuery] Order_Dto orderAdd)
		{
			if (orderAdd == null)
				return BadRequest(ModelState);


			if (!ModelState.IsValid)
				return BadRequest();

			if (orderAdd.OrderDesi < 1)
			{
				ModelState.AddModelError("", "Lütfen geçerli bir desi değeri giriniz");
				return StatusCode(400, ModelState);
			}
				

			var orderMap = _mapper.Map<Orders>(orderAdd);

			var configurations = _configRepository.GetCarrierConfigurations().ToList();
			float minCost = float.MaxValue;
			Carriers carrier = new Carriers();
			foreach (var configuration in configurations)
			{
				if (configuration.CarrierMinDesi <= orderAdd.OrderDesi && configuration.CarrierMaxDesi >= orderAdd.OrderDesi)
				{
					if (configuration.CarrierCost < minCost)
					{
						carrier = _carrierRepository.GetCarrierOfAConfiguration(configuration.CarrierConfigurationId);
						minCost = configuration.CarrierCost;
					}
				}
			}

			if (minCost != float.MaxValue)
			{
				orderMap.Carriers = carrier;
				orderMap.OrderCarrierCost = minCost;
			}
			else
			{
				int closest = int.MaxValue;
				foreach (var configuration in configurations)
				{
					if(orderMap.OrderDesi - configuration.CarrierMaxDesi <= closest)
					{
						closest = orderMap.OrderDesi - configuration.CarrierMaxDesi;
					}
				}
				foreach (var configuration in configurations)
				{
					if (orderMap.OrderDesi - configuration.CarrierMaxDesi == closest)
					{
						if(configuration.CarrierCost + (_carrierRepository.GetCarrierOfAConfiguration(configuration.CarrierConfigurationId).CarrierPlusDesiCost * closest) <= minCost)
						{
							minCost = configuration.CarrierCost + (_carrierRepository.GetCarrierOfAConfiguration(configuration.CarrierConfigurationId).CarrierPlusDesiCost * closest);
							carrier = _carrierRepository.GetCarrierOfAConfiguration(configuration.CarrierConfigurationId);
						}
						
					}
				}

			}
			orderMap.Carriers = carrier;
			orderMap.OrderCarrierCost = minCost;
			
			
			if (!_ordersRepository.AddOrder(orderMap))
			{
				ModelState.AddModelError("", "Kayıt isleminde bir hata gerceklesti");
				return StatusCode(500, ModelState);
			}
			return Ok("Yeni sipariş başarıyla eklendi. Siparişin kargo firması "+orderMap.Carriers.CarrierName+" ,siparişin ücreti "+minCost);
		}

		[HttpDelete("{orderId}")]
		public IActionResult DeleteOrder(int orderId)
		{
			if (!_ordersRepository.OrderExists(orderId))
			{
				return NotFound();
			}
			var orderDelete = _ordersRepository.GetOrders(orderId);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			if (!_ordersRepository.DeleteOrder(orderDelete))
			{
				ModelState.AddModelError("", "Silme işlemi sırasında bir hata meydana geldi");
			}
			return Ok(orderId + " id'li sipariş başarıyla silindi");
		}
	}
}

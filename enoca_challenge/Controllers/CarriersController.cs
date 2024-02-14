using AutoMapper;
using enoca_challenge.Dto;
using enoca_challenge.Interface;
using enoca_challenge.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace enoca_challenge.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CarriersController : Controller
	{
        private readonly ICarriersRepository _carriersRepository;
		private readonly ICConfigRepository _cConfigRepository;
		private readonly IOrdersRepository _ordersRepository;
		private readonly IMapper _mapper;
        public CarriersController(ICarriersRepository carriersRepository, ICConfigRepository cConfigRepository, IOrdersRepository ordersRepository, IMapper mapper)
        {
               _carriersRepository = carriersRepository;
               _cConfigRepository = cConfigRepository;
               _ordersRepository = ordersRepository;
               _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCarriers()
        {
            var carriers = _mapper.Map<List<CarriersDto>>(_carriersRepository.GetCarriers());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(carriers);
        }
        [HttpGet("{carrierId}")]
        public IActionResult GetCarrier(int carrierId)
        {
            if(!_carriersRepository.CarrierExists(carrierId))
                return NotFound();

			var carriers=_mapper.Map<CarriersDto>(_carriersRepository.GetCarriers(carrierId));

            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            return Ok(carriers);
        }

        [HttpGet("/orders/{orderId}")]
        public IActionResult GetCarriersOfAnOrder(int orderId)
        {
			//order id'si alıp o order'ın hangi carrier'da oldugunu donuyor.
			var carrier = _mapper.Map<CarriersDto>(_carriersRepository.GetCarrierOfAnOrder(orderId));
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(carrier);
        }

        
		[HttpGet("/carrierConfigurations/{configurationId}")]
		public IActionResult GetCarrierOfAConfiguration(int configurationId)
		{
            //configuration id'si alıp o configuration'ın hangi carrier'da oldugunu donuyor.
			var carrier = _mapper.Map<CarriersDto>(_carriersRepository.GetCarrierOfAConfiguration(configurationId));
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			return Ok(carrier);
		}
        
		[HttpGet("/carriers/{carrierId}")]
		public IActionResult GetConfigsFromACarrier(int carrierId)
		{
			var configs = _mapper.Map<List<CarrierConfigurationDto>>(_carriersRepository.GetConfigsFromACarrier(carrierId));
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			return Ok(configs);
		}
		

        [HttpPost]
        public IActionResult AddCarrier([FromQuery] Carrier_Dto carriersAdd)
        {
            if(carriersAdd == null)
                return BadRequest(ModelState);

            var carrier = _carriersRepository.GetCarriers().Where(c=>c.CarrierName.Trim().ToUpper() == carriersAdd.CarrierName.Trim().ToUpper()).FirstOrDefault(); ;
            if(carrier != null)
            {
                ModelState.AddModelError("", "Bu kargo firması zaten mevcut");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) 
                return BadRequest();

            var carrierMap = _mapper.Map<Carriers>(carriersAdd);

            if (!_carriersRepository.AddCarrier(carrierMap))
            {
                ModelState.AddModelError("", "Kayıt isleminde bir hata gerceklesti");
                return StatusCode(500, ModelState);
            }
            return Ok("Yeni kargo firması başarıyla eklendi");
        }

        [HttpPut("{carrierId}")]
        public IActionResult UpdateCarrier(int carrierId, [FromBody]Carrier_Dto updatedCarrier)
        {
            if (updatedCarrier == null)
                return BadRequest(ModelState);
            
            if(!_carriersRepository.CarrierExists(carrierId))
                return NotFound();
            if(!ModelState.IsValid)
                return BadRequest();
            var carrierMap=_mapper.Map<Carriers>(updatedCarrier);
            carrierMap.CarrierId = carrierId;
            if (!_carriersRepository.UpdateCarrier(carrierMap))
            {
                ModelState.AddModelError("", "Güncelleme işlemi sırasında bir hata meydana geldi");
                return StatusCode(500, ModelState);
            }
            return Ok(carrierId+" id'li kargo firmasının bilgileri güncellendi");
        }

        [HttpDelete("{carrierId}")]
        public IActionResult DeleteCarrier(int carrierId)
        {
            if (!_carriersRepository.CarrierExists(carrierId))
            {
                return NotFound();
            }
            var carrierDelete=_carriersRepository.GetCarriers(carrierId);
            if (_carriersRepository.GetConfigsFromACarrier(carrierId).Any())
            {
                var configuration= _carriersRepository.GetConfigsFromACarrier(carrierId).ToList();
                foreach (var config in configuration)
                {
                    _cConfigRepository.DeleteCarrierConfiguration(config);
                }

			}
			if (_carriersRepository.GetOrdersFromACarrier(carrierId).Any())
			{
				var orders = _carriersRepository.GetOrdersFromACarrier(carrierId).ToList();
				foreach (var order in orders)
				{
					_ordersRepository.DeleteOrder(order);
				}

			}

			if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if(!_carriersRepository.DeleteCarrier(carrierDelete))
            {
                ModelState.AddModelError("", "Silme işlemi sırasında bir hata meydana geldi");
            }
            return Ok(carrierId+" id'li kargo firması ve bu kargo firması ile bağlantısı olan konfigürasyonlar ve siparişler başarıyla silindi");
        }
	}
}

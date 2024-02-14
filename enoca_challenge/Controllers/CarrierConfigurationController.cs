using AutoMapper;
using enoca_challenge.Dto;
using enoca_challenge.Interface;
using enoca_challenge.Models;
using enoca_challenge.Repository;
using Microsoft.AspNetCore.Mvc;

namespace enoca_challenge.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CarrierConfigurationController : Controller
	{
		private readonly ICConfigRepository _cConfigRepository;
		private readonly ICarriersRepository _carrierRepository;
		private readonly IMapper _mapper;
		public CarrierConfigurationController(ICConfigRepository cConfigRepository, ICarriersRepository carriersRepository, IMapper mapper)
		{
			_cConfigRepository = cConfigRepository;
			_carrierRepository = carriersRepository;
			_mapper = mapper;
		}

		[HttpGet]
		public IActionResult GetCarrierConfigurations()
		{
			var carrierconfigurations = _mapper.Map<List<CarrierConfigurationDto>>(_cConfigRepository.GetCarrierConfigurations());
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			return Ok(carrierconfigurations);
		}

		[HttpGet("{carrierConfigurationId}")]
		public IActionResult GetCarrierConfigurations(int carrierConfigurationId)
		{
			if (!_cConfigRepository.CarrierConfigurationExists(carrierConfigurationId))
				return NotFound();

			var carrierConfiguration = _mapper.Map<CarrierConfigurationDto>(_cConfigRepository.GetCarrierConfigurations(carrierConfigurationId));
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(carrierConfiguration);
		}

		[HttpPost]
		public IActionResult AddCarrierConfiguration( [FromQuery] CarrierConfiguration_Dto configurationAdd, [FromQuery] int CarrierId)
		{
			if (configurationAdd == null)
				return BadRequest(ModelState);
			

			if (!ModelState.IsValid)
				return BadRequest();
			
			var configurationMap = _mapper.Map<CarrierConfigurations>(configurationAdd);

			configurationMap.Carriers = _carrierRepository.GetCarriers(CarrierId);
			if(configurationMap.Carriers == null)
			{
				ModelState.AddModelError("", "Lütfen geçerli(var olan) bir kargo firmasının carrierId'sini girin");
				return StatusCode(404, ModelState);
			}

			if (!_cConfigRepository.AddCarrierConfiguration(configurationMap))
			{
				ModelState.AddModelError("", "Kayıt isleminde bir hata gerceklesti");
				return StatusCode(500, ModelState);
			}
			return Ok("Yeni kargo konfigürasyonu başarıyla eklendi");
		}

		[HttpPut("{carrierConfigurationId}")]
		public IActionResult UpdateCarrier(int carrierConfigurationId, [FromBody] CarrierConfiguration_Dto updatedConfiguration)
		{
			if (updatedConfiguration == null)
				return BadRequest(ModelState);
			
			if (!_cConfigRepository.CarrierConfigurationExists(carrierConfigurationId))
				return NotFound();
			if (!ModelState.IsValid)
				return BadRequest();
			var configurationMap = _mapper.Map<CarrierConfigurations>(updatedConfiguration);
			configurationMap.CarrierConfigurationId = carrierConfigurationId;
			

			if (!_cConfigRepository.UpdateCarrierConfiguration(configurationMap))
			{
				ModelState.AddModelError("", "Güncelleme işlemi sırasında bir hata meydana geldi");
				return StatusCode(500, ModelState);
			}
			return Ok(carrierConfigurationId + " id'li konfigürasyon güncellendi");
		}

		[HttpDelete("{carrierConfigurationId}")]
		public IActionResult DeleteConfiguration(int carrierConfigurationId)
		{
			if (!_cConfigRepository.CarrierConfigurationExists(carrierConfigurationId))
			{
				return NotFound();
			}
			var configurationDelete = _cConfigRepository.GetCarrierConfigurations(carrierConfigurationId);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			if (!_cConfigRepository.DeleteCarrierConfiguration(configurationDelete))
			{
				ModelState.AddModelError("", "Silme işlemi sırasında bir hata meydana geldi");
			}
			return Ok(carrierConfigurationId + " id'li konfigürasyon başarıyla silindi");
		}
	}
}

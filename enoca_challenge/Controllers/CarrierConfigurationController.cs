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
        /* GENERAL NOTES ABOUT CODE (NOT JUST THIS CLASS)
		 * 
		 * -This is not N-Layered Architecture. You should have separate Business Logic, Data Access and UserInterface layers and keep the relevant code in these layers.
		 * Currently there is no architecture in this project.
		 * -There is logic errors in the code (MaxDesi can be higher than MinDesi, Desi can be negative etc.)
		 * -Code could be cleaner and more in line with SOLID principles. (I've pointed some of these out in the code.)
		 * -Endpoints should have defining names. Ex: /api/CarrierConfiguration/GetCarrierConfiguration
		 * -Code needs a standard for naming conventions. For C# the general consensus is CamelCase but just pick one and stick to it
		 * -I would use Dtos for endpoint parameters instead of queries and more crucially I wouldn't use mix of both in the same endpoints parameters.
		 * -I would stick to english in everything including ErrorMessages etc. It's more professional and easier to read overall.
		 * -You should use a BaseEntity class/interface and inherit your other entities (Order, Carrier, CarrierConfiguration) From this. This way you don't have to
		 * repeat yourself (for example ids) and its harder to mess up stuff. And don't name them as plurals.It's the template for a single Carrier. You dont
		 * access all of your Carriers with that. So it should be Carrier and you should call your Db table Carriers.
		 * -You should keep your logic in separate relative services like OrderService, CarrierService etc. After that you should inject these services into relative
		 * controllers. This way your code will be easier to scale, maintain and read.
		 * 
		 * SOME RESOURCES TO TAKE A LOOK AT:
		 * Tim Corey SOLID principles playlist https://www.youtube.com/playlist?list=PLAaFb7UfyShCoS246UzZJNEiXuD8bg02e
		 * Gençay Yıldız Onion Architecture Mini E-Ticaret serisi. (First practice N-Layered Architecture though. Onion Architecture is a good step up after that.)
		 * https://www.youtube.com/@dotnet
		 * 
		 * CONSENSUS
		 * I think there is potential in your code but it needs polish and for that you need practice. So you need to write more code and learn more because you
		 * have some missing pieces in your project.
		 * 
		 * Author: Burak Yavuz Çengel
		 */

        private readonly ICConfigRepository _cConfigRepository;
		private readonly ICarriersRepository _carrierRepository;
		private readonly IMapper _mapper;
		public CarrierConfigurationController(ICConfigRepository cConfigRepository, ICarriersRepository carriersRepository, IMapper mapper)
		{
			_cConfigRepository = cConfigRepository;
			_carrierRepository = carriersRepository;
			_mapper = mapper;
		}

		[HttpGet]//[HttpGet("GetCarrierConfiguration or Get")] would be better.
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
        {//Needs better logic controls as I've pointed out earlier.
            if (configurationAdd == null)
				return BadRequest(ModelState);
			

			if (!ModelState.IsValid)
				return BadRequest();
			
			var configurationMap = _mapper.Map<CarrierConfigurations>(configurationAdd);

			configurationMap.Carriers = _carrierRepository.GetCarriers(CarrierId);
			if(configurationMap.Carriers == null)
			{
                //Instead of putting strings like this around you should use a class/several classes to keep these kinds of constants. (const)
                //Lets say you use this string in 5 different places in your code. What would happen if we need to change this?
                ModelState.AddModelError("", "Lütfen geçerli(var olan) bir kargo firmasının carrierId'sini girin");
                //Instead of returning the whole ModelState you can return the ErrorMessages within for a simplified and digestible output.
                //var errorMessages = ModelState.Values.SelectMany(ms => ms.Errors).Select(e => e.ErrorMessage).ToList(); //like this.
                return StatusCode(404, ModelState);//Returning BadRequest would be more appropriate.
            }

			if (!_cConfigRepository.AddCarrierConfiguration(configurationMap))
            {//same deal here.
                ModelState.AddModelError("", "Kayıt isleminde bir hata gerceklesti");
				return StatusCode(500, ModelState);
			}
            //You can also use a default generic return class to make everything more organized, readable and easier to code around.
            //Something like a GeneralReturnModel<T> class with IsSuccess(bool), Message(string), Data(T) properties.
            //And you can make specialized descendants of a class like this if needed.
            return Ok("Yeni kargo konfigürasyonu başarıyla eklendi");
		}

		[HttpPut("{carrierConfigurationId}")]
		public IActionResult UpdateCarrier(int carrierConfigurationId, [FromBody] CarrierConfiguration_Dto updatedConfiguration)
        //This is not good. Either use Dtos or queries.(I would prefer Dtos in most cases) But not create a mix of both.
        //If you're ever in a stiuation like this where you need to add on to your Dtos properties because they're not enough,
        //you've probably made a mistake while designing your application/approach.
        {
            if (updatedConfiguration == null)//This is mostly personal preference but I would prefer using curly brackets instead of single line approach.
                return BadRequest(ModelState);
			
			if (!_cConfigRepository.CarrierConfigurationExists(carrierConfigurationId))//I like your approach of keeping different cases separate instead of bunching them up in one gigantic if. This gives you more control over different scenarios and better readability. Nice work!
                return NotFound();
			if (!ModelState.IsValid)
				return BadRequest();//Don't just return empty like this. Atleast return a constant string. This doesn't help neither you nor the end user.
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

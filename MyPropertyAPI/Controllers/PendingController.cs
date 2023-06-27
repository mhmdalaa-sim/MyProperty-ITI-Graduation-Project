using BL.Dtos.PendingProperty;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MyPropertyAPI.Controllers
{
    [Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class PendingController : ControllerBase
    {
        private readonly IPendingPropertyManager _pendingPropertyManager;
        private readonly UserManager<IdentityUser> UserManagerFromPackage;
        public PendingController( IPendingPropertyManager pendingPropertyManager , UserManager<IdentityUser> usermanger)
        {
            _pendingPropertyManager = pendingPropertyManager;
            UserManagerFromPackage = usermanger;
        }
        [HttpGet]
        public ActionResult<List<PendingReadDto>> GetAll() { 
            var pendingReq = _pendingPropertyManager.GetAll();
            if ( pendingReq == null ) { return BadRequest(); }
            return pendingReq.ToList();
          
        }
        [HttpGet]
        [Route("/details/{id}")]
        public ActionResult<PendingReadDetailsDto> GetById(int id)
        {
            var appartment = _pendingPropertyManager.GetById(id);
            if (appartment == null) { return NotFound(); }
            return Ok(appartment);


        }
        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete(int id)
        {
            var a = _pendingPropertyManager.GetById(id);
            if(a== null) { return NotFound(); }
            _pendingPropertyManager.Delete(id);
           

            return NoContent();
        }
        [HttpPatch]
        [Route("{id}")]
        public async Task< ActionResult >Accept(int id, string brokerId)
        {
            var user = await UserManagerFromPackage.GetUserAsync(User);
            string managerId = user.Id;
          var apartment= _pendingPropertyManager.Accept(id, brokerId, managerId);
            if (apartment == null) { return BadRequest(); }

            return NoContent();
        }
        [HttpGet]
        [Route("/brokerId")]
        public ActionResult<List<BrokerDataDto>> GetAllBroker()
        {
            return _pendingPropertyManager.GetAllBroker();

        }
        
    }

}

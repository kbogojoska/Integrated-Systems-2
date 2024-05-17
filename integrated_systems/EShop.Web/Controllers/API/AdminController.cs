using EShop.Domain.Domain;
using EShop.Domain.DTO;
using EShop.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Movie_App.Service.Interface;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace EShop.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IConcertService _concertService;
        public AdminController(IOrderService orderService, IConcertService concertService)
        {
            _orderService = orderService;
            _concertService = concertService;
        }
        [HttpGet("[action]")]
        public List<Order> GetAllOrders()
        {
            return this._orderService.GetAllOrders();
        }
        [HttpPost("[action]")]
        public Order GetDetailsForOrder(BaseEntity id)
        {
            var data = this._orderService.GetDetailsForOrder(id);
            return data;
        }

        [HttpPost("[action]")]
        public bool ImportAllConcerts(List<ConcertImportDTO> concerts)
        {
            bool status = true;
            foreach(var item in concerts)
            {
                var concert = new Concert
                {
                    Id = new Guid(),
                    ConcertName = item.ConcertName,
                    ConcertDescription = item.ConcertDescription,
                    Rating = Convert.ToDouble(item.Rating),
                    ConcertImage = item.ConcertImage
                };
                _concertService.CreateNewConcert(concert);
            }

            return status;
        }

    }
}

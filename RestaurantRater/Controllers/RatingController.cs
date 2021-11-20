using RestaurantRater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RestaurantRater.Controllers
{
    public class RatingController : ApiController
    {
        private readonly RestaurantDbContext _context = new RestaurantDbContext

        // Creat new ratings
        [HttpPost]
        public async Task<IHttpActionResult> CreateRating([FromBody] Rating model)
        {
            // check if model is null
            if (model is null)
                return BadRequest("Your request body cannot by empty.");

            // check if model state is invalid 
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // find the Restaurant by model.RestaurantId and see that it exists
            var restaurantEntity = await _context.Restaurants.FindAsync(model.RestaurantId);
            if (restaurantEntity is null)
                return BadRequest($"The target restaurant with the ID of {model.RestaurantId}" +
                    $" does not exist.");


            // Create the Rating

            // Add to the Rating table
            //_context.Ratings.Add(model);

            // Add to the Rataurant Entity
            restaurantEntity.Ratings.Add(model);
            if (await _context.SaveChangesAsync() == 1)
                return Ok($"You rated restaurant {restaurantEntity.Name} successfully!");

            return InternalServerError();


        }




        //Get a Rating by its id

        // Get All Ratings

        // Get All Ratings for specific restaurant by restaurant id

        // Update a Rating

        // Delete a Rating
    }
}

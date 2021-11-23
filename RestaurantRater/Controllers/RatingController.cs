using RestaurantRater.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RestaurantRater.Controllers
{
    public class RatingController : ApiController
    {
        private readonly RestaurantDbContext _context = new RestaurantDbContext();

        // Creat new ratings
        [HttpPost]

        // Create the Rating
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


            

            // Add to the Rating table
            //_context.Ratings.Add(model);

            // Add to the Rataurant Entity
            restaurantEntity.Ratings.Add(model);
            if (await _context.SaveChangesAsync() == 1)
                return Ok($"You rated restaurant {restaurantEntity.Name} successfully!");

            return InternalServerError();


        }

        //Get a Rating by its id

        [HttpGet]
        public async Task<IHttpActionResult> GetById([FromUri] int id)
        {
            Rating rating = await _context.Ratings.FindAsync(id);

            if(rating != null)
            {
                return Ok(rating);
            }
            return NotFound();

        }

        // Get All Ratings
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Rating> ratings = await _context.Ratings.ToListAsync();
            return Ok(ratings);
        }


        // Get All Ratings for specific restaurant by restaurant id
        [HttpGet]

        public async Task<IHttpActionResult> GetAllRatingsByRestaurant([FromUri] int id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            if(restaurant != null)
            {
                return Ok(restaurant.Rating);
            }

            return NotFound();

        }


        // Update a Rating
        // ***Not sure why we would want to create access to change/update a rating... did this more for the practice of the idea of updating results. *** 
        [HttpPut]
        public async Task<IHttpActionResult> UpdateRating([FromUri] int id, [FromBody] Rating updatedRating)
        {
            if(id != updatedRating?.Id)
            {
                return BadRequest("Id's do not match.");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Rating ratings = await _context.Ratings.FindAsync(id);

            if (ratings is null)
                return NotFound();

            ratings.EnvironmentalScore = updatedRating.EnvironmentalScore;
            ratings.CleanlinessScore = updatedRating.CleanlinessScore;
            ratings.FoodScore = updatedRating.FoodScore;

            await _context.SaveChangesAsync();

            return Ok("The rating has been updated");

        }


        // Delete a Rating
        // ***Not sure why we would want to create access to delete a rating... did this more for the practice of the idea of deleting results. *** 
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteRating([FromUri] int id)
        {
            Rating rating = await _context.Ratings.FindAsync(id);

            if (rating is null)
                return NotFound();

            _context.Ratings.Remove(rating);

            if(await _context.SaveChangesAsync() == 1)
            {
                return Ok("The rating has been deleted.");
            }

            return InternalServerError();


        }

    }
}

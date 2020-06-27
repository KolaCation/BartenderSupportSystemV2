﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BartenderSupportSystem.Server.Data;
using BartenderSupportSystem.Server.DomainServices.DbModels.RecommendationSystem;
using BartenderSupportSystem.Shared.Models.RecommendationSystem;

namespace BartenderSupportSystem.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public IngredientsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Ingredients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingredient>>> GetIngredient()
        {
            var ingredientDbModels = await _context.IngredientsSet.ToListAsync();
            var ingredients = _mapper.Map<List<IngredientDbModel>, List<Ingredient>>(ingredientDbModels);
            return ingredients;
        }

        // GET: api/Ingredients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ingredient>> GetIngredient(Guid id)
        {
            var ingredientDbModel = await _context.IngredientsSet.FindAsync(id);

            if (ingredientDbModel == null)
            {
                return NotFound();
            }

            var ingredient = _mapper.Map<IngredientDbModel, Ingredient>(ingredientDbModel);
            return ingredient;
        }

        // PUT: api/Ingredients/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngredient(Guid id, Ingredient ingredient)
        {
            if (!id.Equals(ingredient.Id))
            {
                return BadRequest();
            }

            var ingredientDbModel = _mapper.Map<Ingredient, IngredientDbModel>(ingredient);
            _context.Entry(ingredientDbModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Ingredients
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<IActionResult> PostIngredient(Ingredient ingredient)
        {
            var ingredientDbModel = _mapper.Map<Ingredient, IngredientDbModel>(ingredient);
            await _context.IngredientsSet.AddAsync(ingredientDbModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Ingredients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient(Guid id)
        {
            var ingredientDbModel = await _context.IngredientsSet.FindAsync(id);
            if (ingredientDbModel == null)
            {
                return NotFound();
            }

            _context.IngredientsSet.Remove(ingredientDbModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IngredientExists(Guid id)
        {
            return _context.IngredientsSet.Any(e => e.Id.Equals(id));
        }
    }
}
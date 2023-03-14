﻿using Application.Abstractions.Interfaces;
using Application.DTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AnimalLocationPointService :
        IAnimalLocationPointService
    {

        private readonly IApplicationDbContext _applicationDbContext;

        public AnimalLocationPointService(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<AnimalVisitedLocation>> SearchAsync(
            long animalId,
            LocationFilter filter,
            int from = 0,
            int size = 10)
        {
            var query = _applicationDbContext.AnimalVisitedLocations
                .Where(x => x.AnimalId == animalId);
            if(filter.StartDateTime != null)
                query = query.Where(x => x.DateTimeOfVisitLocationPoint >= filter.StartDateTime);
            if(filter.EndDateTime != null)
                query = query.Where(x => x.DateTimeOfVisitLocationPoint <= filter.EndDateTime);

            return await query.Skip(from).Take(size)
                .OrderBy(x => x.DateTimeOfVisitLocationPoint)
                .ToListAsync();
        }

        public async Task<AnimalVisitedLocation> AddAsync(long animalId, long pointId)
        {
            var animal = await _applicationDbContext.Animals
                .FindAsync(animalId);            
            
            var point = await _applicationDbContext.LocationPoints
                .FindAsync(pointId);

            if(animal == null)
                throw new NotFoundException($"Animal with Id {animalId} is not found");

            if(animal == null)
                throw new NotFoundException($"LocationPoint with Id {pointId} is not found");

            if(animal.LifeStatus == LifeStatus.DEAD)
                throw new InvalidOperationException("The Animal is dead, moving is impossible");

            AnimalVisitedLocation location = new AnimalVisitedLocation()
            {
                AnimalId = animalId,
                LocationPointId = pointId,
                DateTimeOfVisitLocationPoint = DateTimeOffset.UtcNow
            };

            if(animal.VisitedLocations == null)
                animal.VisitedLocations = new List<AnimalVisitedLocation>();

            if(animal.VisitedLocations.Any() == false)
            {
                if(pointId == animal.ChippingLocationId)
                    throw new InvalidOperationException($"Animal is already at this LocationPoint");
                
                else
                {
                    _applicationDbContext.AnimalVisitedLocations
                        .Add(location);
                }
            }
            else
            {
                AnimalVisitedLocation last = animal.VisitedLocations
                    .OrderBy(x => x.DateTimeOfVisitLocationPoint)
                    .Last();

                if(last.Id == pointId)
                    throw new InvalidOperationException($"Animal is already at this LocationPoint");

                else
                {
                    _applicationDbContext.AnimalVisitedLocations
                        .Add(location);
                }
            }




            //var currentLocation = animal.VisitedLocations
            //    .OrderBy(x => x.DateTimeOfVisitLocationPoint)
            //    .LastOrDefault();
            //
            //if(currentLocation == null)
            //    throw new Exception($"Animal doesnt have any location points");
            //
            //if (currentLocation.Id == pointId)
            //    throw new InvalidOperationException("The Animal is already at this LocationPoint");  
            //
            //
            
            //
            //
            //if(animal.VisitedLocations.Any() == false)
            //{
            //    animal.VisitedLocations.Add(location);
            //}
            //else if(animal.VisitedLocations.OrderBy(x => x.DateTimeOfVisitLocationPoint).LastOrDefault())
            //{
            //    var lastLocation = 
            //
            //} 

            

            await _applicationDbContext
                .SaveChangesAsync();
            
            return location;
        }

        public Task<int> RemoveAsync(long animalId, long pointId)
        {
            throw new NotImplementedException();
        }


        public Task<int> UpdateAsync(long animalId, AnimalVisitedLocation location)
        {
            throw new NotImplementedException();
        }
    }
}
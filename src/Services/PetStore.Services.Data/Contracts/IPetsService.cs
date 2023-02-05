﻿namespace PetStore.Services.Data.Contracts
{
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;

    public interface IPetsService
    {
        IQueryable<Pet> GetAllPetsNoTracking();

        bool IsPetExistingInDb(Pet pet);

        Task AddPetAsync(Pet pet);
    }
}

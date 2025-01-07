﻿using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Entities;

namespace MinimalAPIsMovies.Repositories
{
    public interface IActorsRepository
    {
        Task<Actor> Create(Actor actor);
        Task Delete(int id);
        Task<bool> Exists(int id);
        Task<List<int>> Exists(List<int> ids);
        Task<List<Actor>> GetAll(PaginationDTO pagination);       
        Task<Actor?> GetById(int id);
        Task<List<Actor>> GetByName(string name);
        Task Update(Actor actor);
    }
}
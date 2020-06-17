﻿using System;
using BartenderSupportSystem.Domain.RecommendationSystem.Enums;

namespace BartenderSupportSystem.Server.DbModels.RecommendationSystem
{
    internal sealed class CocktailDbModel
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public CocktailType Type { get; private set; }
        public string PhotoPath { get; private set; }

        public CocktailDbModel(Guid id, string name, CocktailType type, string photoPath)
        {
            Id = id;
            Name = name;
            Type = type;
            PhotoPath = photoPath;
        }
    }
}

﻿using System;
using BartenderSupportSystem.Shared.Models.RecommendationSystem.Enums;

namespace BartenderSupportSystem.Server.DomainServices.DbModels.RecommendationSystem
{
    internal sealed class BrandDbModel
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Countries CountryOfOrigin { get; private set; }

        public BrandDbModel(Guid id, string name, Countries countryOfOrigin)
        {
            Id = id;
            Name = name;
            CountryOfOrigin = countryOfOrigin;
        }
    }
}

﻿using System;
using BartenderSupportSystem.Shared.Models.RecommendationSystem.Enums;

namespace BartenderSupportSystem.Shared.Models.RecommendationSystem
{
    public sealed class DrinkDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AlcoholType Type { get; set; }
        public double AlcoholPercentage { get; set; }
        public string Flavor { get; set; }
        public int BrandId { get; set; }
        public BrandDto Brand { get; set; }
        public double PricePerMl { get; set; }
        public string PhotoPath { get; set; }
    }
}
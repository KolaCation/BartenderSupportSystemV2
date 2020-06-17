﻿using BartenderSupportSystem.Domain.RecommendationSystem;
using BartenderSupportSystem.Server.DbModels.RecommendationSystem;
using BartenderSupportSystem.Server.Mappers.Interfaces.RecommendationSystem;

namespace BartenderSupportSystem.Server.Mappers.Implementation.RecommendationSystem
{
    internal sealed class SnackMapper : ISnackMapper
    {
        public Snack DbToDomain(SnackDbModel dbModel)
        {
            return new Snack(dbModel.Id, dbModel.Name, dbModel.PricePerGr);
        }

        public SnackDbModel DomainToDb(Snack domain)
        {
            return new SnackDbModel(domain.Id, domain.Name, domain.PricePerGr);
        }
    }
}

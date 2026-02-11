using Insurance.Domain.Clients;
using Insurance.Domain.Exceptions;
using Insurance.Domain.RiskIndicators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Buildings
{
    public class Building
    {
        public Guid Id { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid CityId { get; private set; }
        public BuildingType Type { get; private set; }
        public string Street { get; private set; } = string.Empty;
        public string Number { get; private set; } = string.Empty;
        public int ConstructionYear { get; private set; } 
        public int NumberOfFloors { get; private set; }
        public decimal SurfaceArea { get; private set; }
        public decimal InsuredValue { get; private set; }

        private Building() { }

        public static Building Create(
            Guid clientId,
            Guid cityId,
            BuildingType type,
            string street,
            string number,
            int constructionYear,
            int numberOfFloors,
            decimal surfaceArea,
            decimal insuredValue)
        {
            if(constructionYear < 1700 || constructionYear>DateTime.Now.Year)
            {
                throw new BuildingConstructionYearNotAllowedException("Invalid construction year.");
            }

            return new Building
            {
                Id = Guid.NewGuid(),
                ClientId = clientId,
                CityId = cityId,
                Type = type,
                Street = street,
                Number = number,
                ConstructionYear = constructionYear,
                NumberOfFloors = numberOfFloors,
                SurfaceArea = surfaceArea,
                InsuredValue = insuredValue
            };
        }

        public void UpdateDetails(
        int constructionYear,
        int numberOfFloors,
        decimal surfaceArea,
        decimal insuredValue,
        string street,
        string number)
        {
            ConstructionYear = constructionYear;
            NumberOfFloors = numberOfFloors;
            SurfaceArea = surfaceArea;
            InsuredValue = insuredValue;
            Street = street;
            Number = number;
        }

        public static Building Rehydrate(
        Guid id,
        Guid clientId,
        Guid cityId,
        BuildingType type,
        string street,
        string number,
        int constructionYear,
        int numberOfFloors,
        decimal surfaceArea,
        decimal insuredValue)
        {
            return new Building
            {
                Id = id,
                ClientId = clientId,
                CityId = cityId,
                Type = type,
                Street = street,
                Number = number,
                ConstructionYear = constructionYear,
                NumberOfFloors = numberOfFloors,
                SurfaceArea = surfaceArea,
                InsuredValue = insuredValue
            };
        }
    }

}

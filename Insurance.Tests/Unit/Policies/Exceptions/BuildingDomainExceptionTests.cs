using Insurance.Domain.Buildings;
using Insurance.Domain.Exceptions;
using Insurance.Domain.RiskIndicators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Policies.Exceptions
{
    public class BuildingDomainExceptionTests
    {
        [Fact]
        public void Create_WithNegativeConstructionYear_ShouldThrowBuildingConstructionYearNotAllowedException()
        {
            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();

            var exception = Assert.Throws<BuildingConstructionYearNotAllowedException>(() =>
                Building.Create(
                    clientId,
                    cityId,
                    BuildingType.Residential,
                    "Main Street",
                    "10",
                    -1, 
                    2,
                    120.5m,
                    150000m));

            Assert.NotNull(exception.Message);
        }

        [Fact]
        public void Create_WithFutureConstructionYear_ShouldThrowBuildingConstructionYearNotAllowedException()
        {
            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();

            var exception = Assert.Throws<BuildingConstructionYearNotAllowedException>(() =>
                Building.Create(
                    clientId,
                    cityId,
                    BuildingType.Residential,
                    "Main Street",
                    "10",
                    DateTime.UtcNow.Year + 10, 
                    2,
                    120.5m,
                    150000m));

            Assert.NotNull(exception.Message);
        }

        [Fact]
        public void Create_WithConstructionYearBeforeMinimum_ShouldThrowBuildingConstructionYearNotAllowedException()
        {
            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();

            var exception = Assert.Throws<BuildingConstructionYearNotAllowedException>(() =>
                Building.Create(
                    clientId,
                    cityId,
                    BuildingType.Residential,
                    "Main Street",
                    "10",
                    1699, 
                    2,
                    120.5m,
                    150000m));

            Assert.NotNull(exception.Message);
        }
    }
}

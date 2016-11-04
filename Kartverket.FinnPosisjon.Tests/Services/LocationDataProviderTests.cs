﻿using FluentAssertions;
using Kartverket.FinnPosisjon.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using Xunit.Sdk;

namespace Kartverket.FinnPosisjon.Services.Tests.Services
{
    [TestClass]
    public class LocationDataProviderTests
    {
        [Fact]
        public void FetchAndSetTest()
        {
            var position = new Position {ReferenceCoordinates = new Coordinates {X = 9.05871164, Y = 59.41283416 }};

            AddressDataProvider.FetchAndSet(position);

            //position.AddressData.Address.Should().Be("Kyrkjevegen");

            position.AddressData.Place.Should().Be("BØ I TELEMARK");

            position.AddressData.ZipCode.Should().Be("3800");

            position.AddressData.DistanceFromPosition.Should().BeLessOrEqualTo(100);
        }
    }
}
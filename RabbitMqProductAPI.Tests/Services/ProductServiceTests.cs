using System.Collections.Generic;
using FluentAssertions;
using Moq;
using RabbitMqProductAPI.Models;
using RabbitMqProductAPI.Services;
using RabbitMqProductAPI.RabbitMQ;
using Xunit;
using AutoFixture;
using AutoFixture.AutoMoq;

namespace RabbitMqProductAPI.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<IRabbitMQProducer> _rabbitMQProducerMock;

        public ProductServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _productServiceMock = _fixture.Freeze<Mock<IProductService>>();
            _rabbitMQProducerMock = _fixture.Freeze<Mock<IRabbitMQProducer>>();
        }

        [Fact]
        public void GetProductList_ShouldReturnListOfProducts()
        {
            // Arrange
            var expectedProducts = _fixture.Create<List<Product>>();
            _productServiceMock
                .Setup(service => service.GetProductList())
                .Returns(expectedProducts);

            var sut = _fixture.Create<IProductService>();

            // Act
            var result = sut.GetProductList();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedProducts);
            result.Count().Should().Be(expectedProducts.Count);
            _productServiceMock.Verify(service => service.GetProductList(), Times.Once);
        }

        [Fact]
        public void GetProductById_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var expectedProduct = _fixture.Create<Product>();
            _productServiceMock
                .Setup(service => service.GetProductById(expectedProduct.ProductId))
                .Returns(expectedProduct);

            var sut = _fixture.Create<IProductService>();

            // Act
            var result = sut.GetProductById(expectedProduct.ProductId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedProduct);
            _productServiceMock.Verify(service => service.GetProductById(expectedProduct.ProductId), Times.Once);
        }
    }
}

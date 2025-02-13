﻿using BlazorIndexedOrm.Core.Transaction;
using BlazorIndexedOrm.Core.Transaction.JsExpression;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MemberTranslation;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation;
using BlazorIndexedOrm.Core.UnitTests.Mock.IndexedDbDatabase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;

namespace BlazorIndexedOrm.Core.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void RegisterIndexedDbDatabase_WithNullServiceCollection_ShouldThrowArgumentNullException()
    {
        // Arrange
        IServiceCollection serviceCollection = null!;

        // Act
        Action action = () => serviceCollection.RegisterIndexedDbDatabase<MockIndexedDbDatabase>();
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("serviceCollection", exception.ParamName);
    }

    [Fact]
    public void RegisterIndexedDbDatabase_ShouldRegisterIndexedDbDatabase()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var mockJsRuntime = new Mock<IJSRuntime>().Object;
        serviceCollection.AddScoped<IJSRuntime>(_ => mockJsRuntime);

        // Act
        serviceCollection.RegisterIndexedDbDatabase<MockIndexedDbDatabase>();

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var database = serviceProvider.GetRequiredService<MockIndexedDbDatabase>();

        Assert.NotNull(database);
    }

    [Fact]
    public void RegisterIndexedDbDatabase_ShouldRegistersJsMethodCallTranslatorFactory()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var mockJsRuntime = new Mock<IJSRuntime>().Object;
        serviceCollection.AddScoped<IJSRuntime>(_ => mockJsRuntime);

        // Act
        serviceCollection.RegisterIndexedDbDatabase<MockIndexedDbDatabase>();

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var methodTranslatorFactory = serviceProvider.GetRequiredService<IMethodCallTranslatorFactory>();

        Assert.NotNull(methodTranslatorFactory);
        Assert.IsType<JsMethodCallTranslatorFactory>(methodTranslatorFactory);
    }

    [Fact]
    public void RegisterIndexedDbDatabase_ShouldRegisterJsMemberTranslatorFactory()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var mockJsRuntime = new Mock<IJSRuntime>().Object;

        // Act
        serviceCollection.AddScoped<IJSRuntime>(_ => mockJsRuntime);
        serviceCollection.RegisterIndexedDbDatabase<MockIndexedDbDatabase>();
        
        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var memberTranslatorFactory = serviceProvider.GetRequiredService<IMemberTranslatorFactory>();
        Assert.NotNull(memberTranslatorFactory);
        Assert.IsType<JsMemberTranslatorFactory>(memberTranslatorFactory);
    }

    [Fact]
    public void RegisterIndexedDbDatabase_ShouldRegisterJsExpressionBuilder()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var mockJsRuntime = new Mock<IJSRuntime>().Object;

        // Act
        serviceCollection.AddScoped<IJSRuntime>(_ => mockJsRuntime);
        serviceCollection.RegisterIndexedDbDatabase<MockIndexedDbDatabase>();
        
        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var expressionBuilder = serviceProvider.GetRequiredService<IExpressionBuilder>();
        Assert.NotNull(expressionBuilder);
        Assert.IsType<JsExpressionBuilder>(expressionBuilder);
    }

    [Fact]
    public void RegisterIndexedDbDatabase_ShouldRegisterIndexedDbTransactionProviderFactory()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var mockJsRuntime = new Mock<IJSRuntime>().Object;
        // Act
        serviceCollection.AddScoped<IJSRuntime>(_ => mockJsRuntime);
        serviceCollection.RegisterIndexedDbDatabase<MockIndexedDbDatabase>();

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var transactionProviderFactory = serviceProvider.GetRequiredService<IIndexedDbTransactionProviderFactory>();
        Assert.NotNull(transactionProviderFactory);
        Assert.IsType<IndexedDbTransactionProviderFactory>(transactionProviderFactory);
    }
}
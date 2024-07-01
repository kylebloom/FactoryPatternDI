# Sample Console App with Factory Pattern, Dependency Injection, and Async/Await

This repository contains a sample C# console application that demonstrates the use of the Factory Pattern combined with Dependency Injection and asynchronous programming using `async` and `await`.

# Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Setup and Installation](#setup-and-installation)
- [Usage](#usage)
- [Code Overview](#code-overview)
  - [Factory Pattern](#factory-pattern)
  - [Dependency Injection](#dependency-injection)
  - [Async/Await](#async-await)

## Introduction

This sample application showcases how to implement the Factory Pattern and use Dependency Injection in a C# console application, along with asynchronous methods using `async` and `await`.

## Features

- Factory Pattern for creating instances of different workers.
- Dependency Injection for managing dependencies.
- Asynchronous methods to simulate I/O-bound operations.


## Usage

The application demonstrates initializing workers that were created via a factory that perform async methods. The worker creation is also async which could simulate fetching necessary resources via api or other. Dependencies injected using the Microsoft.Extensions.DependencyInjection library.

## Code Overview

### Factory Pattern

- **IProduct.cs**: Interface for workers (the 'product' in a factory pattern).
- **ConcreteProductA.cs**: A concrete implementation of IProduct.
- **ConcreteProductB.cs**: Another implementation of IProduct.
- **ProductFactory.cs**: Factory for creating worker instances.

### Dependency Injection

- **Program.cs**: Sets up the DI container and configures services.

### Async/Await

- **ProductFactory.cs**: Contains the `AsyncAwait`code that simulates fetching resources during the creation of the worker instances. Could be useful to retrieve API keys or other config-related resources that are hosted outside of the app.

### Example Code

**IProduct.cs**
```csharp
public interface IProduct
{
    public void DoWork();
}
```

**ConcreteProductA.cs**
```csharp
public class ConcreteProductA : IProduct
{
      public void DoWork()
      {
          Console.WriteLine("ConcreteProductA is doing work.");
      }
}
```

**ConcreteProductB.cs**
```csharp
public class ConcreteProductB : IProduct
{
      public void DoWork()
      {
          Console.WriteLine("ConcreteProductB is doing work.");
      }
}
```

**ProductFactory.cs**
```csharp
public class ProductFactory(IServiceProvider serviceProvider) : IProductFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<IProduct> CreateProduct(string type)
    {

        return type.ToLower() switch
        {
            "a" => await GetWorker<ConcreteProductA>(),
            "b" => await GetWorker<ConcreteProductB>(),
            _ => throw new ArgumentException("Invalid product type"),
        };
    }

    private async Task<TWorker> GetWorker<TWorker>() where TWorker : class, IProduct
    {
        await Task.Delay(3000);
        if (_serviceProvider.GetService(serviceType: typeof(TWorker)) is not TWorker worker)
        {
            throw new ArgumentException($"Error in DI Setup. {typeof(TWorker)} was null.");
        }
        return worker;
    }
}

public interface IProductFactory
{
    Task<IProduct> CreateProduct(string type);
}
```

**Program.cs**
```csharp
using FactoryPatternDI.Factories;
using FactoryPatternDI.Models.Products;
using Microsoft.Extensions.DependencyInjection;

namespace FactoryPatternDI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Setup DI
            var serviceProvider = new ServiceCollection()
                .AddTransient<ConcreteProductA>()
                .AddTransient<ConcreteProductB>()
                .AddSingleton<IProductFactory, ProductFactory>()
                .BuildServiceProvider();

            var factory = serviceProvider.GetService<IProductFactory>();
            if (factory == null)
            {
                Console.WriteLine("Error in setting up DI. Exiting program. Press any key to exit.");
                Console.ReadLine();
                return;
            }

            while (true)
            {
                Console.WriteLine("Enter type of product to do work:");
                var type = Console.ReadLine();
                if (string.IsNullOrEmpty(type))
                {
                    Console.WriteLine("invalid entry. press any key to continue.");
                    Console.ReadLine();
                    continue;
                }
                var worker = await factory.CreateProduct(type);
                worker.DoWork();
                Console.WriteLine("Press any key to continue.");
                Console.ReadLine();
            }
        }
    }
}
```


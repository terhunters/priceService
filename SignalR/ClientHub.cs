using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR.Client;
using PriceService.DataBase;
using PriceService.DTO;

namespace PriceService.SignalR;

public class ClientHub
{
    private readonly IPricesRepository _repository;
    private readonly HubConnection _connection;

    public ClientHub(IPricesRepository repo, string connectionString)
    {
        _repository = repo;
        
        Console.WriteLine($"Connection string to SignalR: {connectionString}");
        _connection = new HubConnectionBuilder().WithUrl(connectionString).Build();
        
        RegisterFunctions();

        ConnectToHub();
    }

    private void RegisterFunctions()
    {
        _connection.On<string>("Notify", LoggerForNotify);
        _connection.On<IEnumerable<CreatePlatformDto>>("ReceivePlatforms", UpdateNotExistingPlatforms);
    }

    private async void ConnectToHub()
    {
        Console.WriteLine("Start connect to server hub(SignalR)");
        try
        {
            await _connection.StartAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Cannot connect to server hub(SignalR): {ex.Message}");
        }
    }

    private void LoggerForNotify(string message)
    {
        Console.WriteLine($"Response from server: {message}");
    }
    
    private void UpdateNotExistingPlatforms(IEnumerable<CreatePlatformDto> platforms)
    {
        Console.WriteLine($"Start updating platforms to synchronize them");
    }

    public async void GetAllPlatforms()
    {
        try
        {
            await _connection.InvokeAsync("GetAllPlatforms");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Cannot get platforms from signalR: {ex.Message}");
        }
    }
}
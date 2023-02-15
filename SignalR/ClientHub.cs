using System;
using System.Collections.Generic;
using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using PriceService.DataBase;
using PriceService.DTO;
using PriceService.Model;

namespace PriceService.SignalR;

enum TypeTransport
{
    SSE,
    LongPooling,
    WebSocket
}

public class ClientHub
{
    private readonly IPricesRepository _repository;
    private readonly IMapper _mapper;
    private readonly HubConnection _connWebSocket;
    private readonly string _connectionStringWebSocket;
    private readonly string _connectionStringLongPooling;
    private readonly string _connectionStringSse;
    private readonly HubConnection _connLongPooling;
    private readonly HubConnection _connSse;
    private Stopwatch _wtchWebSocket;
    private Stopwatch _wtchSse;
    private Stopwatch _wtchLongPooling;

    public ClientHub(IPricesRepository repo, IConfiguration configuration, IMapper mapper)
    {
        _repository = repo;
        _connectionStringWebSocket = configuration.GetConnectionString("WebSocket");
        _connectionStringSse = configuration.GetConnectionString("SSE");
        _connectionStringLongPooling = configuration.GetConnectionString("LongPooling");
        Console.WriteLine($"Connection string to SignalR(WebSocket): {_connectionStringWebSocket}");
        Console.WriteLine($"Connection string to SignalR(SSE): {_connectionStringSse}");
        Console.WriteLine($"Connection string to SignalR(LongPooling): {_connectionStringLongPooling}");
        _connWebSocket = new HubConnectionBuilder().WithUrl(_connectionStringWebSocket).Build();
        _connSse = new HubConnectionBuilder().WithUrl(_connectionStringSse).Build();
        _connLongPooling = new HubConnectionBuilder().WithUrl(_connectionStringLongPooling).Build();
        _mapper = mapper;
        
        RegisterFunctions();

        ConnectToHub();
    }

    private void RegisterFunctions()
    {
        _connWebSocket.On<string>("Notify", message => LoggerForNotify(message, TypeTransport.WebSocket));
        _connWebSocket.On<IEnumerable<CreatePlatformDto>>("ReceivePlatforms", platforms => UpdateNotExistingPlatforms(platforms, TypeTransport.WebSocket));
        _connSse.On<string>("Notify", message => LoggerForNotify(message, TypeTransport.SSE));
        _connSse.On<IEnumerable<CreatePlatformDto>>("ReceivePlatforms", platforms => UpdateNotExistingPlatforms(platforms, TypeTransport.SSE));
        _connLongPooling.On<string>("Notify", message => LoggerForNotify(message, TypeTransport.LongPooling));
        _connLongPooling.On<IEnumerable<CreatePlatformDto>>("ReceivePlatforms", platforms => UpdateNotExistingPlatforms(platforms, TypeTransport.LongPooling));
    }

    private async void ConnectToHub()
    {
        Console.WriteLine("Start connect to server hub(SignalR)");
        try
        {
            await _connWebSocket.StartAsync();
            await _connSse.StartAsync();
            await _connLongPooling.StartAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Cannot connect to server hub(SignalR): {ex.Message}");
        }
    }

    private void LoggerForNotify(string message, TypeTransport typeTransport)
    {
        Console.WriteLine($"Response from server ({typeTransport.ToString()}): {message}");
    }
    
    private void UpdateNotExistingPlatforms(IEnumerable<CreatePlatformDto> platforms, TypeTransport typeTransport)
    {
        Console.WriteLine($"Start updating platforms to synchronize them {typeTransport.ToString()}");

        switch (typeTransport)
        {
            case TypeTransport.WebSocket: 
                _wtchWebSocket.Stop();
                Console.WriteLine($"ElapsedMilliseconds for {typeTransport.ToString()}: {_wtchWebSocket.ElapsedMilliseconds}");
                break;
            case TypeTransport.SSE: 
                _wtchSse.Stop();
                Console.WriteLine($"ElapsedMilliseconds for {typeTransport.ToString()}: {_wtchSse.ElapsedMilliseconds}");
                break;
            case TypeTransport.LongPooling: 
                _wtchLongPooling.Stop();
                Console.WriteLine($"ElapsedMilliseconds for {typeTransport.ToString()}: {_wtchLongPooling.ElapsedMilliseconds}");
                break;
        }

        foreach (var platform in platforms)
        {
            if (!_repository.ExternalIdExist(platform.Id))
            {
                _repository.CreatePlatform(_mapper.Map<Platform>(platform));
            }
        }
    }

    public async void GetAllPlatforms()
    {
        try
        {
            _wtchWebSocket = new Stopwatch();
            _wtchWebSocket.Start();
            await _connWebSocket.InvokeAsync("GetAllPlatforms");

            _wtchSse = new Stopwatch();
            _wtchSse.Start();
            await _connSse.InvokeAsync("GetAllPlatforms");
            
            _wtchLongPooling = new Stopwatch();
            _wtchLongPooling.Start();
            await _connLongPooling.InvokeAsync("GetAllPlatforms");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Cannot get platforms from signalR: {ex.Message}");
        }
    }
}
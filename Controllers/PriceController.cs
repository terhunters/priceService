using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PriceService.DataBase;
using PriceService.DTO;
using PriceService.Model;

namespace PriceService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly IPricesRepository _repository;
        private readonly IMapper _mapper;

        public PriceController(IPricesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PriceDto>> GetAllPrices()
        {
            Console.WriteLine("Get all prices");
            return Ok(_mapper.Map<IEnumerable<PriceDto>>(_repository.GetAllPrices()));
        }

        [HttpPost]
        [Route("{platformId}")]
        public ActionResult<PriceDto> CreatePrice(int platformId, CreatePriceDto createDto)
        {
            Console.WriteLine("Create Price from controller");
            var newPrice = _mapper.Map<Price>(createDto);
            
            if (!_repository.CreatePrice(platformId, newPrice))
            {
                return NotFound($"Platform with {nameof(platformId)} = {platformId.ToString()} was not found");
            }
            
            return Ok(_mapper.Map<PriceDto>(newPrice));
        }
    }
}
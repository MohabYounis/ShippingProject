using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Shipping.DTOs;
using Shipping.DTOs.CityDTOs;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Services.ModelService;

namespace Shipping.SignalRHubs
{
    public class CityHub : Hub
    {
    }
}

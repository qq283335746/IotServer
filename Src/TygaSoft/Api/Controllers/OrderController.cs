using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TygaSoft.Model;
using TygaSoft.IServices;
using TygaSoft.Services;

namespace TygaSoft.Api.Controllers
{
    public class OrderController : Controller
    {
        //private readonly IRailwayService _railwayService;
        // public RailwayController(IRailwayService railwayService)
        // {
        //     _railwayService = railwayService;
        // }

        public OrderController() { }

        public HelloResult GetHelloAsync()
        {
            return new HelloResult { ResCode = ResCodeOptions.Success, Data = "Hello world",Message=SR.Response_Ok };
        }
    }
}
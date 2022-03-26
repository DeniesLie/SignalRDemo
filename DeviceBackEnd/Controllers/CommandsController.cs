using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DeviceBackEnd.Services;
namespace DeviceBackEnd.Controllers
{
    [Route("api/commands")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly LiveTerminalService _liveTerminalService;
        public CommandsController(LiveTerminalService liveTerminalService)
        {
            _liveTerminalService = liveTerminalService;
        }
        
        [HttpPost]
        public IActionResult SendCommand(string command)
        {
            _liveTerminalService.
        }
        
    }
}
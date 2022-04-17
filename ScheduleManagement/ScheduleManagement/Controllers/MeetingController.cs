using BusinessLayer;
using DataLayer.Interface;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingController : ControllerBase
    {
        private readonly ScheduleService _scheduleservice;

        private readonly IRepository<MeetingEvent> _MeetingEvent;

        public MeetingController(IRepository<MeetingEvent> MeetingEvent, ScheduleService ScheduleService)
        {
            _scheduleservice = ScheduleService;
            _MeetingEvent = MeetingEvent;

        }

        [HttpPost("AddMeeting")]
        public async Task<Object> AddMeeting([FromBody] MeetingEvent meeting)
        {
            try
            {
                await _scheduleservice.AddMeeting(meeting);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        [HttpDelete("DeleteMeeting")]
        //[Authorize(Roles = "Admin")]
        public bool DeleteMeeting(int Id)
        {
            try
            {
                _scheduleservice.DeleteMeeting(Id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpPost("UpdateMeeting")]
        // [Authorize(Roles = "Admin, Employee")]
        public bool UpdateMeeting(MeetingEvent meeting)
        {
            try
            {
                _scheduleservice.UpdateMeeting(meeting);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpGet("GetAllMeeting")]
        public Object GetAllMeeting()
        {
            var data = _scheduleservice.GetAllMeeting();
            var json = JsonConvert.SerializeObject(data, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                }
            );
            return json;
        }

        [HttpGet("GetMeetingById")]
        public Object GetMeetingById(int Id)
        {
            var data = _scheduleservice.GetMeetingById(Id);
            var json = JsonConvert.SerializeObject(data, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                }
            );
            return json;
        }
    }
}

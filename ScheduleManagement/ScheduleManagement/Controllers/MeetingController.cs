using BusinessLayer;
using DataLayer.Interface;
using DataLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        [Authorize(Roles = "Employee")]
        public async Task<Object> AddMeeting([FromBody] MeetingEvent meeting)
        {
            try
            {
                await _scheduleservice.AddMeeting(meeting);


                //Calling AWS SNS services.

                //Mobile Notification
                AWSSNS aWSNotification = new AWSSNS();
                aWSNotification.CreateTopic("MeetingInvite", "New Meeting Events");
                aWSNotification.CreateMobileSubscription("MeetingInvite", "9819646351");
                aWSNotification.PublishMobileMessage("Your new meeting invited created.", "9819646351");

                StringBuilder sb = new StringBuilder();
                sb.Append("Dear " + "Employee");
                sb.Append(Environment.NewLine);
                sb.Append("You have set up a meeting invite from: " + meeting.StartDate + " to:" + meeting.EndDate);
                sb.Append(Environment.NewLine);
                sb.Append("Meeting agenda" + meeting.Description);
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append("Thanks.");

                //Email Notification
                aWSNotification.CreateEmailSubscription("MeetingInvite", "diinesh.kanojia@gmail.com");
                aWSNotification.PublishEmailMessage("MeetingInvite", sb.ToString(), "Meeting details.");

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

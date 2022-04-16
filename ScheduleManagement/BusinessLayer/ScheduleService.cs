using DataLayer.Interface;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ScheduleService
    {
        private readonly IRepository<MeetingEvent> _meetingevent;

        public ScheduleService(IRepository<MeetingEvent> meetingevent)
        {
            _meetingevent = meetingevent;
        }


        public IEnumerable<MeetingEvent> GetAllPersons()
        {
            try
            {
                return _meetingevent.GetAll().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MeetingEvent> AddMeeting(MeetingEvent meetingevent)
        {
            return await _meetingevent.Create(meetingevent);
        }

        public bool DeleteMeeting(int id)
        {

            try
            {
                var DataList = _meetingevent.GetAll().Where(x => x.Id == id).ToList();
                foreach (var item in DataList)
                {
                    _meetingevent.Delete(item);
                }
                return true;
            }
            catch (Exception)
            {
                return true;
            }

        }

        public bool UpdateMeeting(MeetingEvent meetingevent)
        {
            try
            {
                var meeting = _meetingevent.GetById(meetingevent.Id);
                if (meeting != null)
                {
                    meeting.EndDate = meetingevent.EndDate;
                    meeting.Description = meetingevent.Description;
                    meeting.StartDate = meetingevent.StartDate;
                    meeting.UserEmail = meetingevent.UserEmail;
                    _meetingevent.Update(meeting);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return true;
        }
    }
}

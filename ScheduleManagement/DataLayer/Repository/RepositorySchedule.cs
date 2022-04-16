using DataLayer.Interface;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class RepositorySchedule : IRepository<MeetingEvent>
    {
        ApplicationDBContext _dbContext;
        public RepositorySchedule(ApplicationDBContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }
        public async Task<MeetingEvent> Create(MeetingEvent _object)
        {
            var obj = await _dbContext.MeetingEvents.AddAsync(_object);
            _dbContext.SaveChanges();
            return obj.Entity;
        }

        public void Delete(MeetingEvent _object)
        {
            _dbContext.Remove(_object);
            _dbContext.SaveChanges();
        }

        public IEnumerable<MeetingEvent> GetAll()
        {
            try
            {
                var meetingEvents = _dbContext.MeetingEvents.ToList();

                return meetingEvents;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public MeetingEvent GetById(int Id)
        {
            var meetingEvents = _dbContext.MeetingEvents.FirstOrDefault(x => x.Id == Id);
            return meetingEvents;
        }

        public void Update(MeetingEvent _object)
        {
            try
            {
                _dbContext.Entry(_object).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

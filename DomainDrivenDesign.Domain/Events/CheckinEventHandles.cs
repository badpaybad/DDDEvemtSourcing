﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainDrivenDesign.Core.Events;
using DomainDrivenDesign.Core.Implements;
using DomainDrivenDesign.Core.Implements.Model;

namespace DomainDrivenDesign.Domain.Events
{
    public class CheckinEventHandles : IEventHandle<CheckinCreated>,
        IEventHandle<CheckinCommentCommented>,IEventHandle<CheckinCompleted>
    {
        public void Handle(CheckinCreated e)
        {
            using (var db=new TestDbContext())
            {
                db.CheckinTests.Add(new CheckinTest()
                {
                    Id = (Guid) e.Id,
                    Duration = e.Duration,
                    StartDate = e.StartDate,
                    Status= e.Status
                });
                db.SaveChanges();
            }
        }

        public void Handle(CheckinCommentCommented e)
        {
            using (var db = new TestDbContext())
            {
                db.CheckinHistoryTests.Add(new CheckinHistoryTest()
                {
                    Id = e.Id,
                    Comment =e.Comment,
                    CreatedOn=e.CreatedOn,
                    UserId=e.UserId,
                    CheckinId=e.CheckinId
                });
                db.SaveChanges();
            }
        }

        public void Handle(CheckinCompleted e)
        {
            using (var db = new TestDbContext())
            {
                var temp = db.CheckinTests.SingleOrDefault(i => i.Id == e.Id);
                if (temp != null)
                {
                    temp.Status = e.Status;
                    db.SaveChanges();
                }
            }
        }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PoolHockeyBOL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Config> Configs { get; set; }
        public virtual DbSet<PastPlayerInfo> PastPlayerInfoes { get; set; }
        public virtual DbSet<PlayerInfo> PlayerInfoes { get; set; }
        public virtual DbSet<PoolLastYear> PoolLastYears { get; set; }
        public virtual DbSet<TeamSchedule> TeamSchedules { get; set; }
        public virtual DbSet<UserFact> UserFacts { get; set; }
        public virtual DbSet<UserInfo> UserInfoes { get; set; }
        public virtual DbSet<PlayoffPlayerInfo> PlayoffPlayerInfoes { get; set; }
        public virtual DbSet<PlayoffUserInfo> PlayoffUserInfoes { get; set; }
    
        public virtual int ClearAllInjuredStatuses()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ClearAllInjuredStatuses");
        }
    
        public virtual int ClearAllStatuses()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ClearAllStatuses");
        }
    
        public virtual int ClearPlayingToday()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ClearPlayingToday");
        }
    
        public virtual ObjectResult<Nullable<System.DateTime>> GetLastUpdate()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.DateTime>>("GetLastUpdate");
        }
    
        public virtual int SetLastUpdate(Nullable<System.DateTime> datetime)
        {
            var datetimeParameter = datetime.HasValue ?
                new ObjectParameter("datetime", datetime) :
                new ObjectParameter("datetime", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SetLastUpdate", datetimeParameter);
        }
    
        public virtual int SetPlayingToday(string team)
        {
            var teamParameter = team != null ?
                new ObjectParameter("Team", team) :
                new ObjectParameter("Team", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SetPlayingToday", teamParameter);
        }
    }
}

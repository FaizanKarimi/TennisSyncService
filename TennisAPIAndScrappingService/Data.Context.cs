﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TennisAPIAndScrappingService
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SportsDataPanelEntities : DbContext
    {
        public SportsDataPanelEntities()
            : base("name=SportsDataPanelEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Players> Players { get; set; }
        public virtual DbSet<GrandSlamInfo> GrandSlamInfo { get; set; }
        public virtual DbSet<Matches> Matches { get; set; }
        public virtual DbSet<MatchScores> MatchScores { get; set; }
        public virtual DbSet<PlayerOverview> PlayerOverview { get; set; }
        public virtual DbSet<RankingPlayersProfiles> RankingPlayersProfiles { get; set; }
    }
}

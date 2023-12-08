//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class MatchScores
    {
        public decimal MatchScoreId { get; set; }
        public decimal MatchId { get; set; }
        public short ScoreInfoTypeId { get; set; }
        public string HomeScore { get; set; }
        public string AwayScore { get; set; }
        public Nullable<bool> TieBreak { get; set; }
        public string HomeTieBreakScore { get; set; }
        public string AwayTieBreakScore { get; set; }
        public Nullable<long> EventCode { get; set; }
        public Nullable<bool> Verified { get; set; }
        public byte[] change_stamp { get; set; }
    
        public virtual Matches Matches { get; set; }
    }
}

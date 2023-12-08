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
    
    public partial class Players
    {
        public int PlayerId { get; set; }
        public Nullable<long> SystemId { get; set; }
        public short SportId { get; set; }
        public Nullable<short> CountryId { get; set; }
        public short GenderId { get; set; }
        public Nullable<short> PlayerPositionId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherName { get; set; }
        public string Hand { get; set; }
        public bool IsActive { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string SportCCPlayerId { get; set; }
        public string MarketValue { get; set; }
        public string Foot { get; set; }
        public string PlayerAgent { get; set; }
        public Nullable<short> BirthCityId { get; set; }
        public Nullable<short> BirthCountryId { get; set; }
        public string PlayerImage { get; set; }
        public Nullable<long> SportingLifePlayerId { get; set; }
        public string MatchedWithProvider { get; set; }
        public string CreatedByRole { get; set; }
        public Nullable<int> SharkScoresId { get; set; }
        public Nullable<int> SoccerwayPlayerId { get; set; }
        public string Age { get; set; }
        public string CountryOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public Nullable<int> StatsComPlayerId { get; set; }
        public byte[] change_stamp { get; set; }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SchoolU_Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class ClassAnnouncement
    {
        public int ClassAnnouncementsId { get; set; }
        public int ClassId { get; set; }
        public string ClassAnnouncement1 { get; set; }
    
        public virtual Class Class { get; set; }
    }
}

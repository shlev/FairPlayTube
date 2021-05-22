﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace FairPlayTube.DataAccess.Models
{
    public partial class VideoInfo
    {
        [Key]
        public long VideoInfoId { get; set; }
        public Guid AccountId { get; set; }
        [Required]
        [StringLength(50)]
        public string VideoId { get; set; }
        [Required]
        [StringLength(50)]
        public string Location { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [Required]
        [StringLength(50)]
        public string FileName { get; set; }
        [Required]
        [StringLength(500)]
        public string VideoBloblUrl { get; set; }
        [Required]
        [StringLength(500)]
        public string IndexedVideoUrl { get; set; }
        /// <summary>
        /// Video Owner Id
        /// </summary>
        public long ApplicationUserId { get; set; }
        public short VideoIndexStatusId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        [InverseProperty("VideoInfo")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [ForeignKey(nameof(VideoIndexStatusId))]
        [InverseProperty("VideoInfo")]
        public virtual VideoIndexStatus VideoIndexStatus { get; set; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Services.Zone.ViewModal
{
    public class RequestGetZoneByType
    {
        public int type { get; set; }
        public string cultureCode { get; set; }
    }
    public class RequestGetZoneDetailById
    {
        public int zoneId { get; set; }
        public string cultureCode { get; set; }
    }

    public class RequestGetPromotionDetailByZoneId
    {
        public int zoneId { get; set; }
        public string cultureCode { get; set; }
    }

    public class RequestGetZoneDetailByAlias
    {
        public string alias { get; set; }
        public string cultureCode { get; set; }
    }
    public class ResponseZoneMinify
    {
        public int id { get; set; }
        public string alias { get; set; }
        public string title { get; set; }
        public string icon { get; set; }
        public int parentId { get; set; }
        public string mapCroods { get; set; }
        public int level { get; set; }
        public string order { get; set; }
        public string avatar { get; set; }
        public int sortOrder { get; set; }
    }

    public class ResponseGetZoneDetailMinify
    {
        public int id { get; set; }
        public string name { get; set; }
        public string googleMapCrood { get; set; }
        public string content { get; set; }
        public string avatar { get; set; }
        public string icon { get; set; }
        public string banner { get; set; }
        public string suggestionSeason { get; set; }
        public string suggestionTraveldDate { get; set; }
        public string latitude { get; set; }
        public string longtitude { get; set; }

    }

    public class ZoneByTreeViewMinify
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int SortOrder { get; set; }
        public string LanguageCode { get; set; }
        public string Icon { get; set; }
        public int root { get; set; }
        public string Url { get; set; }
        public int level { get; set; }
        public string order { get; set; }
        public int IsShowMenu { get; set; }
        public string Banner { get; set; }
        public int Type { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeyword { get; set; }
        public string BannerLink { get; set; }
        public string UrlOld { get; set; }
        public string Filter { get; set; }
        public string ManufacturerId { get; set; }
        public int ZoneSearchType { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public int TotalTour { get; set; }
        public string MapCoords { get; set; }
        public string googleMapCrood { get; set; }
        //public string Filter { get; set; }

    }

    public class ZoneToRedirect
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int isHaveChild { get; set; }
    }

    public class ZoneSugget
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
    }


    public class ZoneDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public int? SortOrder { get; set; }
        public int? ParentId { get; set; }
        public byte Status { get; set; }
        public string Avatar { get; set; }
        public string Background { get; set; }
        public byte? Type { get; set; }
        public string Banner { get; set; }
        public string BannerLink { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string LanguageCode { get; set; }
        public decimal Rate { get; set; }
        public string UrlOld { get; set; }
        public string Filter { get; set; }
        public string comments { get; set; }
        public string faqs { get; set; }
        public string searchTags { get; set; }
        public string dynamicschema { get; set; }
        public List<ZoneByTreeViewMinify> breadcrumbs { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace MI.Entity.Models
{
    public partial class ZoneInLanguage
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public string BannerLink { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public string LanguageCode { get; set; }
        public int ZoneId { get; set; }
        public string MetaWebPage { get; set; }
        public string MetaNoIndex { get; set; }
        public string MetaCanonical { get; set; }
        public string BreadcrumbFirst { get; set; }
        public string UrlOld { get; set; }
        public string BreadcrumUrl { get; set; }
        public string QandA { get; set; } = "";
        public string notePlaceHolder { get; set; } = "";
        public string noteOptions { get; set; } = "";
        public string suggestionSeason { get; set; } = "";
        public string suggestionTraveldDate { get; set; } = "";
        public string comments { get; set; } = "";
        public string faqs { get; set; } = "";
        public string searchTags { get; set; } = "";
        public Language LanguageCodeNavigation { get; set; }
        public Zone Zone { get; set; }
    }
}

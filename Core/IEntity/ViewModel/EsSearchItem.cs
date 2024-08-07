using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace MI.Entity.ViewModel
{
    public class EsSearchItem
    {
        public int itemId { get; set; }
        public string itemName { get; set; }
        public string itemNameNorm { get; set; }
        public string itemAvatar { get; set; }
        public string itemType { get; set; }
        public string itemDescription { get; set; }
        public string itemDescriptionNorm { get; set; }
        public decimal itemPrice { get; set; }
        public string itemSearchKeyword { get; set; }
        public string itemSearchKeywordNorm { get; set; }
        public string languageCode { get; set; }
        public string itemUrl { get; set; }
        public string _score { get; set; }
    }
}

namespace Converter.Models
{
    public class InsertZone
    {
        public int term_id { get; set; }
        public string name { get; set; }
        public string taxonomy { get; set; }
        public int parent { get; set; }
        public int term_taxonomy_id { get; set; }
    }
    public class InsertProduct
    {
        public int ID { get; set; }
        public string post_title { get; set; }
        public string post_content { get; set; }
        public string post_url { get; set; }
        public string avatar { get; set; }
        public int tour_price { get; set; }
        public string ngay_dem { get; set; }
        public string mo_ta { get; set; }
        public string chuong_trinh { get; set; }
        public string bang_gia { get; set; }
        public string khoi_hanh { get; set; }
        public string bao_gom { get; set; }
        public string view_count { get; set; }
    }



    public class InsertProductInZone
    {
        public int ID { get; set; }
        public int zone_id { get; set; }
    }


    public class MergeZone
    {
        public int term_id { get; set; }
        public int term_taxonomy_id { get; set; }
        public string slug { get; set; }
    }
    
    public class MergeProduct
    {
        public int Id { get; set; }
        public string post_name { get; set; }
    }



}


using System.Collections.Generic;

namespace PlatformCMS.DTOModels
{
    public class CustomerGroupCreateModel
    {
        public string Name { get; set; }
        public List<int> CustomerIds { get; set; }
    }

    public class CustomerGroupUpdateModel
    {
        public string Name { get; set; }
        public List<int> CustomerIds { get; set; }
    }
    public class CustomerSearchRequest
    {
        public string Email { get; set; }
        public string Country { get; set; }
        public string Fullname { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

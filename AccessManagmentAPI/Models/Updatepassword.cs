using Microsoft.EntityFrameworkCore;

namespace AccessManagmentAPI.Models
{
    public class Updatepassword
    {
        
        public string username { get; set; }
        public string password { get; set; }
        public string otptext { get; set; }
    }
}

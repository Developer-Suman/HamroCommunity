namespace HamroCommunity.CustomHealthChecks.HealthChecksEndPoints
{
    public class HealthChecksEndPointConfig
    {
        public List<string> Endpoints { get; set; } = new List<string>
        {
            "https://localhost:7202/api/Location/Province/get-all",
             "https://localhost:7202/api/Location/GetAllDistrict",
            "https://localhost:7202/api/Location/City/get-all"

        };



    }
}

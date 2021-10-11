using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Persistence;

namespace API.Config
{
    public class ODataConfig
    {
        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Student>(nameof(Student));
            return builder.GetEdmModel();
        }
    }
}